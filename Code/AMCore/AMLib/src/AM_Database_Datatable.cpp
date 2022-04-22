#include "../include/AM_Database_Datatable.h"
#include <exception>
#include <algorithm>

#pragma region Des_Con
	AM_Database_Datatable::AM_Database_Datatable(AM_Database* DB, AM_Database_TableStruct* TS):
							_tableStruct(TS),_db(DB){}

	AM_Database_Datatable::~AM_Database_Datatable() {}
#pragma endregion Des_Con

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

#pragma region Methods

	void AM_Database_Datatable::load_data()
	{
		_addedRows = 0;
		_modifiedIndexList.clear();

		//TODO: load data to table
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
				_db->insert_row(_tableStruct, _data[i]);
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
				_db->update_row(_tableStruct, _data[i]);
			}

			_modifiedIndexList.clear();
		}
	}

	int AM_Database_Datatable::add_row(std::vector<std::string> Data)
	{
		if(Data.size() <= _tableStruct->columnNames.size())
		{
			std::vector<std::string> newData(
				_tableStruct->columnNames.size(), "-1");

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
#pragma endregion Methods

#pragma region Getters_Setters

	const std::string& AM_Database_Datatable::data(int column, int row)
	{
		check_out_of_bound(column, row);

		return _data[row][column];
	}

#pragma endregion Getters_Setters