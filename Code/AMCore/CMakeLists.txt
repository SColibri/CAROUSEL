# CMakeList.txt : Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required (VERSION 3.8)
set(CMAKE_CXX_STANDARD 17)

project ("AMCore")
set(BUILD_TESTING true)

# TODO: FUTURE, to avoid adding models into the database scheme manually, cmake can manage this for
# us, this can be done by running this script, more details in the file, for now, we leave it deactivated
# because I should now focus on other details. ()
if(1 MATCHES 2)
	include("AMLib/include/Database_implementations/CMake_Database_scheme_builder.cmake")
	include("AMTests/AMLib/CMake_SQLite_Tests_Generator.cmake")
endif()

if(BUILD_TESTING)
	enable_testing()
	add_subdirectory ("AMTests")
endif()


# Include sub-projects.
add_subdirectory ("AMCore")
add_subdirectory ("AMLib")
add_subdirectory ("AM_API_lib")

