# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)
project(AMLib VERSION 1.0.1 DESCRIPTION "AMFramework library")

# Add source to this project's executable.
file(GLOB AMLIB_SRC "src/*.cpp")
file(GLOB AMLIB_H "include/*.hpp")
file(GLOB AMLIB_I "interfaces/*.hpp")

add_library(AMLib STATIC ${AMLIB_SRC} ${AMLIB_H} ${AMLIB_I})

# TODO: Add tests and install targets if needed.
