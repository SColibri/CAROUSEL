#pragma once
#include "Data_stuctures/DBS_All_Structures_Header.h"
#include "../../interfaces/IAM_Database.h"
#include "../../include/Database_implementations/Database_scheme_content.h"
#include "../../include/AM_Database_Datatable.h"
#include "../AM_Config.h"
#include <filesystem>
#include <vector>
#include <string>

class Data_Controller
{
public:
	Data_Controller(IAM_Database* db, 
					AM_Config* configuration,
					int ID) :
		_db(db)
	{
		_project = new DBS_Project(_db, ID);
		_configuration = configuration;
		load_data();
	}

	void set_project_name(std::string Name)
	{
		_project->Name = Name;
		_project->APIName = std::filesystem::path(_configuration->get_api_path()).filename().string();
		_project->save();
	}

private:
	IAM_Database* _db{ nullptr };
	AM_Config* _configuration{ nullptr };
	DBS_Project* _project{ nullptr };

#pragma region DataTables
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

	}

	void load_data()
	{
		if (_project->id() == -1) { _project->save(); return; }
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

	void select_case(int ID)
	{
		clear_temp_data();
		_temp_case = new DBS_Case(_db, ID);

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

