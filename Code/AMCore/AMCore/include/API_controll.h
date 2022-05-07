#pragma once
#include "../../AMLib/include/AM_Config.h"
#include "../../AMLib/include/AM_lua_interpreter.h"
#include "../../AMLib/interfaces/IAM_API.h"
#include <Windows.h>

/// <summary>
/// Handles communication with the dynamic library that
/// contains the implementation of the IAM_API interface
/// </summary>
class API_controll
{
public:
	/// <summary>
	/// Constructor for API_controll needs to know where the
	/// IAM_API implementation is found.
	/// </summary>
	/// <param name="configuration"></param>
	API_controll(AM_Config& configuration);
	~API_controll();

	IAM_API* get_implementation() { return _implementation; }

private:
	HINSTANCE _library{NULL}; // dynamic library instance
	
	/// <summary>
	/// implementation of the IAM_API interface, where
	/// all CALPHAD, and other calculations are done
	/// </summary>
	IAM_API* _implementation{nullptr};

	/// <summary>
	/// Load dynamic library
	/// </summary>
	/// <param name="configuration"></param>
	void load_api(AM_Config& configuration);

#pragma region DynamicLibrary
	/// <summary>
	/// Defines the function that returns the concrete 
	/// implementation of IAM_API
	/// </summary>
	typedef IAM_API* (__cdecl* MYPROC)();

	/// <summary>
	/// Pointer to concrete implementation of IAM_API
	/// </summary>
	/// <param name="hLib"></param>
	/// <returns></returns>
	IAM_API* DLL_get(HINSTANCE hLib);
#pragma endregion

};
