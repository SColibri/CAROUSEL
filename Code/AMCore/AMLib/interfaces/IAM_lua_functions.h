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
#include "../x_Helpers/IPC_winapi.h"
#include "../include/lua/LuaDatabaseModule.h"

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
		int (*newFunction)(lua_State*),
		std::string command_specs = "")
	{
		lua_register(state, function_name.c_str(), newFunction);
		add_to_definition(function_name, output, usage, command_specs);
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
	inline static std::vector<std::string> _functionCommandSpecs; // Command specifications (for gui)
	inline static std::string _luaBUFFER {""};
	inline static bool _cancelCalculations{ false };

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

		// ###-> LUA_GUI
		add_new_function(state, "msgbox", "string OK", "add_function_to_framework <Message>", Bind_lua_msgbox);

		// Database
		add_new_function(state, "database_tableQuery", "string, csv format, delimiter comma char", "database_tableQuery <tablename> <optional-where clause>", baseBind_DatabaseQuery);
		add_new_function(state, "database_table_custom_query", "string, csv format, delimiter comma char", "database_tableFullQuery <Full query>", baseBind_DatabaseByQuery);
		add_new_function(state, "database_create_view", "string, csv format, delimiter comma char", "database_create_view <Full query>", baseBind_DatabaseCreateView);
		add_new_function(state, "database_tableList", "string, csv format, delimiter comma char", "database_tableList", baseBind_DatabaseTableList);
		add_new_function(state, "database_rowNumbers", "int", "database_rowNumbers <TableName> <Where Query - optional>", baseBind_DatabaseRowNumber);

		//Data controller
		add_new_function(state, "dataController_csv", "string, csv format, delimiter comma char", "dataController_csv <enum::DATATABLES>", Bind_dataController_csv);

		//Project
		//------- Binded to open project
		
		add_new_function(state, "project_new", "string", "project_new <string Name>", Bind_project_new);

		add_new_function(state, "project_loadID_L", "string", "project_selectID <int>", Bind_project_loadID); // Deprecate
		add_new_function(state, "project_loadName", "string", "project_loadName <string>", Bind_project_loadName);
		add_new_function(state, "project_setName", "string", "project_setName <new name>", Bind_project_setName);
		add_new_function(state, "project_getData", "string csv format", "project_getData", Bind_project_getData);
		add_new_function(state, "project_setData", "string csv format", "project_setData <int ID>,<string Name>", Bind_project_setData);
		add_new_function(state, "project_selectElements", "string csv format", "project_SelectElements <string element1> (add all alements as parameters sepparated by a space char)", Bind_project_SelectElements);
		add_new_function(state, "project_getSelectedElements", "string csv format", "project_getSelectedElements", Bind_project_getSelectedElements);
		add_new_function(state, "project_clear_content", "string", "project_getSelectedElements <optional INT IDproject>", Bind_project_clearContent);
		add_new_function(state, "project_setReferenceElement", "string", "project_setReferenceElement <string>", Bind_project_setReferenceElement);
		add_new_function(state, "project_getReferenceElement", "string", "project_getReferenceElement", Bind_project_getReferenceElement);
		add_new_function(state, "project_getDataTableCases", "string", "project_getDataTableCases", Bind_project_getDataTableCases);

#pragma region Item_Data
		//###-> Project
#pragma region Project
		add_new_function(state, "project_save", "<INT> ID of new project", "project_save <string> csv format", Bind_Templated_Save<DBS_Project>, "Model_Projects||Save");
		add_new_function(state, "project_delete", "<String> OK", "Project_Delete <string> csv format", Bind_Templated_Delete<DBS_Project>, "Model_Projects||Delete");
		add_new_function(state, "project_loadID", "<String> OK", "project_loadID <string> csv format", Bind_Templated_loadID<DBS_Project>, "Model_Projects||LoadByID");
		add_new_function(state, "project_load_data", "<String> OK", "project_load_data <string> csv format", Bind_Project_LoadData, "Model_Projects||LoadInCore");
		add_new_function(state, "project_remove_dependentData", "<String> OK", "project_remove_dependentData <string> csv format", Bind_Project_Remove_projectData, "Model_Projects||RemoveDependentData");
#pragma endregion

		//###-> Active phases
#pragma region ActivePhases_SaveDelete
		add_new_function(state, "project_active_phases_save", "<ID> returns id of saved item", "project_active_phases_save <String> csv", Bind_Templated_Save<DBS_ActivePhases>,"Model_ActivePhases||Save");
		add_new_function(state, "project_active_phases_delete", "<string> OK", "project_active_phases_delete <String> csv", Bind_Templated_Delete<DBS_ActivePhases>, "Model_ActivePhases||Delete");
		add_new_function(state, "project_active_phases_loadID", "<ID> returns id of saved item", "project_active_phases_loadID <String> csv", Bind_Templated_loadID<DBS_ActivePhases>, "Model_ActivePhases||LoadByID");
		add_new_function(state, "project_active_phases_load_IDProject", "<ID> returns id of saved item", "project_active_phases_load_IDProject <String> csv", Bind_ActivePhases_Load_ByIDProject, "Model_ActivePhases||LoadByIDProject");

		add_new_function(state, "project_active_phases_element_composition_save", "<ID> returns id of saved item", "project_active_phases_element_composition_save <String> csv", Bind_Templated_Save<DBS_ActivePhases_ElementComposition>, "Model_ActivePhasesElementComposition||Save");
		add_new_function(state, "project_active_phases_element_composition_delete", "<string> OK", "project_active_phases_element_composition_delete <String> csv", Bind_Templated_Delete<DBS_ActivePhases_ElementComposition>, "Model_ActivePhasesElementComposition||Delete");
		add_new_function(state, "project_active_phases_element_composition_loadID", "<string> OK", "project_active_phases_element_composition_loadID <String> csv", Bind_Templated_loadID<DBS_ActivePhases_ElementComposition>, "Model_ActivePhasesElementComposition||LoadByID");
		add_new_function(state, "project_active_phases_element_composition_load_IDProject", "<ID> returns id of saved item", "project_active_phases_element_composition_load_IDProject <String> csv", Bind_ActivePhasesElementComposition_Load_ByIDProject, "Model_ActivePhasesElementComposition||LoadByIDProject");

		add_new_function(state, "project_active_phases_configuration_save", "<ID> returns id of saved item", "project_active_phases_configuration_save <String> csv", Bind_Templated_Save<DBS_ActivePhases_Configuration>, "Model_ActivePhasesConfiguration||Save");
		add_new_function(state, "project_active_phases_configuration_delete", "<string> ok", "project_active_phases_configuration_delete <String> csv", Bind_Templated_Delete<DBS_ActivePhases_Configuration>, "Model_ActivePhasesConfiguration||Delete");
		add_new_function(state, "project_active_phases_configuration_loadID", "<string> ok", "project_active_phases_configuration_loadID <String> csv", Bind_Templated_loadID<DBS_ActivePhases_Configuration>, "Model_ActivePhasesConfiguration||LoadByID");
		add_new_function(state, "project_active_phases_configuration_load_IDProject", "<ID> returns id of saved item", "project_active_phases_configuration_load_IDProject <String> csv", Bind_ActivePhasesConfiguration_Load_ByIDProject, "Model_ActivePhasesConfiguration||LoadByIDProject");

#pragma endregion

		//###-> Phases
#pragma region Phases
		add_new_function(state, "phase_save", "<string> OK", "phase_save <INT> ID", Bind_Templated_Save<DBS_Phase>,"Model_Phase||Save");
		add_new_function(state, "phase_delete", "<string> OK", "phase_delete <INT> ID", Bind_Templated_Delete<DBS_Phase>, "Model_Phase||Delete");
		add_new_function(state, "phase_loadID", "<string> OK", "phase_loadID <String> csv", Bind_Templated_loadID<DBS_Phase>, "Model_Phase||LoadByID");
		add_new_function(state, "phase_load_ByName", "<string> OK", "phase_load_ByName <String> csv", Bind_Phase_Load_ByName, "Model_Phase||LoadByName");
#pragma endregion

		//###-> Elements
#pragma region Elements 
		add_new_function(state, "element_save", "<string> OK", "element_save <INT> ID", Bind_Templated_Save<DBS_Element>, "Model_Element||Save");
		add_new_function(state, "element_delete", "<string> OK", "element_delete <INT> ID", Bind_Templated_Delete<DBS_Element>, "Model_Element||Delete");
		add_new_function(state, "element_loadID", "<string> OK", "element_loadID <String> csv", Bind_Templated_loadID<DBS_Element>, "Model_Element||LoadByID");
		add_new_function(state, "element_load_ByName", "<string> OK", "element_load_ByName <String> csv", Bind_Element_Load_ByName, "Model_Element||LoadByIDCase");
#pragma endregion

		//###-> Selected Phases
#pragma region SelectedPhases 
		add_new_function(state, "spc_selectedphase_save", "<string> OK", "spc_selectedphase_save <INT> ID", Bind_Templated_Save<DBS_SelectedPhases>, "Model_SelectedPhases||Save");
		add_new_function(state, "spc_selectedphase_delete", "<string> OK", "spc_selectedphase_delete <INT> ID", Bind_Templated_Delete<DBS_SelectedPhases>, "Model_SelectedPhases||Delete");
		add_new_function(state, "spc_selectedphase_load_id", "<string> OK", "spc_selectedphase_load_id <String> csv", Bind_Templated_loadID<DBS_SelectedPhases>, "Model_SelectedPhases||LoadByID");
		add_new_function(state, "spc_selectedphase_load_id_case", "<string> OK", "spc_selectedphase_load_id_case <String> csv", Bind_SelectedPhase_Load_CaseID, "Model_SelectedPhases||LoadByIDCase");
#pragma endregion

		//###-> Selected Elements
#pragma region SelectedElements
		add_new_function(state, "spc_selectedelement_save", "<string> OK", "spc_selectedelement_save <INT> ID", Bind_Templated_Save<DBS_SelectedElements>, "Model_SelectedElements||Save");
		add_new_function(state, "spc_selectedelement_delete", "<string> OK", "spc_selectedelement_delete <INT> ID", Bind_Templated_Delete<DBS_SelectedElements>, "Model_SelectedElements||Delete");
		add_new_function(state, "spc_selectedelement_load_id", "<string> OK", "spc_selectedelement_load_id <String> csv", Bind_Templated_loadID<DBS_SelectedElements>, "Model_SelectedElements||LoadByID");
		add_new_function(state, "spc_selectedelement_load_id_project", "<string> OK", "spc_selectedelement_load_id_project <String> csv", Bind_SelectedElement_Load_ProjectID, "Model_SelectedElements||LoadByIDProject");
#pragma endregion

		//###-> Selected Elements
#pragma region ElementComposition 
		add_new_function(state, "spc_elementcomposition_save", "<string> OK", "spc_elementcomposition_save <INT> ID", Bind_Templated_Save<DBS_ElementComposition>, "Model_ElementComposition||Save");
		add_new_function(state, "spc_elementcomposition_delete", "<string> OK", "spc_elementcomposition_delete <INT> ID", Bind_Templated_Delete<DBS_ElementComposition>, "Model_ElementComposition||Delete");
		add_new_function(state, "spc_elementcomposition_load_id", "<string> OK", "spc_elementcomposition_load_id <String> csv", Bind_Templated_loadID<DBS_ElementComposition>, "Model_ElementComposition||LoadByID");
		add_new_function(state, "spc_elementcomposition_load_id_case", "<string> OK", "spc_elementcomposition_load_id_project <String> csv", Bind_ElementComposition_Load_CaseID, "Model_ElementComposition||LoadByIDCase");
#pragma endregion

		//###-> Precipitation
#pragma region Precipitation_SaveDelete
		add_new_function(state, "spc_precipitation_domain_save", "<string> OK", "spc_precipitation_domain_save <INT> ID", Bind_Templated_Save<DBS_PrecipitationDomain>, "Model_PrecipitationDomain||Save");
		add_new_function(state, "spc_precipitation_domain_delete", "<string> OK", "spc_precipitation_domain_delete <INT> ID", Bind_Templated_Delete<DBS_PrecipitationDomain>, "Model_PrecipitationDomain||Delete");
		add_new_function(state, "spc_precipitation_domain_loadID", "<string> OK", "spc_precipitation_domain_loadID <INT> ID", Bind_Templated_loadID<DBS_PrecipitationDomain>, "Model_PrecipitationDomain||LoadByID");
		add_new_function(state, "spc_precipitation_domain_load_caseID", "<string> OK", "spc_precipitation_domain_load_caseID <INT> ID", Bind_PrecipitationDomain_Load_CaseID, "Model_PrecipitationDomain||LoadByIDCase");

		add_new_function(state, "spc_precipitation_phase_save", "<string> OK", "spc_precipitation_phase_save <INT> ID", Bind_Templated_Save<DBS_PrecipitationPhase>, "Model_PrecipitationPhase||Save");
		add_new_function(state, "spc_precipitation_phase_delete", "<string> OK", "spc_precipitation_phase_delete <INT> ID", Bind_Templated_Delete<DBS_PrecipitationPhase>, "Model_PrecipitationPhase||Delete");
		add_new_function(state, "spc_precipitation_phase_loadID", "<string> OK", "spc_precipitation_phase_loadID <INT> ID", Bind_Templated_loadID<DBS_PrecipitationPhase>, "Model_PrecipitationPhase||LoadByID");
		add_new_function(state, "spc_precipitation_phase_load_caseID", "<string> OK", "spc_precipitation_phase_load_caseID <INT> ID", Bind_PrecipitationPhase_Load_CaseID, "Model_PrecipitationPhase||LoadByIDCase");

		add_new_function(state, "spc_precipitation_simulation_data_save", "<string> OK", "spc_precipitation_simulation_data_save <INT> ID", Bind_Templated_Save<DBS_PrecipitateSimulationData>, "Model_PrecipitationSimulationData||Save");
		add_new_function(state, "spc_precipitation_simulation_data_delete", "<string> OK", "spc_precipitation_simulation_data_delete <INT> ID", Bind_Templated_Delete<DBS_PrecipitateSimulationData>, "Model_PrecipitationSimulationData||Delete");
		add_new_function(state, "spc_precipitation_simulation_data_loadID", "<string> OK", "spc_precipitation_simulation_data_loadID <INT> ID", Bind_Templated_loadID<DBS_PrecipitateSimulationData>, "Model_PrecipitationSimulationData||LoadByID");
		add_new_function(state, "spc_precipitation_simulation_data_HeatTreatmentID", "<string> OK", "spc_precipitation_simulation_data_HeatTreatmentID <INT> ID", Bind_PrecipitationSimulationData_Load_CaseID, "Model_PrecipitationSimulationData||LoadByIDHeatTreatment");
#pragma endregion

		//###-> Scheil
#pragma region Scheil
		// Configuration
		add_new_function(state, "spc_scheil_configuration_save", "<string> OK", "spc_scheil_configuration_save <INT> ID", Bind_Templated_Save<DBS_ScheilConfiguration>, "Model_ScheilConfiguration||Save");
		add_new_function(state, "spc_scheil_configuration_delete", "<string> OK", "spc_scheil_configuration_delete <INT> ID", Bind_Templated_Delete<DBS_ScheilConfiguration>, "Model_ScheilConfiguration||Delete");
		add_new_function(state, "spc_scheil_configuration_loadID", "<string> OK", "spc_scheil_configuration_loadID <INT> ID", Bind_Templated_loadID<DBS_ScheilConfiguration>, "Model_ScheilConfiguration||LoadByID");
		add_new_function(state, "spc_scheil_configuration_load_caseID", "<string> OK", "spc_scheil_configuration_load_caseID <String> csv", Bind_ScheilConfiguration_Load_CaseID, "Model_ScheilConfiguration||LoadByIDCase");
		
		// Phase fractions
		add_new_function(state, "spc_scheil_phasefraction_save", "<string> OK", "spc_scheil_phasefraction_save <INT> ID", Bind_Templated_Save<DBS_ScheilPhaseFraction>, "Model_ScheilPhaseFraction||Save");
		add_new_function(state, "spc_scheil_phasefraction_delete", "<string> OK", "spc_scheil_phasefraction_delete <INT> ID", Bind_Templated_Delete<DBS_ScheilPhaseFraction>, "Model_ScheilPhaseFraction||Delete");
		add_new_function(state, "spc_scheil_phasefraction_loadID", "<string> OK", "spc_scheil_phasefraction_loadID <String> csv", Bind_Templated_loadID<DBS_ScheilPhaseFraction>, "Model_ScheilPhaseFraction||LoadByID");
		add_new_function(state, "spc_scheil_phasefraction_load_caseID", "<string> OK", "spc_scheil_phasefraction_load_caseID <String> csv", Bind_ScheilPhaseFraction_Load_CaseID, "Model_ScheilPhaseFraction||LoadByIDCase");
#pragma endregion

		//###-> Equilibrium
#pragma region Equilibrium
		// Equilibrium configurations
		add_new_function(state, "spc_equilibrium_configuration_save", "<string> OK", "spc_equilibrium_configuration_save <INT> ID", Bind_Templated_Save<DBS_EquilibriumConfiguration>, "Model_EquilibriumConfiguration||Save");
		add_new_function(state, "spc_equilibrium_configuration_delete", "<string> OK", "spc_equilibrium_configuration_delete <INT> ID", Bind_Templated_Delete<DBS_EquilibriumConfiguration>, "Model_EquilibriumConfiguration||Delete");
		add_new_function(state, "spc_equilibrium_configuration_loadID", "<string> OK", "spc_equilibrium_configuration_loadID <INT> ID", Bind_Templated_loadID<DBS_EquilibriumConfiguration>, "Model_EquilibriumConfiguration||LoadByID");
		add_new_function(state, "spc_equilibrium_configuration_load_caseID", "<string> OK", "spc_equilibrium_configuration_load_caseID <String> csv", Bind_EquilibriumConfiguration_Load_CaseID, "Model_EquilibriumConfiguration||LoadByIDCase");

		//Equilibrium Phase fractions
		add_new_function(state, "spc_equilibrium_phasefraction_save", "<string> OK", "spc_equilibrium_phasefraction_save <INT> ID", Bind_Templated_Save<DBS_EquilibriumPhaseFraction>, "Model_EquilibriumPhaseFraction||Save");
		add_new_function(state, "spc_equilibrium_phasefraction_delete", "<string> OK", "spc_equilibrium_phasefraction_delete <INT> ID", Bind_Templated_Delete<DBS_EquilibriumPhaseFraction>, "Model_EquilibriumPhaseFraction||Delete");
		add_new_function(state, "spc_equilibrium_phasefraction_loadID", "<string> OK", "spc_equilibrium_phasefraction_loadID <String> csv", Bind_Templated_loadID<DBS_EquilibriumPhaseFraction>, "Model_EquilibriumPhaseFraction||LoadByID");
		add_new_function(state, "spc_equilibrium_phasefraction_load_caseID", "<string> OK", "spc_equilibrium_phasefraction_load_caseID <String> csv", Bind_EquilibriumPhaseFraction_Load_CaseID, "Model_EquilibriumPhaseFraction||LoadByIDCase");
#pragma endregion

		//###-> Case
#pragma region Case
		//Bind_Case_LoadÎD
		add_new_function(state, "spc_case_save", "<string> OK", "spc_case_save <String> csv", Bind_Templated_Save<DBS_Case>, "Model_Case||Save");
		add_new_function(state, "spc_case_load_id", "<string> csv", "spc_case_load_id <INT> ID Case", Bind_Templated_loadID<DBS_Case>, "Model_Case||LoadByID");
		add_new_function(state, "spc_case_delete", "<string> OK", "spc_case_delete <String> csv", Bind_Templated_Delete<DBS_Case>, "Model_Case||Delete");
		add_new_function(state, "spc_case_load_project_id", "<string> csv table", "spc_case_load_project_id <INT> ID project", Bind_Case_Load_ProjectID, "Model_Case||LoadByIDProject");
#pragma endregion

		//###-> Heat treatment
#pragma region HeatTreatment
		add_new_function(state, "spc_heat_treatment_save", "<string> OK", "spc_heat_treatment_save <String> csv", Bind_Templated_Save<DBS_HeatTreatment>, "Model_HeatTreatment||Save");
		add_new_function(state, "spc_heat_treatment_load_id", "<string> csv", "spc_heat_treatment_load_id <INT> ID Case", Bind_Templated_loadID<DBS_HeatTreatment>, "Model_HeatTreatment||LoadByID");
		add_new_function(state, "spc_heat_treatment_delete", "<string> OK", "spc_heat_treatment_delete <String> csv", Bind_Templated_Delete<DBS_HeatTreatment>, "Model_HeatTreatment||Delete");
		add_new_function(state, "spc_heat_treatment_load_ByName", "<string> OK", "spc_heat_treatment_load_ByName <String> csv", Bind_HeatTreatment_load_ByName, "Model_HeatTreatment||LoadByName");
		add_new_function(state, "spc_heat_treatment_load_IDCase", "<string> OK", "spc_heat_treatment_load_IDCase <String> csv", Bind_HeatTreatment_load_IDCase, "Model_HeatTreatment||LoadByIDCase");

		add_new_function(state, "spc_heat_treatment_segment_save", "<string> OK", "spc_heat_treatment_segment_save <String> csv", Bind_Templated_Save<DBS_HeatTreatmentSegment>, "Model_HeatTreatmentSegment||Save");
		add_new_function(state, "spc_heat_treatment_segment_load_id", "<string> csv", "spc_heat_treatment_segment_load_id <INT> ID Case", Bind_Templated_loadID<DBS_HeatTreatmentSegment>, "Model_HeatTreatmentSegment||LoadByID");
		add_new_function(state, "spc_heat_treatment_segment_delete", "<string> OK", "spc_heat_treatment_segment_delete <String> csv", Bind_Templated_Delete<DBS_HeatTreatmentSegment>, "Model_HeatTreatmentSegment||Delete");
		add_new_function(state, "spc_heat_treatment_segment_load_IDHeatTreatment", "<string> OK", "spc_heat_treatment_segment_load_IDHeatTreatment <String> csv", Bind_HeatTreatmentSegments_load_IDHeatTreatment, "Model_HeatTreatmentSegment||LoadByIDHeatTreatment");

		add_new_function(state, "spc_heat_treatment_profile_save", "<string> OK", "spc_heat_treatment_profile_save <String> csv", Bind_Templated_Save<DBS_HeatTreatmentProfile>, "Model_HeatTreatmentProfile||Save");
		add_new_function(state, "spc_heat_treatment_profile_load_id", "<string> csv", "spc_heat_treatment_profile_load_id <INT> ID Case", Bind_Templated_loadID<DBS_HeatTreatmentProfile>, "Model_HeatTreatmentProfile||LoadByID");
		add_new_function(state, "spc_heat_treatment_profile_delete", "<string> OK", "spc_heat_treatment_profile_delete <String> csv", Bind_Templated_Delete<DBS_HeatTreatmentProfile>, "Model_HeatTreatmentProfile||Delete");
		add_new_function(state, "spc_heat_treatment_profile_load_IDHeatTreatment", "<string> OK", "spc_heat_treatment_profile_load_IDHeatTreatment <String> csv", Bind_HeatTreatmentProfile_load_IDHeatTreatment, "Model_HeatTreatmentProfile||LoadByIDHeatTreatment");

#pragma endregion

		//###-> Configuration
#pragma region Configuration_SaveDelete

#pragma endregion

#pragma endregion

		//###-> Pixel Case
		add_new_function(state, "template_pixelcase_new", "string", "template_pixelcase_new", Bind_PCTemplate_createNew);
		add_new_function(state, "template_pixelcase_concentrationVariant", "string", 
								"template_pixelcase_concentrationVariant <string Element Name> <double stepSize> <int steps>", Bind_PCTemplate_createConcentrationVariant);
		add_new_function(state, "template_pixelcase_setComposition", "string", "template_pixelcase_setComposition <string> <double> (input in pairs, not limited to one pair)", Bind_PCTemplate_setComposition);
		add_new_function(state, "template_pixelcase_getComposition", "string", "template_pixelcase_getComposition", Bind_PCTemplate_getComposition);
		add_new_function(state, "template_pixelcase_selectPhases", "string", "template_pixelcase_selectPhases <string>", Bind_PCTemplate_selectPhases);
		add_new_function(state, "template_pixelcase_getSelectPhases", "string", "template_pixelcase_getSelectPhases", Bind_PCTemplate_getSelectPhases);

		add_new_function(state, "template_pixelcase_setEquilibriumTemperatureRange", "string", "template_pixelcase_getComposition", Bind_PCTemplate_setEquilibrimTemperatureRange);
		add_new_function(state, "template_pixelcase_setStepSize", "string", "template_pixelcase_setStepSize <double>", Bind_PCTemplate_setEquilibrimStepSize);

		add_new_function(state, "template_pixelcase_setScheilTemperatureRange", "string", "template_pixelcase_setScheilTemperatureRange <double start> <double end>", Bind_PCTemplate_setScheilTemperatureRange);
		add_new_function(state, "template_pixelcase_setScheilLiquidFraction", "string", "template_pixelcase_setScheilLiquidFraction <double>", Bind_PCTemplate_setScheilLiquidFraction);
		add_new_function(state, "template_pixelcase_setScheilDependentPhase", "string", "template_pixelcase_setScheilDependentPhase <string>", Bind_PCTemplate_setScheilDependentPhase);
		add_new_function(state, "template_pixelcase_setScheilStepSize", "string", "template_pixelcase_setScheilStepSize <double>", Bind_PCTemplate_setScheilStepSize);

		//###-> Case
		// Bind_SinglePixel_Case_Save
		add_new_function(state, "singlepixel_case_save", "string - OK", "singlepixel_case_save <string csv input>", Bind_SinglePixel_Case_Save);
		add_new_function(state, "singlepixel_equilibrium_config_save", "string - OK", "singlepixel_equilibrium_config_save <string csv input>", Bind_SinglePixel_EquilibriumConfig_Save);
		add_new_function(state, "singlepixel_scheil_config_save", "string - OK", "singlepixel_scheil_config_save <string csv input>", Bind_SinglePixel_ScheilConfig_Save);


		//------- Generic (needs project ID)


		//AMConfig
		add_new_function(state, "configuration_getAPI_path", "string", "configuration_getAPI_path (gets path to AMFramework dll)", Bind_configuration_getAPIpath);
		add_new_function(state, "configuration_setAPI_path", "string status", "configuration_setAPI_path <string Filename> (set path to AMFramework dll)", Bind_configuration_setAPIpath);
		add_new_function(state, "configuration_getExternalAPI_path", "string", "configuration_getExternalAPI_path (gets directory of external API e.g. matcalc)", Bind_configuration_getExternalAPIpath);
		add_new_function(state, "configuration_setExternalAPI_path", "string status", "configuration_setAPI_path <string Filename> (set path to external dll e.g. matcalc)", Bind_configuration_setExternalAPIpath);
		add_new_function(state, "configuration_get_working_directory", "string", "configuration_get_working_directory", Bind_configuration_getWorkingDirectory);
		add_new_function(state, "configuration_set_working_directory", "string", "configuration_set_working_directory <string Directory>", Bind_configuration_setWorkingDirectory);
		add_new_function(state, "configuration_set_thermodynamic_database_path", "string", "configuration_get_thermodynamic_database_path", Bind_configuration_setThermodynamicDatabasePath);
		add_new_function(state, "configuration_get_thermodynamic_database_path", "string", "configuration_get_thermodynamic_database_path <string filename>", Bind_configuration_getThermodynamicDatabasePath);
		add_new_function(state, "configuration_get_physical_database_path", "string", "configuration_get_physical_database_path", Bind_configuration_getPhysicalDatabasePath);
		add_new_function(state, "configuration_set_physical_database_path", "string", "configuration_set_physical_database_path <string filename>", Bind_configuration_setPhysicalDatabasePath);
		add_new_function(state, "configuration_get_mobility_database_path", "string", "configuration_get_mobility_database_path", Bind_configuration_getMobilityDatabasePath);
		add_new_function(state, "configuration_set_mobility_database_path", "string", "configuration_set_mobility_database_path <string filename>", Bind_configuration_setMobilityDatabasePath);
		add_new_function(state, "configuration_set_max_thread_number", "string", "configuration_set_max_thread_number <int thread number>", Bind_configuration_setMaxThreadNumber);
		add_new_function(state, "configuration_get_max_thread_number", "int", "configuration_get_max_thread_number", Bind_configuration_getMaxThreadNumber);
		add_new_function(state, "configuration_save", "string", "configuration_save", Bind_configuration_save);

		//-------- SYSTEM
		add_new_function(state, "core_cancel_operation", "void", "core_cancel_operation", Bind_CancelCalculations, "CoreMethods||CancelOperation");
		add_new_function(state, "core_buffer", "string", "core_buffer", Bind_GetBUFFER, "CoreMethods||GetBuffer");
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
		std::string fDescription,
		std::string fCommandspecs = "")
	{
		_functionNames.push_back(fName);
		_functionParameters.push_back(fParameters);
		_functionDescription.push_back(fDescription);
		_functionCommandSpecs.push_back(fCommandspecs);

		functionsList_addEntry(std::vector<std::string>{fName, 
														fParameters, 
														fDescription, 
														fCommandspecs });
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

	/// <summary>
	/// For csv format data input, we check for the following:
	/// First entry must be an integer reference to the ID of the object
	/// It has to be of length > 0
	/// </summary>
	/// <param name="state"></param>
	/// <param name="parameters"></param>
	/// <returns></returns>
	int static check_input_csv(lua_State* state, std::vector<std::string>& parameters, std::vector<std::string>& csvF)
	{
		parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Input in csv format not given");
			return 1;
		}

		// get csv input and check type
		csvF = string_manipulators::split_text(string_manipulators::trim_whiteSpace(IAM_Database::csv_join_row(parameters, " ")), ",");
		if (!string_manipulators::isNumber(csvF[0]))
		{
			std::string IDerror = "Wrong type, ID is not a number \'" + csvF[0] + "\'";
			lua_pushstring(state, IDerror.c_str());
			return 1;
		}

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

#pragma region LUA_GUI
	/// <summary>
	/// Using windows api, show messagebox
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_lua_msgbox(lua_State* state)
	{
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() < 1)
		{
			lua_pushstring(state, "Ups... what name should we use?");
			return 1;
		}

		int msgboxID = MessageBox(
			NULL,
			(LPCSTR)parameters[0].c_str(),
			(LPCSTR)"Account Details",
			MB_ICONWARNING | MB_CANCELTRYCONTINUE | MB_DEFBUTTON2
		);


		lua_pushstring(state, "OK");
		return 1;
	}

