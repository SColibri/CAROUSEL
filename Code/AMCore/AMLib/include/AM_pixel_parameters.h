
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
		_project = project;
		_case = new DBS_Case(database, IDCase);
		_case->load();

		_CALPHAD_DB = new DBS_CALPHADDatabase(database, -1);
		load_DBS_CALPHADDB();

		_scheilConfiguration = new DBS_ScheilConfiguration(database, -1);

	}

	void load_DBS_CALPHADDB(int IDCase)
	{
		if (_CALPHAD_DB == nullptr) return;

		AM_Database_Datatable DTable(_db, &AMLIB::TN_CALPHADDatabase());
		DTable.load_data(AMLIB::TN_CALPHADDatabase().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");

		if(DTable.row_count > 0)
		{
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_CALPHAD_DB->load(rowData);
		}
	}

	void load_DBS_ScheilConfig(int IDCase)
	{
		if (_scheilConfiguration == nullptr) return;

		AM_Database_Datatable DTable(_db, &AMLIB::TN_CALPHADDatabase());
		DTable.load_data(AMLIB::TN_CALPHADDatabase().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");

		if (DTable.row_count > 0)
		{
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_CALPHAD_DB->load(rowData);
		}
	}

private:

	//Models
	IAM_DBS* _db;
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