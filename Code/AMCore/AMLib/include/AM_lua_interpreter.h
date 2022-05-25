#pragma once

#include <string>
#include <vector>

extern "C" {
	#include "../external/lua542/include/lua.h"
	#include "../external/lua542/include/lua.hpp"
	#include "../external/lua542/include/lualib.h"
	#include "../external/lua542/include/luaconf.h"
}

/// <summary>
/// lua interpreter class holds a lua class state and handles
/// all interactions with the lua script and commands.
/// 
/// Commands are added by implementing the IAM_lua_functions.
/// </summary>
class AM_lua_interpreter
{
public:

	AM_lua_interpreter();
	~AM_lua_interpreter();

	/// <summary>
	/// open and run lua script
	/// </summary>
	/// <param name="filename"></param>
	void run_file(std::string filename)
	{
		luaL_dofile(_state, filename.c_str());
	}

	/// <summary>
	/// run line command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	std::string run_command(std::string command);

	/// <summary>
	/// run line command with parameters
	/// </summary>
	/// <param name="command"></param>
	/// <param name="parameters"></param>
	/// <returns></returns>
	std::string run_command(std::string command, std::vector<std::string> parameters);

	/// <summary>
	/// get lua state.
	/// </summary>
	/// <returns></returns>
	lua_State* get_state()
	{
		return _state;
	}

private:
	lua_State* _state; // lua state scripting

};