#pragma endregion

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
		if(response == 0)
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

		if(parameters.size() < 1)
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

		std::vector<std::vector<std::string>> outTable = std::vector<std::vector<std::string>>{ std::vector<std::string>{"0"}};

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
		std::vector<std::string> parameters = get_parameters(state);

		if (parameters.size() > 0)
		{
			std::string parameter = IAM_Database::csv_join_row(parameters, " ");
			_configuration->set_api_path(parameter);
			_configuration->save();

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
		std::vector<std::string> parameters = get_parameters(state);

		if (parameters.size() > 0)
		{
			std::string parameter = IAM_Database::csv_join_row(parameters, " ");
			_configuration->set_apiExternal_path(parameter);
			_configuration->save();

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
		std::vector<std::string> parameters = get_parameters(state);

		if(parameters.size() > 0)
		{
			std::string parameter = IAM_Database::csv_join_row(parameters, " ");
			parameter = string_manipulators::trim_whiteSpace(parameter);
			_configuration->set_working_directory(parameter);
			_configuration->save();

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
			_configuration->save();

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
			_configuration->save();

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
			_configuration->save();

			lua_pushstring(state, "OK");
		}
		else
		{
			lua_pushstring(state, "Error; no parameters");
		}

		return 1;
	}

	/// <summary>
	/// sets max thread Number
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_setMaxThreadNumber(lua_State* state)
	{
		std::vector<std::string> parameters = get_parameters(state);

		if (parameters.size() > 0)
		{
			std::string parameter = IAM_Database::csv_join_row(parameters, " ");
			_configuration->set_max_thread_number(std::stoi(parameter));
			_configuration->save();

			lua_pushstring(state, "OK");
		}
		else
		{
			lua_pushstring(state, "Bind_configuration_setMaxThreadNumber Error; no parameters");
		}

		return 1;
	}

	/// <summary>
	/// Returns the max number of threads to use for calculations
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_getMaxThreadNumber(lua_State* state)
	{
		std::string out = std::to_string(_configuration->get_max_thread_number());

		lua_pushstring(state, out.c_str());
		return 1;
	}

	/// <summary>
	/// Save configuration data
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_configuration_save(lua_State* state)
	{
		_configuration->save();
		lua_pushstring(state, "OK");
		return 1;
	}

#pragma endregion


#pragma region Data_controller

#pragma region Project
#pragma region Save
	/// <summary>
	/// Save Projects item using csv data as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Project_Save(lua_State* state)
	{
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Input in csv format not given");
			return 1;
		}

		// get csv input and check type
		std::vector<string> csvF = string_manipulators::split_text(IAM_Database::csv_join_row(parameters, " "), ",");
		if (!string_manipulators::isNumber(csvF[0]))
		{
			std::string IDerror = "Wrong type, ID is not a number \'" + csvF[0] + "\'";
			lua_pushstring(state, IDerror.c_str());
			return 1;
		}

		// create new config object if non existent
		if (std::stoi(csvF[0]) == -1)
		{
			DBS_Project NewConfig(_dbFramework->get_database(), -1);
			NewConfig.load(csvF);
			NewConfig.save();
			csvF[0] = std::to_string(NewConfig.id());
		}

		DBS_Project tempConfig(_dbFramework->get_database(), std::stoi(csvF[0]));
		tempConfig.load(csvF);
		tempConfig.save();

		lua_pushstring(state, std::to_string(tempConfig.id()).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Delete Project item using csv data as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Project_Delete(lua_State* state)
	{
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Input in csv format not given");
			return 1;
		}

		// get csv input and check type
		std::vector<string> csvF = string_manipulators::split_text(IAM_Database::csv_join_row(parameters, " "), ",");
		if (!string_manipulators::isNumber(csvF[0]))
		{
			std::string IDerror = "Wrong type, ID is not a number \'" + csvF[0] + "\'";
			lua_pushstring(state, IDerror.c_str());
			return 1;
		}

		// remove existing ID
		if (std::stoi(csvF[0]) > -1)
		{
			DBS_Project::remove_project_data(_dbFramework->get_database(), std::stoi(csvF[0]));
		}

		lua_pushstring(state, "OK");
		return 1;
	}

	/// <summary>
	/// Removes al dependent data from project
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Project_Remove_projectData(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// remove existing ID
		if (std::stoi(csvF[0]) > -1)
		{
			DBS_Project::remove_project_data(_dbFramework->get_database(), std::stoi(csvF[0]));
		}

		lua_pushstring(state, "OK");
		return 1;
	}

#pragma endregion
#pragma region Load
	/// <summary>
	/// Load Project item returning csv data as output
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Project_LoadData(lua_State* state)
	{
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Input in csv format not given");
			return 1;
		}

		// get csv input and check type
		std::vector<string> csvF = string_manipulators::split_text(IAM_Database::csv_join_row(parameters, " "), ",");
		AM_Database_Datatable loadTable(_dbFramework->get_database(), &AMLIB::TN_Projects());

		if (!string_manipulators::isNumber(string_manipulators::trim_whiteSpace(csvF[0])))
		{
			loadTable.load_data("Name = \'" + string_manipulators::trim_whiteSpace(IAM_Database::csv_join_row(csvF, " ")) + "\' ");
		}
		else
		{
			loadTable.load_data("ID = " + csvF[0]);
		}

		// remove existing ID
		DBS_Project loadedProject(_dbFramework->get_database(), -1);
		std::string out = IAM_Database::csv_join_row(loadedProject.get_input_vector(),",");

		if (loadTable.row_count() > 0)
		{
			loadedProject.load(loadTable.get_row_data(0));
			out = IAM_Database::csv_join_row(loadedProject.get_input_vector(), ",");
		}

		lua_pushstring(state, out.c_str());
		return 1;
	}
