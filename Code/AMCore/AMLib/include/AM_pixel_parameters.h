
#include <string>
#include <vector>
#include "Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "Database_implementations/Data_stuctures/DBS_Project.h"
#include "Database_implementations/Data_stuctures/DBS_Case.h"
#include "Database_implementations/Data_stuctures/DBS_ElementComposition.h"
#include "Database_implementations/Data_stuctures/DBS_ScheilConfiguration.h"
#include "Database_implementations/Data_stuctures/DBS_SelectedPhases.h"
#include "Database_implementations/Data_stuctures/DBS_ScheilPhaseFraction.h"
#include "Database_implementations/Data_stuctures/DBS_CALPHADDatabase.h"
#include "Database_implementations/Data_stuctures/DBS_EquilibriumConfiguration.h"
#include "Database_implementations/Data_stuctures/DBS_EquilibriumPhaseFractions.h"
#include "Database_implementations/Data_stuctures/DBS_SelectedElements.h"

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Block parameters
/// </summary>
class AM_pixel_parameters {

public:
	AM_pixel_parameters(IAM_Database* database, 
						DBS_Project* project, 
						int IDCase)
	{
		_db = database;
		_project = project;
		_case = new DBS_Case(database, IDCase);
		_case->load();

		_CALPHAD_DB = new DBS_CALPHADDatabase(_db, -1);
		_scheilConfiguration = new DBS_ScheilConfiguration(_db, -1);
		_equilibriumConfiguration = new DBS_EquilibriumConfiguration(_db, -1);
		load_all();
	}

	void load_all()
	{
		load_DBS_CALPHADDB(_case->id());
		load_DBS_ScheilConfig(_case->id());
		load_DBS_EquilibriumConfig(_case->id());
		load_DBS_EquilibriumPhaseFraction();
		load_DBS_ScheilPhaseFraction();
		load_DBS_elementComposition();
		load_DBS_selectedPhases();
	}

	void load_DBS_CALPHADDB(int IDCase)
	{
		if (_CALPHAD_DB == nullptr) return;

		AM_Database_Datatable DTable(_db, &AMLIB::TN_CALPHADDatabase());
		DTable.load_data(AMLIB::TN_CALPHADDatabase().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");

		if(DTable.row_count() > 0)
		{
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_CALPHAD_DB->load(rowData);
		}
	}

	void load_DBS_ScheilConfig(int IDCase)
	{
		if (_scheilConfiguration == nullptr) return;

		AM_Database_Datatable DTable(_db, &AMLIB::TN_ScheilConfiguration());
		DTable.load_data(AMLIB::TN_ScheilConfiguration().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");

		if (DTable.row_count() > 0)
		{
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_scheilConfiguration->load(rowData);
		}
	}

	void load_DBS_EquilibriumConfig(int IDCase)
	{
		if (_equilibriumConfiguration == nullptr) return;

		AM_Database_Datatable DTable(_db, &AMLIB::TN_EquilibriumConfiguration());
		DTable.load_data(AMLIB::TN_EquilibriumConfiguration().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");

		if (DTable.row_count() > 0)
		{
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_equilibriumConfiguration->load(rowData);
		}
	}

	void load_DBS_EquilibriumPhaseFraction()
	{
		if (_equilibriumConfiguration == nullptr) return;
		_equilibriumPhaseFractions.clear();

		AM_Database_Datatable DTable(_db, &AMLIB::TN_EquilibriumPhaseFractions());
		DTable.load_data(AMLIB::TN_EquilibriumPhaseFractions().columnNames[1] + " = \'" + std::to_string(_equilibriumConfiguration->id()) + "\'");

		if (DTable.row_count() > 0)
		{
			for(int n1 = 0 ; n1 < DTable.row_count(); n1 ++)
			{
				std::vector<std::string> rowData = DTable.get_row_data(n1);
				DBS_EquilibriumPhaseFraction newEquib(_db, -1);
				newEquib.load(rowData);
				_equilibriumPhaseFractions.push_back(newEquib);
			}
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_equilibriumConfiguration->load(rowData);
		}
	}

	void load_DBS_ScheilPhaseFraction()
	{
		if (_scheilConfiguration == nullptr) return;
		_scheilPhaseFractions.clear();

		AM_Database_Datatable DTable(_db, &AMLIB::TN_EquilibriumPhaseFractions());
		DTable.load_data(AMLIB::TN_EquilibriumPhaseFractions().columnNames[1] + " = \'" + std::to_string(_scheilConfiguration->id()) + "\'");

		if (DTable.row_count() > 0)
		{
			for (int n1 = 0; n1 < DTable.row_count(); n1++)
			{
				std::vector<std::string> rowData = DTable.get_row_data(n1);
				DBS_ScheilPhaseFraction newEquib(_db, -1);
				newEquib.load(rowData);
				_scheilPhaseFractions.push_back(newEquib);
			}
		}
	}

	void load_DBS_elementComposition()
	{
		if (_case == nullptr) return;
		_elementComposition.clear();

		AM_Database_Datatable DTable(_db, &AMLIB::TN_ElementComposition());
		DTable.load_data(AMLIB::TN_ElementComposition().columnNames[1] + " = \'" + std::to_string(_case->id()) + "\'");

		if (DTable.row_count() > 0)
		{
			for (int n1 = 0; n1 < DTable.row_count(); n1++)
			{
				std::vector<std::string> rowData = DTable.get_row_data(n1);
				DBS_ElementComposition newEquib(_db, -1);
				newEquib.load(rowData);
				_elementComposition.push_back(newEquib);
			}
		}
	}

	void load_DBS_selectedPhases()
	{
		if (_case == nullptr) return;
		_selectedPhases.clear();

		AM_Database_Datatable DTable(_db, &AMLIB::TN_SelectedPhases());
		DTable.load_data(AMLIB::TN_SelectedPhases().columnNames[1] + " = \'" + std::to_string(_case->id()) + "\'");

		if (DTable.row_count() > 0)
		{
			for (int n1 = 0; n1 < DTable.row_count(); n1++)
			{
				std::vector<std::string> rowData = DTable.get_row_data(n1);
				DBS_SelectedPhases newEquib(_db, -1);
				newEquib.load(rowData);
				_selectedPhases.push_back(newEquib);
			}
		}
	}


private:

	//Models
	IAM_Database* _db;
	DBS_Project* _project;
	DBS_CALPHADDatabase* _CALPHAD_DB;
	DBS_Case* _case;
	DBS_ScheilConfiguration* _scheilConfiguration;
	DBS_EquilibriumConfiguration* _equilibriumConfiguration;

	std::vector<DBS_EquilibriumPhaseFraction> _equilibriumPhaseFractions;
	std::vector<DBS_ScheilPhaseFraction> _scheilPhaseFractions;
	std::vector<DBS_ElementComposition> _elementComposition;
	std::vector<DBS_SelectedPhases> _selectedPhases;



};
/** @}*/