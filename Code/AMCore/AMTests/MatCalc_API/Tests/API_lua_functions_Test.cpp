#include <catch2/catch_test_macros.hpp>

#include <windows.h>
#include "../../../AMLib/include/AM_Project.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/include/AM_lua_interpreter.h"
#include "../../../AMLib/interfaces/IAM_API.h"
#include "../../../AMLib/interfaces/IAM_lua_functions.h"	
#include "../../../AMLib/include/Database_implementations/Database_Factory.h"

namespace main_setup
{
	static IAM_API* api{ nullptr };
	static AM_Config configuration;
	static AM_Database_Framework* _dbF{ nullptr };
	static IAM_Database* _db{ nullptr };
	HINSTANCE _library{ NULL };

	typedef IAM_API* (__cdecl* MYPROC)(AM_Config*);
	IAM_API* DLL_get(HINSTANCE hLib) {
		MYPROC ProcAdd = (MYPROC)GetProcAddress(hLib, "get_API_Controll");

		if (NULL != ProcAdd)
		{
			IAM_API* Result = ProcAdd(&configuration);
			return Result;
		}

		return nullptr;
	}

	static void init()
	{
		configuration.set_working_directory("C:/Users/drogo/Desktop/Homless");
		configuration.set_apiExternal_path("C:/Program Files/MatCalc 6");
		configuration.set_api_path("../../AM_API_lib/matcalc/AM_MATCALC_Lib.dll");

		// NOTE: when passing paths to matcalc we have to pass with escape characters, otherwise this will not work
		// when passing the argument to matcalc.
		configuration.set_ThermodynamicDatabase_path("C:/Program\\ Files/MatCalc\\ 6/database/thermodynamic/mc_al.tdb");
		configuration.set_MobilityDatabase_path("C:/Program\\ Files/MatCalc\\ 6/database/physical/physical_data.pdb");
		configuration.set_PhysicalDatabase_path("C:/Program\\ Files/MatCalc\\ 6/database/difussion/mc_al.ddb");


		char pBuf[256];
		size_t len = sizeof(pBuf);
		int bytes = GetModuleFileName(NULL, pBuf, len);
		std::filesystem::create_directory(std::filesystem::path(configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE)));

		//Create a new database file
		bool removed{ false };
		std::string filename = configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "/" +
			Database_Factory::get_schema() + ".db";
		if (std::filesystem::exists(filename))
			removed = std::filesystem::remove(std::filesystem::path(filename));

		_dbF = new AM_Database_Framework(&configuration);
		_db = _dbF->get_database();
		
		_library = LoadLibrary(TEXT(configuration.get_api_path().c_str()));
		api = DLL_get(_library);
	}
}

