# Include sub-projects.
file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/AMLIB/Test_resources/pigeon.stl
     DESTINATION ${CMAKE_CURRENT_BINARY_DIR}) 

add_subdirectory ("AMLib")
add_subdirectory ("MatCalc_API")
add_subdirectory ("lib/Catch2")
# add_subdirectory ("MatCalc_API")


