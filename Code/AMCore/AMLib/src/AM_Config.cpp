#include "../include/AM_Config.h"
#include <sstream>
#include <vector>
#include <fstream>
#include "../include/Database_implementations/Database_Factory.h"

// Constructors, destructors and other
// -------------------------------------------------------------------------------------
#pragma region Cons_Des
	AM_Config::AM_Config()
	{

	}

	AM_Config::AM_Config(std::string filename)
	{
		load(filename);
	}

	AM_Config::~AM_Config()
	{

	}

#pragma endregion Cons_Des

// Methods
// -------------------------------------------------------------------------------------
#pragma region Methods
	void AM_Config::save()
	{
		_fileManagement.save_file(AM_FileManagement::FILEPATH::GENERAL,
								  Name + ".config",
								  get_save_string());
	}

	void AM_Config::load(std::string filename)
	{
		std::ifstream streamF;
		streamF.open(filename);

		load_string(streamF);
		streamF.close();
	}
#pragma endregion Methods

// Getters and setters
// -------------------------------------------------------------------------------------
#pragma region Getters_Setters
	std::string AM_Config::get_filename()
	{
		return _fileManagement.get_filePath(workingDirectoryOption) + "/" + Name + ".config";
	}
	std::string AM_Config::get_directory_path(AM_FileManagement::FILEPATH foption)
	{
		return _fileManagement.get_filePath(foption);
	}

	void AM_Config::set_working_directory(std::string mainPath)
	{
		_fileManagement.set_workingDirectory(mainPath);
	}

	const std::string AM_Config::get_working_directory()
	{
		return _fileManagement.get_filePath(workingDirectoryOption);
	}

	void AM_Config::set_config_name(std::string newName)
	{
		Name = newName;
	}

	void AM_Config::set_workingDirectory_option(AM_FileManagement::FILEPATH newOption)
	{
		workingDirectoryOption = newOption;
	}

	AM_FileManagement::FILEPATH AM_Config::get_workingDirectory_option()
	{
		return workingDirectoryOption;
	}

	const std::string& AM_Config::get_api_path()
	{
		return _apiPath;
	}

	void AM_Config::set_api_path(std::string filename)
	{
		_apiPath = filename;
	}

	const std::string& AM_Config::get_apiExternal_path()
	{
		return _apiExternalPath;
	}

	void AM_Config::set_apiExternal_path(std::string filename)
	{
		_apiExternalPath = filename;
	}

	const std::string& AM_Config::get_ThermodynamicDatabase_path()
	{
		return _thermodynamic_database_path;
	}

	void AM_Config::set_ThermodynamicDatabase_path(std::string filename)
	{
		_thermodynamic_database_path = filename;
	}

	const std::string& AM_Config::get_PhysicalDatabase_path()
	{
		return _physical_database_path;
	}

	void AM_Config::set_PhysicalDatabase_path(std::string filename)
	{
		_physical_database_path = filename;
	}

	const std::string& AM_Config::get_MobilityDatabase_path()
	{
		return _mobility_database_path;
	}

	void AM_Config::set_MobilityDatabase_path(std::string filename)
	{
		_mobility_database_path = filename;
	}

#pragma endregion Getters_Setters

// Interfaces
// -------------------------------------------------------------------------------------
#pragma region Interfaces

	std::string AM_Config::get_save_string()
	{
		std::stringstream ss;
	
		ss << "#*** AMFramework config file ****\n";
		ss << "*********************************\n";
		ss << "\n";
		ss << "Name" << separatorChar << Name << std::endl;
		ss << "_apiPath" << separatorChar << _apiPath << std::endl;
		ss << "_thermodynamic_database_path" << separatorChar << _thermodynamic_database_path << std::endl;
		ss << "_physical_database_path" << separatorChar << _physical_database_path << std::endl;
		ss << "_mobility_database_path" << separatorChar << _mobility_database_path << std::endl;
		ss << "_workingDirectory" << separatorChar << _workingDirectory << std::endl;
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
					else if (var.find("AM_DatabaseFactory") != std::string::npos) {
						Database_Factory::load_string(save_string);
					}
				}
				else {
					if (var.find("Name") != std::string::npos) {
						Name = var.substr(var.find(separatorChar) + 1);
					}
					else if(var.find("_apiPath") != std::string::npos) {
						_apiPath = var.substr(var.find(separatorChar) + 1);
					}
					else if (var.find("_thermodynamic_database_path") != std::string::npos) {
						_thermodynamic_database_path = var.substr(var.find(separatorChar) + 1);
					}
					else if (var.find("_physical_database_path") != std::string::npos) {
						_physical_database_path = var.substr(var.find(separatorChar) + 1);
					}
					else if (var.find("_mobility_database_path") != std::string::npos) {
						_mobility_database_path = var.substr(var.find(separatorChar) + 1);
					}
					else if (var.find("_workingDirectory") != std::string::npos) {
						_workingDirectory = var.substr(var.find(separatorChar) + 1);
					}
				}
			}
		}

	}

#pragma endregion Interfaces

