# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#

set(CMAKE_CXX_STANDARD 17)
enable_testing()

set(TEST_SET 
	AM_Config_Test
    AM_Database_Test
)

foreach(target ${TEST_SET})
    add_executable(${target} "Tests/${target}.cpp")
	set_target_properties(${target} PROPERTIES CXX_EXTENSIONS OFF)
    target_compile_features(${target} PUBLIC cxx_std_17)
    target_compile_options(${target} PRIVATE ${DEFAULT_COMPILER_OPTIONS_AND_WARNINGS})
    target_link_libraries(${target} PRIVATE AMLib Catch2::Catch2WithMain sqlite3)

    add_test(NAME ${target} COMMAND ${target})
endforeach()



