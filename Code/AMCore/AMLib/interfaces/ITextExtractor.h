#pragma once

#include <vector>
#include <string>

#include "IAM_Communication.h"

namespace AMFramework 
{
	namespace Interfaces 
	{
		class ITextExtractor
		{
		protected:
			std::string _text{""};

		public:
			/// <summary>
			/// Returns the found values, empty if none
			/// </summary>
			/// <returns></returns>
			virtual std::vector<std::string> extract(IAM_Communication* comm) = 0;
			virtual std::vector<std::string> extract(std::string text) = 0;
			virtual std::vector<std::string> extract() = 0;
		};
	}
}