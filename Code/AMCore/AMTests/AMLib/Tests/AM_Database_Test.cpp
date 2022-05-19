#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/Database_implementations/Database_Sqlite3.h"
#include <catch2/catch_test_macros.hpp>


TEST_CASE("Database", "[classic]")
{
	SECTION("Create a database")
	{
		Database_Sqlite3 db;
		int Response = db.connect("dbTest.db");

		REQUIRE(Response == Response);
	}

}
