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
		public:
			/// <summary>
			/// Callback
			/// </summary>
			static inline MessageCallbackF CallFunction{nullptr};

			/// <summary>
			/// Trigger
			/// </summary>
			/// <param name="callbackData"></param>
			static inline void TriggerCallback(char* callbackData)
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