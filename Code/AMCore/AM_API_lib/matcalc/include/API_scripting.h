#pragma once
#include <string>
#include <ctime>
#include <vector>
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../AMLib/x_Helpers/string_manipulators.h"

namespace API_Scripting
{
	
	std::vector<std::string> static Script_initialize(AM_Config* configuration) 
	{
		
		std::string Oldval = " ";
		std::string Newval = " ";

		std::string ThermoPath = configuration->get_ThermodynamicDatabase_path();

		std::vector<std::string> out
		{
			"use-module core",
			"set-working-directory \"" + configuration->get_working_directory() + "\"",
			"open-thermodyn-database " + configuration->get_ThermodynamicDatabase_path() + "",
			"set-variable-value npc 25"
		};


		return out;
	}

	std::vector<std::string> static script_get_thermodynamic_database(AM_Config* configuration)
	{
		std::vector<std::string> out; //= Script_initialize(configuration);
		out.push_back("open-thermodyn-database " + configuration->get_ThermodynamicDatabase_path() + "");
		out.push_back("list-database-contents equi-database-contents");
		return out;
	}

	std::string static script_set_thermodynamic_database(std::string parameters)
	{
		return "open-thermodyn-database " + parameters;
	}

	std::string static script_set_physical_database(std::string parameters)
	{
		return "read-mobility-database " + parameters;
	}

	std::string static script_set_mobility_database(std::string parameters)
	{
		return "read-physical-database " + parameters;
	}

	std::string static Script_selectElements(std::vector<std::string> Elements) 
	{
		std::string out = "select-elements ";

		for each (std::string elementy in Elements)
		{
			out += elementy + " ";
		}

		out += "\n";

		return out;
	}

	std::string static Script_setReferenceElement(std::string referenceElement)
	{
		std::string out = "set-reference-element " + referenceElement + "\n";

		return out;
	}

	std::string static Script_setComposition_weight(std::vector<std::string> Elements, 
													std::vector<std::string> Values)
	{
		std::string out{};
		std::string compDef{};

		if(Elements.size() == Values.size())
		{
			int Index{0};
			compDef = "enter-composition type=weigth-percent composition=\"";
			for each (std::string elementy in Elements)
			{
				out += "set-variable-value variable=x_" + elementy + "_0 value=" + Values[Index++] + "\n";
				compDef += elementy + "=" + "x_" + elementy + "_0 ";
			}
			compDef += "\"";
			out += compDef + "\n";
		}
		else { out = "$ ERROR setting up compositions, Elements.size <> Values.size"; }

		return out;
	}

	std::string static Script_setTemperature_Celcius(double temperature)
	{
		std::string out = "set-temperature-celcius " + std::to_string(temperature) + "\n";

		return out;
	}

	std::string static Script_setStartValues()
	{
		std::string out = "set-start-values \n ";

		return out;
	}

	std::string static Script_calculateEquilibrium()
	{
		std::string out = "set-automatic-startvalues \n \
						   calculate-equilibrium \n ";

		return out;
	}

	std::string static Script_setupScheilGulliver()
	{
		std::string out = "set-step-option type=scheil  \n \
						   set-step-option range start=700 stop=25 step-width=1 \n \
						   set-step-option scheil-dependent-phase=LIQUID \n \
						   set-step-option scheil-minimum-liquid-fraction=0.01 \n \
						   set-step-option scheil-create-phases-automatically=yes \n \
						   step-equilibrium";

		return out;
	}



	std::string static Script_selectPhases(std::vector<std::string> Phases)
	{
		std::string out = "select-phases ";

		for each (std::string elementy in Phases)
		{
			out += elementy + " ";
		}

		out += "\n";

		return out;
	}

	std::string static Script_readThermodynamicDatabase()
	{
		std::string out = "read-thermodyn-database \n";
		return out;
	}
	
	std::string static Script_readThermodynamicDatabase(AM_Config* configuration)
	{
		std::string out = "read-thermodyn-database \'" + configuration->get_ThermodynamicDatabase_path() + "\'"  + "\n";
		return out;
	}

	std::string static Script_readMobilityDatabase(AM_Config* configuration)
	{
		std::string out = "read-thermodyn-database \'" + configuration->get_MobilityDatabase_path() + "\'" + "\n";
		return out;
	}

	std::string static Script_readPhysicalDatabase(AM_Config* configuration)
	{
		std::string out = "read-thermodyn-database \'" + configuration->get_PhysicalDatabase_path() + "\'" + "\n";
		return out;
	}

	std::string static Script_set_number_of_precipitate_classes(int precipClasses)
	{
		std::string out = "set-variable-value npc " + std::to_string(precipClasses) + "\n";
		return out;
	}

	std::string static Script_Header()
	{
		std::time_t today = std::time(0);
		std::string out = "\
			Script auto generated using AMFramework: " + std::string(std::ctime(&today)) + " \n \
			********************************************************************************************************************************************";

		return out;
	}

#pragma region Equilibrium
	std::vector<std::string> static Script_run_stepEquilibrium(AM_Config* configuration, 
																double startTemperature,
																double endTemperature,
																std::vector<std::string>& Elements, 
																std::vector<std::string>& Compositions, 
																std::vector<std::string>& Phases)
	{
		std::vector<std::string> out = Script_initialize(configuration);
		out.push_back("select-elements " + IAM_Database::csv_join_row(Elements, " "));
		out.push_back("select-phases " + IAM_Database::csv_join_row(Phases, " "));
		out.push_back("read-thermodyn-database \'" + configuration->get_ThermodynamicDatabase_path() + "\'");
		out.push_back("read-mobility-database \'" + configuration->get_MobilityDatabase_path() + "\'");
		out.push_back("read-physical-database \'" + configuration->get_PhysicalDatabase_path() + "\'");
		out.push_back("set-reference-element " + Elements[0]); // TODO let the user select the reference element, here default Index == 0
		out.push_back("set_step-option type=temperature"); // TODO add this parameter to eConfig
		out.push_back("set-step-option temperature-in-celsius=yes"); // TODO this should be based on eConfig
		out.push_back("set-step-option range start=" + std::to_string(startTemperature) + 
					  " stop=" + std::to_string(endTemperature) + " scale=lin step-width=-25");

		// Build string for composition, we omit the first element because this si the reference element
		std::string buildComp{ "" };
		for(int n1 = 1; n1 < Elements.size(); n1++)
		{
			buildComp += Elements[n1] + "=" + Compositions[n1] + " ";
		}
		out.push_back("enter-composition type=weight-percent composition=\"" + buildComp + "\""); //TODO let user define the composition type
		out.push_back("step-equilibrium");

		return out;
	}
#pragma endregion
#pragma region MatcalcBUFFER
	std::string static script_buffer_listContent() 
	{
		return "list-buffer-contents";
	}

	std::string static script_buffer_loadState(int stateNumber)
	{
		return "load-buffer-state line-index=" + std::to_string(stateNumber);
	}

	std::string static script_buffer_getPhaseStatus(std::string phaseName)
	{
		string_manipulators::toCaps(phaseName);
		return "list-phase-status phase=" + phaseName;
	}
#pragma endregion
}
