#pragma once

// c++
#include <string>
#include <vector>
#include <filesystem>

// catch2
#include <catch2/catch_test_macros.hpp>

// Core
#include "../../../AMLib/interfaces/IAM_API.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"

namespace TestSetup
{
	/// <summary>
	/// Gets API dll handle
	/// </summary>
	typedef IAM_API* (__cdecl* MYPROC)(AM_Config*);
	IAM_API* DLL_get(HINSTANCE hLib) {
		MYPROC ProcAdd = (MYPROC)GetProcAddress(hLib, "get_API_Controll");

		if (NULL != ProcAdd)
		{
			AM_Config configuration;
			IAM_API* Result = ProcAdd(&configuration);
			return Result;
		}

		return nullptr;
	}

	/// <summary>
	/// Sets configurations to default values
	/// </summary>
	/// <param name="configuration"></param>
	static void set_config_default(AM_Config* configuration)
	{
		configuration->set_working_directory("");
		configuration->set_apiExternal_path("C:/Program\\ Files/MatCalc\\ 6");
		configuration->set_api_path("../../AM_API_lib/matcalc/AM_MATCALC_Lib.dll");
		configuration->set_ThermodynamicDatabase_path("C:/Program Files/MatCalc 6/database/thermodynamic/mc_al.tdb");
		configuration->set_PhysicalDatabase_path("C:/Program Files/MatCalc 6/database/physical/physical_data.pdb");
		configuration->set_MobilityDatabase_path("C:/Program Files/MatCalc 6/database/diffusion/mc_al.ddb");
		configuration->save();
	}

	/// <summary>
	/// Creates working directory tree, database and config files
	/// </summary>
	/// <param name="configuration"></param>
	static bool remove_previous_database(AM_Config* configuration)
	{
		//Create a new database file
		bool removed{ false };
		std::string filename = configuration->get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "\\" +
			Database_Factory::get_schema() + ".db";
		if (std::filesystem::exists(filename))
			removed = std::filesystem::remove(std::filesystem::path(filename));

		return removed;
	}

	/// <summary>
	/// Returns the database wrapper object
	/// </summary>
	/// <returns></returns>
	static AM_Database_Framework* get_database(AM_Config* configuration)
	{
		AM_Database_Framework* _dbF = new AM_Database_Framework(configuration);
		return _dbF;
	}

#pragma region Testing Variables

	/// <summary>
	/// this is an example of a string containing duplicated phases with # identifiers
	/// and also scheil variable definitions. We are interested on extracting only the phases
	/// that are used and that might not be contained in the database as for example
	/// the generated phases with '#' char.
	/// </summary>
	static inline std::string phaseListString = " This is some previous text \n phases:\n\
			LIQUID                    FCC_A1                    FCC_A1#01\n\
			FCC_A1#02                 BCC_A2                    HCP_A3\n\
			HCP_A3#01                 HCP_A3#02                 DELTA\n\
			GAMMA_DP                  GAMMA_PRIME               LAV_C14\n\
			LAVES                     MU_PHASE                  NIAL\n\
			NIAL#01                   NI2CR                     NITI2\n\
			SIGMA                     M6C                       M23C6\n\
			FCC_A1_S                  FCC_A1#01_S               FCC_A1#02_S\n\
			BCC_A2_S                  HCP_A3_S                  HCP_A3#01_S\n\
			HCP_A3#02_S               DELTA_S                   GAMMA_DP_S\n\
			GAMMA_PRIME_S             LAV_C14_S                 LAVES_S\n\
			MU_PHASE_S                NIAL_S                    NIAL#01_S\n\
			NI2CR_S                   NITI2_S                   SIGMA_S\n\
			M6C_S                     M23C6_S";


	/// <summary>
	/// List of phases contained in databases
	/// </summary>
	static inline std::vector<std::string> phaseDatabaseList{ "LIQUID","BCC_A2","GAMMA_DP","FCC_A1","HCP_A3"};

	/// <summary>
	/// Mixed phase list that contains phases from database and generated ones
	/// </summary>
	static inline std::vector<std::string> phaseList{ "LIQUID","BCC_A2","M23C6_S","GAMMA_DP","FCC_A1","HCP_A3#02_S","FCC_A1#01","FCC_A1#01_S","HCP_A3#02"};

	/// <summary>
	/// Example of vector that contains generated phases that contain the "#". 
	/// Phases with different constituents .
	/// </summary>
	/// <returns></returns>
	static inline std::vector<std::string> generatedPhaseList{"HCP_A3#02_S","FCC_A1#01","FCC_A1#01_S","HCP_A3#02"};
#pragma endregion

}