
#include "AM_API_Matcalc.h"

AM_API_Matcalc::AM_API_Matcalc(AM_Config* configuration) 
{
	_configuration = configuration;
	_luaFunctions = new API_lua_functions(_luaInterpreter.get_state(), configuration);
}
AM_API_Matcalc::~AM_API_Matcalc()
{
	//delete _luaFunctions;
}

#pragma region Implementation
std::string AM_API_Matcalc::run_script(const std::string& filename)
{
	return "not implemented";
}

std::string AM_API_Matcalc::run_lua_script(const std::string& filename)
{
	if (!std::filesystem::exists(filename)) return "file not found!";
	_luaInterpreter.run_file(filename);

	return "Done!";
}

std::string AM_API_Matcalc::run_lua_command(const std::string& command)
{
	std::string out = _luaInterpreter.run_command(command);

	return out;
}

std::string AM_API_Matcalc::run_lua_command(const std::string& command, std::vector<std::string> parameters)
{
	std::string out = _luaInterpreter.run_command(command, parameters);

	return out;
}

std::string AM_API_Matcalc::helloApi() { return "From matcalc"; }

void AM_API_Matcalc::dispose() 
{ 
	std::string out = _luaInterpreter.run_command("exit");
}
#pragma endregion
