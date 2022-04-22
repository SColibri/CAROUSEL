#include "../include/AM_Database.h"
#include <sstream>

AM_Database::~AM_Database() {
	if (_connectionOpen == true) {
		sqlite3_close(db);
	}
}

// Methods
// ---------------------------------------------
#pragma region Methods
	int AM_Database::connect(std::string filename)
	{

		if (sqlite3_open(filename.c_str(), &db) != SQLITE_OK) {
			sqlite3_close(db);
			return 1;
		}

		_connectionOpen = true;
		return 0;
	}

	std::vector<std::string> AM_Database::get_tableNames() 
	{
		std::vector<std::string> Response;
		std::string Query = "SELECT name FROM sqlite_schema WHERE type='table'";
		sqlite3_stmt* stmt;

		int exec_result = sqlite3_prepare_v2(db,Query.c_str(), -1, &stmt, 0);
		if (exec_result == SQLITE_OK) {

			while(sqlite3_step(stmt) == SQLITE_ROW) 
			{
				std::stringstream ss;
				std::string TName;
				ss << sqlite3_column_text(stmt, 0);
				ss >> TName;
				Response.push_back(TName);
			}
			
		}

		return Response;
	}

	

	int AM_Database::add_table(const AM_Database_TableStruct* newTable)
	{
		int Response = 0;

		if (newTable->columnNames.size() > 0 &&
			newTable->columnDataType.size() == newTable->columnNames.size())
		{
			std::string Query = "CREATE TABLE IF NOT EXISTS " +
				newTable->tableName +
				"(ID" + newTable->tableName + " INTEGER PRIMARY KEY AUTOINCREMENT ";
			
			int Index{ 0 };
			for each (std::string column in newTable->columnNames)
			{
				Query += ", " + column + " " + newTable->columnDataType[Index++];
			}

			Query += ")";

			Response = sqlite3_exec(db, Query.c_str(),NULL, NULL, &_errorMessage);
			if (Response != SQLITE_OK) Response = 1;
		}
		
		return Response;
	}

	int AM_Database::drop_table(const AM_Database_TableStruct* tableName)
	{
		std::string Query = "DROP TABLE " + tableName->tableName;

		return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
	}

	int AM_Database::clear_table(const AM_Database_TableStruct* tableName)
	{
		std::string Query = "DELETE FROM " + tableName->tableName;

		return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
	}

	int AM_Database::remove_row(const AM_Database_TableStruct* tableName, int ID)
	{
		std::string Query = "DELETE FROM " + tableName->tableName + " WHERE ID" +
							tableName->tableName + " = " + std::to_string(ID);

		return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
	}

	int AM_Database::remove_row(const AM_Database_TableStruct* tableName, std::string whereClause)
	{
		std::string Query = "DELETE FROM " + tableName->tableName + " WHERE " +
							whereClause;
		return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
	}

	int AM_Database::insert_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData)
	{
		std::string Query = "INSERT INTO " + tableName->tableName + " ( ";

		Query += tableName->columnNames[1];
		for (size_t i = 2; i < tableName->columnNames.size(); i++)
		{
			Query += "," + tableName->columnNames[i];
		}
		Query += " ) VALUES ( ";

		Query += newData[1];
		for (size_t i = 2; i < newData.size(); i++)
		{
			Query += "," + newData[i];
		}
		Query += " );";


		return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
	}

	int AM_Database::update_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData)
	{
		std::string Query = "UPDATE " + tableName->tableName + " SET ";

		Query += tableName->columnNames[1] + " = " + newData[1];
		for (size_t i = 2; i < tableName->columnNames.size(); i++)
		{
			Query += "," + tableName->columnNames[i] + " = " + newData[i];
		}
		Query += " WHERE ID" + tableName->tableName + " = " + newData[0];

		return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
	}

#pragma endregion Methods