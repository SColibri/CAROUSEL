#pragma once
#include <vector>
#include "CALCULATIONS_abstract.h"
#include "../../../../AMLib/interfaces/IAM_DBS.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"

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
		/// Pointer to case data
		/// </summary>
		AM_pixel_parameters* _pixel_parameters;

		/// <summary>
		/// Flag, save after execution automatically
		/// </summary>
		bool _autosave{ false };

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
			_pixel_parameters(pixel_parameters)
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
			_commandList.push_back(new COMMAND_export_variables(mccComm, configuration, _filename, variableType, variableNames, ""));

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

		virtual void BeforeCalculation() override { }

		virtual void AfterCalculation() override 
		{ 
			if (_autosave)
			{
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
		std::string _filename{ "" };

	};
}