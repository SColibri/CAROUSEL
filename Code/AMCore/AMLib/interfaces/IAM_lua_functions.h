#pragma once
#include <string>
#include <vector>
#include <fstream>
#include <iostream>
#include <algorithm>
#include "../interfaces/IAM_Database.h"
#include "../include/AM_Database_Framework.h"
#include "../include/AM_Project.h"
#include "../x_Helpers/string_manipulators.h"

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
		functionsList_clear();
		//add_base_functions(state);
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

	AM_Database_Framework* get_dbFramework()
	{
		return _dbFramework;
	}

protected:
	inline static AM_Database_Framework* _dbFramework{ nullptr };
	inline static AM_Project* _openProject{nullptr};
	inline static std::string _dllPath{};
	inline static AM_Config* _configuration{ nullptr };

	inline static std::vector<std::string> _functionNames; // Function names
	inline static std::vector<std::string> _functionParameters; // Parameters as input
	inline static std::vector<std::string> _functionDescription; // Description

	/// <summary>
	/// Adds all functions defines on the implementation
	/// </summary>
	/// <param name="state"></param>
	virtual void add_functions_to_lua(lua_State* state) = 0;

	void add_base_functions(lua_State* state)
	{
		// LUA
		add_new_function(state, "get_functionList", "string, csv format, delimiter comma char", "get_functionList", baseBind_get_functionList);
		add_new_function(state, "run_lua_script", "string output", "run_lua_script <filename>", baseBind_run_lua_script);
		add_new_function(state, "add_function_to_framework", "string output", "add_function_to_framework <function name, output, usage>", Bind_add_function_to_framework);

		// Database
		add_new_function(state, "database_tableQuery", "string, csv format, delimiter comma char", "database_tableQuery <tablename> <optional-where clause>", baseBind_DatabaseQuery);
		add_new_function(state, "database_tableList", "string, csv format, delimiter comma char", "database_tableList", baseBind_DatabaseTableList);
		
		//Data controller
		add_new_function(state, "dataController_csv", "string, csv format, delimiter comma char", "dataController_csv <enum::DATATABLES>", Bind_dataController_csv);

		//Project
		//------- Binded to open project
		add_new_function(state, "project_loadID", "string", "project_selectID <int>", Bind_project_loadID);
		add_new_function(state, "project_loadName", "string", "project_loadName <string>", Bind_project_loadName);
		add_new_function(state, "project_new", "string", "project_new <string Name>", Bind_project_new);
		add_new_function(state, "project_setName", "string", "projet_setName <new name>", Bind_project_setName);
		add_new_function(state, "project_getData", "string csv format", "project_getData <int ID>", Bind_project_getData);
		add_new_function(state, "project_setData", "string csv format", "project_setData <int ID>,<string Name>", Bind_project_setData);
		add_new_function(state, "project_selectElements", "string csv format", "project_SelectElements <string element1> (add all alements as parameters sepparated by a space char)", Bind_project_SelectElements);
		add_new_function(state, "project_getSelectedElements", "string csv format", "project_getSelectedElements", Bind_project_getSelectedElements);
		add_new_function(state, "project_clear_content", "string", "project_getSelectedElements <optional INT IDproject>", Bind_project_clearContent);
		add_new_function(state, "project_setReferenceElement", "string", "project_setReferenceElement <string>", Bind_project_setReferenceElement);
		add_new_function(state, "project_getReferenceElement", "string", "project_getReferenceElement", Bind_project_getReferenceElement);
		add_new_function(state, "project_getDataTableCases", "string", "project_getDataTableCases", Bind_project_getDataTableCases);

		//###-> Pixel Case
		add_new_function(state, "template_pixelcase_new", "string", "template_pixelcase_new", Bind_PCTemplate_createNew);
		add_new_function(state, "template_pixelcase_concentrationVariant", "string", 
								"template_pixelcase_concentrationVariant <string Element Name> <double stepSize> <int steps>", Bind_PCTemplate_createConcentrationVariant);
		add_new_function(state, "template_pixelcase_setComposition", "string", "template_pixelcase_setComposition <string> <double> (input in pairs, not limited to one pair)", Bind_PCTemplate_setComposition);
		add_new_function(state, "template_pixelcase_getComposition", "string", "template_pixelcase_getComposition", Bind_PCTemplate_getComposition);
		add_new_function(state, "template_pixelcase_selectPhases", "string", "template_pixelcase_selectPhases <string>", Bind_PCTemplate_selectPhases);
		add_new_function(state, "template_pixelcase_getSelectPhases", "string", "template_pixelcase_getSelectPhases", Bind_PCTemplate_getSelectPhases);

		add_new_function(state, "template_pixelcase_setEquilibriumTemperatureRange", "string", "template_pixelcase_getComposition", Bind_PCTemplate_setEquilibrimTemperatureRange);

		add_new_function(state, "template_pixelcase_setScheilTemperatureRange", "string", "template_pixelcase_setScheilTemperatureRange <double start> <double end>", Bind_PCTemplate_setScheilTemperatureRange);
		add_new_function(state, "template_pixelcase_setScheilLiquidFraction", "string", "template_pixelcase_setScheilLiquidFraction <double>", Bind_PCTemplate_setScheilLiquidFraction);
		add_new_function(state, "template_pixelcase_setScheilDependentPhase", "string", "template_pixelcase_setScheilDependentPhase <string>", Bind_PCTemplate_setScheilDependentPhase);
		add_new_function(state, "template_pixelcase_setScheilStepSize", "string", "template_pixelcase_setScheilStepSize <double>", Bind_PCTemplate_setScheilStepSize);


		//------- Generic (needs project ID)


		//AMConfig
		add_new_function(state, "configuration_getAPI_path", "string", "configuration_getAPI_path (gets path to AMFramework dll)", Bind_configuration_getAPIpath);
		add_new_function(state, "configuration_setAPI_path", "string status", "configuration_setAPI_path <string Filename> (set path to AMFramework dll)", Bind_configuration_setAPIpath);
		add_new_function(state, "configuration_getExternalAPI_path", "string", "configuration_getExternalAPI_path (gets directory of external API e.g. matcalc)", Bind_configuration_getExternalAPIpath);
		add_new_function(state, "configuration_setExternalAPI_path", "string status", "configuration_setAPI_path <string Filename> (set path to external dll e.g. matcalc)", Bind_configuration_setExternalAPIpath);
		add_new_function(state, "configuration_get_working_directory", "string", "configuration_get_working_directory", Bind_configuration_getWorkingDirectory);
		add_new_function(state, "configuration_set_working_directory", "string", "configuration_set_working_directory <string Directory>", Bind_configuration_setWorkingDirectory);
		add_new_function(state, "configuration_get_thermodynamic_database_path", "string", "configuration_get_thermodynamic_database_path", Bind_configuration_setThermodynamicDatabasePath);
		add_new_function(state, "configuration_set_thermodynamic_database_path", "string", "configuration_set_thermodynamic_database_path <string filename>", Bind_configuration_getThermodynamicDatabasePath);
		add_new_function(state, "configuration_get_physical_database_path", "string", "configuration_get_physical_database_path", Bind_configuration_getPhysicalDatabasePath);
		add_new_function(state, "configuration_set_physical_database_path", "string", "configuration_set_physical_database_path <string filename>", Bind_configuration_setPhysicalDatabasePath);
		add_new_function(state, "configuration_get_mobility_database_path", "string", "configuration_get_mobility_database_path", Bind_configuration_getMobilityDatabasePath);
		add_new_function(state, "configuration_set_mobility_database_path", "string", "configuration_set_mobility_database_path <string filename>", Bind_configuration_setMobilityDatabasePath);


	}