#pragma endregion

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
		_openProject->set_project_name(parameter, _dbFramework->get_apiPath(), _dbFramework->get_apiExternalPath());
		
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
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Bind_project_new: Error, missing input");
			return 1;
		}

		std::string projectName = IAM_Database::csv_join_row(parameters, " ");
		projectName = string_manipulators::trim_whiteSpace(projectName);

		if (_openProject != nullptr) delete _openProject;
		_openProject = new AM_Project(_dbFramework->get_database(), _configuration, projectName);

		if(_openProject->get_project_ID() == -1)
		{
			_openProject->set_project_name(projectName, _dbFramework->get_apiPath(), _dbFramework->get_apiExternalPath());
		}
		
		lua_pushstring(state, std::to_string(_openProject->get_project_ID()).c_str());
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

		_openProject->clear_project_data();
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

#pragma region ActivePhases
#pragma region Save
	/// <summary>
	/// Save Active phase item using csv data as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ActivePhases_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_ActivePhases(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

	/// <summary>
	/// Save Active phase Element composition item using csv data as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ActivePhasesElementComposition_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_ActivePhases_ElementComposition(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

	/// <summary>
	/// Save Active phase configuration item using csv data as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ActivePhasesConfiguration_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_ActivePhases_Configuration(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}
#pragma endregion
#pragma region Delete
	/// <summary>
	/// Delete Active phase item using csv data as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ActivePhases_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_ActivePhases(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}

	/// <summary>
	/// Delete Active phase Element composition item using csv data as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ActivePhasesElementComposition_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_ActivePhases_ElementComposition(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}

	/// <summary>
	/// Delete Active phase configuration item using csv data as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ActivePhasesConfiguration_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_ActivePhases_Configuration(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}
