#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database.h"
#include <catch2/catch_test_macros.hpp>


TEST_CASE("Database", "[classic]")
{
	SECTION("Create a database")
	{
		AM_Database db;
		int Response = db.connect("dbTest.db");

		REQUIRE(Response == Response);
	}

}
