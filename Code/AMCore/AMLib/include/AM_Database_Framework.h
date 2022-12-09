#pragma once
#include <vector>
#include <string>

#include "AM_Config.h"
#include "AM_FileManagement.h"
#include "../interfaces/IAM_Database.h"
#include "../include/Database_implementations/Database_Factory.h"
#include "../include/Database_implementations/Database_scheme_content.h"
#include "../include/AM_Database_TableStruct.h"
#include "../include/Database_implementations/Data_Controller.h"
#include "../include/Implementations/ObserverObject.h"

/// <summary>
/// This class handles all interactions between the database and the framework.
/// It creates the initial setup of tables, handles all data organizational stuff.
/// </summary>
class AM_Database_Framework : public ObserverObject
{
public:
	AM_Database_Framework(AM_Config* configuration);
	~AM_Database_Framework();

#pragma region Database_Tables
	IAM_Database* get_database()
	{
		return _database;
	}
	/*
	Data_Controller* get_dataController()
	{
		return _dataController;
	}
	*/

	const std::string get_apiExternalPath()
	{
		return _configuration->get_apiExternal_path();
	}

	const std::string get_apiPath()
	{
		return _configuration->get_api_path();
	}
#pragma endregion

#pragma region ObserverObject_Interface
	void update(std::string& ObjectTypeName) override
	{
		database_disconnect();
		update_variables();
	}
#pragma endregion

private:
	inline const static std::string _dbName{"AMDatabase"}; // Name of the database
	AM_Config* _configuration{ nullptr };
	AM_FileManagement _fileManagement;
	IAM_Database* _database{ nullptr };
	// Data_Controller* _dataController{ nullptr };


#pragma region Database
	bool database_connection()
	{
		//TODO: add exception!
		if (_database->connect() == 0) return true;
		return false;
	}

	bool database_disconnect() 
	{
		if (_database->disconnect() == 0) return true;
		return false;
	}

	void database_delete() 
	{
		if (_database != nullptr) delete _database;
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

	void update_variables() 
	{
		_fileManagement = AM_FileManagement(_configuration->get_working_directory());
		create_database();
	}


#pragma endregion


};