#pragma endregion
#pragma region Load
	static int Bind_ActivePhases_Load_ByIDProject(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Error: missing parameter -> ID Project");
			return 1;
		}

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_ActivePhases());
		DT.load_data("IDProject = " + string_manipulators::trim_whiteSpace(parameters[0]) + "");

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
	static int Bind_ActivePhasesConfiguration_Load_ByIDProject(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Error: missing parameter -> ID Project");
			return 1;
		}

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_ActivePhases_Configuration());
		DT.load_data("IDProject = " + string_manipulators::trim_whiteSpace(parameters[0]) + "");

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
	static int Bind_ActivePhasesElementComposition_Load_ByIDProject(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Error: missing parameter -> ID Project");
			return 1;
		}

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_ActivePhases_ElementComposition());
		DT.load_data("IDProject = " + string_manipulators::trim_whiteSpace(parameters[0]) + "");

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
#pragma endregion

#pragma endregion

#pragma region Precipitation
#pragma region Save
	/// <summary>
	/// Save precipitatio Domain item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_PrecipitationDomain_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_PrecipitationDomain(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

	/// <summary>
	/// Save precipitation phase item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_PrecipitationPhase_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_PrecipitationPhase(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}
#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Precipitatio Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_PrecipitationDomain_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_PrecipitationDomain(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}

	/// <summary>
	/// Remove Precipitation phase Item
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_PrecipitationPhase_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_PrecipitationPhase(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}
#pragma endregion
#pragma region load
	/// <summary>
	/// Load case by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_PrecipitationPhase_Load_CaseID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_PrecipitationPhase());
		DT.load_data("IDCase = " + parameters[0]);
		if (DT.row_count() == 0)
		{
			lua_pushstring(state, "Error: no data was found");
			return 1;
		}

		// send csv
		lua_pushstring(state, IAM_Database::csv_join_row(DT.get_row_data(0), ",").c_str());
		return 1;
	}

	/// <summary>
	/// Load case by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_PrecipitationDomain_Load_CaseID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_PrecipitationDomain());
		DT.load_data("IDCase = " + parameters[0]);
		if (DT.row_count() == 0)
		{
			lua_pushstring(state, "Error: no data was found");
			return 1;
		}

		// send csv
		lua_pushstring(state, IAM_Database::csv_join_row(DT.get_row_data(0), ",").c_str());
		return 1;
	}

	/// <summary>
	/// gets all entries related to an heat treatment ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_PrecipitationSimulationData_Load_CaseID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_PrecipitateSimulationData());
		DT.load_data("IDHeatTreatment = " + parameters[0]);
		if (DT.row_count() == 0)
		{
			lua_pushstring(state, "Error: no data was found");
			return 1;
		}

		// send csv
		lua_pushstring(state, IAM_Database::csv_join_row(DT.get_row_data(0), ",").c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region Equilibrium
#pragma region Save
	/// <summary>
	/// Save precipitatio Domain item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_EquilibriumConfiguration_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_EquilibriumConfiguration(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Precipitatio Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_EquilibriumConfiguration_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_EquilibriumConfiguration(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma region load
	/// <summary>
	/// Load case by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_EquilibriumConfiguration_Load_CaseID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_EquilibriumConfiguration());
		DT.load_data("IDCase = " + parameters[0]);
		if (DT.row_count() == 0)
		{
			lua_pushstring(state, "Error: no data was found");
			return 1;
		}
		// send csv
		lua_pushstring(state, IAM_Database::csv_join_row(DT.get_row_data(0),",").c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region Scheil
#pragma region Save
	/// <summary>
	/// Save precipitatio Domain item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ScheilConfiguration_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_ScheilConfiguration(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Precipitatio Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ScheilConfiguration_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_ScheilConfiguration(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma region load
	/// <summary>
	/// Load Scheil Configuration by ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_ScheilConfiguration_LoadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_ScheilConfiguration());
		DT.load_data("ID = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	/// <summary>
	/// Load case by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_ScheilConfiguration_Load_CaseID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_ScheilConfiguration());
		DT.load_data("IDCase = " + parameters[0]);
		if(DT.row_count() == 0)
		{
			lua_pushstring(state,"Error: no data was found");
			return 1;
		}

		// send csv
		lua_pushstring(state, IAM_Database::csv_join_row(DT.get_row_data(0), ",").c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region Phases
#pragma region Save
	/// <summary>
	/// Save precipitatio Domain item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Phase_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_Phase(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Precipitatio Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Phase_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_Phase(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma region load
	/// <summary>
	/// Load case by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_Phase_LoadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_Phase());
		DT.load_data("ID = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	/// <summary>
	/// Load phase by Name
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_Phase_Load_ByName(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters = get_parameters(state);
		if(parameters.size() == 0)
		{
			lua_pushstring(state, "Error: missing parameter -> Name");
			return 1;
		}

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_Phase());
		DT.load_data("Name = '" + string_manipulators::trim_whiteSpace(parameters[0]) + "'");

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region Elements
#pragma region Save
	/// <summary>
	/// Save precipitatio Domain item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Element_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_Element(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Precipitatio Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Element_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_Element(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma region load
	/// <summary>
	/// Load case by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_Element_LoadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_Element());
		DT.load_data("ID = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	/// <summary>
	/// Load phase by Name
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_Element_Load_ByName(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Error: missing parameter -> Name");
			return 1;
		}

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_Element());
		DT.load_data("Name = '" + string_manipulators::trim_whiteSpace(parameters[0]) + "'");

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region SelectedPhase
#pragma region Save
	/// <summary>
	/// Save Selected phase item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SelectedPhase_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_SelectedPhases(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Selected phase Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SelectedPhase_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_SelectedPhases(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma region load
	/// <summary>
	/// Load selected phase by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_SelectedPhase_Load_CaseID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_SelectedPhases());
		DT.load_data("IDCase = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	/// <summary>
	/// Load selected phase by ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_SelectedPhase_LoadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_SelectedPhases());
		DT.load_data("ID = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region SelectedElement
#pragma region Save
	/// <summary>
	/// Save Selected phase item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SelectedElement_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_SelectedElements(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Selected phase Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_SelectedElement_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_SelectedElements(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma region load
	/// <summary>
	/// Load selected phase by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_SelectedElement_Load_ProjectID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_SelectedElements());
		DT.load_data("IDProject = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	/// <summary>
	/// Load selected phase by ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_SelectedElement_LoadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_SelectedElements());
		DT.load_data("ID = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region ElementComposition
#pragma region Save
	/// <summary>
	/// Save Selected phase item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ElementComposition_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_ElementComposition(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Selected phase Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ElementComposition_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_ElementComposition(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma region load
	/// <summary>
	/// Load selected phase by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_ElementComposition_Load_CaseID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_ElementComposition());
		DT.load_data("IDCase = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	/// <summary>
	/// Load selected phase by ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_ElementComposition_LoadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_ElementComposition());
		DT.load_data("ID = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region EquilibriumPhaseFraction
#pragma region Save
	/// <summary>
	/// Save Selected phase item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_EquilibriumPhaseFraction_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_EquilibriumPhaseFraction(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Selected phase Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_EquilibriumPhaseFraction_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_EquilibriumPhaseFraction(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma region load
	/// <summary>
	/// Load selected phase by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_EquilibriumPhaseFraction_Load_CaseID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_EquilibriumPhaseFractions());
		DT.load_data("IDCase = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	/// <summary>
	/// Load selected phase by ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_EquilibriumPhaseFraction_LoadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_EquilibriumPhaseFractions());
		DT.load_data("ID = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region ScheilPhaseFraction
#pragma region Save
	/// <summary>
	/// Save Selected phase item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ScheilPhaseFraction_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_ScheilPhaseFraction(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Selected phase Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_ScheilPhaseFraction_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS_ScheilPhaseFraction(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma region load
	/// <summary>
	/// Load selected phase by CaseID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_ScheilPhaseFraction_Load_CaseID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_ScheilPhaseFraction());
		DT.load_data("IDCase = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	/// <summary>
	/// Load selected phase by ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_ScheilPhaseFraction_LoadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_ScheilPhaseFraction());
		DT.load_data("ID = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
#pragma endregion
#pragma endregion

#pragma region Case
#pragma region load
	/// <summary>
	/// Load case by ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_Case_LoadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		DBS_Case Item(_dbFramework->get_database(), std::stoi(parameters[0]));
		Item.load();
		
		// send csv
		lua_pushstring(state, IAM_Database::csv_join_row(Item.get_input_vector(), ",").c_str());
		return 1;
	}

	/// <summary>
	/// Load case by ID
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	static int Bind_Case_Load_ProjectID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_Case());
		DT.load_data("IDProject = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}
#pragma endregion

#pragma region Save
	/// <summary>
	/// Save precipitatio Domain item using csv as input
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Case_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS_Case(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion
#pragma region Delete
	/// <summary>
	/// Remove Precipitatio Domain entry
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	static int Bind_Case_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state, 
					  (IAM_DBS::remove_byID(&DBS_Case(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}


#pragma endregion
#pragma endregion

#pragma region HeatTreatment
#pragma region load

	static int Bind_HeatTreatment_load_ByName(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;
		if (csvF.size() < 3) 
		{
			lua_pushstring(state, "Error, missing parameters in Bind_HeatTreatment_load_ByName");
			return 1;
		}

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_HeatTreatment());
		DT.load_data("Name = \'" + csvF[1] + "\' AND IDCase = " + csvF[2]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	static int Bind_HeatTreatment_load_IDCase(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_HeatTreatment());
		DT.load_data("IDCase = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	static int Bind_HeatTreatmentSegments_load_IDHeatTreatment(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_HeatTreatmentSegments());
		DT.load_data("IDHeatTreatment = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

	static int Bind_HeatTreatmentProfile_load_IDHeatTreatment(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		AM_Database_Datatable DT(_dbFramework->get_database(), &AMLIB::TN_HeatTreatmentProfile());
		DT.load_data("IDHeatTreatment = " + parameters[0]);

		// send csv
		lua_pushstring(state, DT.get_csv().c_str());
		return 1;
	}

#pragma endregion
#pragma endregion

#pragma region Template_Data_Manage
	/// <summary>
	/// Load a DBS object
	/// </summary>
	/// <param name="state"></param>
	/// <returns>csv format of data</returns>
	template<typename DBS>
	static int Bind_Templated_loadID(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// get data
		DBS Item(_dbFramework->get_database(), std::stoi(parameters[0]));
		Item.load();

		// send csv
		lua_pushstring(state, IAM_Database::csv_join_row(Item.get_input_vector(), ",").c_str());
		return 1;
	}


	/// <summary>
	/// Remove as DBS Object
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	template<typename DBS>
	static int Bind_Templated_Delete(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// Remove data
		lua_pushstring(state,
			(IAM_DBS::remove_byID(&DBS(_dbFramework->get_database(), -1), csvF) == 0) ? "OK" : "Error");
		return 1;
	}

	/// <summary>
	/// Save a DBS object
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	template<typename DBS>
	static int Bind_Templated_Save(lua_State* state)
	{
		// check input
		std::vector<std::string> parameters, csvF;
		if (check_input_csv(state, parameters, csvF) == 1) return 1;

		// save
		lua_pushstring(state, std::to_string(IAM_DBS::save(&DBS(_dbFramework->get_database(), -1), csvF)).c_str());
		return 1;
	}

#pragma endregion


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
	static int Bind_SinglePixel_Case_Save(lua_State* state)
	{
		// get parameters
		std::vector<std::string> parameters = get_parameters(state);
		if(parameters.size() == 0)
		{
			lua_pushstring(state, "Input in csv format not given");
			return 1;
		}

		// get csv input and check type
		std::vector<string> csvF = string_manipulators::split_text(IAM_Database::csv_join_row(parameters, " "), ",");
		if (!string_manipulators::isNumber(csvF[0]))
		{
			std::string IDerror = "Wrong type, ID is not a number \'" + csvF[0] + "\'";
			lua_pushstring(state, IDerror.c_str());
			return 1;
		}

		// if this is a new case, save as template (this will also create all configurations entries)
		if (std::stoi(csvF[0]) == -1)
		{
			DBS_Project tempProject(_dbFramework->get_database(), _openProject->get_project_ID());
			AM_pixel_parameters pointerPixel(_dbFramework->get_database(), &tempProject, -1);
			pointerPixel.save();
			csvF[0] = std::to_string(pointerPixel.get_caseID());
		}

		DBS_Case tempCase(_dbFramework->get_database(), std::stoi(csvF[0]));
		tempCase.load(csvF);
		tempCase.Name = csvF[3];
		tempCase.Date = csvF[5];
		tempCase.save();


		lua_pushstring(state, "OK");
		return 1;
	}
	static int Bind_SinglePixel_EquilibriumConfig_Save(lua_State* state)
	{
		// get parameters
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Input in csv format not given");
			return 1;
		}

		// get csv input and check type
		std::vector<string> csvF = string_manipulators::split_text(IAM_Database::csv_join_row(parameters, " "), ",");
		if (!string_manipulators::isNumber(csvF[0]))
		{
			lua_pushstring(state, "Wrong type");
			return 1;
		}

		DBS_EquilibriumConfiguration tempCase(_dbFramework->get_database(), std::stoi(csvF[0]));
		tempCase.load(csvF);
		tempCase.save();


		lua_pushstring(state, "OK");
		return 1;
	}
	static int Bind_SinglePixel_ScheilConfig_Save(lua_State* state)
	{
		// get parameters
		std::vector<std::string> parameters = get_parameters(state);
		if (parameters.size() == 0)
		{
			lua_pushstring(state, "Input in csv format not given");
			return 1;
		}

		// get csv input and check type
		std::vector<string> csvF = string_manipulators::split_text(IAM_Database::csv_join_row(parameters, " "), ",");
		if (!string_manipulators::isNumber(csvF[0]))
		{
			lua_pushstring(state, "Wrong type");
			return 1;
		}

		DBS_ScheilConfiguration tempCase(_dbFramework->get_database(), std::stoi(csvF[0]));
		tempCase.load(csvF);
		tempCase.save();


		lua_pushstring(state, "OK");
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

	static int Bind_PCTemplate_setEquilibrimStepSize(lua_State* state)
	{
		std::vector<std::string> parameters;
		if (check_global_using_openProject(state, lua_gettop(state), 1,
			" Input has to specify the stepsize ",
			parameters) != 0) return 1;

		// You have to create a template where you do al the configurations you want to run.
		AM_pixel_parameters* pointerPixel = _openProject->get_case_template();
		if (pointerPixel == nullptr)
		{
			lua_pushstring(state, "No template created, please create a template!");
			return 1;
		}

		pointerPixel->set_equilibrium_config_stepSize(std::stold(parameters[0]));
		
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
		std::string out = Data_Controller::get_csv(_dbFramework->get_database(), (Data_Controller::DATATABLES)std::stoi(parameter));

		lua_pushstring(state, out.c_str());
		return 1;
	}

	

#pragma endregion
#pragma endregion

#pragma region LUA
	static std::string run_command(lua_State* state, std::string command)
	{
		int c_out = lua_getglobal(state, command.c_str());

		lua_call(state, 0, 1);

		std::string out;
		try
		{
			out = lua_tostring(state, -1);
		}
		catch (const std::exception&)
		{
			out = "Command not recognized!";
		}

		lua_pop(state, 1);

		return out;
	}

	static std::string run_command(lua_State* state, std::string command, std::vector<std::string> parameters)
	{
		lua_getglobal(state, command.c_str());

		for each (std::string pary in parameters)
		{
			lua_pushstring(state, pary.c_str());
		}

		lua_call(state, parameters.size(), 1);

		std::string out(lua_tostring(state, -1));
		lua_pop(state, 1);

		return out;
	}
#pragma endregion

#pragma region System
	static int Bind_CancelCalculations(lua_State* state)
	{
		_cancelCalculations = true;
		lua_pushstring(state, "Cancelling");
		return 1;
	}

	static int Bind_GetBUFFER(lua_State* state)
	{
		lua_pushstring(state, _luaBUFFER.c_str());
		return 1;
	}
#pragma endregion

#pragma region
};