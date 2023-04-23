#pragma once

// c++
#include <string>
#include <thread>
#include <mutex>
#include <codecvt>

// Core
#include "../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../AMLib/include/Database_implementations/Data_triggers/DBSTrigger_ActivePhases.h"
#include "../../../AMLib/interfaces/IAM_lua_functions.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/interfaces/IAM_Database.h"
#include "../../../AMLib/include/AM_Threading.h"
#include "../../../AMLib/exceptions/CALPHADException.h"
#include "ScriptingCommands/ScriptingDoSolidification.h"

// Core - windows
#include "../../../AMLib/x_Helpers/IPC_winapi.h"

// local
#include "../include/API_scripting.h"
#include "API_matcalc_lib.h"
#include "../include/Commands/COMMAND_ALL.h"
#include "Calculations/CALCULATION_ALL.h"
#include "CalculationsThreadJob.h"


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
	inline static std::mutex _mutex;

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
	/// closes matcalc core
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_exit(lua_State* state);

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
	/// Returns Database element list
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_DatabaseElementNames_command(lua_State* state);

	/// <summary>
	/// Obtains the elements available on a database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_getElementNames_command(lua_State* state);

	/// <summary>
	/// get phases available in the database
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_DatabasePhaseNames_command(lua_State* state);

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

	/// <summary>
	/// Select phases for calculations
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_selectPhases_command(lua_State* state);

	/// <summary>
	/// calculate equilibrium
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_calculateEquilibrium_command(lua_State* state);


