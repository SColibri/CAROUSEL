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
		//if (std::filesystem::exists(filename))
		//	std::filesystem::remove(std::filesystem::path(filename));

		_dbF = new AM_Database_Framework(&configuration);
		_db = _dbF->get_database();

		// create a API_lua object
		interpreter = new AM_lua_interpreter();
		state = interpreter->get_state();

		testInterface = new lua_test_implementation(state, &configuration, _dbF);
	}

	static void Object_test(IAM_DBS& DBSOBJ, std::string saveCommand, std::string loadCommand, std::string deleteCommand)
	{
		std::string OBJ_out = main_setup::interpreter->run_command(saveCommand, std::vector<std::string>{IAM_Database::csv_join_row(DBSOBJ.get_input_vector(), ",")});
		REQUIRE(OBJ_out.compare("-1") != 0);
		DBSOBJ.set_id(std::stoi(OBJ_out));
		std::string inputy = IAM_Database::csv_join_row(DBSOBJ.get_input_vector(), ",");

		std::string loadyCommand = main_setup::interpreter->run_command(loadCommand, std::vector<std::string>{OBJ_out});
		OBJ_out = string_manipulators::trim_whiteSpace(loadyCommand);
		DBSOBJ.load(string_manipulators::split_text(OBJ_out,","));
		
		std::string toCompare = IAM_Database::csv_join_row(DBSOBJ.get_input_vector(), ",");
		REQUIRE(inputy.compare(toCompare) == 0);

		OBJ_out = main_setup::interpreter->run_command(deleteCommand, std::vector<std::string>{string_manipulators::trim_whiteSpace(std::to_string(DBSOBJ.id()))});
		DBSOBJ.load();
		REQUIRE(DBSOBJ.id() == -1);
	}
}



TEST_CASE("lua scripting") 
{
	SECTION("Test initialize")
	{
		main_setup::init();
		REQUIRE(main_setup::_db != nullptr);
	}

	SECTION("script test")
	{
		
		DBS_Case casey(main_setup::_db, -1);
		casey.Name = "Generic name";
		casey.save();

		main_setup::interpreter->run_file("C:\\Users\\drogo\\Desktop\\Homless\\tessave.lua");

		DBS_Case casey2(main_setup::_db, 1);
		casey2.load();
		//REQUIRE(casey2.Name.compare("Hello world from Framework") == 0);

		
	}

	SECTION("Objects")
	{
		DBS_Case OBJ01(main_setup::_db, -1); OBJ01.Name = "Namey";
		main_setup::Object_test(OBJ01, "spc_case_save", "spc_case_load_id", "spc_case_delete");

		DBS_Phase OBJ02(main_setup::_db, -1); OBJ02.Name = "Namey";
		main_setup::Object_test(OBJ02, "phase_save", "phase_loadID", "phase_delete");
		
		DBS_Element OBJ03(main_setup::_db, -1); OBJ03.Name = "Namey";
		main_setup::Object_test(OBJ03, "element_save", "element_loadID", "element_delete");
		
		DBS_SelectedPhases OBJ04(main_setup::_db, -1);
		main_setup::Object_test(OBJ04, "spc_selectedphase_save", "spc_selectedphase_load_id", "spc_selectedphase_delete");

		DBS_ElementComposition OBJ05(main_setup::_db, -1); OBJ05.Value = 0.000005;
		main_setup::Object_test(OBJ05, "spc_elementcomposition_save", "spc_elementcomposition_load_id", "spc_elementcomposition_delete");

		DBS_ScheilConfiguration OBJ06(main_setup::_db, -1); OBJ06.minimumLiquidFraction = 0.000005;
		main_setup::Object_test(OBJ06, "spc_scheil_configuration_save", "spc_scheil_configuration_loadID", "spc_scheil_configuration_delete");

		DBS_ScheilPhaseFraction OBJ07(main_setup::_db, -1);
		main_setup::Object_test(OBJ07, "spc_scheil_phasefraction_save", "spc_scheil_phasefraction_loadID", "spc_scheil_phasefraction_delete");

		DBS_EquilibriumConfiguration OBJ08(main_setup::_db, -1);
		main_setup::Object_test(OBJ08, "spc_equilibrium_configuration_save", "spc_equilibrium_configuration_loadID", "spc_equilibrium_configuration_delete");

		DBS_EquilibriumPhaseFraction OBJ09(main_setup::_db, -1);
		main_setup::Object_test(OBJ09, "spc_equilibrium_phasefraction_save", "spc_equilibrium_phasefraction_loadID", "spc_equilibrium_phasefraction_delete");

		DBS_ActivePhases OBJ10(main_setup::_db, -1);
		main_setup::Object_test(OBJ10, "project_active_phases_save", "project_active_phases_loadID", "project_active_phases_delete");

		DBS_ActivePhases_Configuration OBJ11(main_setup::_db, -1);
		main_setup::Object_test(OBJ11, "project_active_phases_configuration_save", "project_active_phases_configuration_loadID", "project_active_phases_configuration_delete");

		DBS_ActivePhases_ElementComposition OBJ12(main_setup::_db, -1);
		main_setup::Object_test(OBJ12, "project_active_phases_element_composition_save", "project_active_phases_element_composition_loadID", "project_active_phases_element_composition_delete");

		DBS_PrecipitationDomain OBJ13(main_setup::_db, -1);
		main_setup::Object_test(OBJ13, "spc_precipitation_domain_save", "spc_precipitation_domain_loadID", "spc_precipitation_domain_delete");
		
		DBS_PrecipitationPhase OBJ14(main_setup::_db, -1);
		main_setup::Object_test(OBJ14, "spc_precipitation_phase_save", "spc_precipitation_phase_loadID", "spc_precipitation_phase_delete");

	}

	
}