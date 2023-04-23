#pragma once

// c++
#include <string>
#include <vector>
#include <unordered_map>

// Core
#include "../../interfaces/IAM_DBS.h"
#include "../../include/Database_implementations/Data_stuctures/DBS_Phase.h"
#include "../../include/AM_Database_Datatable.h"
#include "../../include/callbackFunctions/ErrorCallback.h"
#include "../../include/callbackFunctions/LogCallback.h"
#include "../../x_Helpers/string_manipulators.h"


namespace AMFramework
{
	namespace PhaseExtension
	{
		/// <summary>
		/// List of phases loaded in memory, with this we avoid multiple IO operations
		/// </summary>
		// static inline std::vector<DBS_Phase*> phasesInMemory;

		static inline std::unordered_map<std::string, DBS_Phase*> phasesInMemory;

		/// <summary>
		/// Removes all phases from memory
		/// </summary>
		static inline void clear_phases_in_memory()
		{
			for (auto phase : phasesInMemory)
			{
				delete phase.second;
			}
			phasesInMemory.clear();
		}

		/// <summary>
		/// Find phase in memory
		/// </summary>
		/// <param name="phaseName"></param>
		/// <returns></returns>
		static inline DBS_Phase* try_find(std::string& phaseName)
		{
			DBS_Phase* result = nullptr;

			// Find in memory
			for (auto phase : phasesInMemory)
			{
				if (phase.second->Name.compare(phaseName) == 0)
				{
					result = phase.second;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Returns found phases from given list
		/// </summary>
		/// <param name="phases"></param>
		/// <returns></returns>
		static inline std::vector<DBS_Phase*> try_find(std::vector<std::string> phases) 
		{
			std::vector<DBS_Phase*> result;

			for (auto& phaseName : phases)
			{
				auto phaseIterator = phasesInMemory.find(phaseName);
				if (phaseIterator != phasesInMemory.end())
				{
					result.push_back(phasesInMemory[phaseName]);
				}
			}

			return result;
		}

		/// <summary>
		/// Loads phase object into memory, Does not throw exception if not found
		/// </summary>
		/// <param name="database"></param>
		/// <param name="name"></param>
		static inline DBS_Phase* add_by_name(IAM_Database* database, std::string name)
		{
			// Load phase
			DBS_Phase* phase = new DBS_Phase(database, -1);
			phase->load_by_name(name);

			if (phase->id() > -1)
			{
				// add found object
				phasesInMemory.insert(std::make_pair(phase->Name, phase));
				return phase;
			}
			else
			{
				// no object was found
				delete phase;
				return nullptr;
			}
		}

		/// <summary>
		/// Loads phase object by id into memory. Does not throw exception if not found
		/// </summary>
		/// <param name="database"></param>
		/// <param name="id"></param>
		static inline DBS_Phase* add_by_id(IAM_Database* database, int& id)
		{
			// Load phase
			DBS_Phase* phase = new DBS_Phase(database, id);
			phase->load();

			if (phase->id() > -1)
			{
				// add found object
				phasesInMemory.insert(std::make_pair(phase->Name, phase));
				return phase;
			}
			else
			{
				// no object was found
				delete phase;
				return nullptr;
			}
		}

		/// <summary>
		/// Find phase in memory
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		static inline DBS_Phase* try_find(int& id)
		{
			DBS_Phase* result = nullptr;

			// Find in memory
			for (auto phase : phasesInMemory)
			{
				if (phase.second->id() == id)
				{
					result = phase.second;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Searches for a phase object that is already loaded into memory.
		/// If the phase is found, a pointer to the phase object is returned.
		/// If the phase is not found, the function tries to add it to memory.
		/// If the phase cannot be added, nullptr is returned.
		/// </summary>
		/// <param name="phaseName"></param>
		/// <returns></returns>
		static inline DBS_Phase* get_phase_by_name(IAM_Database* database, std::string& phaseName)
		{
			// find by name in memory
			DBS_Phase* result = try_find(phaseName);

			// If not found try to load by name 
			result = result != nullptr ? result : add_by_name(database, phaseName);

			// If null, log
			if (result == nullptr)
			{
				std::string msg = "PhaseExtension::get_phase_by_name, no phase was found with the name: " + phaseName;
				AMFramework::Callback::LogCallback::TriggerCallback(&msg[0]);
			}

			return result;
		}

		/// <summary>
		/// Searches for a phase object that is already loaded into memory.
		/// If the phase is found, a pointer to the phase object is returned.
		/// If the phase is not found, the function tries to add it to memory.
		/// If the phase cannot be added, nullptr is returned.
		/// </summary>
		/// <param name="database"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		static inline DBS_Phase* get_phase_by_name(IAM_Database* database, int& id)
		{
			// find by name in memory
			DBS_Phase* result = try_find(id);

			// If not found try to load by name 
			result = result != nullptr ? result : add_by_id(database, id);

			// If null, log
			if (result == nullptr)
			{
				std::string msg = "PhaseExtension::get_phase_by_name, no phase was found with the id: " + std::to_string(id);
				AMFramework::Callback::LogCallback::TriggerCallback(&msg[0]);
			}

			return result;
		}

		/// <summary>
		/// Loads all phases into memory
		/// </summary>
		static inline void load_all_phases(IAM_Database* database)
		{
			try
			{
				// Phase table structure
				AM_Database_Datatable phaseTable(database, &AMLIB::TN_Phase());
				phaseTable.load_data();

				// load data into phase object
				if (phaseTable.row_count() > 0)
				{
					for (int n1 = 0; n1 < phaseTable.row_count(); n1++)
					{
						DBS_Phase* phase = new DBS_Phase(database, std::stoi(phaseTable(0, n1)));
						phasesInMemory.insert(std::make_pair(phase->Name, phase));
					}

					// Success
					std::string msg = "PhaseExtension::load_all_phases, total of phases loaded into memory: " + std::to_string(phasesInMemory.size());
					AMFramework::Callback::LogCallback::TriggerCallback(&msg[0]);
				}
				else
				{
					// No phases found in database
					std::string msg = "PhaseExtension::load_all_phases, there are no phases available in the database  ";
					AMFramework::Callback::ErrorCallback::TriggerCallback(&msg[0]);
				}
			}
			catch (const std::exception& ex)
			{
				// Exception
				std::string msg = "PhaseExtension::load_all_phases, an error occurred while loading all phases: " + std::string(ex.what());
				AMFramework::Callback::ErrorCallback::TriggerCallback(&msg[0]);
			}
		}

		/// <summary>
		/// Loads phases by name, does not throw exception when a phase was not found
		/// </summary>
		/// <param name="phases"></param>
		static inline void load_vector(IAM_Database* database, std::vector<std::string> phases)
		{
			for (std::string phaseName : phases) 
			{
				get_phase_by_name(database, phaseName);
			}
		}

		

		

		
	}
}