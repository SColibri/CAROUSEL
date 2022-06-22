#include "../include/AM_FileManagement.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include <filesystem>

// Constructors, destructors and other
// -------------------------------------------------------------------------------------
#pragma region Cons_Des
	AM_FileManagement::AM_FileManagement() {}
	AM_FileManagement::AM_FileManagement(std::string newWorkingDirectory):_workingDirectory(newWorkingDirectory) {}
	AM_FileManagement::~AM_FileManagement(){}
#pragma endregion Cons_Des

//Methods
// -------------------------------------------------------------------------------------
#pragma region Methods
	std::string AM_FileManagement::save_file(FILEPATH option_, std::string filename, std::string content) {
		std::ofstream class_file;
		std::string Save01 = content;
		std::string fullFileName = filename;

		class_file.open(fullFileName);
		class_file << content;
		class_file.close();

		return fullFileName;
	}

	std::string AM_FileManagement::get_filePath(FILEPATH option_)
	{
		std::string response;

		switch (option_)
		{
		case FILEPATH::SYSTEM:
			response = "";
			break;
		case FILEPATH::GENERAL:
			response = _workingDirectory;
			break;
		case FILEPATH::SCRIPTS:
			response = _workingDirectory + "/Scripts";
			break;
		case FILEPATH::PROJECTS:
			response = _workingDirectory + "/Projects";
			break;
		case FILEPATH::DATABASE:
			response = _workingDirectory + "/Database";
			break;
		default:
			response = "";
			break;
		}

		if(response.length() > 0)
		{
			std::filesystem::path pathName{ response };
			if(!std::filesystem::exists(pathName))
			{
				std::filesystem::create_directory(pathName);
			}
		}

		return response;
	}
#pragma endregion Methods

//getters and setters
// -------------------------------------------------------------------------------------
#pragma region Getters_Setters

	void AM_FileManagement::set_workingDirectory(std::string workDirectory)
	{
		_workingDirectory = workDirectory;
		//TODO create directories
	}

#pragma endregion Getters_Setters

// Interfaces
// -------------------------------------------------------------------------------------
#pragma region Interfaces
	std::string AM_FileManagement::get_save_string()
	{
		std::stringstream ss;

		ss << "#*** AM_FileManagement config file ****\n";
		ss << "***************************************\n";
		ss << "\n";
		ss << "_workingDirectory" << separatorChar << _workingDirectory << std::endl;
		ss << "END\n";

		return ss.str();
	}

	void AM_FileManagement::load_string(std::ifstream& save_string)
	{
		if (save_string.is_open()) {
			std::string var, var2{ "" };
			while (!save_string.eof() && save_string.good()) {
				std::getline(save_string, var);
				if (var.find("END") != std::string::npos) {
					break;
				}
				else if (var.find("_workingDirectory") != std::string::npos) {
					_workingDirectory = var.substr(var.find(separatorChar) + 1);
				}
			}
		}
	}
#pragma endregion Interfaces