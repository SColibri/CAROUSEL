#pragma once

#include <string>
#include "../../interfaces/IAM_lua_functions.h"
#include "../../interfaces/IAM_Database.h"
#include "../../include/AM_Database_Framework.h"

extern "C" {
#include "../../external/lua542/include/lua.h"
#include "../../external/lua542/include/lua.hpp"
#include "../../external/lua542/include/lualib.h"
#include "../../external/lua542/include/luaconf.h"
}

namespace AMFramework 
{
	namespace LUA 
	{
		/// <summary>
		/// Implements all lua basic functionality used in database handling
		/// </summary>
		class LuaDatabaseModule
		{
		private:
			inline static AM_Database_Framework* _dbFramework{ nullptr };

			/// <summary>
			/// Private Constructor, this is a static class
			/// </summary>
			/// <param name="state"></param>
			LuaDatabaseModule() {}

		public:
			
		

			static int baseBind_DatabaseQuery(lua_State* state)
			{
				std::string out{ "" };
				int noParameters = lua_gettop(state);
				std::string parameter = lua_tostring(state, 1);

				if (noParameters == 1)
				{
					out = _dbFramework->get_database()->get_tableRows(parameter);
				}
				else if (noParameters == 2)
				{
					std::string parameter_2 = lua_tostring(state, 2);
					string_manipulators::replace_token_from_socketString(parameter_2);

					out = _dbFramework->get_database()->get_tableRows(parameter, parameter_2);
				}


				lua_pushstring(state, out.c_str());
				return 1;
			}

			static int baseBind_DatabaseCreateView(lua_State* state)
			{
				std::vector<std::string> parameters = get_parameters(state);
				std::string out{ "" };

				if (parameters.size() < 1)
				{
					out = "Error, Please insert a valid SQL view query";
					lua_pushstring(state, out.c_str());
					return 1;
				}

				int response = _dbFramework->get_database()->create_view(IAM_Database::csv_join_row(parameters, " "));
				if (response == 0)
				{
					out = "OK";
				}
				else
				{
					out = "An error ocurred while executing the SQL command: " + IAM_Database::csv_join_row(parameters, " ");
				}

				lua_pushstring(state, out.c_str());
				return 1;
			}

			static int baseBind_DatabaseByQuery(lua_State* state)
			{
				std::vector<std::string> parameters = get_parameters(state);
				std::string out{ "" };

				if (parameters.size() < 1)
				{
					out = "Error, wrong parameters!";
					lua_pushstring(state, out.c_str());
					return 1;
				}

				std::vector<std::vector<std::string>> tableContents = _dbFramework->get_database()->get_fromQuery(IAM_Database::csv_join_row(parameters, " "));
				out = IAM_Database::get_csv(tableContents);

				lua_pushstring(state, out.c_str());
				return 1;
			}

			static int baseBind_DatabaseTableList(lua_State* state)
			{
				std::string out{ "" };
				int noParameters = lua_gettop(state);

				out = IAM_Database::csv_join_row(_dbFramework->get_database()->get_tableNames(), IAM_Database::Delimiter);

				lua_pushstring(state, out.c_str());
				return 1;
			}

			static int baseBind_DatabaseRowNumber(lua_State* state)
			{
				std::vector<std::string> parameters = get_parameters(state);
				if (parameters.size() == 0)
				{
					lua_pushstring(state, "baseBind_DatabaseRowNumber: <TableName> <WHERE Query> as input parameters missing");
				}

				std::vector<std::vector<std::string>> outTable = std::vector<std::vector<std::string>>{ std::vector<std::string>{"0"} };

				if (parameters.size() == 2)
				{
					outTable = _dbFramework->get_database()->get_fromQuery("SELECT COUNT(*) FROM " + parameters[0] + " WHERE " + parameters[1]);
				}
				else
				{
					outTable = _dbFramework->get_database()->get_fromQuery("SELECT COUNT(*) FROM " + parameters[0]);
				}

				lua_pushstring(state, outTable[0][0].c_str());
				return 1;
			}

			static int baseBind_get_functionList(lua_State* state)
			{
				std::ifstream ifs("lua_functions.txt");
				std::string out((std::istreambuf_iterator<char>(ifs)),
					(std::istreambuf_iterator<char>()));
				ifs.close();
				lua_pushstring(state, out.c_str());
				return 1;
			}

			static int baseBind_run_lua_script(lua_State* state)
			{
				int noParameters = lua_gettop(state);
				std::string parameter = lua_tostring(state, 1);

				try
				{
					bool outy = luaL_dofile(state, parameter.c_str());
				}
				catch (std::exception& em)
				{
					std::string err = "Error: Something went wrong while running trying to run the script! is it still there?";
					err += em.what();

					lua_pushstring(state, err.c_str());
				}

				return 1;
			}

			std::vector<std::string> static get_parameters(lua_State* state)
			{
				int noParameters = lua_gettop(state);
				std::vector<std::string> outParameters;

				for (int n1 = 1; n1 <= noParameters; n1++)
				{
					outParameters.push_back(lua_tostring(state, n1));
					string_manipulators::replace_token_from_socketString(outParameters[n1 - 1]);
				}

				return outParameters;
			}

		};
	}
}