#pragma once
#include "../../interfaces/IAM_Database.h"
#include "../../include/Database_implementations/Database_Sqlite3.h"
#include "../../include/AM_Config.h"
#include <fstream>

/// <summary>
/// Database factory 
/// </summary>
class Database_Factory : IStringify
{
public:
	enum class TYPE
	{
		SQLITE3
	};

	Database_Factory(){}
	~Database_Factory(){}

	static IAM_Database* get_database(AM_Config* configuration)
	{
		IAM_Database* out{nullptr};

		switch (_selectedTYPE)
		{
		case Database_Factory::TYPE::SQLITE3:
			out = (IAM_Database*)new Database_Sqlite3(configuration);
			break;
		default:
			break;
		}

		return out;
	}

#pragma region Getters
	static const std::string& get_schema()
	{
		return _dbSchema;
	}
#pragma endregion

	// Interfaces
	// --------------------------------------------
#pragma region Interfaces

	// IStringify - IMPLEMENTATION
	static std::string get_save_string() 
	{
		std::stringstream ss;

		ss << "#*** AM_DatabaseFactory ****\n";
		ss << "*********************************\n";
		ss << "\n";
		ss << "_selectedTYPE" << separatorChar << (int)_selectedTYPE << std::endl;
		ss << "_databaseIP" << separatorChar << _databaseIP << std::endl;
		ss << "_databaseUser" << separatorChar << _databaseUser << std::endl;
		ss << "_databasePassword" << separatorChar << _databasePassword << std::endl;
		ss << "\n";
		ss << "END\n";

		return ss.str();
	}

	static void load_string(std::ifstream& save_string) 
	{
		if (save_string.is_open()) {
			std::string var, var2{ "" };
			while (!save_string.eof() && save_string.good()) {
				std::getline(save_string, var);
				if (var.find("END") != std::string::npos) {
					break;
				}
				else if (var.find("_selectedTYPE") != std::string::npos) {
					_selectedTYPE = (TYPE)std::stoi(var.substr(var.find(separatorChar) + 1));
				}
				else if (var.find("_databaseIP") != std::string::npos) {
					_databaseIP = var.substr(var.find(separatorChar) + 1);
				}
				else if (var.find("_databaseUser") != std::string::npos) {
					_databaseUser = var.substr(var.find(separatorChar) + 1);
				}
				else if (var.find("_databasePassword") != std::string::npos) {
					_databasePassword = var.substr(var.find(separatorChar) + 1);
				}
			}
		}

	}

#pragma endregion Interfaces

private:
	inline static TYPE _selectedTYPE{ TYPE::SQLITE3 };
	inline static std::string _databaseIP{};
	inline static std::string _databaseUser{};
	inline static std::string _databasePassword{};
	inline static std::string _dbSchema{ "AMDatabase" };

};
