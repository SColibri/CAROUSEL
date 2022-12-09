#include <catch2/catch_test_macros.hpp>
#include "../../../AMLib/include/AM_Database_TableStruct.h"

TEST_CASE("Table Struct", "[classic]")
{
	SECTION("Functions")
	{
		AM_Database_TableStruct TS;
		TS.add_new("NAME","TEXT");
		REQUIRE(TS.columnNames.size() > 0);
		REQUIRE(TS.columnDataType.size() > 0);
		REQUIRE(std::strcmp(TS.columnNames[0].c_str(), "NAME") == 0);
		REQUIRE(std::strcmp(TS.columnDataType[0].c_str(), "TEXT") == 0);
	}
}