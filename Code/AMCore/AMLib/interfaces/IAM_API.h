#pragma once
#include <string>
#include "../../AMLib/include/AM_Config.h"
#include "../../AMLib/include/AM_lua_interpreter.h"
#include "IAM_lua_functions.h"

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Interface for creating new API implementations
/// </summary>
class IAM_API
{
public:

	// con/de-structor
	IAM_API(){};
	virtual ~IAM_API()
	{
		if (_luaFunctions != nullptr) delete _luaFunctions;
	};

	/// <summary>
	/// Runs script
	/// </summary>
	/// <param name="filename"></param>
	virtual std::string run_script(const std::string& filename) { return "Not implemented"; }
	virtual std::string run_lua_script(const std::string& filename) { return "Not implemented"; }
	virtual std::string run_lua_command(const std::string& command) { return "Not implemented"; }
	virtual std::string run_lua_command(const std::string& command, std::vector<std::string> parameters) { return "Not implemented"; }
	virtual void load_config(std::string filename){}
	virtual std::string helloApi() { return "From interface"; }

	/// <summary>
	/// obtains list of functions defined for the implementation
	/// </summary>
	/// <returns></returns>
	std::vector<std::vector<std::string>> get_declared_functions() {
		if (_luaFunctions == nullptr) throw "lua pointer to functions not defined";
		return _luaFunctions->get_list_functions();
	}
protected:
	AM_Config* _configuration{nullptr};
	AM_lua_interpreter _luaInterpreter;
	IAM_lua_functions* _luaFunctions{nullptr};
	

};
/** @}*/
