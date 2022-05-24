#include "../include/AM_lua_interpreter.h"

AM_lua_interpreter::AM_lua_interpreter() {
	_state = luaL_newstate();
	luaL_openlibs(_state);
}

AM_lua_interpreter::~AM_lua_interpreter()
{
	lua_close(_state);
}

std::string AM_lua_interpreter::run_command(std::string command)
{
	int c_out = lua_getglobal(_state, command.c_str());

	lua_call(_state, 0, 1);

	std::string out;
	try
	{
		out = lua_tostring(_state, -1);
	}
	catch (const std::exception&)
	{
		out = "Command not recognized!";
	}

	lua_pop(_state, 1);

	return out;
}

std::string AM_lua_interpreter::run_command(std::string command, std::vector<std::string> parameters)
{
	lua_getglobal(_state, command.c_str());

	for each (std::string pary in parameters)
	{
		lua_pushstring(_state, pary.c_str());
	}

	lua_call(_state, parameters.size(), 1);

	std::string out(lua_tostring(_state, -1));
	lua_pop(_state, 1);

	return out;
}