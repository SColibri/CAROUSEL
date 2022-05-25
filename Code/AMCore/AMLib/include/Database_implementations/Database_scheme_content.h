#pragma once

#include "../AM_Database_TableStruct.h"
#include <vector>

/// <summary>
/// Database scheme content, here we list all system tables that are used
/// </summary>
namespace AMLIB
{

#pragma region ProjectSpace
	/// <summary>
	/// Projects table i the node for all content inside a project
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_Projects()
	{
		AM_Database_TableStruct out;
		out.add_new("ID","INTEGER PRIMARY KEY");
		out.add_new("Name","TEXT");
		out.add_new("API_Name", "TEXT");
		out.tableName = "Projects";

		return out;
	}

	/// <summary>
	/// Element table is a table that hosts all elements avilable in current database
	/// and also helps identifying them afterwards. This table should be filled while
	/// loading a new dtabase or when anew element is selected.
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_Element()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("Name", "TEXT");
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
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("Name", "TEXT");
		out.tableName = "Phase";

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
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("ScriptName", "TEXT");
		out.add_new("Date", "TEXT");
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
	/// Scheil configuration is unique for each case, if the user stores information
	/// about a scheil run, we will be able to see the configuration and the results will be
	/// stored inside the table ScheilPhaseFraction.
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_ScheilConfiguration()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("StartTemperature", "REAL");
		out.add_new("EndTemperature", "REAL");
		out.add_new("StepSize", "REAL");
		out.add_new("DependentPhase", "INTEGER");
		out.add_new("Min_Liquid_Fraction", "REAL");
		out.tableName = "SccheilConfiguration";

		return out;
	}

#pragma region ScheilConfig
	/// <summary>
	/// Scheil phase fracton stores the accumulated phase fraction for the selected
	/// phases chose on the case configuration. The scheil phase fraction correspond 
	/// to the ID of the Scheil configuration
	/// </summary>
	/// <returns></returns>
	static AM_Database_TableStruct TN_ScheilPhaseFraction()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDScheilConfig", "INTEGER");
		out.add_new("IDPhase", "INTEGER");
		out.add_new("TypeComposition", "TEXT");
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
		out.add_new("Name", "TEXT");
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
		out.push_back(TN_ElementComposition());
		out.push_back(TN_ScheilConfiguration());
		out.push_back(TN_ScheilPhaseFraction());
		out.push_back(TN_CALPHADDatabase());

		return out;
	}



}