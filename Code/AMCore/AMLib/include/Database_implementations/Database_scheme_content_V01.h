#pragma once
#include "../AM_Database_TableStruct.h"
#include <vector>


/* ------------------------------------------------------
DO NOT EDIT, this file is generated using a cmake script.

Database scheme that holds all table data structures, this
file is used for mapping the data structure into the sql
database.

HOW TO USE:
- Add '[DBS_Parameter]' as a tag for each property that
	should be added to the database.
- For more information refer to the cmake scheme builder
*/// ----------------------------------------------------

namespace AMLIB
{

	static AM_Database_TableStruct TN_ActivePhases()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDProject", "INTEGER");
		out.add_new("IDPhase", "INTEGER");
		out.tableName = "ActivePhases";
		return out;
	}

	static AM_Database_TableStruct TN_ActivePhases_Configuration()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDProject", "INTEGER");
		out.add_new("StartTemp", "INTEGER");
		out.add_new("EndTemp", "INTEGER");
		out.add_new("StepSize", "REAL");
		out.tableName = "ActivePhases_Configuration";
		return out;
	}

	static AM_Database_TableStruct TN_ActivePhases_ElementComposition()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDProject", "INTEGER");
		out.add_new("IDElement", "INTEGER");
		out.add_new("Value", "REAL");
		out.tableName = "ActivePhases_ElementComposition";
		return out;
	}

	static AM_Database_TableStruct TN_CALPHADDatabase()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDProject", "INTEGER");
		out.add_new("Thermodynamic", "TEXT");
		out.add_new("Physical", "TEXT");
		out.add_new("Mobility", "TEXT");
		out.tableName = "CALPHADDatabase";
		return out;
	}

	static AM_Database_TableStruct TN_Case()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDProject", "INTEGER");
		out.add_new("IDGroup", "INTEGER");
		out.add_new("Name", "TEXT");
		out.add_new("Script", "TEXT");
		out.add_new("Date", "TEXT");
		out.add_new("PosX", "REAL");
		out.add_new("PosY", "REAL");
		out.add_new("PosZ", "REAL");
		out.tableName = "Case";
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

	static AM_Database_TableStruct TN_EquilibriumConfiguration()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("Temperature", "REAL");
		out.add_new("StartTemperature", "REAL");
		out.add_new("EndTemperature", "REAL");
		out.add_new("TemperatureType", "TEXT");
		out.add_new("StepSize", "REAL");
		out.add_new("Pressure", "REAL");
		out.tableName = "EquilibriumConfiguration";
		return out;
	}

	static AM_Database_TableStruct TN_EquilibriumPhaseFraction()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("IDPhase", "INTEGER");
		out.add_new("Temperature", "REAL");
		out.add_new("Value", "REAL");
		out.tableName = "EquilibriumPhaseFraction";
		return out;
	}

	static AM_Database_TableStruct TN_HeatTreatment()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("Name", "TEXT");
		out.add_new("MaxTemperatureStep", "INTEGER");
		out.add_new("IDPrecipitationDomain", "INTEGER");
		out.add_new("StartTemperature", "REAL");
		out.tableName = "HeatTreatment";
		return out;
	}

	static AM_Database_TableStruct TN_HeatTreatmentProfile()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDHeatTreatment", "INTEGER");
		out.add_new("Time", "REAL");
		out.add_new("Temperature", "REAL");
		out.tableName = "HeatTreatmentProfile";
		return out;
	}

	static AM_Database_TableStruct TN_HeatTreatmentSegment()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("stepIndex", "INTEGER");
		out.add_new("IDHeatTreatment", "INTEGER");
		out.add_new("IDPrecipitationDomain", "INTEGER");
		out.add_new("EndTemperature", "REAL");
		out.add_new("TemperatureGradient", "REAL");
		out.add_new("Duration", "REAL");
		out.tableName = "HeatTreatmentSegment";
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

	static AM_Database_TableStruct TN_PrecipitateSimulationData()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDPrecipitationPhase", "INTEGER");
		out.add_new("IDHeatTreatment", "INTEGER");
		out.add_new("Time", "REAL");
		out.add_new("PhaseFraction", "REAL");
		out.add_new("NumberDensity", "REAL");
		out.add_new("MeanRadius", "REAL");
		out.tableName = "PrecipitateSimulationData";
		return out;
	}

	static AM_Database_TableStruct TN_PrecipitationDomain()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("Name", "TEXT");
		out.add_new("IDPhase ", "INTEGER");
		out.add_new("InitialGrainDiameter", "REAL");
		out.add_new("EquilibriumDiDe", "REAL");
		out.tableName = "PrecipitationDomain";
		return out;
	}

	static AM_Database_TableStruct TN_PrecipitationPhase()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("IDPhase", "INTEGER");
		out.add_new("NumberSizeClasses", "INTEGER");
		out.add_new("Name", "TEXT");
		out.add_new("NucleationSites", "TEXT");
		out.add_new("IDPrecipitationDomain", "INTEGER");
		out.tableName = "PrecipitationPhase";
		return out;
	}

	static AM_Database_TableStruct TN_Project()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("Name", "TEXT");
		out.add_new("APIName", "TEXT");
		out.add_new("External_APIName", "TEXT");
		out.tableName = "Project";
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
		out.add_new("minimumLiquidFraction", "REAL");
		out.tableName = "ScheilConfiguration";
		return out;
	}

	static AM_Database_TableStruct TN_ScheilCumulativeFraction()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("IDPhase", "INTEGER");
		out.add_new("TypeComposition", "TEXT");
		out.add_new("Temperature", "REAL");
		out.add_new("Value", "REAL");
		out.tableName = "ScheilCumulativeFraction";
		return out;
	}

	static AM_Database_TableStruct TN_ScheilPhaseFraction()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("IDPhase", "INTEGER");
		out.add_new("TypeComposition", "TEXT");
		out.add_new("Temperature ", "REAL");
		out.add_new("Value", "REAL");
		out.tableName = "ScheilPhaseFraction";
		return out;
	}

	static AM_Database_TableStruct TN_SelectedElements()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDProject", "INTEGER");
		out.add_new("IDElement", "INTEGER");
		out.add_new("isReferenceElement", "INTEGER");
		out.tableName = "SelectedElements";
		return out;
	}

	static AM_Database_TableStruct TN_SelectedPhases()
	{
		AM_Database_TableStruct out;
		out.add_new("ID", "INTEGER PRIMARY KEY");
		out.add_new("IDCase", "INTEGER");
		out.add_new("IDPhase", "INTEGER");
		out.tableName = "SelectedPhases";
		return out;
	}

	static std::vector<AM_Database_TableStruct> get_structure()
	{
		std::vector<AM_Database_TableStruct> out;

		out.push_back(TN_ActivePhases());
		out.push_back(TN_ActivePhases_Configuration());
		out.push_back(TN_ActivePhases_ElementComposition());
		out.push_back(TN_CALPHADDatabase());
		out.push_back(TN_Case());
		out.push_back(TN_Element());
		out.push_back(TN_ElementComposition());
		out.push_back(TN_EquilibriumConfiguration());
		out.push_back(TN_EquilibriumPhaseFraction());
		out.push_back(TN_HeatTreatment());
		out.push_back(TN_HeatTreatmentProfile());
		out.push_back(TN_HeatTreatmentSegment());
		out.push_back(TN_Phase());
		out.push_back(TN_PrecipitateSimulationData());
		out.push_back(TN_PrecipitationDomain());
		out.push_back(TN_PrecipitationPhase());
		out.push_back(TN_Project());
		out.push_back(TN_ScheilConfiguration());
		out.push_back(TN_ScheilCumulativeFraction());
		out.push_back(TN_ScheilPhaseFraction());
		out.push_back(TN_SelectedElements());
		out.push_back(TN_SelectedPhases());
	}

}
