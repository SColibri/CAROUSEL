#pragma once
#include <string>
#include <fstream>
#include <ctime>
#include <vector>
#include <mutex>
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../AMLib/x_Helpers/string_manipulators.h"

namespace API_Scripting
{
#pragma region Helpers
	inline static std::mutex _uniqueNumber_mutex;
	inline static size_t _uniqueNumber = 0;
	static size_t get_uniqueNumber()
	{
		size_t out;

		_uniqueNumber_mutex.lock();
		out = _uniqueNumber;
		_uniqueNumber += 1;
		_uniqueNumber_mutex.unlock();

		return out;
	}
#pragma endregion
	
	std::vector<std::string> static Script_initialize(AM_Config* configuration) 
	{
		
		std::string Oldval = " ";
		std::string Newval = " ";

		std::string ThermoPath = configuration->get_ThermodynamicDatabase_path();

		std::vector<std::string> out
		{
			"use-module core",
			"set-working-directory " + configuration->get_working_directory() + "",
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

#pragma region single_commands
	std::string static script_initialize_core() 
	{
		return "use-module core";
	}

	std::string static script_runScript(std::string scriptFilenam)
	{
		return "run-script-file \"" + scriptFilenam + "\"";
	}

	std::string static script_set_working_directory(AM_Config* configuration)
	{
		return "set-working-directory \"" + configuration->get_working_directory() + "\"";
	}

	std::string static script_set_thermodynamic_database(std::string parameters)
	{
		return "open-thermodynamic-database " + parameters;
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

		for(std::string elementy : Elements)
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

	std::string static Script_setStepOptions(std::string stepOptions)
	{
		std::string out = "set-step-option " + stepOptions ;

		return out;
	}
	std::string static Script_stepEquilibrium()
	{
		std::string out = "step-equilibrium";

		return out;
	}

	std::string static Script_setComposition_weight(std::vector<std::string> Elements, 
													std::vector<std::string> Values)
	{
		std::string buildComp{ "enter-composition type=weight-percent composition=\"" };
		for (int n1 = 1; n1 < Elements.size(); n1++)
		{
			buildComp += Elements[n1] + "=" + Values[n1] + " ";
		}
		buildComp += "\"";
		return buildComp;
	}

	std::string static Script_setTemperature_Celcius(double temperature)
	{
		std::string out = "set-temperature-celcius " + std::to_string(temperature) + "\n";

		return out;
	}

	std::string static Script_setStartValues()
	{
		std::string out = "set-start-values";

		return out;
	}

	std::string static Script_calculateEquilibrium()
	{
		std::string out = "calculate-equilibrium";

		return out;
	}

	std::string static Script_selectPhases(std::vector<std::string> Phases)
	{
		std::string out = "select-phases ";

		for each (std::string elementy in Phases)
		{
			out += elementy + " ";
		}

		out += "";

		return out;
	}


	std::string static Script_readThermodynamicDatabase()
	{
		std::string out = "read-thermodynamic-database \n";
		return out;
	}

	std::string static Script_openThermodynamicDatabase(AM_Config* configuration)
	{
		std::string out = "open-thermodynamic-database " + configuration->get_ThermodynamicDatabase_path() + "" + "\n";
		return out;
	}

	std::string static Script_readMobilityDatabase(AM_Config* configuration)
	{
		std::string out = "read-mobility-database " + configuration->get_MobilityDatabase_path() + "" + "\n";
		return out;
	}

	std::string static Script_readPhysicalDatabase(AM_Config* configuration)
	{
		std::string out = "read-physical-database " + configuration->get_PhysicalDatabase_path() + "" + "\n";
		return out;
	}

	std::string static Script_set_number_of_precipitate_classes(int precipClasses)
	{
		std::string out = "set-variable-value npc " + std::to_string(precipClasses) + "\n";
		return out;
	}

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

	std::string static script_buffer_clear(std::string bufferName)
	{
		return "list-phase-status phase=" + bufferName;
	}
#pragma endregion
#pragma region MatcalcVariables
	std::string static script_format_variable_string(const std::string& stringVar ,
													 const std::string& format, 
													 std::vector<std::string>& variableNames) 
	{
		std::string out{ "format-variable-string variable=" + 
						 stringVar + 
					    " format-string=\"%g," + format + "\" T " +
						IAM_Database::csv_join_row(variableNames," ")};

		return out;
	}

	std::vector<std::string> static script_get_phase_equilibrium_variable_name(std::vector<std::string>& Phases)
	{
		std::vector<std::string> out;

		for (std::string phaseN : Phases)
		{
			out.push_back("F$" + phaseN);
		}

		return out;
	}

	std::vector<std::string> static script_get_phase_equilibrium_scheil_variable_name(std::vector<std::string>& Phases)
	{
		std::vector<std::string> out;

		for (std::string phaseN : Phases)
		{
			out.push_back("F$" + phaseN + "_S");
		}

		return out;
	}

	std::string static print_variable_to_console(std::string stringVar)
	{
		std::string out{"send-console-string string=#" + stringVar};
		return out;
	}
#pragma endregion

#pragma endregion

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

	std::string static Script_Header()
	{
		std::time_t today = std::time(0);
		std::string out = "\
			Script auto generated using AMFramework: " + std::string(std::ctime(&today)) + " \n \
			********************************************************************************************************************************************";

		return out;
	}

#pragma region Equilibrium_steps
	std::vector<std::string> static Script_run_stepEquilibrium(AM_Config* configuration, 
																double startTemperature,
																double endTemperature,
																std::vector<std::string>& Elements, 
																std::vector<std::string>& Compositions, 
																std::vector<std::string>& Phases)
	{
		std::vector<std::string> out;
		out.push_back(script_initialize_core());
		out.push_back(script_set_thermodynamic_database(configuration->get_ThermodynamicDatabase_path()));
		out.push_back(Script_selectElements(Elements));
		out.push_back(Script_selectPhases(Phases));
		out.push_back(Script_readThermodynamicDatabase());
		out.push_back(Script_readMobilityDatabase(configuration));
		out.push_back(Script_readPhysicalDatabase(configuration));
		out.push_back(Script_setReferenceElement(Elements[0])); // TODO let the user select the reference element, here default Index == 0
		out.push_back("set-temperature-celsius " + std::to_string((int)startTemperature));
		out.push_back(Script_setComposition_weight(Elements, Compositions)); //TODO let user define the composition type
		out.push_back(Script_setStartValues());
		out.push_back(Script_calculateEquilibrium());
		out.push_back(Script_setStepOptions("type=temperature")); // TODO add this parameter to eConfig
		out.push_back(Script_setStepOptions("temperature-in-celsius=yes")); // TODO this should be based on eConfig
		out.push_back(Script_setStepOptions("range start=" + std::to_string((int)startTemperature) +
												 " stop=" + std::to_string((int)endTemperature) + 
												 " scale=lin step-width=1"));
		out.push_back(Script_stepEquilibrium());

		return out;
	}

	std::vector<std::string> static Script_run_stepScheilEquilibrium(AM_Config* configuration,
		double startTemperature,
		double endTemperature,
		double stepWitdh,
		std::vector<std::string>& Elements,
		std::vector<std::string>& Compositions,
		std::vector<std::string>& Phases)
	{
		std::vector<std::string> out;
		out.push_back(script_initialize_core());
		out.push_back(script_set_thermodynamic_database(configuration->get_ThermodynamicDatabase_path()));
		out.push_back(Script_selectElements(Elements));
		out.push_back(Script_selectPhases(Phases));
		out.push_back(Script_readThermodynamicDatabase());
		out.push_back(Script_readMobilityDatabase(configuration));
		out.push_back(Script_readPhysicalDatabase(configuration));
		out.push_back(Script_setReferenceElement(Elements[0])); // TODO let the user select the reference element, here default Index == 0
		out.push_back("set-temperature-celsius " + std::to_string((int)startTemperature));
		out.push_back(Script_setComposition_weight(Elements, Compositions)); //TODO let user define the composition type
		out.push_back(Script_setStartValues());
		out.push_back(Script_calculateEquilibrium());
		out.push_back(Script_setStepOptions("type=scheil")); // TODO add this parameter to eConfig
		out.push_back(Script_setStepOptions("range start=" + std::to_string((int)startTemperature) +
			" stop=" + std::to_string((int)endTemperature) +
			" step-width=" + std::to_string(stepWitdh)));
		out.push_back(Script_setStepOptions("scheil-dependent-phase=LIQUID")); // TODO let choose the phase
		out.push_back(Script_setStepOptions("scheil-minimum-liquid-fraction=0.01")); // TODO add parameter for this
		out.push_back(Script_setStepOptions("temperature-in-celsius=yes")); // TODO this should be based on eConfig
		out.push_back(Script_setStepOptions("scheil-create-phases-automatically=yes"));
		
		out.push_back(Script_stepEquilibrium());

		return out;
	}
#pragma endregion


#pragma region Script_contents
	std::string static Buffer_to_variable(std::string BUFFERSIZE, std::string FORMATTEDSTRING, std::string TEMPDATA, AM_Config* configuration)
	{
		// create filenames
		std::string tempName = "TempFile" + std::to_string(std::rand() + get_uniqueNumber());
		std::string matcalfFilename = configuration->get_directory_path(AM_FileManagement::FILEPATH::SCRIPTS) + "/" + tempName + ".Framework";

		// create script
		std::string scriptTemplate = "export-open-file file-name=\"" + matcalfFilename + "\" \n";
		scriptTemplate += "for (i;1.." + BUFFERSIZE + ") \n@ load-buffer-state line-index=i \n";
		scriptTemplate += "@ " + FORMATTEDSTRING + " \n";
		scriptTemplate += "@ export-file-variables format-string=\"#" + TEMPDATA + "\" \n";
		scriptTemplate += "endfor \n";
		scriptTemplate += "export-close-file \n";

		// Save script
		std::string fileName = configuration->get_directory_path(AM_FileManagement::FILEPATH::SCRIPTS) +  "/" + tempName + ".mcs";
		std::ofstream striptFile(fileName);
		striptFile << scriptTemplate.c_str() << std::endl;
		striptFile.close();

		return fileName;
	}
#pragma endregion


}
