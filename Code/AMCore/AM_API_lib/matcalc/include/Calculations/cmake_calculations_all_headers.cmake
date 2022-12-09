#----------------------------------------------------------------------------------------------------
# *************************************CALCULATIONS ALL HEADER***************************************
#----------------------------------------------------------------------------------------------------
#[[
	This script creates a all header file that includes all available calculations, this just avoids you
	typing individual include statements :)
]]

#--------------------------------------------------
#                     OPTIONS
#--------------------------------------------------

# Directory that contains the models
set(DATA_MATCALC_CALCULATIONS_DIRECTORY ${PROJECT_SOURCE_DIR}/AM_API_lib/matcalc/include/Calculations)

# Scheme filename
set(MATCALC_CALCULATIONS_ALL_HEADER_FILE ${PROJECT_SOURCE_DIR}/AM_API_lib/matcalc/include/Calculations/CALCULATION_ALL.h)

#--------------------------------------------------
#                     SCRIPT
#--------------------------------------------------
message("Matcalc API: creating all header file for calculations.")

# Get list of all files in DATA_STRUCTURE_DIRECTORY with extension .h
file(GLOB MATCALC_CALCULATIONS_FILES ${DATA_MATCALC_CALCULATIONS_DIRECTORY}/*.h)

# Write file; declare imports and namespace
write_file(${MATCALC_CALCULATIONS_ALL_HEADER_FILE} "#pragma once")

# Parse through all files.
foreach(fname IN LISTS MATCALC_CALCULATIONS_FILES)
	
	# Trim name 
	string(REPLACE "" "" WITHOUT_EXT ${fname})
	string(FIND ${WITHOUT_EXT} "CALCULATION_" INDEX_CALC)
	string(FIND ${WITHOUT_EXT} "CALCULATION_ALL" INDEX_ALL)
	string(LENGTH ${WITHOUT_EXT} LENGTH_CALC)
	math(EXPR INCLUDE_SIZE "${LENGTH_CALC} - ${INDEX_CALC}")

	# Write into file
	if ((${INCLUDE_SIZE} GREATER -1) AND (${INDEX_CALC} GREATER -1) AND (${INDEX_ALL} EQUAL -1))
		string(SUBSTRING ${WITHOUT_EXT} ${INDEX_CALC} ${INCLUDE_SIZE} INCLUDE_NAME)
		write_file(${MATCALC_CALCULATIONS_ALL_HEADER_FILE} "#include \"" ${INCLUDE_NAME} "\"" APPEND)
	    message("Matcalc API: header for ${INCLUDE_NAME} was included")
	else()
		message("Matcalc API: ${fname} was not included as a command")
	endif()

endforeach()

