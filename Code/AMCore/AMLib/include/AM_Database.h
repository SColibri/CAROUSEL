#pragma once

#include <string>
#include <vector>
#include "../../AMLib/external/sqlite3/sqlite3.h"
#include "../../AMLib/include/AM_Database_TableStruct.h"

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Database class that handles all functions needed from mysqlite3
/// as a wrapper for this framework. More information on mysqlite3
/// can be found here: https://www.sqlite.org/
/// </summary>
class AM_Database {
public:

	// Constructors, destructors and other
	// ---------------------------------------------
#pragma region Cons_Des
	AM_Database(){};
	~AM_Database();
#pragma endregion Cons_Des

	// Methods
	// ---------------------------------------------
#pragma region Methods
	/// <summary>
	/// Opens a connection to the sqlite3 database specified by filename.
	/// </summary>
	/// <param name="filename">full or relative path to file</param>
	/// <returns>loaded = 0</returns>
	int connect(std::string filename);

	/// <summary>
	/// Obtains a list of tables that the database contains
	/// </summary>
	/// <returns>List of table names</returns>
	std::vector<std::string> get_tableNames();

	/// <summary>
	/// Adds a new table to the database
	/// </summary>
	/// <param name="newTable">table structure</param>
	/// <returns>table created = 0</returns>
	int add_table(const AM_Database_TableStruct* newTable);

	/// <summary>
	/// Removes table from database
	/// </summary>
	/// <param name="tableName">name of table in schema</param>
	/// <returns>table removed = 0</returns>
	int drop_table(const AM_Database_TableStruct* tableName);

	/// <summary>
	/// Truncates table, removes all data and resets autoincrement index
	/// </summary>
	/// <param name="tableName">name of table in schema</param>
	/// <returns>truncated = 0</returns>
	int clear_table(const AM_Database_TableStruct* tableName);

	/// <summary>
	/// Removes a row identified by its ID
	/// </summary>
	/// <param name="tableName">table name in schema</param>
	/// <param name="ID">ID data</param>
	/// <returns>removed = 0</returns>
	int remove_row(const AM_Database_TableStruct* tableName, int ID);
	
	/// <summary>
	/// Removes a row by a specified parameter e.g. ID = 5 or Name = 'Jack in the box'
	/// </summary>
	/// <param name="tableName"></param>
	/// <param name="whereClause"></param>
	/// <returns></returns>
	int remove_row(const AM_Database_TableStruct* tableName, std::string whereClause);

	int insert_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData);

	int update_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData);


#pragma endregion Methods

private:
	sqlite3* db;
	char* _errorMessage{};
	bool _connectionOpen;
	std::vector<std::string> _tableNames;

private:


};

//// References include:
//// https://www.sqlite.org/faq.html
//// https://zetcode.com/db/sqlitec/
//// https://www.codegrepper.com/code-examples/sql/sqlite3+get+data+from+table+c

/** @}*/
