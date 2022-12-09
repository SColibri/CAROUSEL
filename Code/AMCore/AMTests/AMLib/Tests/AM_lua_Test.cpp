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
		std::string filename = configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "\\" +
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
		
	}

	SECTION("Project load")
	{
		
	}

	SECTION("Template pixelCase")
	{


	}

	SECTION("Precipitation phases")
	{
#pragma region lua_save
		std::string outThis = main_setup::interpreter->run_command("spc_precipitation_phase_save",
			std::vector<std::string> {"-1,1,36,25,AL3TI_L_P0,none,-1,normal,0.000001,0.000002,0.000003,0.05, "});

		REQUIRE(outThis.compare("-1") != 0);

		DBS_PrecipitationPhase tempP(main_setup::_db, std::stoi(outThis));
		tempP.load();
		REQUIRE(tempP.id() == std::stoi(outThis));
#pragma endregion

	}
}
