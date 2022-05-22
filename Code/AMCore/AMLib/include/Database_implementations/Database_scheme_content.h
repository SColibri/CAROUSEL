#pragma once

#include "../AM_Database_TableStruct.h"
#include <vector>

namespace AMLIB
{

#pragma region ProjectSpace
	static AM_Database_TableStruct TN_Projects()
	{
		AM_Database_TableStruct out;
		out.add_new("ID","INTEGER PRIMARY KEY");
		out.add_new("Name","TEXT");
		out.add_new("API_Name", "TEXT");
		out.tableName = "Projects";

		return out;
	}

	static AM_Database_TableStruct TN_Element()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("Name", "TEXT");
		out.tableName = "Element";

		return out;
	}

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