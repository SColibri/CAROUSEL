#[[
	Helper functions used for packaging all needed dll's
]]

function(create_packages)

    set(output_dir ${CMAKE_BINARY_DIR}/Package)
    set(path_matcalc_dll ${CMAKE_BINARY_DIR}/AM_API_lib/matcalc/AM_MATCALC_Lib.dll)
    set(lua_dir ${CMAKE_SOURCE_DIR}/AMGUI/windows/AMFramework/AMFramework/lua)
    set(XERCES_LIBRARY_DLL ${CMAKE_SOURCE_DIR}/AMModels/External/Libs/xerces-c_3_1.dll)

    message("The xerces lib can be found here: ${XERCES_LIBRARY_DLL}")

    # Create package directory
    create_package_folder(${output_dir})

    # Create release package
    copy_files(${path_matcalc_dll} ${output_dir})
    copy_files(${lua_dir} ${output_dir})
    copy_files(${XERCES_LIBRARY_DLL} ${output_dir})

endfunction()

# Creates the package output directory
function(create_package_folder output_dir)
    file(MAKE_DIRECTORY ${output_dir})
endfunction()

# Add file or directories
function(copy_files input_dir output_dir)
    if(EXISTS ${input_dir})
        FILE(COPY ${input_dir} DESTINATION ${output_dir})
        message("Moving into package: ${input_dir}")
    endif()
endfunction()

create_packages()