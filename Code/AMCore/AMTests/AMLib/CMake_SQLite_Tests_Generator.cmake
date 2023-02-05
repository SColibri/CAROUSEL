#----------------------------------------------------------------------------------------------------
# ****************************DATABASE SQLITE3 TESTS GENERATOR***************************************
#----------------------------------------------------------------------------------------------------
#[[
	This script creates basic tests for saving and loading data into the database using SQLite3
	database engine.
]]

#--------------------------------------------------
#                     OPTIONS
#--------------------------------------------------

# Directory that contains the Data structures
set(DATA_STRUCTURE_DIRECTORY ${PROJECT_SOURCE_DIR}/AMLib/include/Database_implementations/Data_stuctures)

# Name that descries the test
set(TEST_NAME "SQLite3_Tests")

# Other modules
include(${PROJECT_SOURCE_DIR}/AMLib/include/Database_implementations/CMake_Database_Methods.cmake)


#--------------------------------------------------
#                     SCRIPT
#--------------------------------------------------

# Get list of all files in DATA_STRUCTURE_DIRECTORY with extension .h
file(GLOB STRUCTURE_FILES ${DATA_STRUCTURE_DIRECTORY}/*.h)

# test filename
set(SCHEME ${PROJECT_SOURCE_DIR}/AMLib/include/Database_implementations/AM_${TEST_NAME}.cpp)

# Includes
write_file(${SCHEME} "#pragma once")

# Test Body
write_file(${SCHEME} "TEST_CASE(\"Database\", \"[classic]\")" APPEND)
write_file(${SCHEME} "{" APPEND)

# Test create new database using default parameters
write_file(${SCHEME} "	SECTION(\"Create a sqlite database\")" APPEND)
write_file(${SCHEME} "	{" APPEND)
write_file(${SCHEME} "		AM_Config config01;" APPEND)
write_file(${SCHEME} "		Database_Sqlite3 db(&config01);" APPEND)
write_file(${SCHEME} "		int Response = db.connect();" APPEND)
write_file(${SCHEME} "		REQUIRE(Response == 0);" APPEND)
write_file(${SCHEME} "	}\n" APPEND)

# Save and load test
write_file(${SCHEME} "	SECTION(\"Save and Load\")" APPEND)
write_file(${SCHEME} "	{" APPEND)
foreach(fname IN LISTS STRUCTURE_FILES)
	# DBName
    get_filename_component(DBNAME ${fname} NAME)
    string(REGEX REPLACE ".cpp" "" DBNAME ${fname})

	write_file(${SCHEME} "// ${DBNAME}" APPEND)
	# Tests
	get_dbs_parameters(${fname} PARAMETERS_NAMES PARAMETERS_TYPES)
	message("Parameters: ${PARAMETERS_NAMES}")
	message("Types: ${PARAMETERS_TYPES}")

	write_file(${SCHEME} "\n" APPEND)
endforeach()
write_file(${SCHEME} "	}" APPEND)

write_file(${SCHEME} "}" APPEND)