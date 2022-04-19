#include "../include/AM_FileManagement.h"
#include <iostream>
#include <fstream>
#include <sstream>

std::string AM_FileManagement::save_file(FILEPATH option_, std::string filename, std::string content) {
	return "";
}

std::string AM_FileManagement::get_filePath(FILEPATH option_)
{
	return std::string();
}

template<typename T>
inline void AM_FileManagement::save(const T& classData, std::string filename)
{
	std::ofstream class_file;

	class_file.open(filename, std::ios::app);
	class_file.write((char*)&classData, sizeof(classData));
	class_file.close();

}

// Interfaces ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

std::string AM_FileManagement::get_save_string()
{
	std::stringstream ss;

	ss << "#*** AM_FileManagement config file ****\n";
	ss << "***************************************\n";
	ss << "\n";
	ss << "_fileManagement \n";

	ss << "END\n";

	return ss.str();
}

void AM_FileManagement::load_string(std::ifstream& save_string)
{

}