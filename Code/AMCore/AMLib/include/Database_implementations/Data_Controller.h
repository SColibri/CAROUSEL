#pragma once
#include "Data_stuctures/DBS_All_Structures_Header.h"
#include "../../interfaces/IAM_Database.h"
#include "../../include/Database_implementations/Database_scheme_content.h"
#include "../../include/AM_Database_Datatable.h"
#include "../AM_Config.h"
#include <filesystem>
#include <vector>
#include <string>

/// <summary>
/// Data controller class handles all database interactions for the 
/// graphical user interface. This specifies how the data should be loaded.
/// 
/// The user is allowed to search data by Element, phases, and other
/// making it easier for comparing between results.
/// </summary>
class Data_Controller
{
public:

	enum DATATABLES
	{
		PROJECTS,
		CASES,
		ELEMENTS,
		PHASES,
		ELEMENTS_COMPOSITION,
		SCHEIL_PHASE_FRACTIONS,
		CALPHAD
	};

	Data_Controller(IAM_Database* db, 
					AM_Config* configuration,
					int ID) :
		_db(db)
	{
		_configuration = configuration;
		_project = new DBS_Project(_db, ID);
		set_project_default();
		create_tables();
		load_data();
	}

#pragma region Project
	void set_project_default()
	{
		if (_project == nullptr) return;
		_project->Name = "New project";
		_project->APIName = _configuration->get_api_path();
	}
	void set_project_ID(int ID)
	{
		if(_project->id() != ID)
		{
			delete _project;
			_project = new DBS_Project(_db, ID);
			set_project_default();
		}
	}

	void set_project_name(std::string Name)
	{
		_project->Name = Name;
		_project->APIName = std::filesystem::path(_configuration->get_api_path()).filename().string();
	}

	std::vector<std::string> static csv_list_SelectedElements(IAM_Database* db, int projectID)
	{
		AM_Database_Datatable dataT(db, &AMLIB::TN_SelectedElements());
		dataT.load_data(AMLIB::TN_SelectedElements().columnNames[1] + " = \'" + std::to_string(projectID) + "\'");
		return dataT.get_column_data(2);
	}

	size_t static get_reference_element_ByID(IAM_Database* db, int projectID)
	{
		AM_Database_Datatable dataT(db, &AMLIB::TN_SelectedElements());
		dataT.load_data(AMLIB::TN_SelectedElements().columnNames[1] + " = \'" + std::to_string(projectID) + "\' AND " +
						AMLIB::TN_SelectedElements().columnNames[3] + " = " + std::to_string(1) + " ");
		
		size_t out;
		sscanf(dataT(2, 0).c_str(), "%zu", &out);
		return out;
	}
#pragma endregion

#pragma region Case
	/// <summary>
	/// Select a Case ID
	/// </summary>
	/// <param name="ID"></param>
	void select_case(int ID)
	{
		clear_temp_data();
		_temp_case = new DBS_Case(_db, ID);
	}

	/// <summary>
	/// set_caseScript content
	/// </summary>
	/// <param name="scriptContent"></param>
	void set_caseScript(std::string scriptContent)
	{
		if (_temp_case == nullptr) select_case(-1);
		_temp_case->Script = scriptContent;
	}

	std::string run_case()
	{
		std::string out{""};
		if (_temp_case == nullptr) 
		{
			out += "Please add a script to run! \n";
		}
		
		if (_temp_case->Script.length() == 0) 
		{
			out += "Please add a script to run! \n";
		}

		// if (_project->id() == -1) _project->save();
		

		return out;

	}
#pragma endregion

	static std::string get_csv(IAM_Database* db,DATATABLES selOption)
	{
		AM_Database_TableStruct tbStruct;

		switch (selOption)
		{
		case Data_Controller::PROJECTS:
			tbStruct = AMLIB::TN_Projects();
			break;
		case Data_Controller::CASES:
			tbStruct = AMLIB::TN_Case(); 
			break;
		case Data_Controller::ELEMENTS:
			tbStruct = AMLIB::TN_Element(); 
			break;
		case Data_Controller::PHASES:
			tbStruct = AMLIB::TN_Phase(); 
			break;
		case Data_Controller::ELEMENTS_COMPOSITION:
			tbStruct = AMLIB::TN_ElementComposition(); 
			break;
		case Data_Controller::SCHEIL_PHASE_FRACTIONS:
			tbStruct = AMLIB::TN_ScheilPhaseFraction(); 
			break;
		case Data_Controller::CALPHAD:
			tbStruct = AMLIB::TN_CALPHADDatabase(); 
			break;
		default:
			break;
		}
		if(tbStruct.columnNames.size() == 0) return "No data available";

		AM_Database_Datatable tableReturn(db, &tbStruct);
		tableReturn.load_data();
		return tableReturn.get_csv();
	}

private:
	IAM_Database* _db{ nullptr };
	AM_Config* _configuration{ nullptr };
	DBS_Project* _project{ nullptr };

#pragma region DataTables
	AM_Database_Datatable* _datatable_projects{ nullptr };
	AM_Database_Datatable* _datatable_cases{ nullptr };
	AM_Database_Datatable* _datatable_elements{ nullptr };
	AM_Database_Datatable* _datatable_phases{ nullptr };
	
