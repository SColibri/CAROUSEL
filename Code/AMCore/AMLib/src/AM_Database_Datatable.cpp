#include "../include/AM_Database_Datatable.h"
#include <exception>
#include <algorithm>

// Constructors, destructors and other
// -------------------------------------------------------------------------------------
#pragma region Des_Con
	AM_Database_Datatable::AM_Database_Datatable(IAM_Database* DB, AM_Database_TableStruct* TS):
						   _db(DB)
	{
		_tableStruct.tableName = TS->tableName;
		_tableStruct.columnNames = TS->columnNames;
		_tableStruct.columnDataType = TS->columnDataType;

	}

	AM_Database_Datatable::~AM_Database_Datatable() {}
#pragma endregion Des_Con

// Overloading
// -------------------------------------------------------------------------------------
#pragma region over
	std::string& AM_Database_Datatable::operator()(int column, int row) 
	{
		check_out_of_bound(column, row);

		if (std::find(_modifiedIndexList.begin(),
			_modifiedIndexList.end(), row) !=
			_modifiedIndexList.end())
			_modifiedIndexList.push_back(row);

		return _data[row][column];
	}
#pragma endregion over

// Methods
// -------------------------------------------------------------------------------------
#pragma region Methods

	void AM_Database_Datatable::load_data()
	{
		_addedRows = 0;
		_modifiedIndexList.clear();
		_data = _db->get_tableRows(&_tableStruct);
	}

	void AM_Database_Datatable::load_data(std::string whereQuery)
	{
		_addedRows = 0;
		_modifiedIndexList.clear();
		_data = _db->get_tableRows(&_tableStruct, whereQuery);
	}

	void AM_Database_Datatable::save()
	{
		save_added_rows();
		update_modified_rows();
	}

	void AM_Database_Datatable::save_added_rows()
	{
		if (_addedRows > 0) {
			int start_index = _data.size() - _addedRows;
			int end_index = _data.size();

			for (size_t i = start_index; i < end_index; i++)
			{
				_db->insert_row(&_tableStruct, _data[i]);
			}

			_addedRows = 0;
		}
	}

	void AM_Database_Datatable::update_modified_rows()
	{
		if (_modifiedIndexList.size() > 0) {
			
			int start_index = 0;
			int end_index = _modifiedIndexList.size();

			for (size_t i = start_index; i < end_index; i++)
			{
				_db->update_row(&_tableStruct, _data[i]);
			}

			_modifiedIndexList.clear();
		}
	}

	int AM_Database_Datatable::add_row(std::vector<std::string> Data)
	{
		if(Data.size() <= _tableStruct.columnNames.size())
		{
			std::vector<std::string> newData(
				_tableStruct.columnNames.size(), "-1");

			for (size_t i = 1; i < Data.size(); i++)
			{
				newData[i] = Data[i];
			}

			_data.push_back(newData);
			_addedRows++;
			return 0;
		}

		return 1;
	}

	

	void AM_Database_Datatable::check_out_of_bound(int column, int row)
	{
		if (_data.size() == 0) { throw "No data in datatable"; }
		if (row >= _data.size() || column >= _data[0].size()) {
			throw "out of index";
		}
	}

	std::vector<std::string> AM_Database_Datatable::get_column_data(int columnNumber)
	{
		std::vector<std::string> out;
		if(_tableStruct.columnNames.size() > columnNumber)
		{
			for each (std::vector<std::string> cell in _data)
			{
				out.push_back(cell[columnNumber]);
			}
		}

		return out;
	}
#pragma endregion Methods

// Getters and setters
// -------------------------------------------------------------------------------------
#pragma region Getters_Setters

	const std::string& AM_Database_Datatable::data(int column, int row)
	{
		check_out_of_bound(column, row);

		return _data[row][column];
	}

#pragma endregion Getters_Setters