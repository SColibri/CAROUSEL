#pragma once

#include <string>
#include "../../include/callbackFunctions/Callbacks.h"

extern "C"
{
	AM_API void RegisterMessageCallback(AMFramework::Callback::MessageCallbackF callF)
	{
		// Save the provided callback function for later use.
		AMFramework::Callback::MessageCallBack::CallFunction = callF;
		//char tert[] = "API is connected!";
		AMFramework::Callback::MessageCallBack::TriggerCallback("Api is working");
	}
}