	AM_Database_Datatable* _datatable_elements_composition{ nullptr };
	AM_Database_Datatable* _datatable_scheil_configurations{ nullptr };
	AM_Database_Datatable* _datatable_scheil_phase_fractions{ nullptr };
	AM_Database_Datatable* _datatable_CALPHADDB{ nullptr };
#pragma endregion

#pragma region TempDataStructures
	DBS_Case* _temp_case{nullptr};
	DBS_CALPHADDatabase* _temp_CALPHADDB{ nullptr };
	std::vector<DBS_ElementComposition> _temp_compositions;
	std::vector<DBS_Element> _temp_elements;
	std::vector<DBS_Phase> _temp_phases;
	DBS_ScheilConfiguration* _temp_ScheilConfiguration {nullptr};
	std::vector<DBS_ScheilPhaseFraction> _temp_ScheilPhaseFractions;
#pragma endregion

	void create_tables()
	{
		if(_datatable_cases == nullptr)
			_datatable_cases = new AM_Database_Datatable(_db, &AMLIB::TN_Case());
		
		if (_datatable_elements == nullptr)
			_datatable_elements = new AM_Database_Datatable(_db, &AMLIB::TN_Element());
		
		if (_datatable_phases == nullptr)
			_datatable_phases = new AM_Database_Datatable(_db, &AMLIB::TN_Phase());
		
		if (_datatable_elements_composition == nullptr)
			_datatable_elements_composition = new AM_Database_Datatable(_db, &AMLIB::TN_ElementComposition());
		
		if (_datatable_scheil_configurations == nullptr)
			_datatable_scheil_configurations = new AM_Database_Datatable(_db, &AMLIB::TN_ScheilConfiguration());
		
		if (_datatable_CALPHADDB == nullptr)
			_datatable_CALPHADDB = new AM_Database_Datatable(_db, &AMLIB::TN_CALPHADDatabase());
		
		if (_datatable_scheil_phase_fractions == nullptr)
			_datatable_scheil_phase_fractions = new AM_Database_Datatable(_db, &AMLIB::TN_ScheilPhaseFraction());

		if (_datatable_projects == nullptr)
			_datatable_projects = new AM_Database_Datatable(_db, &AMLIB::TN_Projects());

	}

	void load_data()
	{
		_datatable_projects->load_data();
		if (_project->id() == -1) { return; }
		create_tables();
		
		_datatable_cases->load_data(AMLIB::TN_Case().columnNames[1] +  " = \'" + std::to_string(_project->id()) + "\'");
		_datatable_elements->load_data();
		_datatable_phases->load_data();

		std::vector<std::string> casesID = _datatable_cases->get_column_data(0);
		std::string queryBuild{ "" };

		for each (std::string idCase in casesID)
		{
			if (queryBuild.length() > 0) queryBuild += " OR ";
			queryBuild += " IDCase = " + idCase;
		}

		_datatable_elements_composition->load_data(queryBuild);
		_datatable_scheil_configurations->load_data(queryBuild);
		_datatable_CALPHADDB->load_data(queryBuild);

		std::vector<std::string> ScheilConfigs = _datatable_scheil_configurations->get_column_data(0);
		std::string queryBuild_Scheil{ "" };

		for each (std::string idCase in casesID)
		{
			if (queryBuild_Scheil.length() > 0) queryBuild_Scheil += " OR ";
			queryBuild_Scheil += " IDScheilConfig = " + idCase;
		}

		_datatable_scheil_phase_fractions->load_data(queryBuild);
	}

	

	void clear_temp_data()
	{
		if (_temp_case != nullptr) delete _temp_case;
		if (_temp_CALPHADDB != nullptr) delete _temp_CALPHADDB;
		if (_temp_ScheilConfiguration != nullptr) delete _temp_ScheilConfiguration;
		_temp_compositions.clear();
		_temp_elements.clear();
		_temp_phases.clear();
		_temp_ScheilPhaseFractions.clear();
	}

	void clear_tables()
	{
		if (_datatable_cases != nullptr) delete _datatable_cases;
		if (_datatable_elements != nullptr) delete _datatable_elements;
		if (_datatable_phases != nullptr) delete _datatable_phases;
		if (_datatable_elements_composition != nullptr) delete _datatable_elements_composition;
		if (_datatable_scheil_configurations != nullptr) delete _datatable_scheil_configurations;
		if (_datatable_scheil_phase_fractions != nullptr) delete _datatable_scheil_phase_fractions;
		if (_datatable_CALPHADDB != nullptr) delete _datatable_CALPHADDB;
		if (_project != nullptr) delete _project;
	}

	void dispose()
	{
		clear_temp_data();
		clear_tables();
	}

};

