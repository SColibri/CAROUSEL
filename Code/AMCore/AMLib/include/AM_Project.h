#pragma once
#include <vector>
#include "AM_pixel_parameters.h"
#include "../interfaces/IAM_Database.h"
#include "Database_implementations/Database_scheme_content.h"
#include "AM_Database_Datatable.h"

/// <summary>
/// Controller class that manages all project functions.
/// </summary>
class AM_Project 
{
public:
	AM_Project(IAM_Database* database, int id)
	{
		_db = database;
		_project = new DBS_Project(database, id);
	}

	~AM_Project()
	{
		if (_project != nullptr) delete _project;
	}

	void set_project_name(std::string newName, std::string apiPath)
	{
		_project->Name = newName;
		_project->APIName = apiPath;
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
		return dataT.get_csv();
	}

	std::string csv_list_selectedPhases(int IDCase)
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_SelectedPhases());
		dataT.load_data(AMLIB::TN_SelectedPhases().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");
		return dataT.get_csv();
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
		if (_project == nullptr) return 1;
		DBS_Case newCase(_db, -1);
		newCase.IDProject = _project->id();
		newCase.Name = newName;
		newCase.IDGroup = 0;
		newCase.save();
		_singlePixel_cases.push_back(AM_pixel_parameters(_db, _project, newCase.id()));
		return 0;
	}
#pragma endregion


private:
	IAM_Database* _db;
	DBS_Project* _project{nullptr};
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