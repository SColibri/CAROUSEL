#----------------------------------------------------------------------------------------------------
# *************************************DATABASE METHODS*********************************************
#----------------------------------------------------------------------------------------------------
#[[
	Helper functions used across all managed code
]]

macro(return_)
  set(${ARGS} ${ARGN} PARENT_SCOPE)
  return()
endmacro()

function(get_dbs_parameters filename out_paramList_type out_paramList_name)
	message("Extracting parameter lists from: ${filename}")
	
	# Read file contents, note that cmake when loading text interprets the char ';' as a new
	# entry for a list variable, thus the output is a list since we use this char in the code.
	file(READ ${filename} DBS_CONTENT)

	# 
	set(LINES "")
	foreach(segment IN LISTS DBS_CONTENT)
		string(REPLACE "\n" ";" LINELIST "${segment}")
		foreach(LINE IN LISTS LINELIST)
			set(LINES "${LINES};${LINE}")
		endforeach()
	endforeach()

	# Get the first line and determine is the class has the correct format before parsing,
	# if not we ignore it.
	list(GET DBS_CONTENT 0 CLine)
	string(FIND ${CLine} "class DBS_" CLASS_START)
	string(FIND ${CLine} ":" CLASS_END)

	# Initialize empty lists to store the tags, parameters and types
	set(tags_list "")
	set(out_paramList_name "")
	set(out_paramList_type "")

	# Initialize a variable to track whether the current line is a comment line
	set(is_comment 0)

	# Check if indexes are greater than -1, this would mean that we found the correct format.
	if((${CLASS_END} GREATER -1) AND (${CLASS_START} GREATER -1))
		
		# Iterate over the lines of the file
		foreach( line IN LISTS LINES )
			
			
			get_variableNameType_fromString("${line}" "${is_comment}")	
			list(GET out_values 0 VAR_NAME)
			list(GET out_values 1 TYPEVAL)
			list(GET out_values 2 is_comment)
			
			set(out_paramList_type "${out_paramList_type};${TYPEVAL}")
			set(out_paramList_name "${out_paramList_name};${VAR_NAME}")
			message("Name is ${VAR_NAME}, value is comment: ${is_comment}")
			
		endforeach()

	else()
		message("Error file!!")
	endif()	
endfunction()

function(testymonial line)
	set(modified_variable1 "${line}_modified1")
    set(modified_variable2 "${line}_modified2")
    set(result_list ${modified_variable1} ${modified_variable2})
    set(out_values "${result_list}" PARENT_SCOPE)
endfunction()

function(get_variableNameType_fromString line is_comment)
	
	# remove tabs
	string(REGEX REPLACE "^[ \\t]+" "" line "${line}")
	message("line content is: ${line}")
	set(VAR_NAME "")
	set(TYPEVAL "")

	set(is_comment2 ${is_comment})
	# Check if the line starts with "// [DBS_Parameter]"
	string(FIND "${line}" "DBS_Parameter" ISParameter)
	if(${ISParameter} GREATER -1)
		set(is_comment2 1)
		message("FOUND!")
	elseif(${is_comment2} EQUAL 1)

		# Get variable type
		get_variableType_fromString("${line}")		
		list(GET out_values 0 out_type)
		list(GET out_values 1 VAR_STARTINDEX)
		list(GET out_values 2 VAR_ENDINDEX)
		
		# Get Variable name
		math(EXPR VAR_SIZE "${VAR_ENDINDEX} - ${VAR_STARTINDEX}")
		string(SUBSTRING "${line}" ${VAR_STARTINDEX} ${VAR_SIZE} out_name)
		
		# set as return values
		set(VAR_NAME "${out_name}" )
		set(TYPEVAL "${out_type}" )
		set(is_comment2 0)
		message("Variable namey is: ${out_name}")
	endif()

	set(response "${VAR_NAME}" "${TYPEVAL}" "${is_comment2}")
	set(out_values "${response}" PARENT_SCOPE)
endfunction()

function(get_variableType_fromString line)
	# Flags to determine variable name end position
	string(FIND "${line}" "{" VAR_C1)
	string(FIND "${line}" "=" VAR_C2)

	set(VAR_ENDINDEX -1)
	if(${VAR_C1} GREATER -1)
		math(EXPR VAR_ENDINDEX "${VAR_C1}")
	endif()
	if(${VAR_C2} GREATER -1)
		math(EXPR VAR_ENDINDEX "${VAR_C2} - 1")
	endif()

	# Flags to determine which type it belong to
	string(FIND "${line}" "string" TYPE_STRING)
	string(FIND "${line}" "int" TYPE_INT)
	string(FIND "${line}" "double" TYPE_DOUBLE)
					
	# Check what type was found
	set(VAR_STARTINDEX -1)
	set(out_type "")
	if(${TYPE_STRING} GREATER -1)
		math(EXPR VAR_STARTINDEX "${TYPE_STRING} + 7")
		set(out_type "string" PARENT_SCOPE)
	elseif(${TYPE_INT} GREATER -1)
		math(EXPR VAR_STARTINDEX "${TYPE_INT} + 4")
		set(out_type "int" PARENT_SCOPE)
	elseif(${TYPE_DOUBLE} GREATER -1)
		math(EXPR VAR_STARTINDEX "${TYPE_DOUBLE} + 7")
		set(out_type "double" PARENT_SCOPE)
	endif()

	# set(VAR_STARTINDEX ${VAR_STARTINDEX} PARENT_SCOPE)
	# set(VAR_ENDINDEX ${VAR_ENDINDEX} PARENT_SCOPE)
	set(response "${out_type}" "${VAR_STARTINDEX}" "${VAR_ENDINDEX}")
	set(out_values "${response}" PARENT_SCOPE)
endfunction()