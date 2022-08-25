#----------------------------------------------------------------------------------------------------
# *************************************DATABASE SCHEME***********************************************
#----------------------------------------------------------------------------------------------------
#[[
	This script creates the scheme file for the database based on the models declared in the directory
	Data_stuctures (yeah, misstyped it but lets leave it as is for now).

	Requirements:
	- Header files have to start with DBS_
	- Parameters that will be stored in the database have to be marked with; '//[DBS_Parameter]'

	Output:
	- File with all tablenames and table columns. 
	- Tablenames are the same as class name after the DBS_ string.

	Note: '//[DBS_Parameter]' goes above the parameter to be marked without extra '\n' e.g.
	
	//[DBS_Parameter]
	std::string This_parameter{""};

	Note_1: Primary key is by default the ID parameter, so we don't have to declare a primary key. The 
	ID parameter is declared in the interface IAM_DBS.h

	Note_2: class has to implement IAM_DBS, thus the format is strict. Please also note that the current
	implementation expects a space character between the ':' char and the class name, e.g:
	
	'DBS_classname : public IAM_DBS'

	If you forget this, you might see the class name truncated with one chaar less than expected (Parser)
]]


#--------------------------------------------------
#                     OPTIONS
#--------------------------------------------------

# Directory that contains the models
set(DATA_STRUCTURE_DIRECTORY ${PROJECT_SOURCE_DIR}/AMLib/include/Database_implementations/Data_stuctures)

# Scheme filename
set(SCHEME ${PROJECT_SOURCE_DIR}/AMLib/include/Database_implementations/Database_scheme_content_V01.h)


#--------------------------------------------------
#                     SCRIPT
#--------------------------------------------------

