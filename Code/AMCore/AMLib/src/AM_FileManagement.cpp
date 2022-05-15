#include "../include/AM_FileManagement.h"
#include <iostream>
#include <fstream>
#include <sstream>

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
		std::string fullFileName = get_filePath(option_) + filename;

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
		case FILEPATH::GENERAL:
			response = _workingDirectory;
			break;
		case FILEPATH::SCRIPTS:
			response = _workingDirectory;
			break;
		case FILEPATH::PROJECTS:
			response = _workingDirectory;
			break;
		default:
			response = "";
			break;
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
		ss << "_fileManagement \n";

		ss << "END\n";

		return ss.str();
	}

	void AM_FileManagement::load_string(std::ifstream& save_string)
	{

	}
#pragma endregion Interfaces