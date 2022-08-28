#pragma once
#include <vector>
#include "CALCULATIONS_abstract.h"

namespace matcalc
{
	class CALCULATION_scheil : public CALCULATION_abstract
	{
		public:

			CALCULATION_scheil(IAM_Database* db,IPC_winapi* mccComm, AM_Config* configuration, DBS_ScheilConfiguration* scheilConfig, AM_Project* project, AM_pixel_parameters* pixel_parameters)
			{
				// get dependent phase
				DBS_Phase dependentPhase(db, scheilConfig->DependentPhase);
				dependentPhase.load();

				
				std::vector<std::string> selectedPhase = pixel_parameters->get_selected_phases_ByName();
				std::vector<std::string> selectedElements = project->get_selected_elements_ByName();
				
				// string format
				std::string variableNames{ "T " }; // by default we leave temperature
				std::string variableType{ "\"%g" };
				for (auto& phase : selectedPhase)
				{
					variableNames += "F$" + string_manipulators::trim_whiteSpace(phase) + "_S ";
					variableType += ",%g";
				}
				variableType += "\"";

				// set filename
				_filename = configuration->get_directory_path(AM_FileManagement::FILEPATH::TEMP) + "/" + std::to_string(COMMAND_export_variables::get_uniqueNumber()) + "_scheilCalculation.AMFramework";

				_commandList.push_back(new COMMAND_set_thermodynamic_database(mccComm, configuration));
				_commandList.push_back(new COMMAND_select_elements(mccComm, configuration, selectedElements));
				_commandList.push_back(new COMMAND_select_phases(mccComm, configuration, selectedPhase)); 
				_commandList.push_back(new COMMAND_read_thermodynamic_database(mccComm, configuration));
				_commandList.push_back(new COMMAND_set_mobility_database(mccComm, configuration));
				_commandList.push_back(new COMMAND_set_physical_database(mccComm, configuration));
				_commandList.push_back(new COMMAND_set_reference_element(mccComm, configuration, "")); 
				_commandList.push_back(new COMMAND_set_composition(mccComm, configuration, selectedElements, pixel_parameters->get_composition_double())); 
				_commandList.push_back(new COMMAND_set_start_values(mccComm, configuration));
				_commandList.push_back(new COMMAND_calculate_equilibrium(mccComm, configuration, 700)); // add user defined temperature!
				_commandList.push_back(new COMMAND_scheil_configuration(mccComm, configuration, scheilConfig, dependentPhase.Name));
				_commandList.push_back(new COMMAND_run_step_equilibrium(mccComm, configuration));
				_commandList.push_back(new COMMAND_export_variables(mccComm, configuration, _filename, variableType, variableNames, ""));

			}

			virtual void BeforeCalculation() override { }
			
			virtual void AfterCalculation() override { }

			/// <summary>
			/// returns dataset as specified
			/// </summary>
			/// <returns></returns>
			std::vector<std::vector<std::string>> Get_data()
			{
				return COMMAND_export_variables::Get_data_from_file(_filename);
			}


	
		private:
			std::string _filename{""};
			
	};
}