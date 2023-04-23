#pragma once

// c++
#include <string>
#include <vector>

// catch2
#include <catch2/catch_test_macros.hpp>

// Core
#include "../../../AMLib/include/AM_Project.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/x_Helpers/string_manipulators.h"

// Matcalc API
#include "../../../AM_API_lib/matcalc/include/Extension/PhasesExtension.h"

// Local
#include "API_Test_setup.h"

TEST_CASE("PhaseExtensions")
{
	// configuration data
	AM_Config configuration;
	TestSetup::set_config_default(&configuration);

	// database connection
	AM_Database_Framework* dbf = TestSetup::get_database(&configuration);

	// Gets phases with the '#' identifier
	SECTION("get_created_phase_names")
	{
		// Execute command
		std::vector<std::string> phaseList = AMFramework::PhaseExtension::get_created_phase_names(TestSetup::phaseList);

		// only 4 phases with #
		REQUIRE(phaseList.size() == 4);
		for (auto& phaseName : phaseList)
		{
			// Check if correct phases are added
			REQUIRE(string_manipulators::find_index_of_keyword(phaseName, "#") != std::string::npos);
		}
	}

	// Check if loaded phases into memory are found
	SECTION("is_contained")
	{
		// Load dummy phases into memory
		for (auto& phaseName : TestSetup::generatedPhaseList)
		{
			DBS_Phase* phase = new DBS_Phase(dbf->get_database(), -1);
			phase->Name = phaseName;
			AMFramework::PhaseExtension::phasesInMemory.insert(std::make_pair(phase->Name, phase));
		}

		// All phases should be found
		REQUIRE(AMFramework::PhaseExtension::is_contained(TestSetup::generatedPhaseList));

		// Not all phases are found
		REQUIRE(!AMFramework::PhaseExtension::is_contained(TestSetup::phaseList));
	}

	// Loads phase to memory, if it does not exist, new phase is created and loaded
	// Only new phases will get added as DBType = 1 -> not selectable
	SECTION("add_created_phases")
	{
		// Add phases, only the ones with the '#' identifier
		AMFramework::PhaseExtension::add_created_phases(dbf->get_database(), TestSetup::phaseList);

		// Not all phases are found
		REQUIRE(!AMFramework::PhaseExtension::is_contained(TestSetup::phaseList));
	}
}