protected:

	/// <summary>
	/// Adds to list of defined functions in lua
	/// </summary>
	/// <param name="fName"></param>
	/// <param name="fParameters"></param>
	/// <param name="fDescription"></param>
	static void add_to_definition(std::string fName,
		std::string fParameters,
		std::string fDescription)
	{
		_functionNames.push_back(fName);
		_functionParameters.push_back(fParameters);
		_functionDescription.push_back(fDescription);

		functionsList_addEntry(std::vector<std::string>{fName, 
														fParameters, 
														fDescription });
	}

	

#pragma region helpers
	void functionsList_clear()
	{
		std::ofstream ofs;
		ofs.open("lua_functions.txt", std::ofstream::out | std::ofstream::trunc);
		ofs.close();
	}

	static void functionsList_addEntry(std::vector<std::string> newEntry)
	{
		std::ofstream ofs;
		ofs.open("lua_functions.txt", std::ofstream::out | std::ofstream::app);
		ofs << IAM_Database::csv_join_row(newEntry, IAM_Database::Delimiter) + "\n";
		ofs.close();
	}

	int static check_for_open_project(lua_State* state) 
	{
		if (_openProject == nullptr)
		{
			lua_pushstring(state, "No selected project!, select or create a project first :)");
			return 1;
		}
		return 0;
	}

	int static check_parameters(lua_State* state, int noParameters, int neededParam, std::string message) 
	{
		if (noParameters < neededParam)
		{
			std::string strBuilder = "Missing parameters: " + message;
			lua_pushstring(state, strBuilder.c_str());
			return 1;
		}
		return 0;
	}

	int static check_global_using_openProject(lua_State* state, int noParameters, 
											 int neededParam, std::string message,
											 std::vector<std::string>& outParameters)
	{
		if (check_for_open_project(state) != 0) return 1;
		if (check_parameters(state, noParameters, neededParam, message) != 0) return 1;
		outParameters = get_parameters(state);

		return 0;
	}

	std::vector<std::string> static get_parameters(lua_State* state) 
	{
		int noParameters = lua_gettop(state);
		std::vector<std::string> outParameters;

		for(int n1 = 1; n1 <= noParameters; n1++)
		{
			outParameters.push_back(lua_tostring(state, n1));
			string_manipulators::replace_token_from_socketString(outParameters[n1-1]);
		}

		return outParameters;
	}
