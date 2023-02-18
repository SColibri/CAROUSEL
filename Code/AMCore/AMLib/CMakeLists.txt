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
file(GLOB AMLIB_DBH "include/Implementations/*.h")
file(GLOB AMLIB_DBSRC "src/Database_implementations/*.cpp")
file(GLOB AMLIB_HELP "x_Helpers/*.h")
file(GLOB AMLIB_MODELING "include/Modeling/*.h")
file(GLOB AMLIB_STL "include/Modeling/STL/*.h")
file(GLOB AMLIB_DS "include/Database_implementations/Data_stuctures/*.h")
file(GLOB AMLIB_EXCEPTIONS "exceptions/*.h")


add_subdirectory ("external")
add_library(AMLib STATIC ${AMLIB_SRC} ${AMLIB_H} ${AMLIB_I} ${AMLIB_DBSRC} ${AMLIB_DBH} ${AMLIB_HELP} ${AMLIB_MODELING} ${AMLIB_STL} ${AMLIB_DS} ${AMLIB_EXCEPTIONS})
target_link_libraries(AMLib ${CMAKE_CURRENT_SOURCE_DIR}/external/lua542/lua54.lib)
target_link_libraries(AMLib sqlite3) 
target_link_libraries(AMLib ftxui::component)
target_link_libraries(AMLib ftxui::screen)
target_link_libraries(AMLib ftxui::dom)
target_link_libraries(AMLib ${CMAKE_CURRENT_SOURCE_DIR}/external/lua542/lua54.lib)
