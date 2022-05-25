#pragma once

#include <vector>
#include "../include/API_lua_functions.h"
#include "../include/API_matcalc_lib.h"
#include "../include/API_scripting.h"
#include "../../../AMLib/include/Database_implementations/Database_Factory.h"

API_lua_functions::API_lua_functions(lua_State* state) : IAM_lua_functions(state)
{
	add_functions_to_lua(state);
}

API_lua_functions::API_lua_functions(lua_State* state, AM_Config* configuration) : IAM_lua_functions(state)
{
	_configuration = configuration;
	set_library(configuration);
	add_functions_to_lua(state);
	_dbFramework = new AM_Database_Framework(configuration);
}

API_lua_functions::~API_lua_functions()
{
	delete _api;
	delete _dbFramework;
}

void API_lua_functions::set_library(AM_Config* configuration) {

	// If _api is not defined or path has changed then initialize with new
	// parameters.
	if(_api == nullptr || _dllPath._Equal(configuration->get_api_path()) == false)
	{
		_api = new API_matcalc_lib(configuration);
	}
	
}


void API_lua_functions::add_functions_to_lua(lua_State* state) 
{
	add_new_function(state, "hello_world", "void", "baby lua script :)", bind_hello_world);
	add_new_function(state, "run_script", "std::string filename", "filename", bind_run_script);
	add_new_function(state, "run_command", "std::string filename", "command", bind_run_command);
	add_new_function(state, "initialize_core", "std::string out", "void", bind_initializeCore_command);
}

#pragma region lua_functions

int API_lua_functions::bind_hello_world(lua_State* state)
{
	int noParameters = lua_gettop(state);
	std::string hw = "hello world";
	lua_pushstring(state, hw.c_str());
	return 1;
}

int API_lua_functions::bind_run_script(lua_State* state)
{
	int noParameters = lua_gettop(state);
	std::string parameter = lua_tostring(state, 1);
	noParameters = _api->MCC_script_read(parameter);

	lua_pushinteger(state, 0);
	return 1;
}

int API_lua_functions::bind_run_command(lua_State* state)
{
	std::string parameter = lua_tostring(state, 1);
	std::string out = _api->MCRCommand(parameter);

	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_initializeCore_command(lua_State* state)
{
	if (_configuration == nullptr) return -1;

	std::string out = runVectorCommands(API_Scripting::Script_initialize(_configuration));
	lua_pushstring(state, out.c_str());
	return 1;
}

#pragma endregion

#pragma region helpers
std::string API_lua_functions::runVectorCommands(std::vector<std::string> parameter)
{
	std::string out{};

	for each (std::string commLine in parameter)
	{
		out += _api->MCRCommand(commLine);
	}

	return out;
}
#pragma endregion
