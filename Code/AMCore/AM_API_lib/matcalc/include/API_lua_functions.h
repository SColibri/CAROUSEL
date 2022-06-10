#pragma once

#include <string>
#include "../include/API_scripting.h"
#include "../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../AMLib/interfaces/IAM_lua_functions.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/interfaces/IAM_Database.h"
#include "API_matcalc_lib.h"

/** \addtogroup AM_API_lib
  *  @{
  */

class API_lua_functions: public IAM_lua_functions
{
public:

	API_lua_functions(lua_State* state);
	API_lua_functions(lua_State* state, AM_Config* configuration);
	~API_lua_functions();
	static void set_library(AM_Config* configuration);

private:
	inline static API_matcalc_lib* _api{nullptr};
	
	/// <summary>
	/// Registers the function's signature in lua 
	/// </summary>
	/// <param name="state"></param>
	virtual void add_functions_to_lua(lua_State* state) override;

#pragma region lua_functions
	/// <summary>
	/// This is just an example of how you should bind a function in lua
	/// so I'll call this baby lua.
	/// </summary>
	/// <param name="state"></param>
	/// <returns>number of parameters it outputs</returns>
	static int bind_hello_world(lua_State* state);

	/// <summary>
	/// This calls the run matcalc script, the parameter passed to lua has to be 
	/// the filename for the script
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_run_script(lua_State* state);

	/// <summary>
	/// runs specific commands using matcalc scripting commands
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_run_command(lua_State* state);

	/// <summary>
	/// Initializes matcalc core module, loads databases and all
	/// for initialization. This function also loads into the
	/// database all elements and phases, ref: bind_getElementNames_command
	/// and bind_getPhaseNames_command
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_initializeCore_command(lua_State* state);

	/// <summary>
	/// Obtains the elements available on a database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_getElementNames_command(lua_State* state);

	/// <summary>
	/// Obtains the elements available on a database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_getPhaseNames_command(lua_State* state);

	/// <summary>
	/// set the number of precipitation classes, 
	/// by default this is set to 25.
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_setValueNPC_command(lua_State* state);

	/// <summary>
	/// set path to thermodynamic database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_selectThermodynamicDatabase_command(lua_State* state);

	/// <summary>
	/// set path to physical database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_selectPhysicalDatabase_command(lua_State* state);

	/// <summary>
	/// set path to mobility database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_selectMobilityDatabase_command(lua_State* state);

	static int bind_selectPhases_command(lua_State* state);
	static int bind_calculateEquilibrium_command(lua_State* state);

#pragma region PixelCase
	/// <summary>
	/// Creates a new instance of AM_Project, thus it does not depend on the
	/// current open project. Be aware that loading a AM_Project might have
	/// a big overhead, use for single operations.
	/// Input Parameters: INT <IDProject> INT <IDCase>
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SPC_StepEquilibrium_ByProjectID(lua_State* state)
	{

		int noParameters = lua_gettop(state);
		if (noParameters < 2)
		{
			lua_pushstring(state, "usage: <INT project ID> <INT Case ID> ");
			return 1;
		}


		std::string idCase = lua_tostring(state, 2);
		std::string idProject = lua_tostring(state, 1);
		DBS_Project tempP(_dbFramework->get_database(), _openProject->get_project_ID());

		// Load the project, if id is -1, the project does not exist.
		AM_Project projectTemp(_dbFramework->get_database(), _configuration, std::stoi(idProject));
		if(projectTemp.get_project_ID() == -1)
		{
			lua_pushstring(state, "Selected IDProject does not exist!");
			return 1;
		}

		// If pixel_parameters is a null pointer, that means that either the case does not exist or
		// it corresponds to another project ID
		AM_pixel_parameters* pixel_parameters = projectTemp.get_pixelCase(std::stoi(idCase));
		if(pixel_parameters == nullptr)
		{
			lua_pushstring(state, "IDCase does not belong to the project or it does not exist!");
			return 1;
		}

		std::string outCommand_1 = runVectorCommands(API_Scripting::Script_run_stepEquilibrium(_configuration, 
																								pixel_parameters->get_EquilibriumConfiguration()->StartTemperature,
																								pixel_parameters->get_EquilibriumConfiguration()->EndTemperature,
																								projectTemp.get_selected_elements_ByName(),
																								pixel_parameters->get_composition_string(),
																								pixel_parameters->get_selected_phases_ByName() ));

		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}

