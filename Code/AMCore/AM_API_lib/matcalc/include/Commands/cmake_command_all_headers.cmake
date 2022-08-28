#----------------------------------------------------------------------------------------------------
# *************************************COMMAND ALL HEADER********************************************
#----------------------------------------------------------------------------------------------------
#[[
	This script creates a all header file that includes all available commands, this just avoids you
	typing individual include statements :)
]]

#--------------------------------------------------
#                     OPTIONS
#--------------------------------------------------

# Directory that contains the models
set(DATA_MATCALC_COMMAND_DIRECTORY ${PROJECT_SOURCE_DIR}/AM_API_lib/matcalc/include/Commands)

# Scheme filename
set(MATCALC_COMMAND_ALL_HEADER_FILE ${PROJECT_SOURCE_DIR}/AM_API_lib/matcalc/include/Commands/COMMAND_ALL.h)

#--------------------------------------------------
#                     SCRIPT
#--------------------------------------------------
message("Matcalc API: creating all header file for commands.")

# Get list of all files in DATA_STRUCTURE_DIRECTORY with extension .h
file(GLOB MATCALC_COMMAND_FILES ${DATA_MATCALC_COMMAND_DIRECTORY}/*.h)

# Write file; declare imports and namespace
write_file(${MATCALC_COMMAND_ALL_HEADER_FILE} "#pragma once")

# Parse through all files.
foreach(fname IN LISTS MATCALC_COMMAND_FILES)
	
	# Trim name 
	string(REPLACE "" "" WITHOUT_EXT ${fname})
	string(FIND ${WITHOUT_EXT} "COMMAND_" INDEX_COMMAND)
	string(FIND ${WITHOUT_EXT} "COMMAND_ALL" INDEX_ALL)
	string(LENGTH ${WITHOUT_EXT} LENGTH_COMMAND)
	math(EXPR INCLUDE_SIZE "${LENGTH_COMMAND} - ${INDEX_COMMAND}")

	# Write into file
	if ((${INCLUDE_SIZE} GREATER -1) AND (${INDEX_ALL} EQUAL -1))
		string(SUBSTRING ${WITHOUT_EXT} ${INDEX_COMMAND} ${INCLUDE_SIZE} INCLUDE_NAME)
		write_file(${MATCALC_COMMAND_ALL_HEADER_FILE} "#include \"" ${INCLUDE_NAME} "\"" APPEND)
	    message("Matcalc API: header for ${INCLUDE_NAME} was included")
	else()
		message("Matcalc API: ${fname} was not included as a command")
	endif()

endforeach()

