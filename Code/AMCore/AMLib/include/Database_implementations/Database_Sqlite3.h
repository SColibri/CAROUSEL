#pragma once

#include <string>
#include <vector>
#include "../../interfaces/IAM_Database.h"
#include "../../external/sqlite3/sqlite3.h"
#include "../AM_Database_TableStruct.h"
#include "../../include/AM_Config.h"


/** \addtogroup AMLib
 *  @{
 */

 /// <summary>
 /// Database class that handles all functions needed from mysqlite3
 /// as a wrapper for this framework. More information on mysqlite3
 /// can be found here: https://www.sqlite.org/
 /// </summary>
class Database_Sqlite3 : public IAM_Database {
public:

	// Constructors, destructors and other
	// ---------------------------------------------
#pragma region Cons_Des
	Database_Sqlite3(AM_Config* configuration) :IAM_Database(configuration) {}
	~Database_Sqlite3();
#pragma endregion Cons_Des

#pragma region Implementation
	virtual int connect() override;
	virtual std::vector<std::string> get_tableNames() override;
	virtual int add_table(const AM_Database_TableStruct* newTable) override;
	virtual int drop_table(const AM_Database_TableStruct* tableName) override;
	virtual int clear_table(const AM_Database_TableStruct* tableName) override;
	virtual int remove_row(const AM_Database_TableStruct* tableName, int ID) override;
	virtual int remove_row(const AM_Database_TableStruct* tableName, std::string whereClause) override;
	virtual int insert_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData) override;
	virtual int update_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData) override;
	virtual int get_last_ID(const AM_Database_TableStruct* tableName) override;
	virtual std::vector<std::vector<std::string>> get_tableRows(const AM_Database_TableStruct* tableName) override;

#pragma endregion

#pragma region Methods


#pragma endregion Methods

private:
	sqlite3* db{ nullptr };
	char* _errorMessage{};
	bool _connectionOpen{ false };
	std::vector<std::string> _tableNames;
	std::string _databaseFile{};

private:

#pragma region Methods
	std::vector<std::string> get_input_row(const AM_Database_TableStruct* tableName,
											sqlite3_stmt* stmt);
#pragma endregion

};

//// References include:
//// https://www.sqlite.org/faq.html
//// https://zetcode.com/db/sqlitec/
//// https://www.codegrepper.com/code-examples/sql/sqlite3+get+data+from+table+c

/** @}*/