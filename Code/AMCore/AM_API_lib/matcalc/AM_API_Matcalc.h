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


class AM_API_Matcalc : public IAM_API {

public:
	AM_API_Matcalc();
	~AM_API_Matcalc();

	virtual std::string run_script(const std::string& filename) override;
	virtual std::string run_lua_script(const std::string& filename) override { return "Matcalc not implemented"; }
	virtual std::string run_lua_command(const std::string& command) override 
	{ 
		std::string out = _luaInterpreter.run_command(command);

		return out; 
	}

	virtual std::string helloApi() override { return "From matcalc"; }
};

extern "C" 
{
	/// <summary>
	/// Pointer to implementation of interface IAM_API
	/// </summary>
	/// <returns></returns>
	__declspec(dllexport) AM_API_Matcalc* get_API_Controll() {
		AM_API_Matcalc* ApiControll = new AM_API_Matcalc();
		return ApiControll;
	}
}

/** @}*/