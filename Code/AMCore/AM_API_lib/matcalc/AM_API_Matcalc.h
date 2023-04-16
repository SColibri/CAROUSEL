#pragma once

#include <filesystem>
#include <functional>
#include <string>

#include "../../AMLib/interfaces/IAM_API.h"
#include "../../AMLib/interfaces/IAM_lua_functions.h"
#include "../../AMLib/include/AM_lua_interpreter.h"
#include "../../AMLib/include/callbackFunctions/Callbacks.h"
#include "../../AMLib/include/callbackFunctions/MessageCallBack.h"
#include "../../AMLib/include/callbackFunctions/CallbackDefinitions.h"
#include "include/API_lua_functions.h"
/** \defgroup AM_API_lib
 *  @{
 *    These libraries are the implementations for the interface IAM_API. The libraries are linked dynamically.
 *  @}
 */

 /** \addtogroup AM_API_lib
  *  @{
  */

/// <summary>
/// API interface implementation of IAM_API for MatCalc
/// </summary>
class AM_API_Matcalc : public IAM_API {

public:
	AM_API_Matcalc(AM_Config* configuration);
	~AM_API_Matcalc();

#pragma region Implementation
	virtual std::string run_script(const std::string& filename) override;
	virtual std::string run_lua_script(const std::string& filename) override;
	virtual std::string run_lua_command(const std::string& command) override;
	virtual std::string run_lua_command(const std::string& command, std::vector<std::string> parameters) override;
	virtual std::string helloApi() override;
	virtual void dispose() override;
#pragma endregion

};

extern "C" 
{
	static std::string _commandOut;

	/// <summary>
	/// Pointer to implementation of interface IAM_API for dynamic linking
	/// </summary>
	/// <returns></returns>
	__declspec(dllexport) AM_API_Matcalc* get_API_Controll(AM_Config* configuration) {
		AM_API_Matcalc* ApiControll = new AM_API_Matcalc(configuration);
		return ApiControll;
	}

	/// <summary>
	/// Pointer to implementation of interface IAM_API for dynamic linking
	/// </summary>
	/// <returns></returns>
	__declspec(dllexport) AM_API_Matcalc* get_API_controll_default() {
		AM_Config* configuration = new AM_Config;
		configuration->load();
		
		AM_API_Matcalc* ApiControll = new AM_API_Matcalc(configuration);
		return ApiControll;
	}

	/// <summary>
	/// Run lua commands with parameters
	/// </summary>
	/// <param name="API_pointer"></param>
	/// <param name="command"></param>
	/// <param name="parameters"></param>
	/// <returns></returns>
	__declspec(dllexport) char const* API_run_lua_command(AM_API_Matcalc* API_pointer, char* command, char* parameters) {
		
		std::vector<std::string> CommCheck = string_manipulators::split_text(command, " ");
		std::string CommandValue(CommCheck[0]);

		std::string ParamValue = "";
		for (size_t n1 = 1; n1 < CommCheck.size(); n1++)
		{
			if (n1 > 1)
			{
				ParamValue += " ";
			}

			ParamValue += CommCheck[n1];
		}
		ParamValue += parameters;

		// std::string ParamValue(parameters);
		std::vector<std::string> Parameters = string_manipulators::split_text(ParamValue,"||");

		_commandOut = API_pointer->run_lua_command(CommandValue, Parameters);
		const char* out = _commandOut.c_str();
		return out;
	}

	__declspec(dllexport) void RegisterAllCallbacks() 
	{
		RegisterMessageCallback(nullptr);
		RegisterErrorCallback(nullptr);
		RegisterProgressUpdateCallback(nullptr);
		RegisterScriptFinishedCallback(nullptr);
	}

	

}

/** @}*/