#pragma endregion

#pragma region BASE_FUNCTIONS

#pragma region LUA
	static int Bind_add_function_to_framework(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		if (noParameters < 3)
		{
			lua_pushstring(state, "Ups... what name should we use?");
			return 1;
		}

		std::vector<std::string> parameters = get_parameters(state);
		add_to_definition(parameters[0], parameters[0], parameters[0]);

		lua_pushstring(state, "OK");
		return 1;
	}

#pragma endregion
	
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

		out = IAM_Database::csv_join_row(_dbFramework->get_database()->get_tableNames(), IAM_Database::Delimiter);

		lua_pushstring(state, out.c_str());
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
		bool outy = luaL_dofile(state, "C:/Users/drogo/Desktop/Homless/TestLua.lua");// parameter.c_str());
		return 1;
	}


#pragma region Configuration
	/// <summary>
	/// returns the directory where the library of the implementation for IAM_API can be found.
	/// e.g. For matcalc the corresponding library is called AM_API_Matcalc.dll
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_getAPIpath(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string out = _configuration->get_api_path();

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Changes the directory where to search the AMFramework API path
	/// corresponding to the implementation for the IAM_API (not the external API)
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_setAPIpath(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		if(noParameters > 0)
		{
			std::string parameter = lua_tostring(state, 1);
			_configuration->set_api_path(parameter);

			lua_pushstring(state, "OK");
		}
		else
		{
			lua_pushstring(state, "Error; no parameters");
		}
		
		return 1;
	}

	/// <summary>
	/// Returns the directory where the library of the external API can be found.
	/// e.g. matcalc.dll
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_getExternalAPIpath(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string out = _configuration->get_apiExternal_path();

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Changes the directory where to search the external API
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_setExternalAPIpath(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		if (noParameters > 0)
		{
			std::string parameter = lua_tostring(state, 1);
			_configuration->set_apiExternal_path(parameter);

			lua_pushstring(state, "OK");
		}
		else
		{
			lua_pushstring(state, "Error; no parameters");
		}

		return 1;
	}

	/// <summary>
	/// Get session working directory.
	/// Note: If you run a script, the working directory changes automatically to the 
	/// directory of that file, you can override this behaviour by setting the 
	/// working directory on the script.
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_getWorkingDirectory(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string out = _configuration->get_working_directory();

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Changes the working directory
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_setWorkingDirectory(lua_State* state)
	{
		int noParameters = lua_gettop(state);

		if(noParameters > 0)
		{
			std::string parameter = lua_tostring(state, 1);
			_configuration->set_working_directory(parameter);

			lua_pushstring(state, "OK");
		}
		else
		{
			lua_pushstring(state, "Error; no parameters");
		}
		
		return 1;
	}

	/// <summary>
	/// Returns the path to the thermodynamic database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_getThermodynamicDatabasePath(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string out = _configuration->get_ThermodynamicDatabase_path();

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Set the path of the thermodynamic database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_setThermodynamicDatabasePath(lua_State* state)
	{
		std::vector<std::string> parameters = get_parameters(state);

		if (parameters.size() > 0)
		{
			std::string parameter = IAM_Database::csv_join_row(parameters, " ");
			_configuration->set_ThermodynamicDatabase_path(parameter);

			lua_pushstring(state, "OK");
		}
		else
		{
			lua_pushstring(state, "Error; no parameters");
		}

		return 1;
	}

	/// <summary>
	/// Returns the path to the physical database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_getPhysicalDatabasePath(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string out = _configuration->get_PhysicalDatabase_path();

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Set the path of the physical database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_setPhysicalDatabasePath(lua_State* state)
	{
		std::vector<std::string> parameters = get_parameters(state);

		if (parameters.size() > 0)
		{
			std::string parameter = IAM_Database::csv_join_row(parameters, " ");
			_configuration->set_PhysicalDatabase_path(parameter);

			lua_pushstring(state, "OK");
		}
		else
		{
			lua_pushstring(state, "Error; no parameters");
		}

		return 1;
	}

	/// <summary>
	/// Returns the path to the mobility database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_getMobilityDatabasePath(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		std::string out = _configuration->get_MobilityDatabase_path();

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Set the path of the mobility database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_setMobilityDatabasePath(lua_State* state)
	{
		std::vector<std::string> parameters = get_parameters(state);

		if (parameters.size() > 0)
		{
			std::string parameter = IAM_Database::csv_join_row(parameters, " ");
			_configuration->set_MobilityDatabase_path(parameter);

			lua_pushstring(state, "OK");
		}
		else
		{
			lua_pushstring(state, "Error; no parameters");
		}

		return 1;
	}

#pragma endregion


#pragma region Data_controller

#pragma region Project
	/// <summary>
	/// Select project by ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_loadID(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		if(noParameters == 0)
		{
			lua_pushstring(state, "Ups... what id should we use?");
			return 1;
		}

		std::string out{ "" };
		std::string parameter = lua_tostring(state, 1);
		if (_openProject != nullptr) delete _openProject;
		_openProject = new AM_Project(_dbFramework->get_database(), _configuration, std::stoi(parameter));

		if(_openProject->get_project_ID() == -1) lua_pushstring(state, "Project with this ID does not exist, sorry!");
		else lua_pushstring(state, "OK");

		return 1;
	}

	/// <summary>
	/// Select project by Name
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_loadName(lua_State* state)
	{
		int noParameters = lua_gettop(state);
		if (noParameters == 0)
		{
			lua_pushstring(state, "Ups... what name should we use?");
			return 1;
		}

		std::string parameters = IAM_Database::csv_join_row(get_parameters(state)," ");
		parameters = string_manipulators::trim_whiteSpace(parameters);

		if (_openProject != nullptr) delete _openProject;
		_openProject = new AM_Project(_dbFramework->get_database(), _configuration, parameters);

		if(_openProject->get_project_ID() == -1)
		{
			lua_pushstring(state, "Project does not exist");
			return 1;
		}


		lua_pushstring(state, "OK");
		return 1;
	}

	/// <summary>
	/// Set project name
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_setName(lua_State* state)
	{
		if (_openProject == nullptr) _openProject = new AM_Project(_dbFramework->get_database(), _configuration, -1);
		int noParameters = lua_gettop(state);
		std::string out{ "" };
		std::string parameter = lua_tostring(state, 1);
		std::replace(parameter.begin(), parameter.end(), '#', ' ');
		_openProject->set_project_name(parameter, _dbFramework->get_apiExternalPath());
		
		lua_pushstring(state, "OK");
		return 1;
	}

	/// <summary>
	/// creates a new project
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_new(lua_State* state)
	{
		if (_openProject != nullptr) delete _openProject;
		_openProject = new AM_Project(_dbFramework->get_database(), _configuration, "Empty project");

		std::vector<std::string> parameters = get_parameters(state);
		if(parameters.size() == 0)
		{
			lua_pushstring(state, "Bind_project_new: Error, missing input");
			return 1;
		}

		_openProject->set_project_name(parameters[0], _dbFramework->get_apiExternalPath());

		lua_pushstring(state, "OK");
		return 1;
	}

	/// <summary>
	/// Returns project data structure in a csv structure
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_getData(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 0,
			" you are golden! ",
			parameters) != 0) return 1;

		std::string outy = _openProject->get_project_data_csv();
		lua_pushstring(state, outy.c_str());
		return 1;
	}

	/// <summary>
	/// set project data in csv form ID,Name,APIPath(optional)
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_setData(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" you are golden! ",
			parameters) != 0) return 1;

		// user can set the API name optionally but by default we use the configuration file
		if (parameters.size() == 2) { parameters.push_back(_dbFramework->get_apiExternalPath()); }

		DBS_Project newP(_dbFramework->get_database(), std::stoi(parameters[0]));
		newP.load(parameters);
		newP.save();
		_openProject->refresh_data();

		lua_pushstring(state, std::to_string(newP.id()).c_str());
		return 1;
	}

	/// <summary>
	/// Selects a new set of elements, this will remove all existing data since
	/// modifying the selection would also mean a different project.
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_SelectElements(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 1,
			" Please add atleast one valid Element ",
			parameters) != 0) return 1;

		lua_pushstring(state, _openProject->set_selected_elements_ByName(parameters).c_str());
		return 1;
	}

	/// <summary>
	/// Show selected elements, for current project
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_getSelectedElements(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 0,
			" you are golden! ",
			parameters) != 0) return 1;

		lua_pushstring(state, _openProject->csv_list_SelectedElements().c_str());
		return 1;
	}

	/// <summary>
	/// Removes all cases and related data. If ID is not specified it will remove data from
	/// the current open project
	/// Input: optional IDProject
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_clearContent(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 0,
			" you are golden! ",
			parameters) != 0) return 1;
		
		if(parameters.size() > 0)
		{
			if (string_manipulators::isNumber(parameters[0]))
			{
				DBS_Project::remove_project_data(_dbFramework->get_database(), std::stoi(parameters[0]));
				lua_pushstring(state, "Bind_project_clearContent: All data has been removed");
				return 1;
			}
			lua_pushstring(state, "Bind_project_clearContent: Input parameter is not an integer for ID project.");
			return 1;
		}
		
		_openProject->clear_project_data();
		lua_pushstring(state, "OK");
		return 1;
	}

	/// <summary>
	/// set reference element, this action does not modify the existing cases, it only
	/// sets the reference element for further operations.
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_setReferenceElement(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 1,
			" plese set a valid element ",
			parameters) != 0) return 1;

		if (_openProject->set_reference_element(parameters[0]) != 0)
		{
			//TDOD: notify the user about the error, be more specific
			lua_pushstring(state, "Something went wrong with the given input, Element might not be selected or template is not created");
			return 1;
		}

		lua_pushstring(state, "OK");
		return 1;
	}

	/// <summary>
	/// returns reference element by name
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_getReferenceElement(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 0,
			" you are golden! ",
			parameters) != 0) return 1;

		lua_pushstring(state, _openProject->get_reference_element_ByName().c_str());
		return 1;
	}

	/// <summary>
	/// returns list of cases that the project holds
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_project_getDataTableCases(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 0,
			" you are golden! ",
			parameters) != 0) return 1;

		lua_pushstring(state, _openProject->csv_list_cases_singlePixel().c_str());
		return 1;
	}

	
