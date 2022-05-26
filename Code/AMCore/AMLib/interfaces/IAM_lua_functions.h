#pragma once
#include <string>
#include <vector>
#include "../interfaces/IAM_Database.h"
#include "../include/AM_Database_Framework.h"

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
/// More information: https://www.cs.usfca.edu/~galles/cs420/lecture/LuaLectures/LuaAndC.html

class IAM_lua_functions
{
public:

	IAM_lua_functions(lua_State* state) 
	{
		add_base_functions(state);
	};
	
	virtual ~IAM_lua_functions() {
		std::string stopHere{};
	};

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
	
	/// <summary>
	/// Add new funtion to lua
	/// </summary>
	/// <param name="state">lua State pointer</param>
	/// <param name="function_name"> name of new function </param>
	/// <param name="output"> output type </param>
	/// <param name="usage"> specify how to use </param>
	/// <param name="newFunction"> ONLY static function pointer </param>
	void add_new_function(lua_State* state,
		std::string function_name,
		std::string output,
		std::string usage,
		int (*newFunction)(lua_State*))
	{
		lua_register(state, function_name.c_str(), newFunction);
		add_to_definition(function_name, output, usage);
	}

protected:
	inline static AM_Database_Framework* _dbFramework{ nullptr };
	//inline static IAM_Database* _database{ nullptr };
	inline static std::string _dllPath{};
	inline static AM_Config* _configuration{ nullptr };

	std::vector<std::string> _functionNames; // Function names
	std::vector<std::string> _functionParameters; // Parameters as input
	std::vector<std::string> _functionDescription; // Description

	/// <summary>
	/// Adds all functions defines on the implementation
	/// </summary>
	/// <param name="state"></param>
	virtual void add_functions_to_lua(lua_State* state) = 0;

	

private:

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

	void add_base_functions(lua_State* state) 
	{
		add_new_function(state, "database_tableQuery", "string, csv format, delimiter space char", "database_tableQuery <tablename> <optional-where clause>", baseBind_DatabaseQuery);
		add_new_function(state, "database_tableList", "string, csv format, delimiter space char", "database_tableList", baseBind_DatabaseQuery);

		add_new_function(state, "dataController_selectProjectID", "string", "dataController_selectProjectID <int>", Bind_dataController_selectProjectID);
		add_new_function(state, "dataController_setProjectName", "string", "dataController_setProjectName <string>", Bind_dataController_setProjectName);
		add_new_function(state, "dataController_selectCase", "string", "dataController_selectCase <ID>", Bind_dataController_selectCase);
		add_new_function(state, "dataController_csv", "string, csv format, delimiter space char", "dataController_csv <enum::DATATABLES>", Bind_dataController_csv);
		//add_new_function(state, "", "", "", baseBind_DatabaseQuery);
	}

#pragma region helpers

#pragma endregion

#pragma region BASE_FUNCTIONS
	static int baseBind_DatabaseQuery(lua_State* state)
	{
		std::string out{""};
		int noParameters = lua_gettop(state);
		std::string parameter = lua_tostring(state, 1);

		if(noParameters == 1)
		{
			out = _dbFramework->get_database()->get_tableRows(parameter);
		}
		else if (noParameters == 2)
		{
			std::string parameter_2 = lua_tostring(state, 2);
			out = _dbFramework->get_database()->get_tableRows(parameter, parameter_2);
		}
		

		lua_pushstring(state, out.c_str());
		return 1;
	}

	static int baseBind_DatabaseTableList(lua_State* state)
	{
		std::string out{ "" };
		int noParameters = lua_gettop(state);
		std::string parameter = lua_tostring(state, 1);

		out = IAM_Database::csv_join_row(_dbFramework->get_database()->get_tableNames(), IAM_Database::Delimiter);

		lua_pushstring(state, out.c_str());
		return 1;
	}

#pragma region Data_controller
	static int Bind_dataController_selectProjectID(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string out{ "" };
		std::string parameter = lua_tostring(state, 1);
		_dbFramework->get_dataController()->set_project_ID(std::stoi(parameter));

		lua_pushstring(state, "Project ID changed");
		return 1;
	}

	static int Bind_dataController_setProjectName(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string out{ "" };
		std::string parameter = lua_tostring(state, 1);
		_dbFramework->get_dataController()->set_project_name(parameter);

		lua_pushstring(state, "Project name changed");
		return 1;
	}

	static int Bind_dataController_selectCase(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string out{ "" };
		std::string parameter = lua_tostring(state, 1);
		_dbFramework->get_dataController()->select_case(std::stoi(parameter));

		lua_pushstring(state, "Case selected");
		return 1;
	}

	static int Bind_dataController_csv(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string parameter = lua_tostring(state, 1);
		std::string out = _dbFramework->get_dataController()->get_csv((Data_Controller::DATATABLES)std::stoi(parameter));

		lua_pushstring(state, out.c_str());
		return 1;
	}

	

#pragma endregion
#pragma endregion

};