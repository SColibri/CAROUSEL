#include <catch2/catch_test_macros.hpp>
#include "../../../AMLib/include/AM_Threading.h"

TEST_CASE("Threading", "[classic]")
{
	SECTION("Threading distribution")
	{
		
		std::vector<int> threadWorkload = AMThreading::thread_workload_distribution(150,150);
		REQUIRE(threadWorkload.size() == 150);

		threadWorkload = AMThreading::thread_workload_distribution(10, 150);
		REQUIRE(threadWorkload.size() == 10);

		threadWorkload = AMThreading::thread_workload_distribution(5, 150);
		REQUIRE(threadWorkload.size() == 5);

		threadWorkload = AMThreading::thread_workload_distribution(2, 150);
		REQUIRE(threadWorkload.size() == 2);

		threadWorkload = AMThreading::thread_workload_distribution(150, 15);
		REQUIRE(threadWorkload.size() == 15);

		threadWorkload = AMThreading::thread_workload_distribution(150, 15);
		REQUIRE(threadWorkload.size() == 15);

		threadWorkload = AMThreading::thread_workload_distribution(150, 1);
		REQUIRE(threadWorkload.size() == 1);

		threadWorkload = AMThreading::thread_workload_distribution(2, 21);
		REQUIRE(threadWorkload.size() == 2);
		REQUIRE(threadWorkload[0] == 10);
		REQUIRE(threadWorkload[1] == 11);

		threadWorkload = AMThreading::thread_workload_distribution(1, 21);
		REQUIRE(threadWorkload.size() == 1);
		REQUIRE(threadWorkload[0] == 21);

		threadWorkload = AMThreading::thread_workload_distribution(3, 21);
		REQUIRE(threadWorkload.size() == 3);
		REQUIRE(threadWorkload[0] == 7);
		REQUIRE(threadWorkload[1] == 7);
		REQUIRE(threadWorkload[2] == 7);

		threadWorkload = AMThreading::thread_workload_distribution(3, 22);
		REQUIRE(threadWorkload.size() == 3);
		REQUIRE(threadWorkload[0] == 7);
		REQUIRE(threadWorkload[1] == 7);
		REQUIRE(threadWorkload[2] == 8);

		threadWorkload = AMThreading::thread_workload_distribution(3, 23);
		REQUIRE(threadWorkload.size() == 3);
		REQUIRE(threadWorkload[0] == 7);
		REQUIRE(threadWorkload[1] == 7);
		REQUIRE(threadWorkload[2] == 9);

		threadWorkload = AMThreading::thread_workload_distribution(3, 24);
		REQUIRE(threadWorkload.size() == 3);
		REQUIRE(threadWorkload[0] == 8);
		REQUIRE(threadWorkload[1] == 8);
		REQUIRE(threadWorkload[2] == 8);

		threadWorkload = AMThreading::thread_workload_distribution(3, 25);
		REQUIRE(threadWorkload.size() == 3);
		REQUIRE(threadWorkload[0] == 8);
		REQUIRE(threadWorkload[1] == 8);
		REQUIRE(threadWorkload[2] == 9);

	}

}