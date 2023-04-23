#pragma once

#include <vector>
#include <string>
#include <iostream>

#include "../../../../AMLib/include/AM_Config.h"
#include "../../../../AMLib/include/AM_pixel_parameters.h"
#include "../../../../AMLib/include/callbackFunctions/LogCallback.h"
#include "../../../../AMLib/include/callbackFunctions/ProgressUpdateCallback.h"
#include "../../../../AMLib/include/callbackFunctions/ErrorCallback.h"
#include "../../../../AMLib/interfaces/IAM_Database.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"
#include "../../../../AMLib/interfaces/IAM_ThreadJob.h"
#include "../../../../AMLib/include/AM_Threading.h"

#include "../Calculations/CALCULATION_scheil.h"
#include "../CalculationsThreadJob.h"


namespace APIMatcalc 
{
	namespace ScriptingCommands 
	{
		class ScriptingDoSolidification 
		{
		private:
			/// <summary>
			/// Static class
			/// </summary>
			ScriptingDoSolidification() {}

		public:
			
			/// <summary>
			/// Runs the scheil precipitation simulation in parallel. Thread number is defined in configuration object.
			/// </summary>
			/// <param name="database"></param>
			/// <param name="configuration"></param>
			/// <param name="IDProject"></param>
			/// <param name="IDCaseStart"></param>
			/// <param name="IDCaseEnd"></param>
			/// <returns></returns>
			static inline std::string run_scheil_simulations(IAM_Database* database,
				AM_Config* configuration, 
				int IDProject,
				int IDCaseStart,
				int IDCaseEnd)
			{
				// Initialize AM_project object to get corresponding pixel cases
				AM_Project Project(database, configuration, IDProject);
				std::vector<AM_pixel_parameters*> pixel_parameters = get_pixel_parameters(IDCaseEnd, IDCaseStart, Project);

				// Create communication to mcc for each thread
				std::vector<int> threadWorkload = AMFramework::Threading::thread_workload_distribution(configuration->get_max_thread_number(), pixel_parameters.size());

				// Create thread workers
				std::vector<AMFramework::Interfaces::IAM_ThreadJob*> taskList = get_scheil_taskList(threadWorkload, database, configuration, pixel_parameters, Project);

				// Execute tasks in parallel
				AMFramework::Threading::run_thread_jobs_in_parallel(taskList);

				// Save Logs
				AM_FileManagement logFile(configuration->get_working_directory());

				std::stringstream ss;
				for(auto& th : taskList)
				{
					ss << th->get_output();
				}

				std::stringstream filename;
				filename << IDProject << "_" << IDCaseStart << "-" << IDCaseEnd;
				logFile.save_file(AM_FileManagement::FILEPATH::LOGS, filename.str(), ss.str());


				// Clear memory
				clear_thread_jobs_vector(taskList);

				return "run_scheil_simulations finished running";
			}

			/// <summary>
			/// Runs the equilibrium precipitation simulation in parallel. Thread number is defined in configuration object.
			/// </summary>
			/// <param name="database"></param>
			/// <param name="configuration"></param>
			/// <param name="IDProject"></param>
			/// <param name="IDCaseStart"></param>
			/// <param name="IDCaseEnd"></param>
			/// <returns></returns>
			static inline std::string run_equilibrium_simulations(IAM_Database* database,
				AM_Config* configuration,
				int IDProject,
				int IDCaseStart,
				int IDCaseEnd)
			{
				// Initialize AM_project object to get corresponding pixel cases
				AM_Project Project(database, configuration, IDProject);
				std::vector<AM_pixel_parameters*> pixel_parameters = get_pixel_parameters(IDCaseEnd, IDCaseStart, Project);

				// Create communication to mcc for each thread
				std::vector<int> threadWorkload = AMFramework::Threading::thread_workload_distribution(configuration->get_max_thread_number(), pixel_parameters.size());

				// Create thread workers
				std::vector<AMFramework::Interfaces::IAM_ThreadJob*> taskList = get_equilibrium_taskList(threadWorkload, database, configuration, pixel_parameters, Project);

				// Execute tasks in parallel
				AMFramework::Threading::run_thread_jobs_in_parallel(taskList);

				// Clear memory
				clear_thread_jobs_vector(taskList);

				return "run_scheil_simulations finished running";
			}

		private:

#pragma region Helper Methods

