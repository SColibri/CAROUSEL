#pragma once
#include <string>
#include <vector>

extern "C" {
#include "../external/lua542/include/lua.h"
#include "../external/lua542/include/lua.hpp"
#include "../external/lua542/include/lualib.h"
#include "../external/lua542/include/luaconf.h"
}

/// <summary>
/// Functions that should be accessible from the IAM_API implementation
/// are stored in this class.
/// 
/// LUA functions have to be declared as:
/// \code{c++}
/// 
/// void bind_lua(lua_state *L)
/// {
///		// get number of arguments
///		int n = lua_gettop(L);
///		double sum = 0;
///		int i;
/// 
///		/* loop through each argument */
///		for (i = 1; i <= n; i++)
///		{
///			/* total the arguments */
///			sum += lua_tonumber(L, i);
///		}
///
///		/* push the average */
///		lua_pushnumber(L, sum / n);
///
///		/* push the sum */
///		lua_pushnumber(L, sum);
///
///		/* return the number of results */
///		return 2;
/// }
/// \endcode
/// 
/// LUA code has to be added as
/// \code{c++}
/// 
/// 
/// \endcode
/// </summary>
/// 

class IAM_lua_functions
{
public:

	IAM_lua_functions() { }

	/// <summary>
	/// get list of functions in a table format as follows:
	/// Function Name || Parameters || Description
	/// Note: don't forget to include the headers (optional for display)
	/// </summary>
	/// <returns>list table in string format</returns>
	virtual std::vector<std::vector<std::string>> get_list_functions()
	{
		std::vector<std::vector<std::string>> Result;

		for(int n1 = 0; n1 < _functionNames.size(); n1++)
		{
			Result.push_back(std::vector<std::string>{_functionNames[n1], 
													  _functionParameters[n1], 
													  _functionDescription[n1]});
		}

		return Result;
	};
	
protected:
	std::vector<std::string> _functionNames; // Function names
	std::vector<std::string> _functionParameters; // Parameters as input
	std::vector<std::string> _functionDescription; // Description

	/// <summary>
	/// Adds all functions defines on the implementation
	/// </summary>
	/// <param name="state"></param>
	virtual void add_functions_to_lua(lua_State* state) = 0;

	/// <summary>
	/// Adds to list of defined functions in lua
	/// </summary>
	/// <param name="fName"></param>
	/// <param name="fParameters"></param>
	/// <param name="fDescription"></param>
	void add_to_definition(std::string fName, 
						   std::string fParameters,
						   std::string fDescription) 
	{
		_functionNames.push_back(fName);
		_functionParameters.push_back(fParameters);
		_functionDescription.push_back(fDescription);
	}


};