TEST_CASE("IAM_lua_functions")
{

	SECTION("Test initialize")
	{
		main_setup::init();
		REQUIRE(main_setup::_db != nullptr);

		REQUIRE(main_setup::api->run_lua_command("configuration_setAPI_path",
			std::vector<std::string> {main_setup::configuration.get_api_path()}).compare("OK") == 0);

		REQUIRE(main_setup::api->run_lua_command("configuration_setExternalAPI_path",
			std::vector<std::string> {main_setup::configuration.get_apiExternal_path()}).compare("OK") == 0);

		REQUIRE(main_setup::api->run_lua_command("configuration_set_working_directory",
			std::vector<std::string> {main_setup::configuration.get_working_directory()}).compare("OK") == 0);

		REQUIRE(main_setup::api->run_lua_command("configuration_set_thermodynamic_database_path",
			std::vector<std::string> {main_setup::configuration.get_ThermodynamicDatabase_path()}).compare(main_setup::configuration.get_ThermodynamicDatabase_path()) == 0);

		REQUIRE(main_setup::api->run_lua_command("configuration_set_physical_database_path",
			std::vector<std::string> {main_setup::configuration.get_PhysicalDatabase_path()}).compare("OK") == 0);

		REQUIRE(main_setup::api->run_lua_command("configuration_set_mobility_database_path",
			std::vector<std::string> {main_setup::configuration.get_MobilityDatabase_path()}).compare("OK") == 0);

		std::string outInitialize = main_setup::api->run_lua_command("initialize_core",
			std::vector<std::string> {""});

		DBS_Element tempElement(main_setup::_db, 1);
		tempElement.load();

		AM_Database_Datatable dT(main_setup::_db, &AMLIB::TN_Element());
		dT.load_data();

		REQUIRE(tempElement.Name.compare("VA") == 0);

		DBS_Phase tempphase(main_setup::_db, 1);
		tempphase.load();
		REQUIRE(tempphase.Name.compare("LIQUID") == 0);

		main_setup::_db->disconnect();
	}

	SECTION("constructor")
	{
		// lua functions
		std::vector<std::vector<std::string>> outVector = main_setup::api->get_declared_functions();
		REQUIRE(main_setup::api->get_declared_functions().size() != 0);

		
	}

	SECTION("IAM_lua_function base functions")
	{
		// create new project
		REQUIRE(main_setup::api->run_lua_command("project_new",
			std::vector<std::string> {"TestProject"}).compare("OK") == 0);

		std::string outTest = main_setup::api->run_lua_command("project_getData");
		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::api->run_lua_command("project_getData"), "TestProject") != std::string::npos);

		// Repeat all previous tests (ok not all, but those that check the data) 
		// checking that the info was saved and loaded correctly
		std::string test = main_setup::api->run_lua_command("project_loadID",
			std::vector<std::string> {"1"});
		REQUIRE(main_setup::api->run_lua_command("project_loadID",
			std::vector<std::string> {"1"}).compare("OK") == 0);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::api->run_lua_command("project_getData"), "1") != std::string::npos);

		std::string outy = main_setup::api->run_lua_command("project_getData");
		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::api->run_lua_command("project_getData"), "TestProject") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("project_selectElements",
			std::vector<std::string> {"AL", "ZR"}), "Error") == std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("project_getSelectedElements",
			std::vector<std::string> {""}), "1") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("project_getSelectedElements",
			std::vector<std::string> {""}), "2") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("project_setReferenceElement",
			std::vector<std::string> {"AL"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("project_getReferenceElement",
			std::vector<std::string> {""}), "AL") != std::string::npos);

	}

	SECTION("Create variant cases and run them all")
	{
#pragma region Template_Setup
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_new",
			std::vector<std::string> {""}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setComposition",
			std::vector<std::string> {"AL", "0.90"}), "0.90") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_getComposition",
			std::vector<std::string> {""}), "AL") != std::string::npos);

		// select non existing phase
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_selectPhases",
			std::vector<std::string> {"NONEXIST"}), "Phase does not exis") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_selectPhases",
			std::vector<std::string> {"LIQUID"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_selectPhases",
			std::vector<std::string> {"LIQUID", "FCC"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_getSelectPhases",
			std::vector<std::string> {"LIQUID", "FCC"}), "LIQUID") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_getSelectPhases",
			std::vector<std::string> {"LIQUID", "FCC"}), "FCC") != std::string::npos);


		// Equilibrium
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setEquilibriumTemperatureRange",
			std::vector<std::string> {"100", "600"}), "OK") != std::string::npos);


		//Scheil
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setScheilTemperatureRange",
			std::vector<std::string> {"100", "600"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setScheilLiquidFraction",
			std::vector<std::string> {"0.02"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setScheilDependentPhase",
			std::vector<std::string> {"FCC"}), "OK") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setScheilStepSize",
			std::vector<std::string> {"25"}), "OK") != std::string::npos);

		// Create cases from template
		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_concentrationVariant",
			std::vector<std::string> {"AL", "10", "10", "ZR", "10"}), "Input has to specify three components") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_concentrationVariant",
			std::vector<std::string> {"AL", "10", "10", "ZR", "10", "2"}), "OK") != std::string::npos);
#pragma endregion
	}

}