#pragma region MatCalc

	static int bind_matcalc_initializeCore(lua_State* state)
	{
		std::string out = _api->APIcommand(API_Scripting::script_initialize_core());

		lua_pushstring(state, out.c_str());
		return 1;
	}

	static int bind_matcalc_setWorkingDirectory(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 0, "No path added");
		std::vector<std::string> parameters = get_parameters(state);

		if (parameters.size() > 0)
			_configuration->set_working_directory(IAM_Database::csv_join_row(parameters, " "));

		std::string out = _api->APIcommand(API_Scripting::script_set_working_directory(_configuration));

		lua_pushstring(state, out.c_str());
		return 1;
	}

	static int bind_matcalc_openThermodynamicDatabase(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 0, "you are golden");
		std::vector<std::string> parameters = get_parameters(state);

		if(parameters.size() > 0)
			_configuration->set_ThermodynamicDatabase_path(IAM_Database::csv_join_row(parameters, " "));
		
		std::string out = _api->APIcommand(API_Scripting::script_set_thermodynamic_database(_configuration->get_ThermodynamicDatabase_path()));

		lua_pushstring(state, out.c_str());
		return 1;
	}

	static int bind_matcalc_selectElements(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 0, "you are golden");
		std::vector<std::string> parameters = get_parameters(state);

		if (parameters.size() > 0)
			_openProject->set_selected_elements_ByName(parameters);

		std::string out = _api->APIcommand(API_Scripting::Script_selectElements(_openProject->get_selected_elements_ByName()));

		lua_pushstring(state, out.c_str());
		return 1;
	}

	static int bind_matcalc_selectPhases(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 1, "Please select at least one phase");
		std::vector<std::string> parameters = get_parameters(state);

		std::string out = _api->APIcommand(API_Scripting::Script_selectPhases(parameters));

		lua_pushstring(state, out.c_str());
		return 1;
	}

	static int bind_matcalc_readThermodynamicDatabase(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 0, "you are golden");
		std::vector<std::string> parameters = get_parameters(state);

		std::string out = _api->APIcommand(API_Scripting::Script_readThermodynamicDatabase());

		lua_pushstring(state, out.c_str());
		return 1;
	}

	static int bind_matcalc_setReferenceElement(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 1, "Please add a reference element");
		std::vector<std::string> parameters = get_parameters(state);

		std::string out = _api->APIcommand(API_Scripting::Script_setReferenceElement(parameters[0]));

		lua_pushstring(state, out.c_str());
		return 1;
	}

	static int bind_matcalc_setStepOption(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 1, "Please add a reference element");
		std::vector<std::string> parameters = get_parameters(state);

		std::string out = _api->APIcommand(API_Scripting::Script_setStepOptions(IAM_Database::csv_join_row(parameters, " ")));

		lua_pushstring(state, out.c_str());
		return 1;
	}

	static int bind_matcalc_stepEquilibrium(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 0, "you are golen!");
		std::vector<std::string> parameters = get_parameters(state);

		std::string out = _api->APIcommand(API_Scripting::Script_stepEquilibrium());

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Obtains the phase cumulative fraction for the equilibrium calculations
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_matcalc_buffer_getEquilibriumPhaseFraction(lua_State* state)
	{
		// we require at least one phase.
		check_parameters(state, lua_gettop(state), 1, "please add a phase");
		std::vector<std::string> parameters = get_parameters(state);

		// construct for string format all double entries
		std::string formatVariable = string_manipulators::get_string_format_numeric_generic(parameters.size(), "%g", ",");

		// set matcalc variable name
		std::string varName = "EquilibriumAM";

		// create a variable in matcalc using string format
		std::string sfv = _api->APIcommand(API_Scripting::script_format_variable_string(varName,
																						formatVariable, 
																						API_Scripting::script_get_phase_equilibrium_variable_name(parameters)));
		// get string by printing to console
		std::string out = _api->APIcommand(API_Scripting::print_variable_to_console(varName));
		out = string_manipulators::ltrim_byToken(out, " = ");
		out = string_manipulators::rtrim_byToken(out, " ");

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Obtains the cumulative fraction for the scheil equilibrium calculations
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_matcalc_buffer_getScheilPhaseFraction(lua_State* state)
	{
		// we require at least one phase.
		check_parameters(state, lua_gettop(state), 1, "please add a phase");
		std::vector<std::string> parameters = get_parameters(state);

		// construct for string format all double entries
		std::string formatVariable = string_manipulators::get_string_format_numeric_generic(parameters.size(), "%g", ",");

		// set matcalc variable name
		std::string varName = "EquilibriumAM";

		// create a variable in matcalc using string format
		std::string sfv = _api->APIcommand(API_Scripting::script_format_variable_string(varName,
																						formatVariable,
																						API_Scripting::script_get_phase_equilibrium_scheil_variable_name(parameters)));
		// get string by printing to console
		std::string out = _api->APIcommand(API_Scripting::print_variable_to_console(varName));

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// matcalc function returns buffer content
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_matcalc_buffer_listContent(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 0, "you are golden");

		std::string out;
		try
		{
			out = matcalc_buffer_listContent();
		}
		catch (const std::exception& e)
		{
			out = e.what();
		}
		
		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Gets the string segment corresponding to the element and phase list from Matcalc
	/// </summary>
	/// <param name="mccComm"></param>
	/// <returns></returns>
	static std::string matcalc_buffer_listContent(IPC_winapi* mccComm = nullptr) 
	{
		std::string out{""};
		if(mccComm == nullptr)
			out = _api->APIcommand(API_Scripting::script_buffer_listContent());
		else
			out = _api->APIcommand(API_Scripting::script_buffer_listContent(), mccComm);

		int index_start_buffer = string_manipulators::find_index_of_keyword(out, "Index");
		int index_end_buffer = string_manipulators::find_index_of_keyword(out, "MC:");

		int index_end = index_end_buffer - index_start_buffer - 2;

		// Check if index is inside bounds
		if (index_start_buffer < index_end ||
			index_start_buffer > out.length() ||
			index_end > out.length() ||
			index_end - index_start_buffer <= 0) 
		{
			std::string errorMessage = "Invalid matcalc response response: " + out;
			AMFramework::Callback::ErrorCallback::TriggerCallback(&out[0]);
			throw AMFramework::Exceptions::CALPHADException(errorMessage);
		}

		return out.substr(index_start_buffer, index_end_buffer - index_start_buffer - 2);
	}

	/// <summary>
	/// Clears the buffer content
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int bind_matcalc_buffer_clear(lua_State* state)
	{
		check_parameters(state, lua_gettop(state), 1, "please add the buffer name");
		std::vector<std::string> parameters = get_parameters(state);

		std::string out = _api->APIcommand(API_Scripting::script_buffer_clear(parameters[0]));
		lua_pushstring(state, out.c_str());
		return 1;
	}

#pragma endregion

#pragma region Project

	/// <summary>
	/// Based on the current openProject, runs a stepped equilibrium calculation
	/// input Parameters: INT <IDCase>
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_get_active_phases_scheil(lua_State* state)
	{
		//check for input
		if (check_parameters(state, lua_gettop(state), 1, "usage <ID Project>") != 0) return 1; 
		
		// get parameters
		std::vector<std::string> parameters = get_parameters(state);

		// create new Matcalc connection instance
		std::wstring externalPath = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(_configuration->get_apiExternal_path() + "/mcc.exe");
		IPC_winapi* mcc_comms = new IPC_winapi(externalPath);

		// Load phases
		runVectorCommands(std::vector<string>{API_Scripting::script_initialize_core(),
											  API_Scripting::script_set_thermodynamic_database(_configuration->get_ThermodynamicDatabase_path()),}, mcc_comms);

		std::string phaseNames = read_matcalc_database_phaseNames(mcc_comms);
		std::vector<std::string> listPhases = string_manipulators::split_text(phaseNames, "\n"); // select all available phases
		listPhases.erase(listPhases.begin(), listPhases.begin() + 2);
		long int idProject = std::stoi(parameters[0]);

		AM_Project ProjectData(_dbFramework->get_database(), _configuration, idProject);
		ProjectData.refresh_data();

		// Remove previous data
		TRIGGERS::DBSTriggers_ActivePhases::remove_project_data(_dbFramework->get_database(), idProject);

		// run calulations
		std::string outCommand_1 = runVectorCommands(API_Scripting::Script_run_stepScheilEquilibrium(_configuration,
			700,
			25,
			0.01,
			ProjectData.get_selected_elements_ByName(),
			ProjectData.get_activePhases_ElementComposition(),
			listPhases), mcc_comms);

		int Index_Phases_S = string_manipulators::find_index_of_keyword(outCommand_1, "phases: ");
		int Index_Phases_E = string_manipulators::find_index_of_keyword(outCommand_1, " linking");
		std::string phasesSelected_string = outCommand_1.substr(Index_Phases_S, Index_Phases_E - Index_Phases_S);
		std::vector<std::string> phasesSelected_list = string_manipulators::split_text(phasesSelected_string," ");
		phasesSelected_list.erase(phasesSelected_list.begin());

		int Index_APhases_S = string_manipulators::find_index_of_keyword(outCommand_1, "Searching initial equilibrium ...");
		int Index_APhases_E = string_manipulators::find_index_of_keyword(outCommand_1, " fraction of phase");
		std::string ActivePhases_string = outCommand_1.substr(Index_APhases_S, Index_APhases_E - Index_APhases_S);
		std::vector<std::string> ActivePhases_rows = string_manipulators::split_text(ActivePhases_string,"\n");
		listPhases.clear();

		for(auto& item: ActivePhases_rows)
		{
			std::vector<std::string> ActivePhases_cells = string_manipulators::split_text(item, ",");
			if (ActivePhases_cells.size() != 6) continue;
			std::vector<std::string> row_phases = string_manipulators::split_text(ActivePhases_cells[5], " ");

			for(int n1 = 0; n1 < row_phases.size(); n1++)
			{
				if(std::find(listPhases.begin(), listPhases.end(), string_manipulators::trim_whiteSpace(row_phases[n1])) == listPhases.end())
				{
					listPhases.push_back(string_manipulators::trim_whiteSpace(row_phases[n1]));
				}
			}

		}

		// This was not possible because of the number of phases we called, thus we obtain active phases by looking into the IPC ouput of
		// the calculated equilibrium
		//std::vector<std::string> BufferRows = read_matcalc_calcphase_buffer(bufferRowEntries.size() - 1, API_Scripting::script_get_phase_equilibrium_scheil_variable_name(listPhases), mcc_comms);

		// Store found values
		std::vector<IAM_DBS*> activePhases;
		for(int n1 = 0; n1 < listPhases.size(); n1++)
		{
			DBS_Phase tempPhase(_dbFramework->get_database(), -1);
			tempPhase.load_by_name(listPhases[n1]);

			DBS_ActivePhases* tempAP = new DBS_ActivePhases(_dbFramework->get_database(), -1);
			tempAP->IDPhase = tempPhase.id();
			tempAP->IDProject = idProject;

			activePhases.push_back(tempAP);
		}

		// save in database and remove from memory
		int resp = IAM_DBS::save(activePhases);
		for (int n1 = 0; n1 < activePhases.size(); n1++)
		{
			delete activePhases[n1];
		}
		activePhases.clear();


		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}

#pragma endregion

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
		if (check_parameters(state, lua_gettop(state), 1, "usage: <INT Case ID>") != 0) return 1;
		std::vector<std::string> parameters = get_parameters(state);

		// clear buffer before starting
		std::string outClearBuffer = run_command(state, "matcalc_buffer_clear", std::vector<std::string> {"_default_"});
		
		// If pixel_parameters is a null pointer, that means that either the case does not exist or
		// it corresponds to another project ID
		DBS_Case tempCase(_dbFramework->get_database(),std::stoi(parameters[0]));
		tempCase.load();

		if (_openProject == nullptr)
		{
			run_command(state, "project_loadID", std::vector<std::string> {std::to_string(tempCase.IDProject)});
		}

		AM_pixel_parameters* pixel_parameters = _openProject->get_pixelCase(std::stoi(parameters[0]));
		if (pixel_parameters == nullptr)
		{
			lua_pushstring(state, "Error: IDCase does not belong to the project or it does not exist!");
			return 1;
		}

		// Initialize case
		//TODO: When no selected elements, gives Error, catch this one
		std::string outCommand_1 = runVectorCommands(API_Scripting::Script_run_stepEquilibrium(_configuration,
			pixel_parameters->get_EquilibriumConfiguration()->StartTemperature,
			pixel_parameters->get_EquilibriumConfiguration()->EndTemperature,
			_openProject->get_selected_elements_ByName(),
			pixel_parameters->get_composition_string(),
			pixel_parameters->get_selected_phases_ByName()));

		// Get buffer content
		std::string buffer_raw = run_command(state, "matcalc_buffer_listContent");
		std::vector<std::string> bufferRowEntries = string_manipulators::split_text(buffer_raw, "\n");
		if (bufferRowEntries.size() < 2) 
		{
			std::string ErrorOut = "Error: Equilibrium calculation was not possible -> " + outCommand_1;
			lua_pushstring(state, ErrorOut.c_str());
			return 1;
		}

		// pixel parameters
		std::vector<std::string> selectedPhases = pixel_parameters->get_selected_phases_ByName();
		std::vector<int> selectedPhases_id = pixel_parameters->get_selected_phases_ByID();
		std::vector<IAM_DBS*> tempPhaseFraction((bufferRowEntries.size() - 1)* selectedPhases.size());

		// Initialize all phase fractions
		for (size_t n1 = 0; n1 < tempPhaseFraction.size(); n1++)
			tempPhaseFraction[n1] = new DBS_EquilibriumPhaseFraction(_dbFramework->get_database(), -1);

		// store buffer into 
		std::vector<std::string> BufferRows = read_matcalc_calcphase_buffer(bufferRowEntries.size() - 1, API_Scripting::script_get_phase_equilibrium_variable_name(selectedPhases));
		
		// copy data to database objects
		for (int n1 = 0; n1 < BufferRows.size(); n1++)
		{
			std::vector<std::string> phaseValues = string_manipulators::split_text(BufferRows[n1], ",");

			for (int n2 = 1; n2 < phaseValues.size(); n2++)
			{
				int indexPhase = (n1) * selectedPhases.size() + (n2 - 1);
				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->IDCase = pixel_parameters->get_caseID();
				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->IDPhase = selectedPhases_id[n2-1];
				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->Temperature = std::stold(phaseValues[0]);
				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->Value = std::stold(phaseValues[n2]);
			}
		}

		// save in database and remove from memory
		int resp = IAM_DBS::save(tempPhaseFraction);
		for (int n1 = 0; n1 < tempPhaseFraction.size(); n1++)
		{
			delete tempPhaseFraction[n1];
		}
		

		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}
	static int Bind_SPC_Parallel_StepEquilibrium(lua_State* state)
	{
		// Check and get parameters
		if (check_parameters(state, lua_gettop(state), 2, "usage: <ID project> <ID Cases>") != 0) return 1;
		std::vector<std::string> parameters = get_parameters(state);

		// Initialize AM_project object, this is used for checking if cases belong to the project
		AM_Project Project(_dbFramework->get_database(),_configuration,std::stoi(parameters[0]));
		std::vector<AM_pixel_parameters*> pixel_parameters;

		// Get pointers for all cases
		std::vector<std::string> rangeIDCase = string_manipulators::split_text(parameters[1],"-");
		int start = std::stoi(rangeIDCase[0]);
		int end = std::stoi(rangeIDCase[1]);
		int range = end - start;

		// run equilibrium simulations
		APIMatcalc::ScriptingCommands::ScriptingDoSolidification::run_equilibrium_simulations(_dbFramework->get_database(), _configuration, std::stoi(parameters[0]), start, end);

		//for(int n1 = 0; n1 < range + 1; n1++)
		//{
		//	pixel_parameters.push_back(Project.get_pixelCase(start + n1));
		//	if(pixel_parameters[n1] == nullptr)
		//	{
		//		std::string ErrorOut = "Error: Selected ID case is not part of this project!";
		//		lua_pushstring(state, ErrorOut.c_str());
		//		return 1;
		//	}
		//}

		//// Create communication to mcc for each thread
		//std::vector<int> threadWorkload = AMThreading::thread_workload_distribution(_configuration->get_max_thread_number(), pixel_parameters.size());
		//std::wstring externalPath = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(_configuration->get_apiExternal_path() + "/mcc.exe");
		//std::vector<IPC_winapi*> mcc_comms;
		//for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		//{
		//	mcc_comms.push_back(new IPC_winapi(externalPath));
		//	mcc_comms[n1]->set_endflag("MC:");
		//	runVectorCommands(std::vector<string>{API_Scripting::script_initialize_core()}, mcc_comms[n1]);
		//}		

		//auto funcStep = [](IPC_winapi* mccComm, std::vector<AM_pixel_parameters*> PixelList, AM_Project* projectM )
		//{
		//	for(AM_pixel_parameters* pixel_parameters : PixelList)
		//	{
		//		std::string outCommand_1 = runVectorCommands(API_Scripting::Script_run_stepEquilibrium(_configuration,
		//			pixel_parameters->get_EquilibriumConfiguration()->StartTemperature,
		//			pixel_parameters->get_EquilibriumConfiguration()->EndTemperature,
		//			projectM->get_selected_elements_ByName(),
		//			pixel_parameters->get_composition_string(),
		//			pixel_parameters->get_selected_phases_ByName()), mccComm);

		//		// Get buffer content, check if buffer contains data 
		//		// before continuing
		//		std::string buffer_raw = matcalc_buffer_listContent(mccComm);
		//		std::vector<std::string> bufferRowEntries = string_manipulators::split_text(buffer_raw, "\n");
		//		if (bufferRowEntries.size() < 2) continue;

		//		// pixel parameters
		//		std::vector<std::string> selectedPhases = pixel_parameters->get_selected_phases_ByName();
		//		std::vector<int> selectedPhases_id = pixel_parameters->get_selected_phases_ByID();
		//		std::vector<IAM_DBS*> tempPhaseFraction((bufferRowEntries.size() - 1) * selectedPhases.size());

		//		// Initialize all phase fractions
		//		for (size_t n1 = 0; n1 < tempPhaseFraction.size(); n1++)
		//			tempPhaseFraction[n1] = new DBS_EquilibriumPhaseFraction(_dbFramework->get_database(), -1);

		//		// store buffer into 
		//		std::vector<std::string> BufferRows = read_matcalc_calcphase_buffer(bufferRowEntries.size() - 1, API_Scripting::script_get_phase_equilibrium_variable_name(selectedPhases), mccComm);

		//		// copy data to database objects
		//		for (int n1 = 0; n1 < BufferRows.size(); n1++)
		//		{
		//			std::vector<std::string> phaseValues = string_manipulators::split_text(BufferRows[n1], ",");

		//			for (int n2 = 1; n2 < phaseValues.size(); n2++)
		//			{
		//				int indexPhase = (n1)*selectedPhases.size() + (n2 - 1);
		//				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->IDCase = pixel_parameters->get_caseID();
		//				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->IDPhase = selectedPhases_id[n2 - 1];
		//				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->Temperature = std::stold(phaseValues[0]);
		//				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->Value = std::stold(phaseValues[n2]);
		//			}
		//		}

		//		// save in database and remove from memory
		//		_mutex.lock();
		//		int resp = IAM_DBS::save(tempPhaseFraction);
		//		for (int n1 = 0; n1 < tempPhaseFraction.size(); n1++)
		//		{
		//			delete tempPhaseFraction[n1];
		//		}
		//		_mutex.unlock();

		//		//mccComm->send_command("exit\r\n");
		//		//delete mccComm;

		//		//mccComm = new IPC_winapi(L"C:/Program Files/MatCalc 6/mcc.exe");
		//		//mccComm->set_endflag("MC:");
		//	}
		//};

		//int Index = 0;
		//std::vector<std::thread> threadList;
		//for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		//{
		//	std::vector<AM_pixel_parameters*> tempVector(pixel_parameters.begin() + Index, pixel_parameters.begin() + Index + threadWorkload[n1]);
		//	threadList.push_back(std::thread(funcStep, mcc_comms[n1], tempVector, &Project));
		//	Index += threadWorkload[n1];
		//}
		//	
		//for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		//{
		//	threadList[n1].join();
		//}

		//for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		//{
		//	mcc_comms[n1]->send_command("exit\r\n");
		//	delete mcc_comms[n1];
		//}
		//mcc_comms.clear();

		std::string outmsg = "OK";
		lua_pushstring(state, outmsg.c_str());
		return 1;
	}

	/// <summary>
	/// Based on the current openProject, runs a stepped equilibrium calculation
	/// input Parameters: INT <IDCase>
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SPC_StepScheil(lua_State* state)
	{
		if (check_parameters(state, lua_gettop(state), 1, "usage: <INT Case ID>") != 0) return 1;
		std::vector<std::string> parameters = get_parameters(state);

		// clear buffer before starting
		std::string outClearBuffer = run_command(state, "matcalc_buffer_clear", std::vector<std::string> {"_default_"});

		// If pixel_parameters is a null pointer, that means that either the case does not exist or
		// it corresponds to another project ID
		AM_pixel_parameters* pixel_parameters = _openProject->get_pixelCase(std::stoi(parameters[0]));
		if (pixel_parameters == nullptr)
		{
			lua_pushstring(state, "Error: IDCase does not belong to the project or it does not exist!");
			return 1;
		}

		// Initialize case
		std::string outCommand_1 = runVectorCommands(API_Scripting::Script_run_stepScheilEquilibrium(_configuration,
			pixel_parameters->get_ScheilConfiguration()->StartTemperature,
			pixel_parameters->get_ScheilConfiguration()->EndTemperature,
			pixel_parameters->get_scheil_config_StepSize(),
			_openProject->get_selected_elements_ByName(),
			pixel_parameters->get_composition_string(),
			pixel_parameters->get_selected_phases_ByName()));

		// Get buffer content
		std::string buffer_raw = run_command(state, "matcalc_buffer_listContent");
		std::vector<std::string> bufferRowEntries = string_manipulators::split_text(buffer_raw, "\n");
		if (bufferRowEntries.size() < 2)
		{
			std::string ErrorOut = "Error: Equilibrium calculation was not possible -> " + outCommand_1;
			lua_pushstring(state, ErrorOut.c_str());
			return 1;
		}

		// pixel parameters
		std::vector<std::string> selectedPhases = pixel_parameters->get_selected_phases_ByName();
		std::vector<int> selectedPhases_id = pixel_parameters->get_selected_phases_ByID();
		std::vector<IAM_DBS*> tempPhaseFraction((bufferRowEntries.size() - 1) * selectedPhases.size());
		std::vector<IAM_DBS*> tempPhaseCumulativeFraction((bufferRowEntries.size() - 1) * selectedPhases.size());

		for (size_t n1 = 0; n1 < tempPhaseFraction.size(); n1++)
		{
			tempPhaseFraction[n1] = new DBS_ScheilPhaseFraction(_dbFramework->get_database(), -1);
			tempPhaseCumulativeFraction[n1] = new DBS_ScheilCumulativeFraction(_dbFramework->get_database(), -1);
		}

		// store buffer into 
		std::vector<std::string> BufferRows = read_matcalc_calcphase_buffer(bufferRowEntries.size() - 1, API_Scripting::script_get_phase_equilibrium_scheil_variable_name(selectedPhases));

		// copy data to database objects
		for (int n1 = 0; n1 < BufferRows.size(); n1++)
		{
			std::vector<std::string> phaseValues = string_manipulators::split_text(BufferRows[n1], ",");

			for (int n2 = 1; n2 < phaseValues.size(); n2++)
			{
				int indexPhase = (n1)*selectedPhases.size() + (n2 - 1);
				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->IDCase = pixel_parameters->get_caseID();
				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->IDPhase = selectedPhases_id[n2 - 1];
				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->Temperature = std::stold(phaseValues[0]);
				((DBS_EquilibriumPhaseFraction*)tempPhaseFraction[indexPhase])->Value = std::stold(phaseValues[n2]);
			}
		}

		// cumulative fraction, save as table
		for (int n1 = 1; n1 < tempPhaseFraction.size(); n1++)
		{
			for (int n2 = 0; n2 < selectedPhases.size(); n2++)
			{
				((DBS_ScheilCumulativeFraction*)tempPhaseCumulativeFraction[n1])->Value = ((DBS_ScheilPhaseFraction*)tempPhaseFraction[n1])->Value + 
																						  ((DBS_ScheilCumulativeFraction*)tempPhaseCumulativeFraction[n1-1])->Value;
			}
		}

		// Save all entries
		tempPhaseCumulativeFraction.insert(tempPhaseCumulativeFraction.end(), tempPhaseFraction.begin(), tempPhaseFraction.end());
		
		_mutex.lock();
		int resp = IAM_DBS::save(tempPhaseCumulativeFraction);
		_mutex.unlock();
		for (int n1 = 0; n1 < tempPhaseFraction.size(); n1++)
		{
			delete tempPhaseFraction[n1];
			delete tempPhaseCumulativeFraction[n1];
		}

		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}
	static int Bind_SPC_Parallel_StepScheil(lua_State* state) 
	{

		// Check and get parameters
		if (check_parameters(state, lua_gettop(state), 2, "usage: <ID project> <ID Cases>") != 0) return 1;
		std::vector<std::string> parameters = get_parameters(state);

		// Initialize AM_project object, this is used for checking if cases belong to the project
		AM_Project Project(_dbFramework->get_database(), _configuration, std::stoi(parameters[0]));
		std::vector<AM_pixel_parameters*> pixel_parameters;

		// Get pointers for all cases
		std::vector<std::string> rangeIDCase = string_manipulators::split_text(parameters[1], "-");
		int start = std::stoi(rangeIDCase[0]);
		int end = std::stoi(rangeIDCase[1]);
		int range = end - start;

		// run scheil simulations, we use the ScriptingDoSolidification here because of time issues, this has to be finished soon. So 
		// refactoring is in order.
		// Note: to use scheil calculation from CALCULATIONS, you need to manage the jobs as done in ScriptingDoSolidification
		APIMatcalc::ScriptingCommands::ScriptingDoSolidification::run_scheil_simulations(_dbFramework->get_database(), _configuration, std::stoi(parameters[0]), start, end );

		//for (int n1 = 0; n1 < range + 1; n1++)
		//{
		//	pixel_parameters.push_back(Project.get_pixelCase(start + n1));
		//	if (pixel_parameters[n1] == nullptr)
		//	{
		//		std::string ErrorOut = "Error: Selected ID case is not part of this project!";
		//		lua_pushstring(state, ErrorOut.c_str());
		//		return 1;
		//	}
		//}

		//// Create communication to mcc for each thread
		//std::vector<int> threadWorkload = AMThreading::thread_workload_distribution(_configuration->get_max_thread_number(), pixel_parameters.size());
		//std::wstring externalPath = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(_configuration->get_apiExternal_path() + "/mcc.exe");
		//std::vector<IPC_winapi*> mcc_comms;
		//for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		//{
		//	mcc_comms.push_back(new IPC_winapi(externalPath));
		//	mcc_comms[n1]->set_endflag("MC:");
		//	runVectorCommands(std::vector<string>{API_Scripting::script_initialize_core()}, mcc_comms[n1]);
		//}

		//// define the lambda function
		//auto funcStep = [](IPC_winapi* mccComm, std::vector<AM_pixel_parameters*> PixelList, AM_Project* projectM)
		//{
		//	int IndexPixel = 0;
		//	for (AM_pixel_parameters* pixel_parameters : PixelList)
		//	{
		//		_luaBUFFER += "scheil calculation:  IDCase -> " + std::to_string(pixel_parameters->get_caseID()) + " current " + std::to_string(IndexPixel) + " / " + std::to_string(PixelList.size()); IndexPixel++;
		//		std::string outCommand_1 = runVectorCommands(API_Scripting::Script_run_stepScheilEquilibrium(_configuration,
		//			pixel_parameters->get_ScheilConfiguration()->StartTemperature,
		//			pixel_parameters->get_ScheilConfiguration()->EndTemperature,
		//			pixel_parameters->get_ScheilConfiguration()->StepSize,
		//			projectM->get_selected_elements_ByName(),
		//			pixel_parameters->get_composition_string(),
		//			pixel_parameters->get_selected_phases_ByName()), mccComm);

		//		// Get buffer content, check if buffer contains data 
		//		// before continuing
		//		std::string buffer_raw;
		//		std::vector<std::string> bufferRowEntries;
		//		
		//		try
		//		{
		//			buffer_raw = matcalc_buffer_listContent(mccComm);
		//			bufferRowEntries = string_manipulators::split_text(buffer_raw, "\n");

		//			if (bufferRowEntries.size() < 2)
		//			{
		//				std::string erroOut = "Error Matcalc, content output too short: " + buffer_raw;
		//				AMFramework::Callback::ErrorCallback::TriggerCallback(&erroOut[0]);
		//				continue;
		//			}
		//		}
		//		catch (const std::exception& e)
		//		{
		//			std::string erroOut = "Error running scheil simulation\n project id: " + std::to_string(projectM->get_project_ID()) + "\n Case: " + std::to_string(pixel_parameters->get_caseID()) + "\n" + e.what();
		//			AMFramework::Callback::ErrorCallback::TriggerCallback(&erroOut[0]);
		//			continue;
		//		}
		//		

		//		// pixel parameters
		//		std::vector<std::string> selectedPhases = pixel_parameters->get_selected_phases_ByName();
		//		std::vector<int> selectedPhases_id = pixel_parameters->get_selected_phases_ByID();
		//		std::vector<IAM_DBS*> tempPhaseFraction((bufferRowEntries.size() - 1) * selectedPhases.size());
		//		std::vector<IAM_DBS*> tempPhaseCumulativeFraction((bufferRowEntries.size() - 1) * selectedPhases.size());

		//		// Initialize all phase fractions
		//		for (size_t n1 = 0; n1 < tempPhaseFraction.size(); n1++)
		//		{
		//			tempPhaseFraction[n1] = new DBS_ScheilPhaseFraction(_dbFramework->get_database(), -1);
		//			tempPhaseCumulativeFraction[n1] = new DBS_ScheilCumulativeFraction(_dbFramework->get_database(), -1);
		//		}
		//			

		//		// store buffer into 
		//		std::vector<std::string> BufferRows = read_matcalc_calcphase_buffer(bufferRowEntries.size() - 1, API_Scripting::script_get_phase_equilibrium_scheil_variable_name(selectedPhases), mccComm);

		//		// copy data to database objects
		//		for (int n1 = 0; n1 < BufferRows.size(); n1++)
		//		{
		//			std::vector<std::string> phaseValues = string_manipulators::split_text(BufferRows[n1], ",");

		//			for (int n2 = 1; n2 < phaseValues.size(); n2++)
		//			{
		//				int indexPhase = (n1)*selectedPhases.size() + (n2 - 1);
		//				((DBS_ScheilPhaseFraction*)tempPhaseFraction[indexPhase])->IDCase = pixel_parameters->get_caseID();
		//				((DBS_ScheilPhaseFraction*)tempPhaseFraction[indexPhase])->IDPhase = selectedPhases_id[n2 - 1];
		//				((DBS_ScheilPhaseFraction*)tempPhaseFraction[indexPhase])->Temperature = std::stold(phaseValues[0]);
		//				((DBS_ScheilPhaseFraction*)tempPhaseFraction[indexPhase])->Value = std::stold(phaseValues[n2]);
		//			}
		//		}

		//		// cumulative fraction, save as table
		//		for (int n1 = 1; n1 < tempPhaseFraction.size(); n1++)
		//		{
		//			((DBS_ScheilCumulativeFraction*)tempPhaseCumulativeFraction[n1])->Value = 0;
		//			for (int n2 = 0; n2 < selectedPhases.size(); n2++)
		//			{
		//				((DBS_ScheilCumulativeFraction*)tempPhaseCumulativeFraction[n1])->Value = ((DBS_ScheilPhaseFraction*)tempPhaseFraction[n1])->Value +
		//					((DBS_ScheilCumulativeFraction*)tempPhaseCumulativeFraction[n1 - 1])->Value;
		//			}
		//		}

		//		// save in database and remove from memory
		//		//_mutex.lock();
		//		tempPhaseCumulativeFraction.insert(tempPhaseCumulativeFraction.end(), tempPhaseFraction.begin(), tempPhaseFraction.end());
		//		int resp = IAM_DBS::save(tempPhaseCumulativeFraction);
		//		for (int n1 = 0; n1 < tempPhaseFraction.size(); n1++)
		//		{
		//			delete tempPhaseFraction[n1];
		//			delete tempPhaseCumulativeFraction[n1];
		//		}
		//		
		//		if (_cancelCalculations) break;
		//	}
		//};


		//int Index = 0;
		//std::vector<std::thread> threadList;
		//for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		//{	
		//	std::vector<AM_pixel_parameters*> tempVector(pixel_parameters.begin() + Index, pixel_parameters.begin() + Index + threadWorkload[n1]);
		//	threadList.push_back(std::thread(funcStep, mcc_comms[n1], tempVector, &Project));
		//	Index += threadWorkload[n1];
		//}

		//for (int n1 = 0; n1 < threadList.size(); n1++)
		//{
		//	threadList[n1].join();
		//}

		//for (int n1 = 0; n1 < threadList.size(); n1++)
		//{
		//	mcc_comms[n1]->send_command("exit\r\n");
		//	delete mcc_comms[n1];
		//}
		//mcc_comms.clear();

		//std::string outCommand_1 = "OK";
		//if (_cancelCalculations) 
		//{
		//	Core_CancelExecution(state);
		//	_cancelCalculations = false;
		//	outCommand_1 = "Operation was cancelled";
		//}
		
		std::string outCommand_1 = "OK";
		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}

	/// <summary>
	/// This will run all pending cases in current project
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SPC_run_cases(lua_State* state)
	{
		_luaBUFFER = "";

		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 0,
			" No input required ",
			parameters) != 0) return 1;
		
		// we have to run all configurations
		std::vector<std::string> out;
		for (int n1 = 0; n1 < _openProject->get_singlePixel_Cases().size(); n1++)
		{
			// TODO: do this a nicer way if you get the chance, I noticed you don't sleep
			// and now you are talking to a machine.
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

	/// <summary>
	/// Calculate precipitate distribution
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SPC_parallel_calculate_precipitate_distribution(lua_State* state)
	{

		_luaBUFFER = "";

		// Check and get parameters
		if (check_parameters(state, lua_gettop(state), 2, "usage: <ID project> <ID Cases>") != 0) return 1;
		std::vector<std::string> parameters = get_parameters(state);

		// Initialize AM_project object, this is used for checking if cases belong to the project
		AM_Project Project(_dbFramework->get_database(), _configuration, std::stoi(parameters[0]));
		std::vector<AM_pixel_parameters*> pixel_parameters;

		// Get pointers for all cases
		std::vector<std::string> rangeIDCase = string_manipulators::split_text(parameters[1], "-");
		if (rangeIDCase.size() < 2) 
		{
			lua_pushstring(state, "Bind_SPC_parallel_calculate_precipitate_distribution: No range has been specified e.g. 1-1");
			return 1;
		}

		int start = std::stoi(rangeIDCase[0]);
		int end = std::stoi(rangeIDCase[1]);
		int range = end - start;

		for (int n1 = 0; n1 < range + 1; n1++)
		{
			pixel_parameters.push_back(Project.get_pixelCase(start + n1));
			if (pixel_parameters[n1] == nullptr)
			{
				std::string ErrorOut = "Error: Selected ID case is not part of this project!";
				lua_pushstring(state, ErrorOut.c_str());
				return 1;
			}
		}

		// Create communication to mcc for each thread
		std::vector<int> threadWorkload = AMThreading::thread_workload_distribution(_configuration->get_max_thread_number(), pixel_parameters.size());
		std::wstring externalPath = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(_configuration->get_apiExternal_path() + "/mcc.exe");
		std::vector<IPC_winapi*> mcc_comms;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			mcc_comms.push_back(new IPC_winapi(externalPath));
			mcc_comms[n1]->set_endflag("MC:");
			runVectorCommands(std::vector<string>{API_Scripting::script_initialize_core()}, mcc_comms[n1]);
		}

		// define the parallel function
		auto funcStep = [](IPC_winapi* mccComm, std::vector<AM_pixel_parameters*> PixelList, AM_Project* projectM)
		{
			int IndexPixel = 0;
			for (AM_pixel_parameters* pixel_parameters : PixelList)
			{
				_luaBUFFER += "precipitate distribution calculation:  IDCase -> " + std::to_string(pixel_parameters->get_caseID()) + " current " + std::to_string(IndexPixel) + " / " + std::to_string(PixelList.size()); IndexPixel++;
				if (pixel_parameters->get_precipitation_phases().size() == 0) continue;
				// create all script commands
				std::vector<std::string> ScriptInstructions = API_Scripting::Script_run_stepScheilEquilibrium(_configuration,
					pixel_parameters->get_ScheilConfiguration()->StartTemperature,
					pixel_parameters->get_ScheilConfiguration()->EndTemperature,
					pixel_parameters->get_ScheilConfiguration()->StepSize,
					projectM->get_selected_elements_ByName(),
					pixel_parameters->get_composition_string(),
					pixel_parameters->get_selected_phases_ByName());
				API_Scripting::Script_run_ScheilPrecipitation(_dbFramework->get_database(), ScriptInstructions, pixel_parameters->get_precipitation_phases(), _configuration->get_directory_path(AM_FileManagement::FILEPATH::TEMP));
				
				matcalc::CALCULATION_scheil calcScheil(_dbFramework->get_database(), mccComm, _configuration, pixel_parameters->get_ScheilConfiguration(), projectM, pixel_parameters);
				calcScheil.Calculate();
				calcScheil.Save_to_database(_dbFramework->get_database(), pixel_parameters);

				//Run script commands
				//std::string commandToString = IAM_Database::csv_join_row(ScriptInstructions, "\n");
				//std::string outCommand_1 = runVectorCommands(ScriptInstructions, mccComm);
				

				// Load csv file into database
				if (1 == 1) continue;
				for(auto& ItempPhase: pixel_parameters->get_precipitation_phases())
				{
					DBS_Phase tempPhaseName(_dbFramework->get_database(), ItempPhase->IDPhase);
					tempPhaseName.load();

					ItempPhase->PrecipitateDistribution = IAM_Database::csv_join_row(string_manipulators::read_file_to_end(_configuration->get_directory_path(AM_FileManagement::FILEPATH::TEMP) + "\\" + std::to_string(ItempPhase->id()) + "_" + tempPhaseName.Name + ".txt"),"\n");
					ItempPhase->save();
				}
				
				if (_cancelCalculations) { break; }
			}
		};


		int Index = 0;
		std::vector<std::thread> threadList;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			std::vector<AM_pixel_parameters*> tempVector(pixel_parameters.begin() + Index, pixel_parameters.begin() + Index + threadWorkload[n1]);
			threadList.push_back(std::thread(funcStep, mcc_comms[n1], tempVector, &Project));
			Index += threadWorkload[n1];
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			threadList[n1].join();
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			mcc_comms[n1]->send_command("exit\r\n");
			delete mcc_comms[n1];
		}
		mcc_comms.clear();

		std::string outCommand_1 = "OK";
		if (_cancelCalculations)
		{
			Core_CancelExecution(state);
			_cancelCalculations = false;
			outCommand_1 = "Operation was cancelled";
		}

		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}


	static int Bind_SPC_parallel_calculate_precipitate_distribution_V02(lua_State* state)
	{

		_luaBUFFER = "";

		// Check and get parameters
		if (check_parameters(state, lua_gettop(state), 2, "usage: <ID project> <ID Cases>") != 0) return 1;
		std::vector<std::string> parameters = get_parameters(state);

		// Initialize AM_project object, this is used for checking if cases belong to the project
		AM_Project Project(_dbFramework->get_database(), _configuration, std::stoi(parameters[0]));
		std::vector<AM_pixel_parameters*> pixel_parameters;

		// Get pointers for all cases
		std::vector<std::string> rangeIDCase = string_manipulators::split_text(parameters[1], "-");
		int start = std::stoi(rangeIDCase[0]);
		int end = std::stoi(rangeIDCase[1]);
		int range = end - start;

		for (int n1 = 0; n1 < range + 1; n1++)
		{
			pixel_parameters.push_back(Project.get_pixelCase(start + n1));
			if (pixel_parameters[n1] == nullptr)
			{
				std::string ErrorOut = "Error: Selected ID case is not part of this project!";
				lua_pushstring(state, ErrorOut.c_str());
				return 1;
			}
		}

		// Create communication to mcc for each thread
		std::vector<int> threadWorkload = AMThreading::thread_workload_distribution(_configuration->get_max_thread_number(), pixel_parameters.size());
		std::wstring externalPath = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(_configuration->get_apiExternal_path() + "/mcc.exe");
		std::vector<IPC_winapi*> mcc_comms;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			mcc_comms.push_back(new IPC_winapi(externalPath));
			mcc_comms[n1]->set_endflag("MC:");
			runVectorCommands(std::vector<string>{API_Scripting::script_initialize_core()}, mcc_comms[n1]);
		}

		_luaBUFFER = "";
		// define the parallel function
		auto funcStep = [](IPC_winapi* mccComm, std::vector<AM_pixel_parameters*> PixelList, AM_Project* projectM)
		{
			for (AM_pixel_parameters* pixel_parameters : PixelList)
			{
				matcalc::CALCULATION_scheilPrecipitation_distribution distP(_dbFramework->get_database(), mccComm, _configuration, pixel_parameters->get_ScheilConfiguration(), projectM, pixel_parameters);
				_luaBUFFER += distP.Calculate() + "\n\n---COMMANDS---\n\n\n";
				_luaBUFFER += distP.Get_script_text();
			}
		};


		int Index = 0;
		std::vector<std::thread> threadList;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			std::vector<AM_pixel_parameters*> tempVector(pixel_parameters.begin() + Index, pixel_parameters.begin() + Index + threadWorkload[n1]);
			threadList.push_back(std::thread(funcStep, mcc_comms[n1], tempVector, &Project));
			Index += threadWorkload[n1];
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			threadList[n1].join();
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			mcc_comms[n1]->send_command("exit\r\n");
			delete mcc_comms[n1];
		}
		mcc_comms.clear();

		std::string outCommand_1 = _luaBUFFER;
		if (_cancelCalculations)
		{
			Core_CancelExecution(state);
			_cancelCalculations = false;
			outCommand_1 = "Operation was cancelled";
		}
		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}

	static int Bind_SPC_parallel_calculate_heat_treatment(lua_State* state)
	{
		
		_luaBUFFER = "";

		// Check and get parameters
		if (check_parameters(state, lua_gettop(state), 3, "usage: <ID project> <ID Cases> <Heat treatment name>") != 0) return 1;
		std::vector<std::string> parameters = get_parameters(state);

		// Initialize AM_project object, this is used for checking if cases belong to the project
		AM_Project Project(_dbFramework->get_database(), _configuration, std::stoi(parameters[0]));
		std::vector<AM_pixel_parameters*> pixel_parameters;

		// Get pointers for all cases
		std::vector<std::string> rangeIDCase = string_manipulators::split_text(parameters[1], "-");
		int start = std::stoi(rangeIDCase[0]);
		int end = std::stoi(rangeIDCase[1]);
		int range = end - start;

		for (int n1 = 0; n1 < range + 1; n1++)
		{
			pixel_parameters.push_back(Project.get_pixelCase(start + n1));
			if (pixel_parameters[n1] == nullptr)
			{
				std::string ErrorOut = "Error: Selected ID case is not part of this project!";
				lua_pushstring(state, ErrorOut.c_str());
				return 1;
			}
		}

		// Create communication to mcc for each thread
		std::vector<int> threadWorkload = AMThreading::thread_workload_distribution(_configuration->get_max_thread_number(), pixel_parameters.size());
		std::wstring externalPath = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(_configuration->get_apiExternal_path() + "/mcc.exe");
		std::vector<IPC_winapi*> mcc_comms;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			mcc_comms.push_back(new IPC_winapi(externalPath));
			mcc_comms[n1]->set_endflag("MC:");
			runVectorCommands(std::vector<string>{API_Scripting::script_initialize_core()}, mcc_comms[n1]);
		}

		
		// define the parallel function
		auto funcStep = [](IPC_winapi* mccComm, std::vector<AM_pixel_parameters*> PixelList, AM_Project* projectM, std::string& HeatTreatmentName)
		{
			for (AM_pixel_parameters* pixel_parameters : PixelList)
			{
				DBS_HeatTreatment tempRef(_dbFramework->get_database(), -1);
				tempRef.load_by_name(HeatTreatmentName);

				if (pixel_parameters->get_precipitation_phases().size() == 0) continue;
				// create all script commands
				std::vector<std::string> ScriptInstructions;
				std::vector<std::string> FileList;
				API_Scripting::Script_run_heat_treatment(_dbFramework->get_database(), _configuration, HeatTreatmentName, ScriptInstructions, FileList);
				
				//Run script commands
				std::string commandToString = IAM_Database::csv_join_row(ScriptInstructions, "\n");
				std::string outCommand_1 = runVectorCommands(ScriptInstructions, mccComm);
				_luaBUFFER = outCommand_1;
				_luaBUFFER += "\n\n ----- COMMANDS -----\n\n" + commandToString;
				// Load csv file into database
				// "%12.2d  %12.6f  %12.6d" variable-name="StepValue t$c r_mean$AL3TI_L_P0"
				//DBS_HeatTreatment tempRef(_dbFramework->get_database(), -1);
				tempRef.load_by_name(HeatTreatmentName);

				AM_Database_Datatable phasesTable(_dbFramework->get_database(), &AMLIB::TN_PrecipitationPhase());
				phasesTable.load_data("IDCase = " + std::to_string(pixel_parameters->get_caseID()));

				if (phasesTable.row_count() == 0) continue;
				std::string stringFormat = "%12.2g %12.2f ";
				std::string variableName = "StepValue t$c ";

				std::vector<DBS_PrecipitationPhase> phaseList;
				for (int n1 = 0; n1 < phasesTable.row_count(); n1++)
				{
					phaseList.push_back(DBS_PrecipitationPhase(_dbFramework->get_database(), -1));
					phaseList.back().load(phasesTable.get_row_data(n1));

					stringFormat += "%12.2g %12.2g %12.2g ";
					variableName += "NUM_PREC$" + phaseList.back().Name + " ";
					variableName += "F_PREC$" + phaseList.back().Name + " ";
					variableName += "R_MEAN$" + phaseList.back().Name + " ";
				}

				int totalVariables = phasesTable.row_count() * 3 + 2; 

				// create script for loading data into text  file
				std::string loadscript = API_Scripting::Buffer_to_variable_V02(_configuration, stringFormat, variableName, "");
				std::string datafilename = loadscript;
				string_manipulators::replace_token(datafilename, ".mcs", ".Framework");

				// load from matcalc into textfile
				std::string getVariablesCommand = runVectorCommands(std::vector<std::string> {API_Scripting::script_runScript(loadscript)}, mccComm);

				// load from textfile
				if (!std::filesystem::exists(datafilename)) return;

				std::ifstream iStream(datafilename);
				
				std::string tempString;
				std::vector<IAM_DBS*> saveVector;
				int saveInterval = 100;
				while (std::getline(iStream, tempString)) {
					std::vector<std::string> tempVector = string_manipulators::split_text(tempString," ");
					string_manipulators::remove_empty_entries(tempVector);
					if (tempVector.size() != totalVariables) continue;
					int index = 0;

					DBS_HeatTreatmentProfile* newProfile = new DBS_HeatTreatmentProfile(_dbFramework->get_database(), -1);
					newProfile->IDHeatTreatment = tempRef.id();
					newProfile->Time = std::stod(tempVector[index++]);
					newProfile->Temperature = std::stod(tempVector[index++]);
					saveVector.push_back(newProfile);

					for (auto& item : phaseList)
					{
						DBS_PrecipitateSimulationData* phaseyTemp = new DBS_PrecipitateSimulationData(_dbFramework->get_database(), -1);
						phaseyTemp->IDHeatTreatment = tempRef.id();
						phaseyTemp->IDPrecipitationPhase = item.id();
						phaseyTemp->NumberDensity = std::stod(tempVector[index++]);
						phaseyTemp->PhaseFraction = std::stod(tempVector[index++]);
						phaseyTemp->MeanRadius = std::stod(tempVector[index++]);

						saveVector.push_back(phaseyTemp);
					}

					if (saveVector.size() > saveInterval)
					{
						IAM_DBS::save(saveVector);

						for (auto& item : saveVector) 
						{
							delete item;
						}

						saveVector.clear();
					}

				}

				if (saveVector.size() > 0)
				{
					IAM_DBS::save(saveVector);

					for (auto& item : saveVector)
					{
						delete item;
					}

					saveVector.clear();
				}

#pragma region TempFile_cleanup
				// clear temp files
				for (auto& item : FileList) 
				{
					try
					{
						filesystem::remove(item);
					}
					catch (const std::exception&)
					{
						// temp file was not possible to remove
					}
				}

				try
				{
					filesystem::remove(loadscript);
					filesystem::remove(datafilename);
				}
				catch (const std::exception&)
				{

				}
#pragma endregion

				if (_cancelCalculations) { break; }
			}
		};


		int Index = 0;
		std::vector<std::thread> threadList;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			std::vector<AM_pixel_parameters*> tempVector(pixel_parameters.begin() + Index, pixel_parameters.begin() + Index + threadWorkload[n1]);
			threadList.push_back(std::thread(funcStep, mcc_comms[n1], tempVector, &Project, parameters[2]));
			Index += threadWorkload[n1];
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			threadList[n1].join();
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			mcc_comms[n1]->send_command("exit\r\n");
			delete mcc_comms[n1];
		}
		mcc_comms.clear();

		std::string outCommand_1 = _luaBUFFER;
		if (_cancelCalculations)
		{
			Core_CancelExecution(state);
			_cancelCalculations = false;
			outCommand_1 = "Operation was cancelled";
		}
		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}


	static int Bind_SPC_parallel_calculate_heat_treatment_V02(lua_State* state)
	{

		_luaBUFFER = "";

		// Check and get parameters
		if (check_parameters(state, lua_gettop(state), 3, "usage: <ID project> <ID Cases> <Heat treatment name>") != 0) return 1;
		std::vector<std::string> parameters = get_parameters(state);

		// Initialize AM_project object, this is used for checking if cases belong to the project
		AM_Project Project(_dbFramework->get_database(), _configuration, std::stoi(parameters[0]));
		std::vector<AM_pixel_parameters*> pixel_parameters;

		// Get pointers for all cases
		std::vector<std::string> rangeIDCase = string_manipulators::split_text(parameters[1], "-");
		int start = std::stoi(rangeIDCase[0]);
		int end = std::stoi(rangeIDCase[1]);
		int range = end - start;

		for (int n1 = 0; n1 < range + 1; n1++)
		{
			pixel_parameters.push_back(Project.get_pixelCase(start + n1));
			if (pixel_parameters[n1] == nullptr)
			{
				std::string ErrorOut = "Error: Selected ID case is not part of this project!";
				lua_pushstring(state, ErrorOut.c_str());
				return 1;
			}
		}

		// Create communication to mcc for each thread
		std::vector<int> threadWorkload = AMThreading::thread_workload_distribution(_configuration->get_max_thread_number(), pixel_parameters.size());
		std::wstring externalPath = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(_configuration->get_apiExternal_path() + "/mcc.exe");
		std::vector<IPC_winapi*> mcc_comms;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			mcc_comms.push_back(new IPC_winapi(externalPath));
			mcc_comms[n1]->set_endflag("MC:");
			runVectorCommands(std::vector<string>{API_Scripting::script_initialize_core()}, mcc_comms[n1]);
		}

		_luaBUFFER = "";
		// define the parallel function
		auto funcStep = [](IPC_winapi* mccComm, std::vector<AM_pixel_parameters*> PixelList, AM_Project* projectM, std::string& HeatTreatmentName)
		{
			for (AM_pixel_parameters* pixel_parameters : PixelList)
			{
				DBS_HeatTreatment tempRef(_dbFramework->get_database(), -1);
				tempRef.load_by_name(HeatTreatmentName);

				matcalc::CALCULATION_scheilPrecipitation_heatTreatment hT_calc(_dbFramework->get_database(), mccComm, _configuration, projectM, pixel_parameters, &tempRef);
				_luaBUFFER += hT_calc.Calculate() + "\n\n---COMMANDS---\n\n\n";
				_luaBUFFER += hT_calc.Get_script_text();
			}
		};


		int Index = 0;
		std::vector<std::thread> threadList;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			std::vector<AM_pixel_parameters*> tempVector(pixel_parameters.begin() + Index, pixel_parameters.begin() + Index + threadWorkload[n1]);
			threadList.push_back(std::thread(funcStep, mcc_comms[n1], tempVector, &Project, parameters[2]));
			Index += threadWorkload[n1];
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			threadList[n1].join();
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			mcc_comms[n1]->send_command("exit\r\n");
			delete mcc_comms[n1];
		}
		mcc_comms.clear();

		std::string outCommand_1 = _luaBUFFER;
		if (_cancelCalculations)
		{
			Core_CancelExecution(state);
			_cancelCalculations = false;
			outCommand_1 = "Operation was cancelled";
		}
		lua_pushstring(state, outCommand_1.c_str());
		return 1;
	}

	static int Bind_SPC_run_all_Heat_treatments(lua_State* state)
	{
		_luaBUFFER = "";

		// Check and get parameters
		if (check_parameters(state, lua_gettop(state), 1, "usage: <ID project>") != 0) return 1;
		std::vector<std::string> parameters = get_parameters(state);

		// Initialize AM_project object, this is used for checking if cases belong to the project
		AM_Project Project(_dbFramework->get_database(), _configuration, std::stoi(parameters[0]));
		std::vector<AM_pixel_parameters*> pixel_parameters = Project.get_singlePixel_Cases();

		// Create communication to mcc for each thread
		std::vector<int> threadWorkload = AMThreading::thread_workload_distribution(_configuration->get_max_thread_number(), pixel_parameters.size());
		std::wstring externalPath = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(_configuration->get_apiExternal_path() + "/mcc.exe");
		std::vector<IPC_winapi*> mcc_comms;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			mcc_comms.push_back(new IPC_winapi(externalPath));
			mcc_comms[n1]->set_endflag("MC:");
			runVectorCommands(std::vector<string>{API_Scripting::script_initialize_core()}, mcc_comms[n1]);
		}

		_luaBUFFER = "";
		// define the parallel function
		auto funcStep = [](IPC_winapi* mccComm, std::vector<AM_pixel_parameters*> PixelList, AM_Project* projectM)
		{
			for (AM_pixel_parameters* pixel_parameters : PixelList)
			{
				std::vector<DBS_HeatTreatment* > htList = pixel_parameters->get_heat_treatments();

				for (DBS_HeatTreatment* ht : htList)
				{
					matcalc::CALCULATION_scheilPrecipitation_heatTreatment hT_calc(_dbFramework->get_database(), mccComm, _configuration, projectM, pixel_parameters, ht);
					std::string calcOut = hT_calc.Calculate();
					calcOut += "\n\n\n COMMANDS \n\n\n" + hT_calc.Get_script_text();

					char buffer[80];

					time_t rTime;
					struct tm* timeinfo;
					time(&rTime);
					timeinfo = localtime(&rTime);
					strftime(buffer, 80, "%H%M%S%m%Y", timeinfo);

					std::string filename = _configuration->get_working_directory() + "/Logs/HeatTreatment__run_kinetic_simulation_" + buffer + ht->Name + ".txt";
					string_manipulators::write_to_file(filename, calcOut);
				}
				
			}
		};

		int Index = 0;
		std::vector<std::thread> threadList;
		for (int n1 = 0; n1 < threadWorkload.size(); n1++)
		{
			std::vector<AM_pixel_parameters*> tempVector(pixel_parameters.begin() + Index, pixel_parameters.begin() + Index + threadWorkload[n1]);
			threadList.push_back(std::thread(funcStep, mcc_comms[n1], tempVector, &Project));
			Index += threadWorkload[n1];
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			threadList[n1].join();
		}

		for (int n1 = 0; n1 < threadList.size(); n1++)
		{
			mcc_comms[n1]->send_command("exit\r\n");
			delete mcc_comms[n1];
		}
		mcc_comms.clear();

		std::string outCommand_1 = _luaBUFFER;
		if (_cancelCalculations)
		{
			Core_CancelExecution(state);
			_cancelCalculations = false;
			outCommand_1 = "Operation was cancelled";
		}
		lua_pushstring(state, outCommand_1.c_str());
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
	static std::string runVectorCommands(std::vector<std::string> parameter, IPC_winapi* mcc_comm);

	static std::vector<std::string> read_matcalc_calcphase_buffer(int BufferSize, std::vector<std::string>& selectedPhases, IPC_winapi* mccComm = nullptr)
	{
		// store buffer into 
		std::string formatVariable = string_manipulators::get_string_format_numeric_generic(selectedPhases.size(), "%g", ",");
		std::string FORMATString = API_Scripting::script_format_variable_string("TempData", formatVariable, selectedPhases);
		std::string scriptFile = API_Scripting::Buffer_to_variable(std::to_string(BufferSize), FORMATString, "TempData", _configuration);
		
		// run script
		std::string run_;
		if(mccComm == nullptr)
		{
			run_ = _api->APIcommand(API_Scripting::script_runScript(scriptFile));
		}
		else
		{
			run_ = _api->APIcommand(API_Scripting::script_runScript(scriptFile), mccComm);
		}

		std::filesystem::remove(scriptFile);

		std::vector<std::string> BufferRows = string_manipulators::read_file_to_end(scriptFile.substr(0, scriptFile.size() - 4) + ".Framework");
		std::filesystem::remove(scriptFile.substr(0, scriptFile.size() - 4) + ".Framework");
		return BufferRows;
	}
	static std::string read_matcalc_database_phaseNames(IPC_winapi* comm) 
	{
		if (_configuration == nullptr) return "NONE";
		std::string commOut = runVectorCommands(API_Scripting::script_get_thermodynamic_database(_configuration), comm);
		size_t IndexPhases = string_manipulators::find_index_of_keyword(commOut, "# of phases in database");
		size_t IndexExitCode = string_manipulators::find_index_of_keyword(commOut.substr(IndexPhases, commOut.size() - IndexPhases), "MC:") + IndexPhases;

		std::string out;
		if (IndexPhases == std::string::npos || IndexExitCode == std::string::npos) out = "Error, data was not found!";
		else
		{
			out = commOut.substr(IndexPhases, IndexExitCode - IndexPhases);
		}

		return out;
	}
#pragma endregion

	static void Core_CancelExecution(lua_State* state)
	{
		std::string _exceptionString = "assert(false,\"Execution has been cancelled.\")";
		std::string outputCanel = run_command(state, _exceptionString);
	}

};
/** @}*/
