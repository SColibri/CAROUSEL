#pragma once
#include "../../../AMLib/include/IPCWindows.h"
#include "../../../AMLib/include/AM_Config.h"
#include <catch2/catch_test_macros.hpp>
#include <string>
#include <codecvt>

TEST_CASE("IPCWindows", "[classic]")
{
	SECTION("Start process and read output")
	{
		AM_Config configuration;
		configuration.set_working_directory("C:/Users/drogo/Desktop/Homless");
		configuration.set_apiExternal_path("C:/Program Files/MatCalc 6");
		configuration.set_api_path("../../AM_API_lib/matcalc/AM_MATCALC_Lib.dll");
		configuration.set_ThermodynamicDatabase_path("C:/Program\\ Files/MatCalc\\ 6/database/thermodynamic/mc_al.tdb");
		configuration.set_PhysicalDatabase_path("C:/Program\\ Files/MatCalc\\ 6/database/physical/physical_data.pdb");
		configuration.set_MobilityDatabase_path("C:/Program\\ Files/MatCalc\\ 6/database/diffusion/mc_al.ddb");


		// For this test you need a working subprocess
		std::wstring externalPath = L"C:/Program Files/MatCalc 6/mcc.exe";
		AMFramework::IPC::IPCWindows ipc(externalPath);
		AMFramework::IPC::IPCWindows ipc_clone(externalPath);
		std::this_thread::sleep_for(std::chrono::seconds(3));
		std::string endFlag = "MC:";
		std::string outMessage = ipc.send_command("this command\r\n");
		ipc.Dispose();
	}
}