#pragma once
#include <string>
#include <vector>

/// <summary>
/// structure of a table in the database
/// </summary>
struct AM_Database_TableStruct 
{

	std::string tableName; // name of table
	std::vector<std::string> columnNames; // column names in table
	std::vector<std::string> columnDataType; // column data type in table 

};
