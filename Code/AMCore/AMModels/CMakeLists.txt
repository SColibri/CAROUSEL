# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)
project(AMModels VERSION 1.0.0 DESCRIPTION "Fraework data models")

# More info on configuring xsd2c++ in cmake can be found here:
# https://github.com/CEINMS/CEINMS/blob/master/cmake/FindXSD.cmake

# xsd
set(XSD_EXECUTABLE "${CMAKE_CURRENT_SOURCE_DIR}/External/xsdInstall/xsd/bin/xsd.exe")
set(XSD_INCLUDE_DIR "${CMAKE_CURRENT_SOURCE_DIR}/External")

set_property(GLOBAL PROPERTY XSD_EXECUTABLE_P "${XSD_EXECUTABLE}")
set_property(GLOBAL PROPERTY XSD_INCLUDE_DIR_P "${XSD_INCLUDE_DIR}")

mark_as_advanced( XSD_INCLUDE_DIR XSD_EXECUTABLE )

# xerces 
set(XERCES_LIBRARY "${CMAKE_CURRENT_SOURCE_DIR}/External/Libs/xerces-c_3.lib")
set(XERCES_INCLUDE_DIR "${CMAKE_CURRENT_SOURCE_DIR}/External/xercesc")
set(XERCES_LIBRARY_DLL "${CMAKE_CURRENT_SOURCE_DIR}/External/Libs/xerces-c_3_1.dll")

set_property(GLOBAL PROPERTY XERCES_INCLUDE_DIR_P "${XERCES_INCLUDE_DIR}")
set_property(GLOBAL PROPERTY XERCES_LIBRARY_DLL_P "${XERCES_LIBRARY_DLL}")
set_property(GLOBAL PROPERTY XERCES_LIBRARY_P "${XERCES_LIBRARY}")

mark_as_advanced( XERCES_LIBRARY XERCES_INCLUDE_DIR XERCES_LIBRARY_DLL )
message("XERCES dll file path: ${XERCES_LIBRARY_DLL}")

# xml input and output paths
set(XSD_SCHEMA_FILE ${CMAKE_CURRENT_SOURCE_DIR}/modelSchema.xsd)
set(XSD_SCHEMA_FILE_OUT ${CMAKE_CURRENT_SOURCE_DIR}/include)
  
# Build from schema configuration/command
add_custom_target(create_schema
                  COMMAND ${XSD_EXECUTABLE} cxx-tree 
                  --std c++11 
                  --type-naming knr
                  --hxx-suffix .h 
                  --cxx-suffix .cpp
                  --generate-serialization
                  --generate-ostream
                  --generate-comparison
                  --root-element-none
                  --output-dir ${XSD_SCHEMA_FILE_OUT}
                  ${XSD_SCHEMA_FILE}
                  DEPENDS ${XSD_SCHEMA_FILE}
                  WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR})

# Add include and source files
file(GLOB AMMODEL_SRC "include/*.cpp")
file(GLOB AMMODEL_H "include/*.h")

# Include xsd
include_directories(${XSD_INCLUDE_DIR})
message("xsd inckude folder is from schema: ${XSD_INCLUDE_DIR}")

# Create library
add_library(AMModels STATIC ${AMMODEL_H} ${AMMODEL_SRC})
target_link_libraries(AMModels PUBLIC "${XERCES_LIBRARY}")

# -------------------------------------------------------
# SCHEMA BUILD
# -------------------------------------------------------

# Check if xsd.exe exists
if(NOT EXISTS ${XSD_EXECUTABLE})
    set(REBUILD_SCHEMA false)
    message("Error: xsd.exe was not found, please follow the readme instructions and try again if you want to re-build the schema")
endif()

if(${REBUILD_SCHEMA})
    # Before build create dependencies
    add_dependencies(AMModels create_schema)
endif()



