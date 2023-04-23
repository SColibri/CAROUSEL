#pragma once

// c++
#include <string>
#include <vector>
#include <mutex>

// Core
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_Phase.h"
#include "../../../../AMLib/include/callbackFunctions/ErrorCallback.h"
#include "../../../../AMLib/interfaces/IAM_Database.h"
#include "../../../../AMLib/x_Helpers/string_manipulators.h"
#include "../../../../AMLib/include/Extensions/PhaseExtension.h"

namespace AMFramework
{
	namespace PhaseExtension
	{
		/// <summary>
		/// Database mutex for blocking operations
		/// </summary>
		static inline std::mutex _lockDatabase;

		/// <summary>
		/// Gets all phases that are marked with #, these are "duplicated" phase names with 
		/// different constituents
		/// </summary>
		/// <param name="phases"></param>
		/// <returns></returns>
		static inline std::vector<std::string> get_created_phase_names(std::vector<std::string> phases)
		{
			// output
			std::vector<std::string> result;

			// find phases by name
			for (std::string phaseName : phases)
			{
				if (string_manipulators::find_index_of_keyword(phaseName, "#") != std::string::npos)
				{
					result.push_back(phaseName);
				}
			}

			return result;
		}

		/// <summary>
		/// returns true if phases are loaded into memory
		/// </summary>
		/// <param name="phases"></param>
		/// <returns></returns>
		static inline bool is_contained(std::vector<std::string> phases)
		{
			// Search by name
			for (std::string phaseName : phases)
			{
				IAM_DBS* phaseObject = try_find(phaseName);
				if (phaseObject == nullptr)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// This adds all created phases that are marked with # into the database and
		/// marks the phase as "not selectable" which means that the phase is a derivate from
		/// another phase but different constituents
		/// </summary>
		/// <param name="phases">List of phases</param>
		static inline std::vector<DBS_Phase*> add_created_phases(IAM_Database* database, std::vector<std::string> phases)
		{
			// Get phases of interest 
			std::vector<std::string> phaseSelection = get_created_phase_names(phases);

			// Use mutex to avoid multiple saves of same or similar list of phases
			_lockDatabase.lock();

			// Check if phases are loaded in memory, if not add them. If phases don't exist they also get added into the database
			if (!is_contained(phaseSelection))
			{
				try
				{
					// Phases to save
					std::vector<IAM_DBS*> phasesToSave;

					for (std::string phaseName : phaseSelection)
					{
						DBS_Phase* phase =  try_find(phaseName);

						if (phase == nullptr)
						{
							DBS_Phase* tempPhase = new DBS_Phase(database, -1);
							tempPhase->Name = phaseName;
							tempPhase->DBType = 1;

							phasesToSave.push_back(tempPhase);
						}
					}

					// save and load into memory
					IAM_DBS::save(phasesToSave);

					// Will only load phases that are not already loaded
					load_vector(database, phaseSelection);
				}
				catch (const std::exception& e)
				{
					// Log the error
					std::string msg = "PhaseExtension::add_created_phases Encountered an error: " + std::string(e.what());
					AMFramework::Callback::ErrorCallback::TriggerCallback(&msg[0]);
				}
			}

			_lockDatabase.unlock();

			// Get pointers
			std::vector<DBS_Phase*> loadedPhases = try_find(phaseSelection);

			return loadedPhases;
		}

		

		
	}
}