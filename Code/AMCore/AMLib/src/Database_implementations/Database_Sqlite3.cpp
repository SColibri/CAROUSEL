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
	if (_connectionOpen) return 0;

	std::string filename = _configuration->get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "\\" + 
							Database_Factory::get_schema() + ".db";

	if (sqlite3_open(filename.c_str(), &db) != SQLITE_OK) {
		sqlite3_close(db);
		return 1;
	}

	int timeoutSetup = sqlite3_busy_timeout(db, 1000);
	_connectionOpen = true;
	return 0;
}

int Database_Sqlite3::disconnect()
{
	if (!_connectionOpen) return 0;

	sqlite3_close(db);
	_connectionOpen = false;

	return 0;
}

int Database_Sqlite3::execute_query(std::string Query) 
{ 
	return sqlite3_exec(db, Query.c_str(), NULL, NULL, &_errorMessage);
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

	sqlite3_finalize(stmt);
	return Response;
}

std::vector<std::string> Database_Sqlite3::get_columnNames(std::string& tableName)
{
	std::vector<std::string> Response;
	std::string Query = "SELECT * FROM " + tableName + " LIMIT 1";
	sqlite3_stmt* stmt;

	int exec_result = sqlite3_prepare_v2(db, Query.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {

		int columns = sqlite3_column_count(stmt);
		for (int i = 0; i < columns; i++)
			Response.push_back(sqlite3_column_name(stmt, i));

	}

	sqlite3_finalize(stmt);
	return Response;
}

std::vector<std::string> Database_Sqlite3::get_columnDatatype(std::string& tableName)
{
	std::vector<std::string> Response;
	std::string Query = "PRAGMA table_xinfo(" + tableName + ")";
	sqlite3_stmt* stmt;

	int exec_result = sqlite3_prepare_v2(db, Query.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {
		
		while (sqlite3_step(stmt) == SQLITE_ROW)
		{
			std::stringstream ss;
			std::string TName;
			ss << sqlite3_column_text(stmt, 2);
			ss >> TName;
			Response.push_back(TName);
		}

	}

	sqlite3_finalize(stmt);
	return Response;
}

AM_Database_TableStruct Database_Sqlite3::get_tableStruct(std::string& tableName)
{
	AM_Database_TableStruct newStr;
	newStr.tableName = tableName;
	
	newStr.columnDataType = get_columnDatatype(tableName);
	newStr.columnNames = get_columnNames(tableName);

	return newStr;
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

int Database_Sqlite3::create_view(std::string createQuery)
{
	return sqlite3_exec(db, createQuery.c_str(), NULL, NULL, &_errorMessage);
}

int Database_Sqlite3::remove_row(const AM_Database_TableStruct* tableName, int ID)
{
	std::string Query = "DELETE FROM \'" + tableName->tableName + "\' WHERE " +
		tableName->columnNames[0] + " = " + std::to_string(ID);

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
	int result = sqlite3_exec(db, get_insert_query(tableName, newData).c_str(), NULL, NULL, &_errorMessage);
	return result;
}

std::string Database_Sqlite3::get_insert_query(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData)
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

	return Query;
}

int Database_Sqlite3::update_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData)
{
	std::string Query = "UPDATE \'" + tableName->tableName + "\' SET ";

	Query += tableName->columnNames[1] + " = \'" + newData[1] + "\'";
	for (size_t i = 2; i < tableName->columnNames.size(); i++)
	{
		Query += "," + tableName->columnNames[i] + " = \'" + newData[i] + "\'";
	}
	Query += " WHERE " + tableName->columnNames[0] + " = " + newData[0];

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

	sqlite3_finalize(stmt);
	return Response;
}

std::vector<std::vector<std::string>> Database_Sqlite3::get_tableRows(const AM_Database_TableStruct* tableName)
{
	std::vector<std::vector<std::string>> out;
	std::string Query = "SELECT * FROM \'" + tableName->tableName + "\' ";
	sqlite3_stmt* stmt = nullptr;
	
	int exec_result = sqlite3_prepare_v2(db, Query.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {

		while (sqlite3_step(stmt) == SQLITE_ROW)
		{
			out.push_back(get_input_row(tableName, stmt));	
		}

		sqlite3_finalize(stmt);
	}

	return out;
}

std::vector<std::vector<std::string>> Database_Sqlite3::get_tableRows(const AM_Database_TableStruct* tableName, std::string whereQuery)
{
	std::vector<std::vector<std::string>> out;
	std::string Query = "SELECT * FROM \'" + tableName->tableName + "\' WHERE " + whereQuery;
	sqlite3_stmt* stmt = nullptr;

	int exec_result = sqlite3_prepare_v2(db, Query.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {

		while (sqlite3_step(stmt) == SQLITE_ROW)
		{
			out.push_back(get_input_row(tableName, stmt));
		}
	}

	sqlite3_finalize(stmt);
	return out;
}

std::vector<std::vector<std::string>> Database_Sqlite3::get_tableRows_joint(const AM_Database_TableStruct* tableName1,
	const AM_Database_TableStruct* tableName2, std::string columnName1, std::string columnName2, std::string whereQuery)
{
	std::vector<std::vector<std::string>> out;
	std::string Query = "SELECT * FROM \'" + tableName1->tableName + "\' INNER JOIN \'" + tableName2->tableName + "\' ON \'" + 
		tableName1->tableName + "." + columnName1 + "\' = \' " + tableName2->tableName + "." + columnName2 + "\' ";
	if (whereQuery.length() > 0) { Query += " WHERE " + whereQuery; }

	sqlite3_stmt* stmt;

	int exec_result = sqlite3_prepare_v2(db, Query.c_str(), -1, &stmt, 0);
	if (exec_result == SQLITE_OK) {

		while (sqlite3_step(stmt) == SQLITE_ROW)
		{
			// TODO check ti query
			out.push_back(get_input_row(tableName1, stmt));
		}
	}

	sqlite3_finalize(stmt);
	return out;
}

std::vector<std::vector<std::string>> Database_Sqlite3::get_fromQuery(std::string whereQuery)
{
	std::vector<std::vector<std::string>> out;
	sqlite3_stmt* stmt;

	//TODO check query
	int exec_result = sqlite3_prepare_v2(db, whereQuery.c_str(), -1, &stmt, 0);
	
	if (exec_result == SQLITE_OK) {

		while (sqlite3_step(stmt) == SQLITE_ROW)
		{
			int columnCount = sqlite3_data_count(stmt);
			std::vector<std::string> rowContent;
			for (int n1 = 0; n1 < columnCount; n1++)
			{
				const unsigned char* testy = sqlite3_column_text(stmt, n1);
				
				if (testy == nullptr) continue;
				rowContent.push_back(std::string((const char*)testy));
			}

			out.push_back(rowContent);
		}

	}

	sqlite3_finalize(stmt);

	
	return out;
}

std::string Database_Sqlite3::get_tableRows(std::string& tableName)
{
	std::string Query = "SELECT * FROM \'" + tableName + "\' ";
	AM_Database_TableStruct dataStruct = get_tableStruct(tableName);

	std::vector<std::vector<std::string>> outRows = get_tableRows(&dataStruct);
	std::string out = get_csv(outRows);
	return out;
}

std::string Database_Sqlite3::get_tableRows(std::string& tableName, std::string& whereQuery)
{
	std::string Query = "SELECT * FROM \'" + tableName + "\' WHERE " + whereQuery;
	AM_Database_TableStruct dataStruct = get_tableStruct(tableName);

	std::vector<std::vector<std::string>> outRows = get_tableRows(&dataStruct, whereQuery);
	std::string out = get_csv(outRows);
	return out;
}

std::vector<std::string> Database_Sqlite3::get_input_row(const AM_Database_TableStruct* tableName,
										sqlite3_stmt* stmt)
{
	int Index{ 0 };
	std::vector<std::string> rowContent;
	for each (std::string column in tableName->columnNames)
	{
		const unsigned char* testy = sqlite3_column_text(stmt, Index++);

		if (testy == nullptr) continue;
		rowContent.push_back((const char*)testy);
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

	sqlite3_finalize(stmt);
	return out;
}


#pragma endregion