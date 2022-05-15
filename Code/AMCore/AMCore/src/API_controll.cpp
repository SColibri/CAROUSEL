#include "../include/API_controll.h"

API_controll::API_controll(AM_Config& configuration)
{
	_configuration = &configuration;
	load_api(configuration);
}

API_controll::~API_controll()
{
	if (_implementation !=  nullptr) 
		delete _implementation;
}

void API_controll::load_api(AM_Config& configuration)
{
	_library = LoadLibrary(TEXT(configuration.get_api_path().c_str()));
	_implementation = DLL_get(_library);
}

#pragma region DynamicLibrary
IAM_API* API_controll::DLL_get(HINSTANCE hLib) {
	MYPROC ProcAdd = (MYPROC)GetProcAddress(hLib, "get_API_Controll");

	if (NULL != ProcAdd)
	{
		IAM_API* Result = ProcAdd(_configuration);
		return Result;
	}

	return nullptr;
}
#pragma endregion
