#include <catch2/catch_test_macros.hpp>

#include <windows.h>
#include <mutex>
#include "../../../AMLib/include/AM_Project.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/include/AM_lua_interpreter.h"
#include "../../../AMLib/interfaces/IAM_API.h"
#include "../../../AMLib/interfaces/IAM_lua_functions.h"	
#include "../../../AMLib/include/Database_implementations/Database_Factory.h"
#include "../../../AMLib/x_Helpers/IPC_pipe.h"
#include "../../../AMLib/x_Helpers/IPC_winapi.h"
namespace main_setup
{
	static bool isLoaded{ false };
	static IAM_API* api{ nullptr };
	static AM_Config configuration;
	static AM_Database_Framework* _dbF{ nullptr };
	static IAM_Database* _db{ nullptr };
	static HINSTANCE _library{ NULL };
	static std::mutex LoadProject_mutex;

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
		configuration.set_PhysicalDatabase_path("C:/Program\\ Files/MatCalc\\ 6/database/physical/physical_data.pdb");
		configuration.set_MobilityDatabase_path("C:/Program\\ Files/MatCalc\\ 6/database/diffusion/mc_al.ddb");

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
		isLoaded = true;
	}

	static void parallel_run(std::vector<std::string> ListIDCases)
	{
		LoadProject_mutex.lock();
		HINSTANCE library = LoadLibrary(TEXT(configuration.get_api_path().c_str()));
		IAM_API* api = DLL_get(library);
		
		
		api->run_lua_command("project_loadID", std::vector<std::string> {"1"});
		LoadProject_mutex.unlock();

		for (int n1 = 0; n1 < ListIDCases.size(); n1++)
		{
			std::string outAll = main_setup::api->run_lua_command("pixelcase_stepEquilibrium",
				std::vector<std::string> {ListIDCases[n1]});

			std::string outAll_Scheil = main_setup::api->run_lua_command("pixelcase_stepScheil",
				std::vector<std::string> {ListIDCases[n1]});
		}
		

		api->dispose();
		
	}
}

TEST_CASE("Initialize for all")
{
	main_setup::init();
}

