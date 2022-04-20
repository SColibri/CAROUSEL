#include "../include/AM_FileManagement.h"
#include <iostream>
#include <fstream>
#include <sstream>

std::string AM_FileManagement::save_file(FILEPATH option_, std::string filename, std::string content) {
	std::ofstream class_file;
	std::string Save01 = content;
	std::string fullFileName = get_filePath(option_) + filename;

	class_file.open(fullFileName);
	class_file << content;
	class_file.close();

	return fullFileName;
}

std::string AM_FileManagement::get_filePath(FILEPATH option_)
{
	return std::string();
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