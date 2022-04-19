#include "../include/AM_Config.h"
#include <sstream>
#include <vector>
#include <fstream>

AM_Config::AM_Config()
{
}

AM_Config::AM_Config(std::string filename)
{
}

AM_Config::~AM_Config()
{
}

void AM_Config::save()
{
}

void AM_Config::load(std::string filename)
{
}

AM_FileManagement& AM_Config::get_fileManagement()
{
	return _fileManagement;
}

void AM_Config::set_mainPath(std::string mainPath)
{
}



// Interfaces ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

std::string AM_Config::get_save_string()
{
	std::stringstream ss;
	
	ss << "#*** AMFramework config file ****\n";
	ss << "*********************************\n";
	ss << "\n";
	ss << "Name" << separatorChar << Name;
	ss << "\n";
	ss << _fileManagement.get_save_string();
	ss << "\n";
	ss << "END\n";

	return ss.str();
}

void AM_Config::load_string(std::ifstream& save_string)
{
	if (save_string.is_open()) {
		std::string var, var2{ "" };
		while (!save_string.eof() && save_string.good()) {
			std::getline(save_string, var);
			if (var[0] == '#') { // find headers
				if (var.find("AM_FileManagement") != std::string::npos) {
					_fileManagement.load_string(save_string);
				}
			}
			else {
				if (var.find("Name") != std::string::npos) {
					Name = var.substr(var.find(separatorChar) + 1);
				}
			}
		}
	}

}