#pragma once

#include "../../../AMLib/interfaces/IAM_lua_functions.h"
#include "../../../AMLib/include/AM_Config.h"
#include "API_matcalc_lib.h"

class API_lua_functions: public IAM_lua_functions
{
public:

	API_lua_functions(lua_State* state);
	API_lua_functions(lua_State* state, AM_Config* configuration);
	~API_lua_functions();
	static void set_library(AM_Config* configuration);

private:
	inline static API_matcalc_lib* _api{nullptr};
	inline static std::string _dllPath{};
	inline static AM_Config* _configuration{nullptr};

	/// <summary>
	/// Registers the function's signature in lua 
	/// </summary>
	/// <param name="state"></param>
	virtual void add_functions_to_lua(lua_State* state) override;

#pragma region lua_functions
	/// <summary>
	/// This is just an example of how you should bind a function in lua
	/// so I'll call this baby lua.
	/// </summary>
	/// <param name="state"></param>
	/// <returns>number of parameters it outputs</returns>
	static int bind_hello_world(lua_State* state);

	/// <summary>
	/// This calls the run matcalc script, the parameter passed to lua has to be 
	/// the filename for the script
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_run_script(lua_State* state);

	/// <summary>
	/// runs specific commands using matcalc scripting commands
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_run_command(lua_State* state);

	/// <summary>
	/// Initializes matcalc core module, loads databases and all
	/// for initialization.
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_initializeCore_command(lua_State* state);
#pragma endregion

#pragma region helpers
	/// <summary>
	/// runs commands contained by a vector, one by one.
	/// </summary>
	/// <param name="parameter"></param>
	/// <returns></returns>
	static std::string runVectorCommands(std::vector<std::string> parameter);
#pragma endregion

};
