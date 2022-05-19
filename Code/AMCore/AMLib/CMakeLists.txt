# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)
project(AMLib VERSION 1.0.1 DESCRIPTION "AMFramework library")

# Add source to this project's executable.
file(GLOB AMLIB_SRC "src/*.cpp")
file(GLOB AMLIB_H "include/*.h")
file(GLOB AMLIB_I "interfaces/*.h")
file(GLOB AMLIB_DBH "include/Database_implementations/*.h")
file(GLOB AMLIB_DBSRC "src/Database_implementations/*.cpp")

add_subdirectory ("external")
add_library(AMLib STATIC ${AMLIB_SRC} ${AMLIB_H} ${AMLIB_I} ${AMLIB_DBSRC} ${AMLIB_DBH})
target_link_libraries(AMLib ${CMAKE_CURRENT_SOURCE_DIR}/external/lua542/lua54.lib)
target_link_libraries(AMLib sqlite3) 
# TODO: Add tests and install targets if needed.
