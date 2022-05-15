#pragma once
#include <string>
#include <ctime>
#include <vector>
#include "../../../AMLib/include/AM_Config.h"

namespace API_Scripting
{
	
	std::vector<std::string> static Script_initialize(AM_Config* configuration) 
	{
		std::string out2 = "use-module core \n \
						  set-working-directory " + configuration->get_working_directory() + " \n \
						  open-thermodyn-database " + configuration->get_ThermodynamicDatabase_path() + " \n ";

		std::vector<std::string> out
		{
			"use-module core",
			"set-working-directory " + configuration->get_working_directory(),
			"open-thermodyn-database " + configuration->get_ThermodynamicDatabase_path()
		};


		return out;
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
	}
	
	std::string static Script_readThermodynamicDatabase(AM_Config* configuration)
	{
		std::string out = "read-thermodyn-database " + configuration->get_ThermodynamicDatabase_path() + "\n";
	}

	std::string static Script_readMobilityDatabase(AM_Config* configuration)
	{
		std::string out = "read-thermodyn-database " + configuration->get_MobilityDatabase_path() + "\n";
	}

	std::string static Script_readPhysicalDatabase(AM_Config* configuration)
	{
		std::string out = "read-thermodyn-database " + configuration->get_PhysicalDatabase_path() + "\n";
	}

	std::string static Script_Header()
	{
		std::time_t today = std::time(0);
		std::string out = "\
			Script auto generated using AMFramework: " + std::string(std::ctime(&today)) + " \n \
			********************************************************************************************************************************************";

		return out;
	}

}
