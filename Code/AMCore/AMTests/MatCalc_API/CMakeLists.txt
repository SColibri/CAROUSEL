# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#

set(CMAKE_CXX_STANDARD 17)
enable_testing()

# Add source to this project's executable.
file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/../../AMGUI/windows/AMFramework/AMFramework/lua
     DESTINATION ${CMAKE_CURRENT_BINARY_DIR}) 
message(${CMAKE_CURRENT_BINARY_DIR})

# Data models require xercesc library
file(COPY ${XERCES_LIBRARY_DLL}
     DESTINATION ${CMAKE_CURRENT_BINARY_DIR}) 

# Directory that contains the tests
set(APILib_MATCALC_TEST_DIRECTORY ${PROJECT_SOURCE_DIR}/AMTests/MatCalc_API/Tests)

# Get list of all files
file(GLOB APILib_MATCALC_TESTS ${APILib_MATCALC_TEST_DIRECTORY}/*.cpp)
file(GLOB APILib_MATCALC_H ${APILib_MATCALC_TEST_DIRECTORY}/*.h)

set(XSD_INCLUDE_DIR "C:/Users/drogo/Downloads/xsd/libxsd")

# Include xsd
get_property(XSD_INCLUDE_DIR GLOBAL PROPERTY XSD_INCLUDE_DIR_P)
include_directories(${XSD_INCLUDE_DIR})

message("xsd inckude folder is: ${XSD_INCLUDE_DIR}")

# Create all tests
foreach(target IN LISTS APILib_MATCALC_TESTS)
    
    # Get test name
    get_filename_component(fname ${target} NAME)
    string(REGEX REPLACE ".cpp" "" fname ${fname})

    
    # create tests
    message("Looking into filename: ${fname}")
    add_executable(${fname} "${target}" ${APILib_MATCALC_H})
	set_target_properties(${fname} PROPERTIES CXX_EXTENSIONS OFF)
    target_compile_features(${fname} PUBLIC cxx_std_17)
    target_compile_options(${fname} PRIVATE ${DEFAULT_COMPILER_OPTIONS_AND_WARNINGS})
    target_link_libraries(${fname} PRIVATE AMLib AMModels Catch2::Catch2WithMain sqlite3)
    
    add_test(NAME ${fname} COMMAND ${fname})
endforeach()



