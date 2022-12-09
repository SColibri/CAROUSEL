#pragma once
#include <catch2/catch_test_macros.hpp>
#include "../../../AMLib/include/Modeling/STL/AM_STL_Reader.h"


TEST_CASE("Binvox_Reader", "[classic]")
{
	SECTION("Load file")
	{
		auto info = stl_reader::parse_stl("../pigeon.stl");
	}
}
