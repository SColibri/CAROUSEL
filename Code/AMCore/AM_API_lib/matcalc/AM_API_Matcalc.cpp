
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

typedef void(__cdecl* MYPROC_2)();

std::string AM_API_Matcalc::run_script(const std::string& filename)
{
	return "not implemented";
}
