#pragma once

#include <vector>
#include <list>
#include "../include/AM_Database_TableStruct.h"
#include "../interfaces/IAM_Database.h"

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Datatable allows the user to ask and modify data without having to handle 
/// sql queries. wrapper for the framework.
/// </summary>
class AM_Database_Datatable
{
private:
	IAM_Database* _db; // connection to database
	AM_Database_TableStruct _tableStruct; // Table structure
	std::vector<std::vector<std::string>> _data; // Table data
	int _addedRows{ 0 }; // Rows added to table, not contained in the database.
	std::list<int> _modifiedIndexList; // rows that have been called by the operator ()

public:

#pragma region Des_Con
	AM_Database_Datatable(IAM_Database* DB, AM_Database_TableStruct* TS);
	~AM_Database_Datatable();
#pragma endregion Des_Con

#pragma region over
	/// <summary>
	/// Gets a modifiable reference from the data using a 2D matrix 
	/// indexing format. using this function will add the row to the
	/// update list.
	/// </summary>
	/// <param name="column">column index</param>
	/// <param name="row">row index</param>
	/// <returns>value in column as string</returns>
	std::string& operator()(int column, int row);


#pragma endregion over

#pragma region Methods

	/// <summary>
	/// load all data in table
	/// </summary>
	void load_data();

	/// <summary>
	/// Loads data as specified on the Query
	/// </summary>
	/// <param name="whereQuery"></param>
	void load_data(std::string whereQuery);

	/// <summary>
	/// Saves new data and updates all rows flagged by the 
	/// update list
	/// </summary>
	void save();

	/// <summary>
	/// Add new row to datatable
	/// </summary>
	/// <param name="Data"></param>
	/// <returns></returns>
	int add_row(std::vector<std::string> Data);

	/// <summary>
	/// Obtains a read only reference of the string conatined
	/// in the table. Using this function will not add
	/// to the update list.
	/// </summary>
	/// <param name="column"></param>
	/// <param name="row"></param>
	/// <returns></returns>
	const std::string& data(int column, int row);

	/// <summary>
	/// Obtains row data from a specific column
	/// </summary>
	/// <param name="columnNumber"></param>
	/// <returns></returns>
	std::vector<std::string> get_column_data(int columnNumber);

	/// <summary>
	/// get table structure (columnnames and datatype)
	/// </summary>
	/// <returns></returns>
	const AM_Database_TableStruct get_table_structure();

	std::string get_csv()
	{
		std::string out = IAM_Database::get_csv(_data);
		return out;
	}

	int row_count()
	{
		return _data.size();
	}

	int column_count()
	{
		return _tableStruct.columnNames.size();
	}

	std::vector<std::string>& get_row_data(int row_index)
	{
		if (row_index > row_count() - 1) throw "Index out of bounds";
		return _data[row_index];
	}

#pragma endregion Methods


private:
	/// <summary>
	/// checks if the index is out of bound
	/// </summary>
	/// <param name="column"></param>
	/// <param name="row"></param>
	void check_out_of_bound(int column, int row);

	/// <summary>
	/// Saves the last items added and flagged by addedRows
	/// when calling the function add_rows
	/// </summary>
	void save_added_rows();

	/// <summary>
	/// Updates in the database all indexes called by the 
	/// operator () - overloading.
	/// </summary>
	void update_modified_rows();
};
/** @}*/