#pragma region SinglePixelCase
	
	static int Bind_project_new_SPC(lua_State* state)
	{
		// We require an existing and open project
		if (_openProject == nullptr)
		{
			lua_pushstring(state, "No selected project!, select or create a project first :)");
			return 1;
		}
		
		int noParameters = lua_gettop(state);
		if (noParameters < 1)
		{
			lua_pushstring(state, "Ups... did you forget to name the Case?");
			return 1;
		}

		std::string parameter = lua_tostring(state, 1);
		string_manipulators::replace_token_from_socketString(parameter);
		_openProject->new_singlePixel_Case(parameter);

		lua_pushstring(state, _openProject->csv_list_SelectedElements().c_str());
		return 1;
	}

#pragma endregion

#pragma region PixelCase
	static int Bind_PCTemplate_createNew(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 1,
			" Please set a temperature value ",
			parameters) != 0) return 1;


		_openProject->create_case_template(parameters[0]);
		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PC_createConcentrationVariant(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" you are golden ",
			parameters) != 0) return 1;


		AM_pixel_parameters* pointerPixel = _openProject->get_pixelCase(std::stoi(parameters[0]));
		pointerPixel->set_equilibrium_config_endTemperature(std::stold(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PCTemplate_createConcentrationVariant(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 3,
			" Please set a temperature value ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if(pointerPixel == nullptr) 
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		// input parameters have to be set in three component input
		if(parameters.size() % 3 != 0)
		{
			lua_pushstring(state, "Input has to specify three components for each varying element as; Element name - step size - steps, e.g: AL 0.5 10");
			return 1;
		}
		
		// get parameters from vector
		std::vector<int> ElementsID;
		std::vector<double> stepSize;
		std::vector<double> steps;
		for (int n1 = 0; n1 < parameters.size(); n1 = n1 + 3)
		{
			//TODO check for user input, this might lead to an error! need to hurry the demo that is why this is sloppy :(
			// but I promisse that I'll be back!
			DBS_Element tempElement(_dbFramework->get_database(), -1);
			tempElement.load_by_name(parameters[n1]);
			ElementsID.push_back(tempElement.id());

			stepSize.push_back(std::stold(parameters[n1 + 1]));
			steps.push_back(std::stold(parameters[n1 + 2]));
		}

		pointerPixel->create_cases_vary_concentration(ElementsID, stepSize, steps, pointerPixel->get_composition_double(), 0);
		lua_pushstring(state, "OK");
		_openProject->refresh_data();
		return 1;
	}

	static int Bind_PCTemplate_getComposition(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 0,
			" Please set a temperature value ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		
		lua_pushstring(state, pointerPixel->csv_composition_ByName().c_str());
		return 1;
	}

	static int Bind_PCTemplate_setComposition(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Please set a temperature value ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		// input parameters have to be set in three component input
		if (parameters.size() % 2 != 0)
		{
			lua_pushstring(state, "Input has to specify two components for each varying element as; Element name - step size - steps, e.g: AL 0.5 10");
			return 1;
		}

		for (int n1 = 0; n1 < parameters.size(); n1 = n1 + 2)
		{
			if(pointerPixel->set_composition(parameters[n1], std::stold(parameters[n1+1])) != 0)
			{
				lua_pushstring(state, "Ups.. an error ocurred when assigning the concentration, are all the elements selected?");
				return 1;
			}
		}

		lua_pushstring(state, pointerPixel->csv_composition_ByName().c_str());
		return 1;
	}

	static int Bind_PCTemplate_selectPhases(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 1,
			" Please set a temperature value ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		if(pointerPixel->select_phases(parameters) != 0) 
		{
			lua_pushstring(state, "Phase does not exist");
			return 1;
		}

		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PCTemplate_getSelectPhases(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 0,
			" you are golden! ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		lua_pushstring(state, IAM_Database::csv_join_row(pointerPixel->get_selected_phases_ByName(),IAM_Database::Delimiter).c_str());
		return 1;
	}

	// Equilibrium config
	static int Bind_PCTemplate_setEquilibrimTemperatureRange(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Input has to specify two components Start Temperature and End Temperature ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		pointerPixel->set_equilibrium_config_startTemperature(std::stold(parameters[0]));
		pointerPixel->set_equilibrium_config_endTemperature(std::stold(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}


	// Scheil config
	static int Bind_PCTemplate_setScheilTemperatureRange(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Input has to specify two components Start Temperature and End Temperature ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		pointerPixel->set_scheil_config_startTemperature(std::stold(parameters[0]));
		pointerPixel->set_scheil_config_endTemperature(std::stold(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PCTemplate_setScheilLiquidFraction(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 1,
			" Input has to specify two components Start Temperature and End Temperature ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		pointerPixel->set_scheil_config_minimumLiquidFraction(std::stold(parameters[0]));
		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PCTemplate_setScheilDependentPhase(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 1,
			" Input has to specify the name of the phase ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		DBS_Phase tempPhase(_dbFramework->get_database(), -1);
		tempPhase.load_by_name(parameters[0]);

		if(tempPhase.id() == -1)
		{
			lua_pushstring(state, "Phase was not found");
			return 1;
		}

		pointerPixel->set_scheil_config_dependentPhase(tempPhase.id());
		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PCTemplate_setScheilStepSize(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 1,
			" Input has to specify the step size ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		pointerPixel->set_scheil_config_stepSize(std::stold(parameters[0]));
		lua_pushstring(state, "OK");
		return 1;
	}

	

#pragma region Equilibrium
	static int Bind_PC_equilibrium_setStartTemperature(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
										   " Please set a temperature value ", 
										   parameters) != 0) return 1;


		AM_pixel_parameters* pointerPixel= _openProject->get_pixelCase(std::stoi(parameters[0]));
		pointerPixel->set_equilibrium_config_startTemperature(std::stold(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PC_equilibrium_setEndTemperature(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Please set a temperature value ",
			parameters) != 0) return 1;


		AM_pixel_parameters* pointerPixel = _openProject->get_pixelCase(std::stoi(parameters[0]));
		pointerPixel->set_equilibrium_config_endTemperature(std::stold(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}

#pragma endregion

#pragma region Scheil
	// TODO: add text here, remember all these functions require two parameters
	// IDCase and the value to change, also note it depends on the current openProject
	// this is more efficient since we do not load all data for each function call.
	// maybe we should think on a way to do function calls for specific, but first lets make it
	// wor andd the optmize ;) - note from myself to myself

	static int Bind_PC_scheil_setStartTemperature(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Please set a temperature value ",
			parameters) != 0) return 1;


		AM_pixel_parameters* pointerPixel = _openProject->get_pixelCase(std::stoi(parameters[0]));
		pointerPixel->set_scheil_config_startTemperature(std::stold(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PC_scheil_setEndTemperature(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Please set a temperature value ",
			parameters) != 0) return 1;


		AM_pixel_parameters* pointerPixel = _openProject->get_pixelCase(std::stoi(parameters[0]));
		pointerPixel->set_scheil_config_endTemperature(std::stold(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PC_scheil_setStepSize(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Please set a step size value ",
			parameters) != 0) return 1;


		AM_pixel_parameters* pointerPixel = _openProject->get_pixelCase(std::stoi(parameters[0]));
		pointerPixel->set_scheil_config_stepSize(std::stold(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PC_scheil_setDependentPhase_ByID(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Please set a step size value ",
			parameters) != 0) return 1;


		AM_pixel_parameters* pointerPixel = _openProject->get_pixelCase(std::stoi(parameters[0]));
		pointerPixel->set_scheil_config_dependentPhase(std::stoi(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PC_scheil_setDependentPhase_ByName(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Please set a step size value ",
			parameters) != 0) return 1;


		AM_pixel_parameters* pointerPixel = _openProject->get_pixelCase(std::stoi(parameters[0]));
		int outResult = pointerPixel->set_scheil_config_dependentPhase(parameters[1]);

		if(outResult != 0) 
		{
			lua_pushstring(state, "Phase does not exist or is not selected!");
			return 1;
		}

		lua_pushstring(state, "OK");
		return 1;
	}

	static int Bind_PC_scheil_setMinimumLiquidFraction(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 2,
			" Please set a step size value ",
			parameters) != 0) return 1;


		AM_pixel_parameters* pointerPixel = _openProject->get_pixelCase(std::stoi(parameters[0]));
		pointerPixel->set_scheil_config_minimumLiquidFraction(std::stold(parameters[1]));

		lua_pushstring(state, "OK");
		return 1;
	}
#pragma endregion

#pragma endregion

#pragma endregion



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