#pragma once

#include <vector>
#include <string>
#include "../AM_Database_Framework.h"
#include "../AM_Config.h"

#define	LUABUFFER 2048

namespace AMFramework
{
	namespace LUA 
	{
		class LuaBase
		{
		private:
			std::vector<std::string> _functionNames; // Function names
			std::vector<std::string> _functionParameters; // Parameters as input
			std::vector<std::string> _functionDescription; // Description
			std::vector<std::string> _functionCommandSpecs; // Command specifications (for gui)
			AM_Database_Framework* _dbFramework{ nullptr };
			AM_Config* _configuration{ nullptr };
			char _buffer[LUABUFFER];
			bool _cancelCalculations{ false };
		};
	}
}