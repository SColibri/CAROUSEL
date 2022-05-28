#pragma once
#include <string>
#include <vector>
#include "../include/AM_Database_TableStruct.h"
#include "../include/AM_Config.h"

/// <summary>
/// Database interface class that specifies how the database classes have to be specified.
/// </summary>
class IAM_Database
{

public:
	inline static const std::string Delimiter{","}; //csv delimiter

	IAM_Database(AM_Config* configuration) { _configuration = configuration; }
	~IAM_Database() {}

	/// <summary>
	/// Opens a connection to the database specified by filename.
	/// </summary>
	/// <param name="filename">full or relative path to file</param>
	/// <returns>loaded = 0</returns>
	virtual int connect() { return -1; }

	/// <summary>
	/// Obtains a list of tables that the database contains
	/// </summary>
	/// <returns>List of table names</returns>
	virtual std::vector<std::string> get_tableNames() { return std::vector<std::string>(); }

	/// <summary>
	/// Obtains a list of column names from table
	/// </summary>
	/// <returns>List of table names</returns>
	virtual std::vector<std::string> get_columnNames(std::string& tableName) { return std::vector<std::string>(); }

	/// <summary>
	/// Obtains a list of column names from table
	/// </summary>
	/// <returns>List of table names</returns>
	virtual std::vector<std::string> get_columnDatatype(std::string& tableName) { return std::vector<std::string>(); }

	/// <summary>
	/// Returns a table struct from table name
	/// </summary>
	/// <param name="tableName"></param>
	/// <returns></returns>
	virtual AM_Database_TableStruct get_tableStruct(std::string& tableName) { return AM_Database_TableStruct(); }

	/// <summary>
	/// Adds a new table to the database
	/// </summary>
	/// <param name="newTable">table structure</param>
	/// <returns>table created = 0</returns>
	virtual int add_table(const AM_Database_TableStruct* newTable) { return -1; }

	/// <summary>
	/// Removes table from database
	/// </summary>
	/// <param name="tableName">name of table in schema</param>
	/// <returns>table removed = 0</returns>
	virtual int drop_table(const AM_Database_TableStruct* tableName) { return -1; }

	/// <summary>
	/// Truncates table, removes all data and resets autoincrement index
	/// </summary>
	/// <param name="tableName">name of table in schema</param>
	/// <returns>truncated = 0</returns>
	virtual int clear_table(const AM_Database_TableStruct* tableName) { return -1; }

	/// <summary>
	/// Removes a row identified by its ID
	/// </summary>
	/// <param name="tableName">table name in schema</param>
	/// <param name="ID">ID data</param>
	/// <returns>removed = 0</returns>
	virtual int remove_row(const AM_Database_TableStruct* tableName, int ID) { return -1; }

	/// <summary>
	/// Removes a row by a specified parameter e.g. ID = 5 or Name = 'Jack in the box'
	/// </summary>
	/// <param name="tableName"></param>
	/// <param name="whereClause"></param>
	/// <returns></returns>
	virtual int remove_row(const AM_Database_TableStruct* tableName, std::string whereClause) { return -1; }

	/// <summary>
	/// Add new row to table
	/// </summary>
	/// <param name="tableName"></param>
	/// <param name="newData"></param>
	/// <returns></returns>
	virtual int insert_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData) { return -1; }

	/// <summary>
	/// Updates data in table
	/// </summary>
	/// <param name="tableName"></param>
	/// <param name="newData"></param>
	/// <returns></returns>
	virtual int update_row(const AM_Database_TableStruct* tableName, std::vector<std::string>& newData) { return -1; }

	/// <summary>
	/// gets last ID value
	/// </summary>
	/// <param name="tableName"></param>
	/// <param name="newData"></param>
	/// <returns></returns>
	virtual int get_last_ID(const AM_Database_TableStruct* tableName) { return -1; }

	/// <summary>
	/// getd table row data
	/// </summary>
	/// <param name="tableName"></param>
	/// <returns></returns>
	virtual std::vector<std::vector<std::string>> get_tableRows(const AM_Database_TableStruct* tableName) { return std::vector<std::vector<std::string>>(); }
	virtual std::vector<std::vector<std::string>> get_tableRows(const AM_Database_TableStruct* tableName, 
																std::string whereQuery) { return std::vector<std::vector<std::string>>(); }

	/// <summary>
	/// get table content in csv format
	/// </summary>
	/// <param name="tableName"></param>
	/// <returns></returns>
	virtual std::string get_tableRows(std::string& tableName) { return std::string("Not implemented"); }
	virtual std::string get_tableRows(std::string& tableName, std::string& whereQuery) { return std::string("Not implemented"); }


	/// <summary>
	/// Obtains row data from the database
	/// </summary>
	/// <param name="tableName"></param>
	/// <param name="Query"></param>
	/// <returns></returns>
	virtual std::vector<std::string> get_row(const AM_Database_TableStruct* tableName, 
											 std::string Query) { return std::vector<std::string>(); }

	/// <summary>
	/// from row vector we extract a csv formatted string
	/// </summary>
	/// <param name="tableData"></param>
	/// <returns></returns>
	static std::string get_csv(std::vector<std::vector<std::string>> tableData)
	{
		std::string out{ "" };

		for each (std::vector<std::string> rowItem in tableData)
		{
			out += csv_join_row(rowItem, Delimiter) + "\n";
		}

		return out;
	}

	/// <summary>
	/// join vector data into one string
	/// </summary>
	/// <param name="strings"></param>
	/// <param name="delim"></param>
	/// <returns></returns>
	static std::string csv_join_row(std::vector<std::string> const& strings, std::string delim)
	{
		std::stringstream ss;
		std::copy(strings.begin(), strings.end(),
			std::ostream_iterator<std::string>(ss, delim.c_str()));
		return ss.str();
	}

protected:
	AM_Config* _configuration{nullptr}; // configuration file

	
};