	/// <summary>
	/// Based on the current openProject, runs a stepped equilibrium calculation
	/// input Parameters: INT <IDCase>
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SPC_StepEquilibrium(lua_State* state)
	{
		if(_openProject == nullptr)
		{
			lua_pushstring(state, "No open project is available, please create a new project or open an existing one ");
			return 1;
		}

		int noParameters = lua_gettop(state);
		if (noParameters < 1)
		{
			lua_pushstring(state, "usage: <INT Case ID> ");
			return 1;
		}

		std::string idCase = lua_tostring(state, 1);
		
		// If pixel_parameters is a null pointer, that means that either the case does not exist or
		// it corresponds to another project ID
		AM_pixel_parameters* pixel_parameters = _openProject->get_pixelCase(std::stoi(idCase));
		if (pixel_parameters == nullptr)
		{
			lua_pushstring(state, "IDCase does not belong to the project or it does not exist!");
			return 1;
		}

		std::string outCommand_1 = runVectorCommands(API_Scripting::Script_run_stepEquilibrium(_configuration,
			pixel_parameters->get_EquilibriumConfiguration()->StartTemperature,
			pixel_parameters->get_EquilibriumConfiguration()->EndTemperature,
			_openProject->get_selected_elements_ByName(),
			pixel_parameters->get_composition_string(),
			pixel_parameters->get_selected_phases_ByName()));

		std::string outCommand_2 = _api->MCRCommand(API_Scripting::script_buffer_listContent());
		int index_start_buffer = string_manipulators::find_index_of_keyword(outCommand_2, "Index");
		int index_end_buffer = string_manipulators::find_index_of_keyword(outCommand_2, "Exit");
		std::string buffer_raw = outCommand_2.substr(index_start_buffer, index_end_buffer - index_start_buffer -2);
		std::vector<std::string> bufferRowEntries = string_manipulators::split_text(buffer_raw, "\n");

		std::vector<DBS_EquilibriumPhaseFraction> tempPhaseFraction;
		std::vector<std::string> selectedPhases = pixel_parameters->get_selected_phases_ByName();
		std::vector<int> selectedPhases_id = pixel_parameters->get_selected_phases_ByID();
		for(int n1 = 0; n1 < selectedPhases.size(); n1++)
		{
			for (int n2 = 1; n2 < bufferRowEntries.size(); n2++) // start at 1 because off headers
			{
				std::vector<std::string> bufferCells = string_manipulators::split_text(bufferRowEntries[n2], " ");
				tempPhaseFraction.push_back(DBS_EquilibriumPhaseFraction(_dbFramework->get_database(), -1));
				tempPhaseFraction.back().IDCase = pixel_parameters->get_caseID();
				tempPhaseFraction.back().IDPhase = selectedPhases_id[n1];

				if (bufferCells.size() < 2) continue;
				tempPhaseFraction.back().Temperature = std::stold(bufferCells[1]);

				std::string outCommand_loadState = _api->MCRCommand(API_Scripting::script_buffer_loadState(n2-1));
				std::string outCommand_statusPhase = _api->MCRCommand(API_Scripting::script_buffer_getPhaseStatus(selectedPhases[n1]));
				
				int tokenPhase = string_manipulators::find_index_of_keyword(outCommand_statusPhase, "mole-fraction in system:");
				std::string bufferPhase_raw = outCommand_statusPhase.substr(tokenPhase);
				std::vector<std::string> splitRaw = string_manipulators::split_text(bufferPhase_raw, ":");
				if (splitRaw.size() < 2) continue;
				std::vector<std::string> splitRaw_value = string_manipulators::split_text(splitRaw[1], "\n");
				tempPhaseFraction.back().Value = std::stold(splitRaw_value[0]);
				tempPhaseFraction.back().save();
			}
		}
		

		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}


	static int Bind_SPC_StepScheil(lua_State* state)
	{
		

		lua_pushstring(state, "Not implemented");
		return 1;
	}

	/// <summary>
	/// This will run all pending cases in current project
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SPC_run_cases(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 0,
			" No input required ",
			parameters) != 0) return 1;
		
		// we have to run all configurations
		std::vector<std::string> out;
		for (int n1 = 0; n1 < _openProject->get_singlePixel_Cases().size(); n1++)
		{
			// TODO: do this a nicer way if you get the chance, I noticed you don't sleep
			// and now you are taling to a machine.
			// do step equilibrium
			lua_getglobal(state, "pixelcase_stepEquilibrium");
			lua_pushstring(state, std::to_string(_openProject->get_singlePixel_Cases()[n1]->get_caseID()).c_str());
			lua_pcall(state, 1, 1, 0);
			out.push_back(lua_tostring(state,-1));
			lua_pop(state, 1);

			lua_getglobal(state, "pixelcase_stepScheil");
			lua_pushstring(state, std::to_string(_openProject->get_singlePixel_Cases()[n1]->get_caseID()).c_str());
			lua_pcall(state, 1, 1, 0);
			out.push_back(lua_tostring(state, -1));
			lua_pop(state, 1);
		}

		lua_pushstring(state, "OK");
		return 1;
	}
#pragma endregion

#pragma endregion

#pragma region helpers
	/// <summary>
	/// runs commands contained by a vector, one by one.
	/// </summary>
	/// <param name="parameter"></param>
	/// <returns></returns>
	static std::string runVectorCommands(std::vector<std::string> parameter);
#pragma endregion

};
/** @}*/
