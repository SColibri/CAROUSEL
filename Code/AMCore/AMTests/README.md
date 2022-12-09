# Tests

Using Catch2 framework for our testing needs.

## How to use

Don't forget to point the path environment to src/catch2 inside the lib folder (in case you don't have it already).

### Adding tests

Add the test subdirectory to the CMakeLists.txt file (current directory) and inside the subdirectory you can implement a similar CMakeLists.txt file:

```CMake
set(CMAKE_CXX_STANDARD 17)
enable_testing()

# Set a list of test names
set(TEST_SET 
	testName_1
    testName_2
)

foreach(target ${TEST_SET})
    add_executable(${target} "<test_filename.cpp>")
	set_target_properties(${target} PROPERTIES CXX_EXTENSIONS OFF)
    target_compile_features(${target} PUBLIC cxx_std_17)
    target_compile_options(${target} PRIVATE ${DEFAULT_COMPILER_OPTIONS_AND_WARNINGS})
    target_link_libraries(${target} PRIVATE Catch2::Catch2WithMain <List_Needed_Libraries_Here>)

    add_test(NAME ${target} COMMAND ${target})
endforeach()
```