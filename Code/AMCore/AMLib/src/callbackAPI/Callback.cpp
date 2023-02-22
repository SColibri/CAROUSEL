#pragma once

#include <string>
#include "../../include/callbackFunctions/Callbacks.h"

extern "C"
{
	/// <summary>
	/// Message callback
	/// </summary>
	/// <param name="callF">method(char*)</param>
	/// <returns></returns>
	AM_API void RegisterMessageCallback(AMFramework::Callback::MessageCallbackF callF)
	{
		// Save the provided callback function for later use.
		AMFramework::Callback::MessageCallBack::CallFunction = callF;
		AMFramework::Callback::MessageCallBack::TriggerCallback("MessageCallback");
	}

	/// <summary>
	/// Callback used to report errors
	/// </summary>
	/// <param name="callF">method(char*)</param>
	/// <returns></returns>
	AM_API void RegisterErrorCallback(AMFramework::Callback::ErrorCallbackF callF) 
	{
		AMFramework::Callback::ErrorCallback::CallFunction = callF;
		AMFramework::Callback::ErrorCallback::TriggerCallback("MessageCallback");
	}

	/// <summary>
	/// Callback that reports the progress
	/// </summary>
	/// <param name="callF">method(char*, double)</param>
	/// <returns></returns>
	AM_API void RegisterProgressUpdateCallback(AMFramework::Callback::ProgressUpdateCallbackF callF) 
	{
		AMFramework::Callback::ProgressUpdateCallback::CallFunction = callF;
		AMFramework::Callback::ProgressUpdateCallback::TriggerCallback("ProgressUpdateCallback", 0);
	}

	/// <summary>
	/// Callback that reports when a script finished running
	/// </summary>
	/// <param name="callF">method(char*)</param>
	/// <returns></returns>
	AM_API void RegisterScriptFinishedCallback(AMFramework::Callback::ScriptFinishedCallbackF callF) 
	{
		AMFramework::Callback::ScriptFinishedCallback::CallFunction = callF;
		AMFramework::Callback::ScriptFinishedCallback::TriggerCallback("ScriptFinishedCallback");
	}
}
