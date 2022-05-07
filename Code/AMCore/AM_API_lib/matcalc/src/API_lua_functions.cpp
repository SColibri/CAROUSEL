#pragma once

#include "../include/API_lua_functions.h"

API_lua_functions::API_lua_functions(lua_State* state)
{
	add_functions_to_lua(state);
}


void API_lua_functions::add_functions_to_lua(lua_State* state) 
{
	lua_register(state, "hello_world", bind_hello_world);
	add_to_definition("hello_world","","baby lua script :)");


}

#pragma region lua_functions
/// <summary>
/// This is just an example of how you should bind a function in lua
/// so I'll call this baby lua.
/// </summary>
/// <param name="state"></param>
/// <returns>number of parameters it outputs</returns>
int API_lua_functions::bind_hello_world(lua_State* state)
{
	int noParameters = lua_gettop(state);
	std::string hw = "hello world";
	lua_pushstring(state, hw.c_str());
	return 1;
}
#pragma endregion
