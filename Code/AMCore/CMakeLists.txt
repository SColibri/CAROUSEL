# CMakeList.txt : Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required (VERSION 3.8)
set(CMAKE_CXX_STANDARD 17)

project ("AMCore")
set(BUILD_TESTING false)

if(BUILD_TESTING)
	enable_testing()
	add_subdirectory ("AMTests")
endif()

# Include sub-projects.
add_subdirectory ("AMCore")
add_subdirectory ("AMLib")
add_subdirectory ("AM_API_lib")
