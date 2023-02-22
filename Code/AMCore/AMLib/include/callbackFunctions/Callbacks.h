#pragma once

#include "CallbackDefinitions.h"
#include "MessageCallBack.h"

#ifdef MYLIB_EXPORTS
#define AM_API __declspec(dllimport)
#else
#define AM_API __declspec(dllexport)
#endif

extern "C"
{
	/// <summary>
	/// Callback used to send a message to any registered function
	/// </summary>
	/// <param name="callF">method</param>
	/// <returns></returns>
	
	AM_API void RegisterMessageCallback(AMFramework::Callback::MessageCallbackF callF);
}