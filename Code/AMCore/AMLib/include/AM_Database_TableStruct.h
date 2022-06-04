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

	void add_new(std::string columnName, std::string columnDataType)
	{
		bool isContained{ false };

		for each (std::string ColName in columnNames)
		{
			if(std::strcmp(ColName.c_str(), columnName.c_str()) == 0)
			{	
				isContained = true;
				break;
			}
		}

		if(!isContained)
		{
			this->columnNames.push_back(columnName);
			this->columnDataType.push_back(columnDataType);
		}
	}

	static AM_Database_TableStruct join(AM_Database_TableStruct& Table1,
								 AM_Database_TableStruct& Table2)
	{
		AM_Database_TableStruct newStruct;

		for (int i = 0; i < Table1.columnNames.size(); i++)
		{
			newStruct.add_new(Table1.columnNames[i], Table1.columnDataType[i]);
		}

		for (int i = 0; i < Table2.columnNames.size(); i++)
		{
			newStruct.add_new(Table2.columnNames[i], Table2.columnDataType[i]);
		}

		return newStruct;
	}
};
