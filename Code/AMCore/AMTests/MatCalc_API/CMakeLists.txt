# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#

set(CMAKE_CXX_STANDARD 17)
enable_testing()

# Add source to this project's executable.
file(GLOB AMCORE_SRC "src/*.cpp")
file(GLOB AMCORE_H "include/*.hpp")
file(GLOB AMLIBRARY "../AMLib/include/*.hpp")
file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/../../AMGUI/windows/AMFramework/AMFramework/lua
     DESTINATION ${CMAKE_CURRENT_BINARY_DIR}) 

set(TEST_SET 
	API_lua_functions_Test
    API_lua_script_test
)

foreach(target ${TEST_SET})
    add_executable(${target} "Tests/${target}.cpp")
	set_target_properties(${target} PROPERTIES CXX_EXTENSIONS OFF)
    target_compile_features(${target} PUBLIC cxx_std_17)
    target_compile_options(${target} PRIVATE ${DEFAULT_COMPILER_OPTIONS_AND_WARNINGS})
    target_link_libraries(${target} AMLib Catch2::Catch2WithMain sqlite3)

    add_test(NAME ${target} COMMAND ${target})
endforeach()



