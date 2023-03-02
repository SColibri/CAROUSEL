#pragma once
#include "CallbackDefinitions.h"

namespace AMFramework 
{
	namespace Callback
	{
		/// <summary>
		/// Message callback, gets message from core implementation
		/// </summary>
		class MessageCallBack
		{
		private:
			MessageCallBack() {}
		public:
			static inline char MessageBuffer[512] = "hello world";

			/// <summary>
			/// Callback
			/// </summary>
			static inline MessageCallbackF CallFunction{nullptr};

			/// <summary>
			/// Trigger
			/// </summary>
			/// <param name="callbackData"></param>
			static void TriggerCallback(char* callbackData)
			{
				// Check if callback has been set
				if (CallFunction != nullptr)
				{
					(*CallFunction)(callbackData);
				}
			}

		};
	}
}