#pragma once
#include <vector>
#include "CALCULATIONS_abstract.h"

namespace matcalc 
{
	/// <summary>
	/// Get active phases using scheil solidification simulation
	/// TODO
	/// </summary>
	class CALCULATION_scheilActivePhases : public CALCULATION_abstract
	{
	public:

		CALCULATION_scheilActivePhases(IAM_Database* db, IPC_winapi* mccComm, AM_Config* configuration, DBS_ActivePhases_Configuration* scheilConfig, AM_Project* project, AM_pixel_parameters* pixel_parameters)
		{

			// get dependent phase TODO: Active phases does not have a dependent phase option, for now we use LIQUID hard coded, which is not correct
			DBS_Phase dependentPhase(db, -1);
			dependentPhase.load_by_name("LIQUID");

			std::vector<std::string> selectedPhase = pixel_parameters->get_selected_phases_ByName();
			std::vector<std::string> selectedElements = project->get_selected_elements_ByName();

			// string format
			std::string variableNames{ "t$c " }; // by default we leave temperature
			std::string variableType{ "%12.10f" };
			for (auto& phase : selectedPhase)
			{
				if (string_manipulators::trim_whiteSpace(phase).compare("LIQUID") != 0)
				{
					variableNames += "F$" + string_manipulators::trim_whiteSpace(phase) + "_S ";
				}
				else
				{
					variableNames += "F$" + string_manipulators::trim_whiteSpace(phase) + " ";
				}

				variableType += " %.6f";
			}
			variableType += "";

			// set filename
			_filename = configuration->get_directory_path(AM_FileManagement::FILEPATH::TEMP) + "\\" + std::to_string(COMMAND_export_variables::get_uniqueNumber()) + "_scheilCalculation.AMFramework";

			_commandList.push_back(new COMMAND_set_thermodynamic_database(mccComm, configuration));
			_commandList.push_back(new COMMAND_select_elements(mccComm, configuration, selectedElements));
			_commandList.push_back(new COMMAND_select_phases(mccComm, configuration, selectedPhase));
			_commandList.push_back(new COMMAND_read_thermodynamic_database(mccComm, configuration));
			_commandList.push_back(new COMMAND_set_mobility_database(mccComm, configuration));
			_commandList.push_back(new COMMAND_set_physical_database(mccComm, configuration));
			_commandList.push_back(new COMMAND_set_reference_element(mccComm, configuration, "Al")); // set reference element
			_commandList.push_back(new COMMAND_set_composition(mccComm, configuration, selectedElements, pixel_parameters->get_composition_double()));
			_commandList.push_back(new COMMAND_set_start_values(mccComm, configuration));
			_commandList.push_back(new COMMAND_calculate_equilibrium(mccComm, configuration, scheilConfig->StartTemp)); // add user defined temperature!
			// _commandList.push_back(new COMMAND_scheil_configuration(mccComm, configuration, scheilConfig, dependentPhase.Name)); TODO: Convert active phases config to scheil config
			_commandList.push_back(new COMMAND_run_step_equilibrium(mccComm, configuration));
			_commandList.push_back(new COMMAND_export_variables(mccComm, configuration, _filename, variableType, variableNames, ""));

		}
		~CALCULATION_scheilActivePhases()
		{
			// Remove text file if created
			if (std::filesystem::exists(_filename)) std::remove(_filename.c_str());

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

		/// <summary>
		/// Save phase fraction step-equilibrium into database, returns the solidification temperature
		/// </summary>
		/// <param name="db"></param>
		/// <param name="pixel_parameters"></param>
		/// <returns></returns>
		double Save_to_database(IAM_Database* db, AM_pixel_parameters* pixel_parameters)
		{
			// retrieve data from file
			std::vector<std::vector<std::string>> TBdATA = Get_data();

			// fill all models before saving into database
			std::vector<IAM_DBS*> entries;
			std::vector<int> phasesID = pixel_parameters->get_selected_phases_ByID();
			for (auto& row : TBdATA)
			{
				string_manipulators::remove_empty_entries(row);
				for (int n1 = 1; n1 < row.size(); n1++)
				{
					DBS_ScheilPhaseFraction* SPF = new DBS_ScheilPhaseFraction(db, -1);
					SPF->IDCase = pixel_parameters->get_caseID();
					SPF->Temperature = std::stod(row[0]);
					SPF->Value = std::stold(row[n1]);
					SPF->IDPhase = phasesID[n1 - 1];
					SPF->TypeComposition = "weight";

					entries.push_back(SPF);
				}
			}

			// Do nothing if no entries
			if (entries.size() == 0) return -1;

			// Save all entries
			IAM_DBS::save(entries);

			// clear memory
			for (auto& item : entries)
			{
				delete item;
			}

			entries.clear();

			return std::stod(TBdATA[TBdATA.size() - 1][0]);
		}



	private:
		std::string _filename{ "" };
	};
}