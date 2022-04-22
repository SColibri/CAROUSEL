#pragma once

#include <vector>
#include <list>
#include "../include/AM_Database_TableStruct.h"
#include "../include/AM_Database.h"

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Datatable allows the user to ask and modify data without having to handle 
/// sql queries. wrapper for the framework.
/// </summary>
class AM_Database_Datatable
{
public:

#pragma region Des_Con
	AM_Database_Datatable(AM_Database* DB, AM_Database_TableStruct* TS);
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

	void load_data();

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

#pragma endregion Methods

private:
	AM_Database* _db; // connection to database
	const AM_Database_TableStruct* _tableStruct; // Table structure
	std::vector<std::vector<std::string>> _data; // Table data
	int _addedRows{0}; // Rows added to table, not contained in the database.
	std::list<int> _modifiedIndexList; // rows that have been called by the operator ()

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