			/// <summary>
			/// Gets all pixel cases defined from/to IDCase in a project object
			/// </summary>
			/// <param name="IDCaseEnd"></param>
			/// <param name="IDCaseStart"></param>
			/// <param name="Project"></param>
			/// <returns></returns>
			static inline std::vector<AM_pixel_parameters*> get_pixel_parameters(int IDCaseEnd, int IDCaseStart, AM_Project& Project)
			{
				std::vector<AM_pixel_parameters*> pixel_parameters;

				// Fetch all pixel cases
				int range = IDCaseEnd - IDCaseStart;
				for (int n1 = 0; n1 < range + 1; n1++)
				{
					// Gets pixel case
					AM_pixel_parameters* pixelObject = Project.get_pixelCase(IDCaseStart + n1);

					// If no ID was found report as callback
					if (pixelObject != nullptr)
					{
						pixel_parameters.push_back(pixelObject);
					}
					else
					{
						std::string ErrorOut = "Error: Selected ID case is not part of this project!, IDCase: " + std::to_string(IDCaseStart + n1);
						AMFramework::Callback::ErrorCallback::TriggerCallback(&ErrorOut[0]);
					}
				}

				return pixel_parameters;
			}

			/// <summary>
			/// Gets the tasklist for scheil calculations
			/// </summary>
			/// <returns></returns>
			static inline std::vector<AMFramework::Interfaces::IAM_ThreadJob*> get_scheil_taskList(std::vector<int> threadWorkload, 
				IAM_Database* database, AM_Config* configuration, std::vector<AM_pixel_parameters*> pixel_parameters, AM_Project& Project)
			{
				std::vector<AMFramework::Interfaces::IAM_ThreadJob*> taskList;

				int pixelIndex = 0;
				for (int n1 = 0; n1 < threadWorkload.size(); n1++)
				{
					// Create new worker
					APIMatcalc::Threading::CalculationsThreadJob* taskObject = new APIMatcalc::Threading::CalculationsThreadJob(database, configuration);

					// Add Calculations to worker
					for (int n2 = 0; n2 < threadWorkload[n1]; n2++)
					{
						// create new scheil calculation set
						matcalc::CALCULATION_scheil* calcScheil = new matcalc::CALCULATION_scheil(database,
							taskObject->get_comm(),
							configuration,
							pixel_parameters[pixelIndex]->get_ScheilConfiguration(),
							&Project,
							pixel_parameters[pixelIndex]);

						// Save after execution
						calcScheil->set_auto_save();

						// Add calculation to task list
						taskObject->add_calculation(calcScheil);

						pixelIndex++;
					}

					// Add worker to vector
					taskList.push_back(taskObject);
				}

				return taskList;
			}

			/// <summary>
			/// Gets the tasklist for equilibrium calculations
			/// </summary>
			/// <returns></returns>
			static inline std::vector<AMFramework::Interfaces::IAM_ThreadJob*> get_equilibrium_taskList(std::vector<int> threadWorkload,
				IAM_Database* database, AM_Config* configuration, std::vector<AM_pixel_parameters*> pixel_parameters, AM_Project& Project)
			{
				std::vector<AMFramework::Interfaces::IAM_ThreadJob*> taskList;

				int pixelIndex = 0;
				for (int n1 = 0; n1 < threadWorkload.size(); n1++)
				{
					// Create new worker
					APIMatcalc::Threading::CalculationsThreadJob* taskObject = new APIMatcalc::Threading::CalculationsThreadJob(database, configuration);

					// Add Calculations to worker
					for (int n2 = 0; n2 < threadWorkload[n1]; n2++)
					{
						// create new scheil calculation set
						APIMatcalc::Calculations::CALCULATIONS_Equilibrium* calcScheil = new APIMatcalc::Calculations::CALCULATIONS_Equilibrium(database,
							taskObject->get_comm(),
							configuration,
							&Project,
							pixel_parameters[pixelIndex]);

						// Save after execution
						calcScheil->set_auto_save();

						// Add calculation to task list
						taskObject->add_calculation(calcScheil);

						pixelIndex++;
					}

					// Add worker to vector
					taskList.push_back(taskObject);
				}

				return taskList;
			}

			/// <summary>
			/// Removes from memory all thread job objects
			/// </summary>
			/// <param name="taskList"></param>
			inline static void clear_thread_jobs_vector(std::vector<AMFramework::Interfaces::IAM_ThreadJob*> taskList) 
			{
				for (auto pixel : taskList)
				{
					pixel->Dispose();
					delete pixel;
				}
				taskList.clear();
			}

#pragma endregion

		};
	}
}