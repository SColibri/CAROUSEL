
# Add header files and libraries.
file(GLOB_RECURSE ssh_H "include/*.h")
file(GLOB_RECURSE ssh_LIB "*.lib")
file(GLOB_RECURSE ssh_DLL "*.dll")

file(COPY ${ssh_LIB} DESTINATION ${CMAKE_CURRENT_BINARY_DIR}) 
file(COPY ${ssh_DLL} DESTINATION ${CMAKE_CURRENT_BINARY_DIR}) 
