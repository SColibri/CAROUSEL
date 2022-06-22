#pragma once
#include <filesystem>

#include "../../AMLib/interfaces/IAM_API.h"
#include "../../AMLib/interfaces/IAM_lua_functions.h"
#include "../../AMLib/include/AM_lua_interpreter.h"
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
	/// <summary>
	/// Pointer to implementation of interface IAM_API for dynamic linking
	/// </summary>
	/// <returns></returns>
	__declspec(dllexport) AM_API_Matcalc* get_API_Controll(AM_Config* configuration) {
		AM_API_Matcalc* ApiControll = new AM_API_Matcalc(configuration);
		return ApiControll;
	}
}

/** @}*/