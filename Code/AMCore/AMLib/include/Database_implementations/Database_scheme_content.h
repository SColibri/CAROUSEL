#pragma once

#include "../AM_Database_TableStruct.h"
#include <vector>

/// <summary>
/// Database scheme content, here we list all system tables that are used.
/// To add a new system table: 
/// -> Create a TN_function, as standars always add an autoincrement ID
/// -> Add it to the get_structure() function
/// -> Create a DBS object that inherits IAM_DBS interface and implement all virtual functions.
/// </summary>
namespace AMLIB
{

#pragma region GlobalSpace
	/// <summary>
	/// Element table is a table that hosts all elements avilable in current database
	/// and also helps identifying them afterwards. This table should be filled while
	/// loading a new dtabase or when anew element is selected.
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_Element()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY"); // Autoincrement ID
		out.add_new("Name", "TEXT"); // Element name
		out.tableName = "Element";

		return out;
	}

	/// <summary>
	/// Phase table contains all available phases that were selected.
	/// phases should be added when called.
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_Phase()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY"); // Auto increment ID
		out.add_new("Name", "TEXT"); // Name of phase
		out.tableName = "Phase";

		return out;
	}
#pragma endregion

#pragma region ProjectSpace
	/// <summary>
	/// Projects table i the node for all content inside a project
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_Projects()
	{
		AM_Database_TableStruct out;
		out.add_new("ID","INTEGER PRIMARY KEY"); // Auto increment ID
		out.add_new("Name","TEXT"); // User defined project name
		out.add_new("API_Name", "TEXT"); // API used -> API dll name
		out.tableName = "Projects";

		return out;
	}

#pragma endregion

#pragma region Case_Space
	/// <summary>
	/// Each case contains information for a specific composition setup
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_Case()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY"); // Auto increment ID
		out.add_new("IDProject", "INTEGER"); // ID of project
		out.add_new("IDGroup", "INTEGER"); // Group ID, cases are grouped by this input. By default IDCase = 0 is single pixel case group (not for objects)
		out.add_new("Name", "TEXT"); // Name of the case
		out.add_new("Script", "TEXT"); // Script content or script name
		out.add_new("Date", "TEXT"); // Date of creation
		out.add_new("PosX", "REAL"); // x position
		out.add_new("PosY", "REAL"); // y position
		out.add_new("PosZ", "REAL"); // z position (layer)
		out.tableName = "Case";

		return out;
	}

	/// <summary>
	/// Element composition table contains the composition for each element,
	/// the group is identified by the ID of the Case
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_ElementComposition()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("IDElement", "INTEGER");
		out.add_new("TypeComposition", "TEXT");
		out.add_new("Value", "REAL");
		out.tableName = "ElementComposition";

		return out;
	}

	/// <summary>
	/// Selected Phases that are going to be analized
	/// Each phase is identified by an ID number
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_SelectedPhases()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("IDPhase", "INTEGER");
		out.tableName = "SelectedPhases";

		return out;
	}

	/// <summary>
	/// Selected Phases that are going to be analized
	/// Each phase is identified by an ID number
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_SelectedElements()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY"); // Autoincrement ID
		out.add_new("IDProject", "INTEGER"); // Selection corresponding to project
		out.add_new("IDElement", "INTEGER"); // ID of element
		out.tableName = "SelectedElements";

		return out;
	}


#pragma region Equilibrium
	/// <summary>
	/// Equilibrium configuration
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_EquilibriumConfiguration()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY"); // Autoincrement ID
		out.add_new("IDCase", "INTEGER"); // ID to case
		out.add_new("Temperature", "REAL"); // Equilibrium at temperature
		out.add_new("StartTemperature", "REAL");
		out.add_new("EndTemperature", "REAL");
		out.add_new("TemperatureType", "TEXT"); // Defines if in Celsius or Kelvin
		out.add_new("Pressure", "REAL"); // Pressure for equilibrium calculation
		out.tableName = "EquilibriumConfiguration";

		return out;
	}

	/// <summary>
	/// Equilibrium phase fraction with respect to the equilibrium configuration
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_EquilibriumPhaseFractions()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY"); // Autoincrement ID
		out.add_new("IDCase", "INTEGER"); // ID to Equilibrium configuration
		out.add_new("IDPhase", "INTEGER"); // Equilibrium at temperature
		out.add_new("Temperature", "REAL"); // Equilibrium at temperature
		out.add_new("Value", "REAL"); // Defines if in Celsius or Kelvin
		out.tableName = "EquilibriumPhaseFracction";

		return out;
	}

#pragma endregion

#pragma region ScheilConfig
	/// <summary>
	/// Scheil configuration is unique for each case, if the user stores information
	/// about a scheil run, we will be able to see the configuration and the results will be
	/// stored inside the table ScheilPhaseFraction.
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_ScheilConfiguration()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY"); // Autoincrement ID
		out.add_new("IDCase", "INTEGER");
		out.add_new("StartTemperature", "REAL");
		out.add_new("EndTemperature", "REAL");
		out.add_new("StepSize", "REAL");
		out.add_new("DependentPhase", "INTEGER");
		out.add_new("Min_Liquid_Fraction", "REAL");
		out.tableName = "ScheilConfiguration";

		return out;
	}

	/// <summary>
	/// Scheil phase fracton stores the accumulated phase fraction for the selected
	/// phases chose on the case configuration. The scheil phase fraction correspond 
	/// to the ID of the Scheil configuration
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_ScheilPhaseFraction()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY"); // Autoincrement ID
		out.add_new("IDCase", "INTEGER");
		out.add_new("IDPhase", "INTEGER");
		out.add_new("TypeComposition", "TEXT");
		out.add_new("Temperature", "REAL");
		out.add_new("Value", "REAL");
		out.tableName = "ScheilPhaseFraction";

		return out;
	}
#pragma endregion

	/// <summary>
	/// Specifies the name of the database used for the current case run.
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_CALPHADDatabase()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("Thermodynamic", "TEXT");
		out.add_new("Physical", "TEXT");
		out.add_new("Mobility", "TEXT");
		out.tableName = "CALPHADDatabase";

		return out;
	}

#pragma endregion

	/// <summary>
	/// returns a vector with the whole system database scheme, this is used
	/// when creating a new database.
	/// </summary>
	/// <returns></returns>
	static std::vector<AM_Database_TableStruct> get_structure()
	{
		std::vector<AM_Database_TableStruct> out;
		
		out.push_back(TN_Projects());
		out.push_back(TN_Element());
		out.push_back(TN_Phase());
		out.push_back(TN_Case());
		out.push_back(TN_SelectedElements());
		out.push_back(TN_SelectedPhases());
		out.push_back(TN_EquilibriumConfiguration());
		out.push_back(TN_EquilibriumPhaseFractions());
		out.push_back(TN_ElementComposition());
		out.push_back(TN_ScheilConfiguration());
		out.push_back(TN_ScheilPhaseFraction());
		out.push_back(TN_CALPHADDatabase());

		return out;
	}



}