# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)
project(AMLib VERSION 1.0.0 DESCRIPTION "AMFramework library")

# Add source to this project's executable.
file(GLOB AMLIB_SRC "src/*.cpp")
file(GLOB AMLIB_CALLBACK "src/callbackAPI/*.cpp")
file(GLOB AMLIB_DBSRC "src/Database_implementations/*.cpp")

file(GLOB_RECURSE AMLIB_H "include/*.h")
file(GLOB AMLIB_I "interfaces/*.h")
file(GLOB AMLIB_DBH "include/Database_implementations/*.h")
file(GLOB AMLIB_DBH "include/Implementations/*.h")
file(GLOB AMLIB_HELP "x_Helpers/*.h")
file(GLOB AMLIB_MODELING "include/Modeling/*.h")
file(GLOB AMLIB_STL "include/Modeling/STL/*.h")
file(GLOB AMLIB_DS "include/Database_implementations/Data_stuctures/*.h")
file(GLOB AMLIB_EXCEPTIONS "exceptions/*.h")
file(GLOB AMLIB_LUAMODULES "include/lua/*.h")
file(GLOB AMLIB_EVENTS "include/callbackFunctions/*.h")
file(GLOB AMMODEL_H "../AMModels/include/*.h")

# xml input and output paths
set(XSD_SCHEMA_FILE ${CMAKE_CURRENT_SOURCE_DIR}/modelSchema.xsd)
set(XSD_SCHEMA_FILE_OUT ${CMAKE_CURRENT_SOURCE_DIR}/include)

# Add include and source files
file(GLOB AMMODEL_SRC "../AMModels/include/*.cpp")
file(GLOB AMMODEL_H "../AMModels/include/*.h")

# Include AMModels
include_directories("../AMModels/include")

# Include xsd
get_property(XSD_INCLUDE_DIR GLOBAL PROPERTY XSD_INCLUDE_DIR_P)
include_directories(${XSD_INCLUDE_DIR})

add_subdirectory ("external")
add_library(AMLib STATIC ${AMMODEL_H} ${AMLIB_SRC} ${AMLIB_CALLBACK} ${AMLIB_H} ${AMLIB_I} ${AMLIB_DBSRC} ${AMLIB_DBH} ${AMLIB_HELP} ${AMLIB_MODELING} ${AMLIB_STL} ${AMLIB_DS} ${AMLIB_EXCEPTIONS} ${AMLIB_LUAMODULES} ${AMLIB_EVENTS})
target_link_libraries(AMLib ${CMAKE_CURRENT_SOURCE_DIR}/external/lua542/lua54.lib)
target_link_libraries(AMLib sqlite3)
target_link_libraries(AMLib ${CMAKE_CURRENT_SOURCE_DIR}/external/sshlib/ssh.lib)
target_link_libraries(AMLib AMModels)

# target_link_libraries(AMLib ftxui::component)
# target_link_libraries(AMLib ftxui::screen)
# target_link_libraries(AMLib ftxui::dom)