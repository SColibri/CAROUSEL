#include <catch2/catch_test_macros.hpp>

#include "../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../AMLib/include/Database_implementations/Database_scheme_content.h"

#include "../../../AMLib/include/AM_Project.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/include/AM_lua_interpreter.h"
#include "../../../AMLib/interfaces/IAM_lua_functions.h"	
#include "../../../AMLib/include/Database_implementations/Database_Factory.h"
#include "../../../AM_API_lib/matcalc/include/API_lua_functions.h"

namespace main_setup
{

	class lua_test_implementation : public IAM_lua_functions
	{
	public: 
		lua_test_implementation(lua_State* state, AM_Config* configuration, AM_Database_Framework* dbFrame) : IAM_lua_functions(state)
		{
			_configuration = configuration;
			add_functions_to_lua(state);
			add_base_functions(state);
			_dbFramework = dbFrame;
			//AMBaseFunctions::_dbFramework = _dbFramework;
		}

		virtual void add_functions_to_lua(lua_State* state) override
		{
			// Do nothing
		}
	};

	static AM_Config configuration;
	static AM_Database_Framework* _dbF{ nullptr };
	static IAM_Database* _db{ nullptr };
	static lua_State* state{ nullptr };
	static AM_lua_interpreter* interpreter{ nullptr };
	static lua_test_implementation* testInterface{ nullptr };

	static void init()
	{
		configuration.set_working_directory("");
		Database_Factory::set_schema("projectDB");

		//Create a new database file
		std::string filename = configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "/" +
			Database_Factory::get_schema() + ".db";
		if (std::filesystem::exists(filename))
			std::filesystem::remove(std::filesystem::path(filename));

		_dbF = new AM_Database_Framework(&configuration);
		_db = _dbF->get_database();

		// create a API_lua object
		interpreter = new AM_lua_interpreter();
		state = interpreter->get_state();

		testInterface = new lua_test_implementation(state, &configuration, _dbF);
		
		// Add elements
		DBS_Element tempElement(_db, -1);
		tempElement.Name = "AL";
		tempElement.save();

		tempElement.set_id(-1);
		tempElement.Name = "ZR";
		tempElement.save();

		//add phases
		DBS_Phase tempPhase(_db, -1);
		tempPhase.Name = "LIQUID";
		tempPhase.save();

		tempPhase.set_id(-1);
		tempPhase.Name = "FCC";
		tempPhase.save();
	}

	
}

TEST_CASE("IAM_lua_functions")
{

	SECTION("Test initialize")
	{
		main_setup::init();
		REQUIRE(main_setup::_db != nullptr);
	}

	SECTION("constructor")
	{
		// checking initial state
		REQUIRE(main_setup::testInterface->get_list_functions().size() != 0);
		REQUIRE(string_manipulators::find_index_of_keyword(
				main_setup::interpreter->run_command("project_getData"), 
				"No selected project!") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_setData"),
			"No selected project!") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_selectElements"),
			"No selected project!") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_getSelectedElements"),
			"No selected project!") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_selectElements"),
			"No selected project!") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_clear_content"),
			"No selected project!") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_setReferenceElement"),
			"No selected project!") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_getReferenceElement"),
			"No selected project!") != std::string::npos);
		
	}

	SECTION("Project actions")
	{
		// load invalid project
		REQUIRE(main_setup::interpreter->run_command("project_loadID", 
			std::vector<std::string> {"1"}).compare("OK") != 0);
		
		REQUIRE(main_setup::interpreter->run_command("project_loadName",
			std::vector<std::string> {"NewProject"}).compare("OK") != 0);

		// create new project
		REQUIRE(main_setup::interpreter->run_command("project_setName",
			std::vector<std::string> {"TestProject"}).compare("OK") == 0);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_getData"), "TestProject") != std::string::npos);

		REQUIRE(main_setup::interpreter->run_command("project_new",
			std::vector<std::string> {"TestProject_new"}).compare("OK") == 0);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_getData"), "TestProject_new") != std::string::npos);

		// set data
		REQUIRE(main_setup::interpreter->run_command("project_setData",
			std::vector<std::string> {"2","projectAAA"}).compare("-1") != 0); //if ID is -1 something went wrong

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_getData"), "2") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_getData"), "projectAAA") != std::string::npos);

		// add invalid elements
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_selectElements",
			std::vector<std::string> {"AL", "MN"}), "Error") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_getSelectedElements",
			std::vector<std::string> {""}), "AL") == std::string::npos);

		// add valid elements
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_selectElements",
			std::vector<std::string> {"AL", "ZR"}), "Error") == std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_getSelectedElements",
			std::vector<std::string> {""}), "1") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_getSelectedElements",
			std::vector<std::string> {""}), "2") != std::string::npos);

		// reference element
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_getReferenceElement",
			std::vector<std::string> {""}), "Not found!") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_setReferenceElement",
			std::vector<std::string> {"AL"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_getReferenceElement",
			std::vector<std::string> {""}), "AL") != std::string::npos);
	}

	SECTION("Project load")
	{
		// Repeat all previous tests (ok not all, but those that check the data) 
		// checking that the info was saved and loaded correctly
		REQUIRE(main_setup::interpreter->run_command("project_loadID",
			std::vector<std::string> {"2"}).compare("OK") == 0);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_getData"), "2") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::interpreter->run_command("project_getData"), "projectAAA") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_getSelectedElements",
			std::vector<std::string> {""}), "1") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_getSelectedElements",
			std::vector<std::string> {""}), "2") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("project_getReferenceElement",
			std::vector<std::string> {""}), "AL") != std::string::npos);

	}

	SECTION("Template pixelCase")
	{
#pragma region NonExistingTemplate
		// actions on non template cases
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_concentrationVariant",
			std::vector<std::string> {"AL", "10", "1"}), "No template created") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setComposition",
			std::vector<std::string> {"AL", "0.90"}), "No template created") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_getComposition",
			std::vector<std::string> {""}), "No template created") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_selectPhases",
			std::vector<std::string> {""}), "No template created") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setEquilibriumTemperatureRange",
			std::vector<std::string> {"100","200"}), "No template created") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setScheilTemperatureRange",
			std::vector<std::string> {"100","200"}), "No template created") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setScheilLiquidFraction",
			std::vector<std::string> {"0.01"}), "No template created") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setScheilDependentPhase",
			std::vector<std::string> {"AL"}), "No template created") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setScheilStepSize",
			std::vector<std::string> {"25"}), "No template created") != std::string::npos);
