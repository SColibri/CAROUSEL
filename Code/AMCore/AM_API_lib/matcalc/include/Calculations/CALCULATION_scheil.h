#pragma once

// c++
#include <vector>
#include <mutex>

// core
#include "../../../../AMLib/interfaces/IAM_DBS.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"

// local
#include "CALCULATIONS_abstract.h"
#include "../TextExtractor/PhaseNamesExtractor.h"
#include "../Extension/PhasesExtension.h"

namespace matcalc
{
	/// <summary>
	/// Collection of commands that execute the Scheil precipitation simulation
	/// </summary>
	class CALCULATION_scheil : public CALCULATION_abstract
	{
	private:
		/// <summary>
		/// Pointer to database manager
		/// </summary>
		IAM_Database* _db;

		/// <summary>
		/// Pointer to configuration object
		/// </summary>
		AM_Config* _configuration;

		/// <summary>
		/// Pointer to case data
		/// </summary>
		AM_pixel_parameters* _pixel_parameters;

		/// <summary>
		/// Flag, save after execution automatically
		/// </summary>
		bool _autosave{ false };

		/// <summary>
		/// Reference to communication object
		/// </summary>
		AMFramework::Interfaces::IAM_Communication* _comm;

	public:

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="db"></param>
		/// <param name="mccComm"></param>
		/// <param name="configuration"></param>
		/// <param name="scheilConfig"></param>
		/// <param name="project"></param>
		/// <param name="pixel_parameters"></param>
		CALCULATION_scheil(IAM_Database* db, AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, DBS_ScheilConfiguration* scheilConfig, AM_Project* project, AM_pixel_parameters* pixel_parameters) :
			_db(db),
			_pixel_parameters(pixel_parameters),
			_comm(mccComm),
			_configuration(configuration)
		{
			// get dependent phase
			DBS_Phase dependentPhase(db, scheilConfig->DependentPhase);
			dependentPhase.load();
			std::vector<std::string> selectedPhase = pixel_parameters->get_selected_phases_ByName();
			std::vector<std::string> selectedElements = project->get_selected_elements_ByName();

			// string format
			std::string variableNames{ "t$c " }; // by default we leave temperature
			std::string variableType{ "%12.2f" };
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

				variableType += " %12.2g";
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
			_commandList.push_back(new COMMAND_set_reference_element(mccComm, configuration, project->get_reference_element_ByName())); // set reference element
			_commandList.push_back(new COMMAND_set_composition(mccComm, configuration, selectedElements, pixel_parameters->get_composition_double()));
			_commandList.push_back(new COMMAND_set_start_values(mccComm, configuration));
			_commandList.push_back(new COMMAND_calculate_equilibrium(mccComm, configuration, scheilConfig->StartTemperature)); // add user defined temperature!
			_commandList.push_back(new COMMAND_scheil_configuration(mccComm, configuration, scheilConfig, dependentPhase.Name));
			_commandList.push_back(new COMMAND_run_step_equilibrium(mccComm, configuration));
			//_commandList.push_back(new COMMAND_export_variables(mccComm, configuration, _filename, variableType, variableNames, ""));
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~CALCULATION_scheil()
		{
			// Remove text file if created
			if (std::filesystem::exists(_filename)) std::remove(_filename.c_str());

		}

		/// <summary>
		/// Sets flag for saving after command execution
		/// </summary>
		void set_auto_save()
		{
			_autosave = true;
		}

		/// <summary>
		/// Implementation for before calculation
		/// </summary>
		virtual void BeforeCalculation() override
		{
			// empty
		}

		/// <summary>
		/// Implementation for after calculation
		/// </summary>
		virtual void AfterCalculation() override
		{
			if (_autosave)
			{
				export_phase_fraction_data();
				Save_to_database(_db, _pixel_parameters);
			}
		}

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
		/// <summary>
		/// filename for saving exported data
		/// </summary>
		std::string _filename{ "" };

		/// <summary>
		/// Gets the formatted string command for extracting the phase fractions from matcalc.
		/// This methods looks for all calculated phases and not only the selected phases, so
		/// if more phases where added these get added to the selected phases and into the
		/// database if these phases are missing from the database
		/// </summary>
		/// <returns></returns>
		std::string export_phase_fraction_data()
		{
			// ouput
			std::string format;

			// Get active phases
			APIMatcalc::Extractors::PhaseNameExtractor pne;
			std::vector<std::string> foundPhases = pne.extract(_comm);

			// Since phases with '#' are created after the scheil/Equilibrium calculations
			// we have to check and make sure that all involved phases are correctly selected
			update_selected_phases(foundPhases);
			std::vector<std::string> selectedPhase = _pixel_parameters->get_selected_phases_ByName();

			// string format -> by default we leave temperature in celsius
			std::string variableNames{ "t$c " };
			std::string variableType{ "%12.2f" };
			for (auto& phase : selectedPhase)
			{
				if (string_manipulators::find_index_of_keyword(phase, "_S") == std::string::npos)
				{
					if (string_manipulators::trim_whiteSpace(phase).compare("LIQUID") != 0)
					{
						variableNames += "F$" + string_manipulators::trim_whiteSpace(phase) + "_S ";
					}
					else
					{
						variableNames += "F$" + string_manipulators::trim_whiteSpace(phase) + " ";
					}

					variableType += " %12.2g";
				}
			}
			variableType += "";

			// Export data into a file
			COMMAND_export_variables* exportCommand = new COMMAND_export_variables(_comm, _configuration, _filename, variableType, variableNames, "");
			exportCommand->DoAction();
			delete exportCommand;

			return format;
		}

		/// <summary>
		/// Checks if any phases where added during the calculation phase
		/// </summary>
		/// <param name="phases"></param>
		void update_selected_phases(std::vector<std::string>& phases)
		{
			// Add phases
			std::vector<DBS_Phase*> addedPhases = AMFramework::PhaseExtension::add_created_phases(_db, phases);

			for (int i = 0; i < addedPhases.size(); i++)
			{
				// Method checks if phase is already selected and if not it gets added
				_pixel_parameters->add_selectedPhase(addedPhases[i]->Name);
			}

			// Save update
			_pixel_parameters->save();
		}

	};
}