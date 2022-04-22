# CMakeList.txt : CMake project for AMCore, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)

# Add source to this project's executable.
file(GLOB AMCORE_SRC "src/*.cpp")
file(GLOB AMCORE_H "include/*.hpp")
file(GLOB AMLIBRARY "../AMLib/include/*.hpp")

# We will link dynamically to any library API or similar
#link_directories("C:/Program Files/MatCalc 6")
#add_library(matcalc STATIC IMPORTED "C:/Program Files/MatCalc 6/mc_core.dll")
#set_target_properties(matcalc PROPERTIES LINKER_LANGUAGE C IMPORTED_LOCATION "C:/Program Files/MatCalc 6/mc_core.lib")

add_executable (AMCore "AMCore.cpp" "AMCore.h"  ${AMCORE_SRC} ${AMCORE_H} ${AMLIBRARY}  )
#target_link_libraries(AMCore matcalc)
target_link_libraries(AMCore AMLib) 
target_link_libraries(AMCore sqlite3) 

# TODO: Add tests and install targets if needed.
