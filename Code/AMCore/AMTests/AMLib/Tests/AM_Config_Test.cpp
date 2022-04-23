#include "../../../AMLib/include/AM_Config.h"
#include <catch2/catch_test_macros.hpp>


TEST_CASE("AM_Config", "[classic]")
{
	SECTION("Saving and loading file")
	{
		AM_Config testConfig;
		testConfig.set_config_name("NameChanged");
		testConfig.save();
		std::string pathToTest = testConfig.get_filename();

		// Loading a config file
		AM_Config testConfig_Compare(pathToTest);
		REQUIRE(testConfig_Compare.get_filename() == testConfig.get_filename());
	}

	SECTION("Test2")
	{
		AM_Config testConfig;
		testConfig.set_config_name("NameChanged");
		testConfig.save();
		std::string pathToTest = testConfig.get_filename();

		// Loading a config file
		AM_Config testConfig_Compare(pathToTest);
		REQUIRE(1 == 1);
	}

}



