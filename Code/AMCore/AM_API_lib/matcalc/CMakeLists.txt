# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)
project(AM_MATCALC_Lib VERSION 1.0.0 DESCRIPTION "AMFramework library using matcalc APIs")

# Add source to this project's executable.
file(GLOB AM_MATCALC_LIB_SRC "src/*.cpp")
file(GLOB_RECURSE AM_MATCALC_LIB_H "include/*.h")
file(GLOB AM_MATCALC_LIBCOMMANDS_H "include/Commands/*.h")

# Data models require xercesc library
get_property(XERCES_LIBRARY_DLL GLOBAL PROPERTY XERCES_LIBRARY_DLL_P)

file(COPY ${XERCES_LIBRARY_DLL}
     DESTINATION "${CMAKE_CURRENT_BINARY_DIR}/${CMAKE_BUILD_TYPE}") 
message("xerces dll is copied from: ${XERCES_LIBRARY_DLL} \nto: ${CMAKE_CURRENT_BINARY_DIR}")

# Include xsd
get_property(XSD_INCLUDE_DIR GLOBAL PROPERTY XSD_INCLUDE_DIR_P)

include_directories(${XSD_INCLUDE_DIR})
message("include dir is ${XSD_INCLUDE_DIR}")

add_library(AM_MATCALC_Lib SHARED AM_API_Matcalc.cpp AM_API_Matcalc.h ${AM_MATCALC_LIB_SRC} ${AM_MATCALC_LIB_H} ${AM_MATCALC_LIBCOMMANDS_H} "include/Commands/COMMAND_start_precipitate_simulation.h")
target_link_libraries(AM_MATCALC_Lib AMLib)

# target_link_libraries(AM_MATCALC_Lib "C:/Program Files/MatCalc 6/mc_core.lib")


# TODO: Add tests and install targets if needed.

# Include packages
include("../../package.cmake")

# Add a custom command to run after building
add_custom_command(
    TARGET ${PROJECT_NAME} POST_BUILD
    COMMAND ${CMAKE_COMMAND} -P "${CMAKE_SOURCE_DIR}/package.cmake"  -E echo "Build complete."
)

# Add a custom target that depends on the custom command
add_custom_target(
    MyPostBuild
    DEPENDS ${PROJECT_NAME}
)