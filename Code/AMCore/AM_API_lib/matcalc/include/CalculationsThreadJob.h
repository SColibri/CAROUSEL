#pragma once

// c++
#include <codecvt>
#include <vector>

// Core
#include "../../../AMLib/include/callbackFunctions/LogCallback.h"
#include "../../../AMLib/include/callbackFunctions/ErrorCallback.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/interfaces/IAM_Calculation.h"
#include "../../../AMLib/interfaces/IAM_Communication.h"
#include "../../../AMLib/interfaces/IAM_ThreadJob.h"
#include "../../../AMLib/interfaces/IAM_Database.h"

// Core - windows
#include "../../../AMLib/x_Helpers/IPC_winapi.h"

// local
#include "Calculations/CALCULATION_ALL.h"
#include "API_CommunicationFactory.h"

namespace APIMatcalc 
{
	namespace Threading 
	{
		/// <summary>
		/// CalculationsThreadJob contains all tasks to be executed per thread
		/// </summary>
		class CalculationsThreadJob : public AMFramework::Interfaces::IAM_ThreadJob
		{
		private:
			/// <summary>
			/// IPC to matcalc application
			/// </summary>
			AMFramework::Interfaces::IAM_Communication* _comm{ nullptr };

			/// <summary>
			/// System configuration
			/// </summary>
			AM_Config* _configuration{ nullptr };

			/// <summary>
			/// List of calculations to be executed on current thread
			/// </summary>
			std::vector<IAM_Calculation*> _calculations;

			/// <summary>
			/// Returns true if comm was initialized correctly
			/// </summary>
			bool _commStatus{ false };

		public:
			/// <summary>
			/// Constructor
			/// </summary>
			CalculationsThreadJob(IAM_Database* db, AM_Config* configuration) : 
				_configuration(configuration)
			{
				// Get communication object
				_comm = APIMatcalc::APICommunicationFactory::get_communication_object(configuration);
				
				// Check if comm was created
				if (_comm != nullptr)
				{
					_commStatus = true;
				}
			}

			/// <summary>
			/// Destructor
			/// </summary>
			~CalculationsThreadJob()
			{
				// dispose comm
				_comm->Dispose();
				delete _comm;
			}

			/// <summary>
			/// Execute commands
			/// </summary>
			/// <returns></returns>
			virtual int execute() override
			{
				// Check if comm is initialized and ready
				if (!_commStatus)
				{
					return 1;
				}

				try
				{
					// run calculations
					for (auto calculation : _calculations)
					{
						calculation->Calculate();
					}
				}
				catch (const std::exception& e)
				{
					// Catch error
					std::string errMessage = "CalculationsThreadJob - Thread execution failed:\n" + std::string(e.what());
					AMFramework::Callback::ErrorCallback::TriggerCallback(&errMessage[0]);
					return 1;
				}

				return 0;
			}

#pragma region Getters
			/// <summary>
			/// Returns true if comm was initialized correctly
			/// </summary>
			const bool& get_status() 
			{
				return _commStatus;
			}

			/// <summary>
			/// Returns communication object
			/// </summary>
			/// <returns></returns>
			AMFramework::Interfaces::IAM_Communication* get_comm()
			{
				return _comm;
			}
#pragma endregion

#pragma region Methods
			/// <summary>
			/// Adds a calculation to the task list
			/// </summary>
			/// <param name="calculation"></param>
			void add_calculation(IAM_Calculation* calculation)
			{
				_calculations.push_back(calculation);
			}

			/// <summary>
			/// Removes from memory all calculation objects
			/// </summary>
			void delete_calculations() 
			{
				// remove objects
				for (auto calculation : _calculations)
				{
					delete calculation;
				}

				// Clear pointer list
				_calculations.clear();
			}
#pragma endregion



		};
	}
}