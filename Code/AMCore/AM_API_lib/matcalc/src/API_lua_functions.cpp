#pragma once

#include <vector>
#include "../include/API_lua_functions.h"
#include "../include/API_matcalc_lib.h"
#include "../../../AMLib/include/Database_implementations/Database_Factory.h"



API_lua_functions::API_lua_functions(lua_State* state) : IAM_lua_functions(state)
{
	//AMBaseFunctions::add_base_functions(state, this);bind_matcalc_initializeCore
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
	//Matcalc functions
	add_new_function(state, "matcalc_initialize_core", "string", "matcalc_initialize_core", bind_matcalc_initializeCore);
	add_new_function(state, "matcalc_set_working_directory", "string", "matcalc_set_working_directory", bind_matcalc_setWorkingDirectory);
	add_new_function(state, "matcalc_open_thermodynamic_database", "string", "matcalc_open_thermodynamic_database", bind_matcalc_openThermodynamicDatabase);
	add_new_function(state, "matcalc_select_elements", "string", "matcalc_selectElements", bind_matcalc_selectElements);
	add_new_function(state, "matcalc_read_thermodynamic_database", "string", "matcalc_read_thermodynamic_database", bind_matcalc_readThermodynamicDatabase);
	add_new_function(state, "matcalc_selectPhases", "string", "matcalc_selectPhases", bind_matcalc_selectPhases);
	add_new_function(state, "matcalc_setReferenceElement", "string", "matcalc_setReferenceElement", bind_matcalc_setReferenceElement);
	add_new_function(state, "matcalc_set_step_option", "string", "matcalc_setStepOption", bind_matcalc_setStepOption);
	add_new_function(state, "matcalc_step_equilibrium", "string", "matcalc_stepEquilibrium", bind_matcalc_stepEquilibrium);
	add_new_function(state, "matcalc_buffer_get_equilibrium_phase_fraction", "string", "matcalc_buffer_get_equilibrium_phase_fraction", bind_matcalc_buffer_getEquilibriumPhaseFraction);
	add_new_function(state, "matcalc_buffer_get_scheil_phase_cumulative_fraction", "string", "matcalc_buffer_get_scheil_phase_fraction", bind_matcalc_buffer_getEquilibriumPhaseFraction);
	add_new_function(state, "matcalc_buffer_listContent", "string", "matcalc_buffer_listContent", bind_matcalc_buffer_listContent);
	add_new_function(state, "matcalc_buffer_clear", "string", "matcalc_buffer_clear", bind_matcalc_buffer_clear);
	add_new_function(state, "matcalc_database_phaseNames", "string", "matcalc_database_phaseNames", bind_DatabasePhaseNames_command);
	
	//
	add_new_function(state, "hello_world", "void", "baby lua script :)", bind_hello_world);
	add_new_function(state, "run_script", "std::string filename", "filename", bind_run_script);
	add_new_function(state, "run_command", "std::string filename", "command", bind_run_command);
	add_new_function(state, "initialize_core", "std::string out", "void", bind_initializeCore_command);

	add_new_function(state, "get_elementNames", "std::string out", "void", bind_getElementNames_command);
	add_new_function(state, "get_phaseNames", "std::string out", "void", bind_getPhaseNames_command);

	//
	add_new_function(state, "pixelcase_stepEquilibrium_IDProject", "std::string out", "<int IDProject> <int IDCase>", Bind_SPC_StepEquilibrium_ByProjectID);
	add_new_function(state, "pixelcase_stepEquilibrium", "std::string out", "<int IDCase>", Bind_SPC_StepEquilibrium);
	add_new_function(state, "pixelcase_step_equilibrium_parallel", "std::string out", "<int ID project> <int IDCase (optional more than 1)>", Bind_SPC_Parallel_StepEquilibrium);
	add_new_function(state, "pixelcase_stepScheil", "std::string out", "<int IDCase>", Bind_SPC_StepScheil);
	add_new_function(state, "pixelcase_run_cases", "std::string out", "pixelcase_run_cases", Bind_SPC_run_cases);
	// Bind_SPC_Parallel_StepEquilibrium
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

int API_lua_functions::bind_exit(lua_State* state)
{
	std::string hw = _api->APIcommand("exit");
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
	std::string out = _api->APIcommand(parameter);

	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_initializeCore_command(lua_State* state)
{
	if (_configuration == nullptr) return -1;
	std::string out = runVectorCommands(API_Scripting::Script_initialize(_configuration));
	//TODO: if not loaded this causes an error you missed this one
	// 
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

int API_lua_functions::bind_DatabasePhaseNames_command(lua_State* state)
{
	if (_configuration == nullptr) return -1;
	std::string commOut = runVectorCommands(API_Scripting::script_get_thermodynamic_database(_configuration));
	size_t IndexPhases = string_manipulators::find_index_of_keyword(commOut, "# of phases in database");
	size_t IndexExitCode = string_manipulators::find_index_of_keyword(commOut.substr(IndexPhases, commOut.size() - IndexPhases), "MC:") + IndexPhases;

	std::string out;
	if (IndexPhases == std::string::npos || IndexExitCode == std::string::npos) out = "Error, data was not found!";
	else
	{
		out = commOut.substr(IndexPhases, IndexExitCode - IndexPhases);
	}

	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_getPhaseNames_command(lua_State* state)
{
	if (_configuration == nullptr) return -1;

	std::string out = run_command(state, "matcalc_database_phaseNames");
	if (string_manipulators::find_index_of_keyword(out, "Error") != std::string::npos) out = "Error, data was not found!";
	else 
	{ 
		std::vector<std::string> outSplit = string_manipulators::split_text(out, "\n");

		for(size_t n1 = 2; n1 < outSplit.size() - 1 ; n1++)
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

	std::string out = _api->APIcommand(API_Scripting::Script_set_number_of_precipitate_classes(std::stoi(parameter)));
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_selectThermodynamicDatabase_command(lua_State* state)
{
	std::string parameter = lua_tostring(state, 1);

	std::string out = _api->APIcommand(API_Scripting::script_set_thermodynamic_database(parameter));
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_selectPhysicalDatabase_command(lua_State* state)
{
	std::string parameter = lua_tostring(state, 1);

	std::string out = _api->APIcommand(API_Scripting::script_set_physical_database(parameter));
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_selectMobilityDatabase_command(lua_State* state)
{
	std::string parameter = lua_tostring(state, 1);

	std::string out = _api->APIcommand(API_Scripting::script_set_mobility_database(parameter));
	lua_pushstring(state, out.c_str());
	return 1;
}

int API_lua_functions::bind_selectPhases_command(lua_State* state)
{
	std::vector<std::string> parameters = get_parameters(state);
	//TODO: validate phases first!

	std::string out = _api->APIcommand(API_Scripting::Script_selectPhases(parameters));
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
		out += _api->APIcommand(commLine);
	}

	return out;
}

std::string API_lua_functions::runVectorCommands(std::vector<std::string> parameter, IPC_winapi* mcc_comm)
{
	std::string out{};

	for each (std::string commLine in parameter)
	{
		out += _api->APIcommand(commLine, mcc_comm);
	}

	return out;
}
#pragma endregion
