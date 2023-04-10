#pragma once

#include <string>
#include <codecvt>
#include <filesystem>
#include <sstream>
#include <iomanip>
#include <algorithm>

// Core
#include "../../../AMLib/include/callbackFunctions/ErrorCallback.h"
#include "../../../AMLib/include/callbackFunctions/LogCallback.h"
#include "../../../AMLib/interfaces/IAM_Communication.h"

// -- Windows IPC
#include "../../../AMLib/x_Helpers/IPC_winapi.h"

// Local
#include "Commands/COMMAND_ALL.h"


namespace APIMatcalc
{
	/// <summary>
	/// Static class
	/// </summary>
	class APICommunicationFactory
	{
	private:
		/// <summary>
		/// Static class
		/// </summary>
		APICommunicationFactory() {};

	public:

		/// <summary>
		/// Communication type to use
		/// </summary>
		enum COMMTYPE
		{
			none,
			IPC
		};

		/// <summary>
		/// CommType to use, for now only IPC is implemented so this variable is set
		/// as a const, but if this changes in the future we should handle this 
		/// </summary>
		static inline const COMMTYPE commType{ COMMTYPE::IPC };

		/// <summary>
		/// Returns the communication object to be used for sending commands to matcalc, this can be done
		/// using IPC or their API, which they told us that they might no longer support so that is why we went for the
		/// IPC option.
		/// </summary>
		/// <returns></returns>
		static inline AMFramework::Interfaces::IAM_Communication* get_communication_object(AM_Config* configuration) 
		{
			// Get communication type
			if(commType == COMMTYPE::IPC)
			{
				try
				{
					return get_ipc_comm(configuration);
				}
				catch (const std::exception& e)
				{
					std::string errMessage = "APICommunicationFactory - get_communication_object was not able to create a communication channel to matcalc:\n" + std::string(e.what());
					AMFramework::Callback::ErrorCallback::TriggerCallback(&errMessage[0]);
				}
			}

			return nullptr;
		}

	private:
		/// <summary>
		/// Helper method for initializing a matcalc node and setup IPC 
		/// </summary>
		static inline AMFramework::Interfaces::IAM_Communication* get_ipc_comm(AM_Config* configuration)
		{
			// Create new instance of IPC_winapi - (windows module only)
			std::string exePath = string_manipulators::rtrim_whiteSpace(configuration->get_apiExternal_path()) + "\\mcc.exe";

			std::wstring externalPath = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(exePath);
			
			AMFramework::Interfaces::IAM_Communication* comm = new IPC_winapi(externalPath);
			((IPC_winapi*)comm)->set_endflag("MC:");

			// Initialize matcalc
			COMMAND_initialize_core initComm(comm, configuration);
			initComm.DoAction();

			// Log 
			AMFramework::Callback::LogCallback::TriggerCallback("APICommunicationFactory: Matlab IPC instance created and ready");
			return comm;
		}


	};
}