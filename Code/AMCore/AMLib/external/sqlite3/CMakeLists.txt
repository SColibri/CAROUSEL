# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)
project(sqlite3 VERSION 1.0.1 DESCRIPTION "sqlite3 library")

# Add source to this project's executable.
file(GLOB sqlite3_SRC "*.c")
file(GLOB sqlite3_H "*.h")

add_library(sqlite3 STATIC ${sqlite3_SRC} ${sqlite3_H})

# TODO: Add tests and install targets if needed.