TEST_CASE("IAM_lua_functions")
{
	while(main_setup::isLoaded == false)
	{
		std::this_thread::sleep_for(std::chrono::seconds(3));
	}

	SECTION("Test initialize")
	{
		REQUIRE(main_setup::_db != nullptr);

		REQUIRE(main_setup::api->run_lua_command("configuration_setAPI_path",
			std::vector<std::string> {main_setup::configuration.get_api_path()}).compare("OK") == 0);

		REQUIRE(main_setup::api->run_lua_command("configuration_setExternalAPI_path",
			std::vector<std::string> {main_setup::configuration.get_apiExternal_path()}).compare("OK") == 0);

		REQUIRE(main_setup::api->run_lua_command("configuration_set_working_directory",
			std::vector<std::string> {main_setup::configuration.get_working_directory()}).compare("OK") == 0);

		std::string config = main_setup::api->run_lua_command("configuration_set_thermodynamic_database_path",
			std::vector<std::string> {main_setup::configuration.get_ThermodynamicDatabase_path()});

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("configuration_set_thermodynamic_database_path",
			std::vector<std::string> {main_setup::configuration.get_ThermodynamicDatabase_path()}), "OK") != std::string::npos);

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

		AM_Database_Datatable dP(main_setup::_db, &AMLIB::TN_Phase());
		dP.load_data();

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

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::api->run_lua_command("project_getData"), "TestProject") != std::string::npos);

		// Repeat all previous tests (ok not all, but those that check the data) 
		// checking that the info was saved and loaded correctly
		REQUIRE(main_setup::api->run_lua_command("project_loadID",
			std::vector<std::string> {"1"}).compare("OK") == 0);

		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::api->run_lua_command("project_getData"), "1") != std::string::npos);

		std::string outy = main_setup::api->run_lua_command("project_getData");
		REQUIRE(string_manipulators::find_index_of_keyword(
			main_setup::api->run_lua_command("project_getData"), "TestProject") != std::string::npos);

		REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("project_selectElements",
			std::vector<std::string> {"AL", "MG", "SI", "TI", "CU", "CR", "FE", "MN", "ZN", "VA"}), "Error") == std::string::npos);

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
				std::vector<std::string> {	"MG", "1.0",
											"SI", "0.6",
											"TI", "1.0",
											"CU", "0.3",
											"CR", "0.1",
											"FE", "0.001",
											"MN", "0.001",
											"ZN", "0.001"}), "1.00") != std::string::npos);

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_getComposition",
				std::vector<std::string> {""}), "AL") != std::string::npos);

			// select non existing phase
			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_selectPhases",
				std::vector<std::string> {"NONEXIST"}), "Phase does not exis") != std::string::npos);

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_selectPhases",
				std::vector<std::string> {"LIQUID"}), "OK") != std::string::npos);

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_selectPhases",
				std::vector<std::string> {"LIQUID", "FCC_A1", "AL13CR4SI4", "ALCRFEMNSI_A", "AL12MN", "AL3TI_L",
				"MG2SI_B", "S_PHASE", "THETA_AL2CU" }), "OK") != std::string::npos);

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_getSelectPhases",
				std::vector<std::string> {""}), "LIQUID") != std::string::npos);

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_getSelectPhases",
				std::vector<std::string> {""}), "THETA_AL2CU") != std::string::npos);


			// Equilibrium
			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setEquilibriumTemperatureRange",
				std::vector<std::string> {"700", "25"}), "OK") != std::string::npos);


			//Scheil
			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setScheilTemperatureRange",
				std::vector<std::string> {"700", "25"}), "OK") != std::string::npos);

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setScheilLiquidFraction",
				std::vector<std::string> {"0.01"}), "OK") != std::string::npos);

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setScheilDependentPhase",
				std::vector<std::string> {"LIQUID"}), "OK") != std::string::npos);

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_setScheilStepSize",
				std::vector<std::string> {"1"}), "OK") != std::string::npos);

			// Create cases from template
			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("template_pixelcase_concentrationVariant",
				std::vector<std::string> {"FE", "0.001", "10",
											"MN", "0.001", "5",
											"ZN", "0.001", "3"}), "OK") != std::string::npos);

			//IPC_pipe::IPC_pipe tempOne("C:/Program Files/MatCalc 6/mcc.exe");
			//tempOne.send_command("use-module core \r\n");

			//IPC_winapi tempTwo(L"C:/Program Files/MatCalc 6/mcc.exe");
			//tempTwo.set_endflag("MC:");
			//std::string outResponse = tempTwo.send_command("use-module core\r\n");
			//std::string outResponse_2 = tempTwo.send_command("exit\r\n");
			// pixelcase_stepEquilibrium
			 // REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("pixelcase_stepEquilibrium",
			//	std::vector<std::string> {"1"}), "OK") != std::string::npos);

			main_setup::_db->connect();
			AM_Project tempProject(main_setup::_db, &main_setup::configuration, 1);
			AM_pixel_parameters* pixel_parameters = tempProject.get_pixelCase(1);
			//main_setup::_db->disconnect();

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("matcalc_initialize_core",
				std::vector<std::string> {""}), "MC:") != std::string::npos);

			//REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("matcalc_set_working_directory",
			//	std::vector<std::string> {""}), "Exit code: 0") == std::string::npos);

			std::string comm_01 = main_setup::api->run_lua_command("matcalc_open_thermodynamic_database",
				std::vector<std::string> {main_setup::configuration.get_ThermodynamicDatabase_path()});

			std::string comm_02 = main_setup::api->run_lua_command("matcalc_select_elements",
				std::vector<std::string> {""});

			std::string comm_03 = main_setup::api->run_lua_command("matcalc_selectPhases",
				std::vector<std::string> {pixel_parameters->get_selected_phases_ByName()});

			std::string comm_04 = main_setup::api->run_lua_command("matcalc_read_thermodynamic_database",
				std::vector<std::string> {""});

			std::string comm_05 = main_setup::api->run_lua_command("matcalc_setReferenceElement",
				std::vector<std::string> {"AL"});

			std::string test01 = main_setup::api->run_lua_command("matcalc_set_step_option",
				std::vector<std::string> {"range start=700 stop=25 step-width=1 " });

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("matcalc_set_step_option",
				std::vector<std::string> {"range start=700 stop=25 step-width=1 "}), "MC:") != std::string::npos);

			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("matcalc_set_step_option",
				std::vector<std::string> {"scheil-create-phases-automatically=yes" }), "MC:") != std::string::npos);

			std::string outTest_1 = main_setup::api->run_lua_command("matcalc_read_thermodynamic_database",
				std::vector<std::string> {"" });

			std::string outTest = main_setup::api->run_lua_command("matcalc_step_equilibrium",
				std::vector<std::string> {"" });

			
			REQUIRE(string_manipulators::find_index_of_keyword(main_setup::api->run_lua_command("matcalc_step_equilibrium",
				std::vector<std::string> {"" }), "MC:") != std::string::npos);


			tempProject.refresh_data();

		std::string outAll = main_setup::api->run_lua_command("pixelcase_step_equilibrium_parallel",
				std::vector<std::string> {"1","1-150"});

			/*
			for (int n1 = 0; n1 < tempProject.get_singlePixel_Cases().size(); n1++)
			{
				std::string outAll = main_setup::api->run_lua_command("pixelcase_stepEquilibrium",
					std::vector<std::string> {std::to_string(tempProject.get_singlePixel_Cases()[n1]->get_caseID())});
				 
				std::string outAll_Scheil = main_setup::api->run_lua_command("pixelcase_stepScheil",
					std::vector<std::string> {std::to_string(tempProject.get_singlePixel_Cases()[n1]->get_caseID())});
			}*/
			



#pragma endregion
		}



}