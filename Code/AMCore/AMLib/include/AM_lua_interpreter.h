#pragma once

#include <string>
#include <vector>

extern "C" {
	#include "../external/lua542/include/lua.h"
	#include "../external/lua542/include/lua.hpp"
	#include "../external/lua542/include/lualib.h"
	#include "../external/lua542/include/luaconf.h"
}


class AM_lua_interpreter
{
public:

	AM_lua_interpreter();

	~AM_lua_interpreter();

	void reg_function(std::string functionName, lua_CFunction functionHandle)
	{
		lua_register(_state, functionName.c_str(), functionHandle);
	}

	void run_file(std::string filename)
	{
		luaL_dofile(_state, filename.c_str());
	}

	std::string run_command(std::string command);
	std::string run_command(std::string command, std::vector<std::string> parameters);

	lua_State* get_state()
	{
		return _state;
	}

private:
	lua_State* _state;

};