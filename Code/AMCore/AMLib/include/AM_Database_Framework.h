#pragma once
#include <vector>
#include <string>

#include "AM_Config.h"
#include "AM_FileManagement.h"
#include "../interfaces/IAM_Database.h"
#include "../include/Database_implementations/Database_Factory.h"
#include "../include/Database_implementations/Database_scheme_content.h"
#include "../include/AM_Database_TableStruct.h"

/// <summary>
/// This class handles all interactions between the database and the framework.
/// It creates the initial setup of tables, handles all data organizational stuff.
/// </summary>
class AM_Database_Framework
{
public:
	AM_Database_Framework(AM_Config* configuration);
	~AM_Database_Framework(){};

#pragma region Database_Tables

#pragma endregion

private:
	inline const static std::string _dbName{"AMDatabase"}; // Name of the database
	const AM_Config* _configuration;
	AM_FileManagement _fileManagement;
	IAM_Database* _database;


#pragma region Database
	bool database_connection()
	{
		//TODO: add exception!
		if (_database->connect() == 0) return true;
		return false;
	}

	int create_database()
	{
		if (!database_connection()) return -1;

		std::vector<std::string> availableTables = _database->get_tableNames();
		if (availableTables.size() == 0)
		{
			std::vector<AM_Database_TableStruct> structList = AMLIB::get_structure();
			for each (AM_Database_TableStruct tableStructure in structList)
			{
				_database->add_table(&tableStructure);
			}
		}

		return 0;
	}
#pragma endregion
};