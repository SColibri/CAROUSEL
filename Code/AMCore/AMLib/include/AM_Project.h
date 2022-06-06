#pragma once
#include <vector>
#include <filesystem>
#include "AM_pixel_parameters.h"
#include "../interfaces/IAM_Database.h"
#include "../interfaces/IAM_lua_functions.h"
#include "Database_implementations/Database_scheme_content.h"
#include "AM_Database_Datatable.h"

/// <summary>
/// Controller class that manages all project functions.
/// </summary>
class AM_Project 
{
public:
	AM_Project(IAM_Database* database, AM_Config* configuration, int id)
	{
		_db = database; 
		_configuration = configuration;
		_project = new DBS_Project(database, id);
		if (_project->load() != 0) _project->set_id(-1);
	}

	AM_Project(IAM_Database* database, AM_Config* configuration, std::string projectName)
	{
		_db = database;
		_configuration = configuration;
		_project = new DBS_Project(database, -1);
		_project->load_ByName(projectName);

		if (_project->load() != 0) _project->set_id(-1);
	}

	~AM_Project()
	{
		if (_project != nullptr) delete _project;
		if (_tempPixel != nullptr) delete _tempPixel;
	}

	void set_project_name(std::string newName, std::string apiPath)
	{
		_project->Name = newName;
		_project->APIName = std::filesystem::path(apiPath).filename().string();
		_project->save();
	}

	const std::string& get_project_name() 
	{
		return _project->Name;
	}

	const std::string& get_project_APIName()
	{
		return _project->APIName;
	}

	const int& get_project_ID()
	{
		return _project->id();
	}

	std::string get_project_data_csv()
	{
		return IAM_Database::csv_join_row(_project->get_input_vector(), IAM_Database::Delimiter);
	}

	std::vector<AM_pixel_parameters>& get_singlePixel_Cases()
	{
		return _singlePixel_cases;
	}

	void refresh_data()
	{
		load_singlePixel_Cases();
		load_DBS_selectedElements();
	}

	std::vector<std::string> get_selected_elements_ByName() 
	{
		std::vector<std::string> out;

		for each (DBS_SelectedElements var in _selectedElements)
		{
			DBS_Element tempElement(_db, var.IDElement);
			tempElement.load();
			out.push_back(tempElement.Name);
		}

		return out;
	}

	bool project_is_valid() 
	{
		if (_project = nullptr) return false;
		if (_project->id() == -1) _project->save();
		if (_project->id() == -1) return false;

		return true;
	}

#pragma region Project
	std::string csv_list_cases_singlePixel() 
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_Case());
		dataT.load_data(AMLIB::TN_Case().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\' AND " + 
						AMLIB::TN_Case().columnNames[2] + " = \'0\' ");
		return dataT.get_csv();
	}

	std::string csv_list_cases_Object()
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_Case());
		dataT.load_data(AMLIB::TN_Case().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\' AND " +
			AMLIB::TN_Case().columnNames[2] + " != 0 ");
		return dataT.get_csv();
	}

	std::string csv_list_SelectedElements()
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_SelectedElements());
		dataT.load_data(AMLIB::TN_SelectedElements().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\'");
		return IAM_Database::csv_join_row(dataT.get_column_data(2), IAM_Database::Delimiter);
	}

	std::vector<std::string> static csv_list_SelectedElements(IAM_Database* db, int projectID)
	{
		AM_Database_Datatable dataT(db, &AMLIB::TN_SelectedElements());
		dataT.load_data(AMLIB::TN_SelectedElements().columnNames[1] + " = \'" + std::to_string(projectID) + "\'");
		return dataT.get_column_data(2);
	}

	std::string csv_list_selectedPhases(int IDCase)
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_SelectedPhases());
		dataT.load_data(AMLIB::TN_SelectedPhases().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");
		return IAM_Database::csv_join_row(dataT.get_column_data(2), IAM_Database::Delimiter);
	}

	std::string csv_list_equilibriumConfiguration(int IDCase)
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_EquilibriumConfiguration());
		dataT.load_data(AMLIB::TN_EquilibriumConfiguration().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");
		return dataT.get_csv();
	}

	std::string csv_list_scheilConfiguration(int IDCase)
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_ScheilConfiguration());
		dataT.load_data(AMLIB::TN_ScheilConfiguration().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");
		return dataT.get_csv();
	}

#pragma endregion

#pragma region SinglePixel_Cases
	int new_singlePixel_Case(std::string newName)
	{
		if (!project_is_valid()) return 1;

		int newIDCase = AM_pixel_parameters::create_new_pixel(_db, _configuration, _project->id(), newName);
		_singlePixel_cases.push_back(AM_pixel_parameters(_db, _project, newIDCase));
		return 0;
	}
#pragma endregion

#pragma region Cases
	AM_pixel_parameters* get_pixelCase(int IDCase)
	{
		AM_pixel_parameters* out{ nullptr };

		//find in singlePixels
		auto SPC_it = std::find_if(_singlePixel_cases.begin(), _singlePixel_cases.end(), [&IDCase](AM_pixel_parameters& obj) {return obj.get_caseID() == IDCase; });
		if(SPC_it != _singlePixel_cases.end())
		{
			auto SPC_index = std::distance(_singlePixel_cases.begin(), SPC_it);
			out = &_singlePixel_cases[SPC_index];
		}

		//TODO: add search pattern when adding objects and layers in the project
		if(out == nullptr)
		{
			// search on the object list
		}

		return out;
	}

	void create_case_template() 
	{
		if (_tempPixel != nullptr) delete _tempPixel;
		_tempPixel = new AM_pixel_parameters(_db, _project, -1);
	}

	AM_pixel_parameters* get_case_template()
	{
		return _tempPixel;
	}
	
#pragma endregion


private:
	IAM_Database* _db;
	AM_Config* _configuration;
	DBS_Project* _project{nullptr};
	AM_pixel_parameters* _tempPixel {nullptr};
	std::vector<AM_pixel_parameters> _singlePixel_cases;
	std::vector<DBS_SelectedElements> _selectedElements;

	void load_singlePixel_Cases()
	{
		AM_Database_Datatable CaseTables(_db, &AMLIB::TN_Case());
		CaseTables.load_data(AMLIB::TN_Case().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\' AND " +
			AMLIB::TN_Case().columnNames[2] + " = \'0\'");

		if (CaseTables.row_count() > 0)
		{
			for (int n1 = 0; n1 < CaseTables.row_count(); n1++)
			{
				AM_pixel_parameters caseObject(_db, _project, std::stoi(CaseTables(0, n1)));
				_singlePixel_cases.push_back(caseObject);
			}
		}
	}
	void load_DBS_selectedElements()
	{
		if (_project == nullptr) return;
		_selectedElements.clear();

		AM_Database_Datatable DTable(_db, &AMLIB::TN_SelectedElements());
		DTable.load_data(AMLIB::TN_SelectedElements().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\'");

		if (DTable.row_count() > 0)
		{
			for (int n1 = 0; n1 < DTable.row_count(); n1++)
			{
				std::vector<std::string> rowData = DTable.get_row_data(n1);
				DBS_SelectedElements newEquib(_db, -1);
				newEquib.load(rowData);
				_selectedElements.push_back(newEquib);
			}
		}
	}

};