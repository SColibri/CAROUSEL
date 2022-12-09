#pragma once
#include <catch2/catch_test_macros.hpp>
#include "../../../AMLib/include/Database_implementations/Data_Controller.h"
#include "../../../AMLib/interfaces/IAM_Database.h"
#include "../../../AMLib/include/Database_implementations/Database_Sqlite3.h"
#include "../../../AMLib/include/AM_Config.h"

TEST_CASE("Data controller", "[classic]")
{
	SECTION("General setup")
	{
		AM_Config configuration;
		Database_Sqlite3 _db(&configuration);
		//Data_Controller DC((IAM_Database*) & _db, &configuration, -1);
	}

}