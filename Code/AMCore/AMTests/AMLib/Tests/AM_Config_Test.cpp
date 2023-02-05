#include "../../../AMLib/include/AM_Config.h"
#include <catch2/catch_test_macros.hpp>


TEST_CASE("AM_Config", "[classic]")
{
	SECTION("Saving and loading file")
	{
		AM_Config testConfig;
		testConfig.set_config_name("NameChanged");
		testConfig.set_api_path("Path to API");
		testConfig.set_apiExternal_path("Path to software");
		testConfig.set_working_directory("OkyTests");
		testConfig.save();
		std::string pathToTest = testConfig.get_filename();

		// Loading a config file
		AM_Config testConfig_Compare(pathToTest);
		std::string test01 = testConfig_Compare.get_filename();
		std::string test02 = testConfig.get_filename();
		REQUIRE(std::strcmp(testConfig_Compare.get_filename().c_str(), 
							testConfig.get_filename().c_str()) == 0);
		REQUIRE(std::strcmp(testConfig_Compare.get_api_path().c_str(),
			testConfig.get_api_path().c_str()) == 0);
	}
}



