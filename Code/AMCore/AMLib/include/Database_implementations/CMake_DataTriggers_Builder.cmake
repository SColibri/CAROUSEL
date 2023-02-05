#----------------------------------------------------------------------------------------------------
# *************************************DATABASE TRIGGERS*********************************************
#----------------------------------------------------------------------------------------------------
#[[
	This script contains methods that create a template trigger for all models. Triggers are fired 
	under specific actions e.g. add new record, delete record and others.

	You can also setup triggers using sql technology. For now scripts are compiled until further
	instruction if given.

	Requirements:
	- Based on the same as CMake_Database_scheme_builder

	Output:
	- trigger classes for all models.
	- if trigger already exists, it just ignores the current filename.

	Future:
	- Add relations based on their ID's
	- or use sql triggers
]]

#[[
	This method creates a single trigger template.

	Input:
	- workingDir : Directiry where the file should be created
	- className : Class name to which it should relate
]]
function(create_triggers workingDir className)
	# Required input
	cmake_parse_arguments(REQUIRED "${workingDir}" "${className}" "${ARGN}")

	# Trigger filename
	set(TRIGGER_FILENAME "${workingDir}/DBSTrigger_${className}.h")
	
	# Check if file exists, we do not want to override existing files because, as for now we do
	# not plan to handle all relations automatically, but could be nice.
	if(NOT EXISTS "${TRIGGER_FILENAME}")
		message("Creating datatrigger file...")

		# Get list of all files in DATA_STRUCTURE_DIRECTORY with extension .h
		write_file(${TRIGGER_FILENAME} "#pragma once")
		write_file(${TRIGGER_FILENAME} "#include \"../../../interfaces/IAM_DBS.h\"" APPEND)
		write_file(${TRIGGER_FILENAME} "\n\n" APPEND)
		write_file(${TRIGGER_FILENAME} "/* ------------------------------------------------------" APPEND)
		write_file(${TRIGGER_FILENAME} "File generated by cmake script. This is a standard \n" APPEND)
		write_file(${TRIGGER_FILENAME} "template and will only be generated if the file does \n" APPEND)
		write_file(${TRIGGER_FILENAME} "not exist, feel free to edit. \n" APPEND)
		write_file(${TRIGGER_FILENAME} "*/// ----------------------------------------------------\n" APPEND)

		write_file(${TRIGGER_FILENAME} "namespace TRIGGERS" APPEND)
		write_file(${TRIGGER_FILENAME} "{ \n" APPEND)
		write_file(${TRIGGER_FILENAME} "class DBSTriggers_${className}" APPEND)
		write_file(${TRIGGER_FILENAME} "{" APPEND)

		# Create private constructor
		write_file(${TRIGGER_FILENAME} "private:" APPEND)
		write_file(${TRIGGER_FILENAME} "/// <summary>" APPEND)
		write_file(${TRIGGER_FILENAME} "/// Static class" APPEND)
		write_file(${TRIGGER_FILENAME} "/// <summary>" APPEND)
		write_file(${TRIGGER_FILENAME} "DBSTriggers_${className}() {};" APPEND)
		
		# public static methods
		write_file(${TRIGGER_FILENAME} "public:" APPEND)
		write_file(${TRIGGER_FILENAME} "// Add your static data-triggers here." APPEND)

		# Close class
		write_file(${TRIGGER_FILENAME} "};" APPEND)
		# Close namespace
		write_file(${TRIGGER_FILENAME} "}" APPEND)
	else()
		message("${TRIGGER_FILENAME} already exists, skipping file")
	endif()
endfunction()

#[[
	Creates a all header file that includes all triggers.

	Input:
	- workingDir : Directory where trigger files are found
]]
function(create_triggers_allfile workingDir)

	file(GLOB TRIGGER_FILES ${workingDir}/*.h)
	set(TRIGGER_FILENAME "${workingDir}/DBSTrigger_ALL.h")
	write_file(${TRIGGER_FILENAME} "#pragma once")
	
	foreach(fname IN LISTS TRIGGER_FILES)
		#Ignore All.h
		string(FIND "${fname}" "DBSTrigger_ALL" ALLINDEX)

		if(${ALLINDEX} GREATER -1)
			continue()
		endif()

		get_filename_component(fobject ${fname} NAME)
		write_file(${TRIGGER_FILENAME} "#include \"${fobject}\"" APPEND)	
	endforeach()

endfunction()