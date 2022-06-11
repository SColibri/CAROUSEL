#include <catch2/catch_test_macros.hpp>

#include "../../../AMLib/include/AM_Project.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/include/AM_lua_interpreter.h"
#include "../../../AMLib/interfaces/IAM_lua_functions.h"	
#include "../../../AMLib/include/Database_implementations/Database_Factory.h"
#include "../../../AM_API_lib/matcalc/include/API_lua_functions.h"

namespace main_setup
{
	static AM_Config configuration;
	static AM_Database_Framework* _dbF{ nullptr };
	static IAM_Database* _db{ nullptr };
	static lua_State* state{ nullptr };
	static AM_lua_interpreter* interpreter{ nullptr };
	static API_lua_functions* apiLua{ nullptr };

	static void init()
	{
		configuration.set_working_directory("");
		Database_Factory::set_schema("projectDB");

		//Create a new database file
		std::string filename = configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "/" +
			Database_Factory::get_schema() + ".db";
		if (std::filesystem::exists(filename))
			std::filesystem::remove(std::filesystem::path(filename));

		_dbF = new AM_Database_Framework(&configuration);
		_db = _dbF->get_database();

		// create a API_lua object
		interpreter = new AM_lua_interpreter();
		state = interpreter->get_state();

		apiLua = new API_lua_functions(state, &configuration);
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
		// 
		REQUIRE(main_setup::apiLua->get_list_functions().size() != 0);

	}

}