# Get list of all files in DATA_STRUCTURE_DIRECTORY with extension .h
file(GLOB MODEL_FILES ${DATA_STRUCTURE_DIRECTORY}/*.h)

# Write file; declare imports and namespace
write_file(${SCHEME} "#pragma once")
write_file(${SCHEME} "#include \"../AM_Database_TableStruct.h\"" APPEND)
write_file(${SCHEME} "#include <vector>" APPEND)
write_file(${SCHEME} "namespace AMLIB" APPEND)
write_file(${SCHEME} "{ \n" APPEND)

# Parse through text and declare all found parameters
foreach(fname IN LISTS MODEL_FILES)
	message("Looking into filename: ${fname}")
	
	# Read file contents, note that cmake when loading text interprets the char ';' as a new
	# entry for a list variable, thus the output is a list since we use this char in the code.
	file(READ ${fname} TEMP_DATA)

	# Get the first line and determine is the class has the correct format before parsing,
	# if not we ignore it.
	list(GET TEMP_DATA 0 CLine)
	string(FIND ${CLine} "class DBS_" CLASS_START)
	string(FIND ${CLine} ":" CLASS_END)

	# Check if indexes are greater than -1, this would mean that we found the correct format.
	if((${CLASS_END} GREATER -1) AND (${CLASS_START} GREATER -1))
	
		# We use the index CLASS_START and add 10 becaue we are interested on the class name
		# so we truncate after the 'DBS_' string.
		math(EXPR CLASS_RStart "${CLASS_START} + 10")
		math(EXPR CLASS_SIZE "${CLASS_END} - ${CLASS_RStart} - 1")

		# Extract the classname using the indexes from above, note here that if the format is 
		# not correct, it might truncate one letter of the class name.
		string(SUBSTRING ${CLine} ${CLASS_RStart} ${CLASS_SIZE} CLASS_NAME)
		message("Class name: " ${CLASS_NAME})

		# Create function for new table
		write_file(${SCHEME} "static AM_Database_TableStruct TN_" ${CLASS_NAME} "()" APPEND)
		write_file(${SCHEME} "{" APPEND)
		write_file(${SCHEME} "	AM_Database_TableStruct out;" APPEND)
		write_file(${SCHEME} "	out.add_new(\"ID\",\"INTEGER PRIMARY KEY\");" APPEND)

		list(APPEND TABLE_FUNCTIONS "TN_${CLASS_NAME}()")

		# Find all marked as parameters using //[DBS_Parameter]
		foreach(CSeg IN LISTS TEMP_DATA)
			
			# Create a new list based on the '\n' char (line by line)
			string(REPLACE "\n" ";" TEMP_LIST ${CSeg})
			
			# Check if this segment has a marked parameter, we use the index inside the for loop
			# for looking into the next line where we expect to find the declaration of the
			# variable.
			set(LIndex 0)
			foreach(LineS IN LISTS TEMP_LIST)
				
				# We expect to find the variable in the next line, thus the index will always be 1 ahead
				# of the current index (LineS).
				math(EXPR LIndex "${LIndex} + 1")

				# Check if marked as parameter
				string(FIND ${LineS} "DBS_Parameter" VARFOUND)
				message(${LineS})

				# Determine variable type and create entry
				if(${VARFOUND} GREATER -1)

					# Get list entry that contains the line with the declaration
					list(GET TEMP_LIST ${LIndex} VLINE)
					message("variable found  ${VLINE}")

					# Create variable to append all properties
					set(VARLINE "	out.add_new(")

					# Flags to determine variable name end position
					string(FIND ${VLINE} "{" VAR_C1)
					string(FIND ${VLINE} "=" VAR_C2)

					set(VAR_ENDINDEX -1)
					if(${VAR_C1} GREATER -1)
						math(EXPR VAR_ENDINDEX "${VAR_C1}")
					endif()
					if(${VAR_C2} GREATER -1)
						math(EXPR VAR_ENDINDEX "${VAR_C2} - 1")
					endif()

					# Flags to determine which type it belong to
					string(FIND ${VLINE} "string" TYPE_STRING)
					string(FIND ${VLINE} "int" TYPE_INT)
					string(FIND ${VLINE} "double" TYPE_DOUBLE)
					
					# Types correspond to SQLite3, however, conversion is trivial if we plan on
					# implementing the interface for another database provider e.g. MySQL, other
					set(VAR_STARTINDEX -1)
					set(FOUND_TYPE "")
					if(${TYPE_STRING} GREATER -1)
						math(EXPR VAR_STARTINDEX "${TYPE_STRING} + 7")
						string(APPEND FOUND_TYPE "\"TEXT\"")
					endif()
					if(${TYPE_INT} GREATER -1)
						math(EXPR VAR_STARTINDEX "${TYPE_STRING} + 4")
						string(APPEND FOUND_TYPE "\"INTEGER\"")
					endif()
					if(${TYPE_DOUBLE} GREATER -1)
						math(EXPR VAR_STARTINDEX "${TYPE_DOUBLE} + 7")
						string(APPEND FOUND_TYPE "\"REAL\"")
					endif()

					# Get Variable name
					math(EXPR VAR_SIZE "${VAR_ENDINDEX} - ${VAR_STARTINDEX}")
					string(SUBSTRING ${VLINE} ${VAR_STARTINDEX} ${VAR_SIZE} VAR_NAME)
					
					# Add to line
					string(APPEND VARLINE "\"" ${VAR_NAME} "\"," ${FOUND_TYPE})
					string(APPEND VARLINE ")\;")

					# Write to file
					write_file(${SCHEME} ${VARLINE} APPEND)
				endif()
			endforeach()
		endforeach()

		# Declare the table as the class name
		write_file(${SCHEME} "	out.tableName = \"" ${CLASS_NAME} "\";" APPEND)

		# End of funcion should return a AM_Database_TableStruct object
		write_file(${SCHEME} "	return out;" APPEND)
		write_file(${SCHEME} "} \n" APPEND)

	endif()	
endforeach()

# Create a function that returns all tables that belong to this scheme
write_file(${SCHEME} "static std::vector<AM_Database_TableStruct> get_structure() \n{" APPEND)
write_file(${SCHEME} "	std::vector<AM_Database_TableStruct> out; \n" APPEND)

foreach(TName IN LISTS TABLE_FUNCTIONS)
	write_file(${SCHEME} "	out.push_back(${TName});" APPEND)
endforeach()

write_file(${SCHEME} "} \n" APPEND)

# Close namespace
write_file(${SCHEME} "}" APPEND)
