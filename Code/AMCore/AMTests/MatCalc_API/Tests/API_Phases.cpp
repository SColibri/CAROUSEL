#include <catch2/catch_test_macros.hpp>
#include "../../../AMLib/include/AM_Project.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/include/AM_lua_interpreter.h"
#include "../../../AMLib/interfaces/IAM_API.h"
#include "../../../AMLib/interfaces/IAM_lua_functions.h"	
#include "../../../AMLib/include/Database_implementations/Database_Factory.h"
#include "../../../AMLib/x_Helpers/IPC_winapi.h"

namespace main_setup
{
	static bool isLoaded{ false };
	static IAM_API* api{ nullptr };
	static AM_Config configuration;
	static AM_Database_Framework* _dbF{ nullptr };
	static IAM_Database* _db{ nullptr };
	static HINSTANCE _library{ NULL };

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
		configuration.set_working_directory("");
		configuration.set_apiExternal_path("C:/Program Files/MatCalc 6");
		configuration.set_api_path("../../AM_API_lib/matcalc/AM_MATCALC_Lib.dll");

		// NOTE: when passing paths to matcalc we have to pass with escape characters, otherwise this will not work
		// when passing the argument to matcalc.
		configuration.set_ThermodynamicDatabase_path("C:/Program Files/MatCalc 6/database/thermodynamic/mc_al.tdb");
		configuration.set_PhysicalDatabase_path("C:/Program Files/MatCalc 6/database/physical/physical_data.pdb");
		configuration.set_MobilityDatabase_path("C:/Program Files/MatCalc 6/database/diffusion/mc_al.ddb");

		char pBuf[256];
		size_t len = sizeof(pBuf);
		int bytes = GetModuleFileName(NULL, pBuf, len);
		std::filesystem::create_directory(std::filesystem::path(configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE)));

		//Create a new database file
		bool removed{ false };
		std::string filename = configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "\\" +
			Database_Factory::get_schema() + ".db";
		if (std::filesystem::exists(filename))
			removed = std::filesystem::remove(std::filesystem::path(filename));

		_dbF = new AM_Database_Framework(&configuration);
		_db = _dbF->get_database();

		_library = LoadLibrary(TEXT(configuration.get_api_path().c_str()));
		api = DLL_get(_library);
		isLoaded = true;
	}
}

TEST_CASE("Load Phases")
{
	main_setup::init();
	
	// -------------------------------------------------------
	//					ALUMINIUM DATABASE
	// -------------------------------------------------------
	
	// Load phases from the AL database
	main_setup::configuration.set_ThermodynamicDatabase_path("C:/Program Files/MatCalc 6/database/thermodynamic/mc_al.tdb");
	main_setup::api->run_lua_command("get_phaseNames");

	// -------------------------------------------------------
	//					NIQUEL DATABASE
	// -------------------------------------------------------

	// load phases from the Ni database
	main_setup::configuration.set_ThermodynamicDatabase_path("C:/Program Files/MatCalc 6/database/thermodynamic/mc_ni.tdb");
	main_setup::api->run_lua_command("get_phaseNames");

}