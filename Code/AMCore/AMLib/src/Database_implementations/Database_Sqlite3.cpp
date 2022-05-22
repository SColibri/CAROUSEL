#include "../../include/Database_implementations/Database_Sqlite3.h"
#include <sstream>
#include "../../include/Database_implementations/Database_Factory.h"

Database_Sqlite3::~Database_Sqlite3() {
	if (_connectionOpen == true) {
		sqlite3_close(db);
	}
}

#pragma region Implementation
int Database_Sqlite3::connect()
{
	std::string filename = _configuration->get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "/" + 
							Database_Factory::get_schema() + ".db";

	if (sqlite3_open(filename.c_str(), &db) != SQLITE_OK) {
		sqlite3_close(db);
		return 1;
	}

	_connectionOpen = true;
	return 0;
}

std::vector<std::string> Database_Sqlite3::get_tableNames()
{
	std::vector<std::string> Response;
	std::string Query = "SELECT name FROM sqlite_schema WHERE type='table'";
	sqlite3_stmt* stmt;

	int exec_result = sqlite3_prepare_v2(db, Query.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {

		while (sqlite3_step(stmt) == SQLITE_ROW)
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



int Database_Sqlite3::add_table(const AM_Database_TableStruct* newTable)
{
	int Response = 0;

	if (newTable->columnNames.size() > 0 &&
		newTable->columnDataType.size() == newTable->columnNames.size())
	{
		std::string Query = "CREATE TABLE IF NOT EXISTS \'" +
			newTable->tableName +
			"\' ( ";

		int Index{ 0 };
		for each (std::string column in newTable->columnNames)
		{
			if (Index > 0) Query += ", ";
			Query += column + " " + newTable->columnDataType[Index++];
		}

		Query += " )";

		Response = sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
		if (Response != SQLITE_OK) Response = 1;
	}

	return Response;
}

int Database_Sqlite3::drop_table(const AM_Database_TableStruct* tableName)
{
	std::string Query = "DROP TABLE \'" + tableName->tableName + "\'";

	return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
}

int Database_Sqlite3::clear_table(const AM_Database_TableStruct* tableName)
{
	std::string Query = "DELETE FROM \'" + tableName->tableName + "\'";

	return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
}

int Database_Sqlite3::remove_row(const AM_Database_TableStruct* tableName, int ID)
{
	std::string Query = "DELETE FROM \'" + tableName->tableName + "\' WHERE ID" +
		tableName->tableName + " = " + std::to_string(ID);

	return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
}

int Database_Sqlite3::remove_row(const AM_Database_TableStruct* tableName, std::string whereClause)
{
	std::string Query = "DELETE FROM \'" + tableName->tableName + "\' WHERE " +
		whereClause;
	return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
}

int Database_Sqlite3::insert_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData)
{
	std::string Query = "INSERT INTO \'" + tableName->tableName + "\' ( ";

	Query += tableName->columnNames[1];
	for (size_t i = 2; i < tableName->columnNames.size(); i++)
	{
		Query += ", " + tableName->columnNames[i];
	}
	Query += " ) VALUES ( ";

	Query += "\'" + newData[1] + "\'";
	for (size_t i = 2; i < newData.size(); i++)
	{
		Query += ", \'" + newData[i] + "\'";
	}
	Query += " );";


	return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
}

int Database_Sqlite3::update_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData)
{
	std::string Query = "UPDATE \'" + tableName->tableName + "\' SET ";

	Query += tableName->columnNames[1] + " = " + newData[1];
	for (size_t i = 2; i < tableName->columnNames.size(); i++)
	{
		Query += "," + tableName->columnNames[i] + " = " + newData[i];
	}
	Query += " WHERE ID" + tableName->tableName + " = " + newData[0];

	return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
}

int Database_Sqlite3::get_last_ID(const AM_Database_TableStruct* tableName)
{
	std::string Query = "SELECT ID FROM \'" + tableName->tableName + "\' ORDER BY ID DESC LIMIT 1 ";
	int Response{ -1 };
	sqlite3_stmt* stmt;

	int exec_result = sqlite3_prepare_v2(db, Query.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {

		while (sqlite3_step(stmt) == SQLITE_ROW)
		{
			std::stringstream ss;
			std::string TName;
			ss << sqlite3_column_text(stmt, 0);
			ss >> TName;
			Response = std::stoi(TName);
		}

	}

	return Response;
}

std::vector<std::vector<std::string>> Database_Sqlite3::get_tableRows(const AM_Database_TableStruct* tableName)
{
	std::vector<std::vector<std::string>> out;
	std::string Query = "SELECT * FROM \'" + tableName->tableName + "\' ";
	sqlite3_stmt* stmt;
	
	int exec_result = sqlite3_prepare_v2(db, Query.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {

		while (sqlite3_step(stmt) == SQLITE_ROW)
		{
			out.push_back(get_input_row(tableName, stmt));	
		}
	}

	return out;
}

std::vector<std::vector<std::string>> Database_Sqlite3::get_tableRows(const AM_Database_TableStruct* tableName, std::string whereQuery)
{
	std::vector<std::vector<std::string>> out;
	std::string Query = "SELECT * FROM \'" + tableName->tableName + "\' WHERE " + whereQuery;
	sqlite3_stmt* stmt;

	int exec_result = sqlite3_prepare_v2(db, Query.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {

		while (sqlite3_step(stmt) == SQLITE_ROW)
		{
			out.push_back(get_input_row(tableName, stmt));
		}
	}

	return out;
}

std::vector<std::string> Database_Sqlite3::get_input_row(const AM_Database_TableStruct* tableName,
										sqlite3_stmt* stmt)
{
	std::stringstream ss;
	std::string TName;

	int Index{ 0 };
	std::vector<std::string> rowContent;
	for each (std::string column in tableName->columnNames)
	{
		const unsigned char* testy = sqlite3_column_text(stmt, Index++);
		rowContent.push_back(std::string((const char*)testy));
	}

	return rowContent;
}

std::vector<std::string> Database_Sqlite3::get_row(const AM_Database_TableStruct* tableName, std::string Query)
{
	std::vector<std::string> out;
	std::string inQuery = "SELECT * FROM \'" + tableName->tableName + "\' WHERE " + Query;
	sqlite3_stmt* stmt;

	int exec_result = sqlite3_prepare_v2(db, inQuery.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {

		while (sqlite3_step(stmt) == SQLITE_ROW)
		{
			out = get_input_row(tableName, stmt);
			break;
		}
	}

	return out;
}


#pragma endregion