#pragma endregion

#pragma region TemplateTesting
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_new",
			std::vector<std::string> {""}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setComposition",
			std::vector<std::string> {"AL", "0.90"}), "0.90") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_getComposition",
			std::vector<std::string> {""}), "AL") != std::string::npos);

		// select non existing phase
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_selectPhases",
			std::vector<std::string> {"NONEXIST"}), "Phase does not exis") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_selectPhases",
			std::vector<std::string> {"LIQUID"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_selectPhases",
			std::vector<std::string> {"LIQUID", "FCC"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_getSelectPhases",
			std::vector<std::string> {"LIQUID", "FCC"}), "LIQUID") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_getSelectPhases",
			std::vector<std::string> {"LIQUID", "FCC"}), "FCC") != std::string::npos);


		// Equilibrium
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setEquilibriumTemperatureRange",
			std::vector<std::string> {"100", "600"}), "OK") != std::string::npos);


		//Scheil
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setScheilTemperatureRange",
			std::vector<std::string> {"100", "600"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setScheilLiquidFraction",
			std::vector<std::string> {"0.02"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setScheilDependentPhase",
			std::vector<std::string> {"FCC"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_setScheilStepSize",
			std::vector<std::string> {"25"}), "OK") != std::string::npos);

		// Create cases from template
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_concentrationVariant",
			std::vector<std::string> {"AL", "10", "10", "ZR", "10"}), "Input has to specify three components") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::interpreter->run_command("template_pixelcase_concentrationVariant",
			std::vector<std::string> {"AL", "10", "10", "ZR", "10", "2"}), "OK") != std::string::npos);

		// check data in database---------------------------
		std::string CasesTable = main_setup::interpreter->run_command("project_getDataTableCases",
									std::vector<std::string> {""});

		AM_Database_Datatable tempTable(main_setup::_db, &AMLIB::TN_Case());
		tempTable.load_data(" IDProject = 2 ");
		REQUIRE(tempTable.row_count() == 20);
		REQUIRE(tempTable(1, 0).compare("2") == 0);

		for (int n1 = 0 ; n1 < tempTable.row_count(); n1 ++)
		{
#pragma region Equilibrium_config
			AM_Database_Datatable tempTable_2(main_setup::_db, &AMLIB::TN_EquilibriumConfiguration());
			tempTable_2.load_data(" IDCase = " + tempTable( 0, n1 ));
			REQUIRE(tempTable_2.row_count() == 1);
			REQUIRE(tempTable_2(1, 0).compare(tempTable(0, n1)) == 0);
			REQUIRE(tempTable_2(2, 0).compare("700.0") == 0);
			REQUIRE(tempTable_2(3, 0).compare("100.0") == 0);
			REQUIRE(tempTable_2(4, 0).compare("600.0") == 0);
#pragma endregion

#pragma region Scheil_config
			AM_Database_Datatable tempTable_3(main_setup::_db, &AMLIB::TN_ScheilConfiguration());
			tempTable_3.load_data(" IDCase = " + tempTable(0, n1));
			REQUIRE(tempTable_3.row_count() == 1);
			REQUIRE(tempTable_2(1, 0).compare(tempTable(0, n1)) == 0);
			REQUIRE(tempTable_3(2, 0).compare("100.0") == 0);
			REQUIRE(tempTable_3(3, 0).compare("600.0") == 0);
			REQUIRE(tempTable_3(4, 0).compare("25.0") == 0);
			REQUIRE(tempTable_3(5, 0).compare("2") == 0);
			REQUIRE(tempTable_3(6, 0).compare("0.02") == 0);
#pragma endregion

#pragma region CALPHAD
			AM_Database_Datatable tempTable_4(main_setup::_db, &AMLIB::TN_CALPHADDatabase());
			tempTable_4.load_data(" IDCase = " + tempTable(0, n1));
			REQUIRE(tempTable_4.row_count() == 1);
			// NOTE: CALPHAD database if filled when the case configuration is ran
			// thus testing for CALPHAD here is not needed.
			//REQUIRE(tempTable_4(1, 0).compare(std::filesystem::path(main_setup::configuration.get_ThermodynamicDatabase_path()).filename().string()) == 0);
			//REQUIRE(tempTable_4(2, 0).compare(std::filesystem::path(main_setup::configuration.get_PhysicalDatabase_path()).filename().string()) == 0);
			//REQUIRE(tempTable_4(3, 0).compare(std::filesystem::path(main_setup::configuration.get_MobilityDatabase_path()).filename().string()) == 0);
#pragma endregion

		}


		

#pragma endregion


	}
}
