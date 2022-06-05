#pragma once

#include <vector>
#include "../include/API_lua_functions.h"
#include "../include/API_matcalc_lib.h"
#include "../include/API_scripting.h"
#include "../../../AMLib/include/Database_implementations/Database_Factory.h"
#include "../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"


API_lua_functions::API_lua_functions(lua_State* state) : IAM_lua_functions(state)
{
	//AMBaseFunctions::add_base_functions(state, this);
	add_base_functions(state);
	add_functions_to_lua(state);
}

API_lua_functions::API_lua_functions(lua_State* state, AM_Config* configuration) : IAM_lua_functions(state)
{
	_configuration = configuration;
	set_library(configuration);
	add_functions_to_lua(state);
	add_base_functions(state);
	_dbFramework = new AM_Database_Framework(configuration);
	//AMBaseFunctions::_dbFramework = _dbFramework;
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

	add_new_function(state, "get_elementNames", "std::string out", "void", bind_getElementNames_command);
	add_new_function(state, "get_phaseNames", "std::string out", "void", bind_getPhaseNames_command);
	//add_new_function(state, "database_tableList", "std::string out", "void", AMBaseFunctions::baseBind_DatabaseTableList);
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

	// Load database elements and phases each time this function is called
	bind_getElementNames_command(state); // Elements
	out += lua_tostring(state, -1); lua_pop(state, 1);
	bind_getPhaseNames_command(state); // Phases
	out += lua_tostring(state, -1); lua_pop(state, 1);
	
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_getElementNames_command(lua_State* state)
{
	if (_configuration == nullptr) return -1;
	std::string commOut = runVectorCommands(API_Scripting::script_get_thermodynamic_database(_configuration));
	size_t IndexElements = string_manipulators::find_index_of_keyword(commOut, "# of elements in database");
	size_t IndexPhases = string_manipulators::find_index_of_keyword(commOut, "# of phases in database");

	std::string out;
	if (IndexPhases == std::string::npos || IndexElements == std::string::npos) out = "Error, data was not found!";
	else 
	{
		out = commOut.substr(IndexElements, IndexPhases - IndexElements);
		std::vector<std::string> outSplit = string_manipulators::split_text(out, "\n");

		for (size_t n1 = 1; n1 < outSplit.size() - 1 ; n1++)
		{
			if (outSplit[n1].length() > 0)
			{
				DBS_Element newElement(_dbFramework->get_database(), -1);
				newElement.Name = string_manipulators::trim_whiteSpace(outSplit[n1]);
				newElement.save();
			}
		}
	}
	
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_getPhaseNames_command(lua_State* state)
{
	if (_configuration == nullptr) return -1;
	std::string commOut = runVectorCommands(API_Scripting::script_get_thermodynamic_database(_configuration));
	size_t IndexPhases = string_manipulators::find_index_of_keyword(commOut, "# of phases in database");
	size_t IndexExitCode = string_manipulators::find_index_of_keyword(commOut, "Exit code");

	std::string out;
	if (IndexPhases == std::string::npos || IndexExitCode == std::string::npos) out = "Error, data was not found!";
	else 
	{ 
		out = commOut.substr(IndexPhases, IndexExitCode - IndexPhases);
		std::vector<std::string> outSplit = string_manipulators::split_text(out, "\n");

		for(size_t n1 = 1; n1 < outSplit.size() - 1 ; n1++)
		{
			if(outSplit[n1].length() > 0)
			{
				DBS_Phase newPhase(_dbFramework->get_database(), -1);
				newPhase.Name = string_manipulators::trim_whiteSpace(outSplit[n1]);
				newPhase.save();
			}
		}
	}
	
	
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_setValueNPC_command(lua_State* state)
{
	std::string parameter = lua_tostring(state, 1);
	if (parameter.compare("-1") == 0) { parameter = "25"; } // set to default value if -1

	std::string out = _api->MCRCommand(API_Scripting::Script_set_number_of_precipitate_classes(std::stoi(parameter)));
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_selectThermodynamicDatabase_command(lua_State* state)
{
	std::string parameter = lua_tostring(state, 1);

	std::string out = _api->MCRCommand(API_Scripting::script_set_thermodynamic_database(parameter));
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_selectPhysicalDatabase_command(lua_State* state)
{
	std::string parameter = lua_tostring(state, 1);

	std::string out = _api->MCRCommand(API_Scripting::script_set_physical_database(parameter));
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_selectMobilityDatabase_command(lua_State* state)
{
	std::string parameter = lua_tostring(state, 1);

	std::string out = _api->MCRCommand(API_Scripting::script_set_mobility_database(parameter));
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
