#pragma once

#include "CallbackDefinitions.h"
#include "MessageCallBack.h"
#include "ErrorCallback.h"
#include "ProgressUpdateCallback.h"
#include "ScriptFinishedCallback.h"

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
	/// <param name="callF">method(char*)</param>
	/// <returns></returns>

	AM_API void RegisterMessageCallback(AMFramework::Callback::MessageCallbackF callF);

	/// <summary>
	/// Callback used to report errors
	/// </summary>
	/// <param name="callF">method(char*)</param>
	/// <returns></returns>
	AM_API void RegisterErrorCallback(AMFramework::Callback::ErrorCallbackF callF);

	/// <summary>
	/// Callback that reports the progress
	/// </summary>
	/// <param name="callF">method(char*, double)</param>
	/// <returns></returns>
	AM_API void RegisterProgressUpdateCallback(AMFramework::Callback::ProgressUpdateCallbackF callF);

	/// <summary>
	/// Callback that reports when a script finished running
	/// </summary>
	/// <param name="callF">method(char*)</param>
	/// <returns></returns>
	AM_API void RegisterScriptFinishedCallback(AMFramework::Callback::ScriptFinishedCallbackF callF);
}