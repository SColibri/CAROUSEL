#pragma once
#include <string>
#include "../../AMLib/interfaces/IAM_Database.h"
#include "../../AMLib/include/Database_implementations/Database_scheme_content.h"
#include "../../AMLib/include/AM_Database_Datatable.h"
#include "../../AMLib/include/AM_Database_TableStruct.h"

/// <summary>
/// Database object interface, handles loading and saving data
/// into the database. The concept is that each table is an object
/// described as a DataBaseStructure (DBS)_tablename. We can use this
/// to load specific data into an object oriented scheme.
/// However this might not be the best approach when handeling large
/// databases, thus for large amount of data we should use 
/// AM_Database_Datatable.h for loading a large amount of data.
/// </summary>
class IAM_DBS
{
public:
	IAM_DBS(IAM_Database* database):_db(database) {}
	
#pragma region virtual

	/// <summary>
	/// Constructs a vector that contains all varibles in an ordered fashion and
	/// as declared under the database_scheme_content.h 
	/// </summary>
	/// <returns></returns>
	virtual std::vector<std::string> get_input_vector()
	{
		return std::vector<std::string>();
	}

	/// <summary>
	/// Returns a string for building the query sent to the load fundtion.
	/// here we only add the content for the WHERE clause. for example: "ID = 1"
	/// Note: Do not add the WHERE, add only the statement that comes before that
	/// like column names, limit or ordering schemes
	/// </summary>
	/// <returns></returns>
	virtual std::string get_load_string()
	{
		return std::string("");
	}

	/// <summary>
	/// Loads data and udpates all variable contents
	/// </summary>
	/// <returns></returns>
	virtual int load()
	{
		std::vector<std::string> rawData = get_rawData();
		return load(rawData);
	}

	/// <summary>
	/// Load from available data
	/// </summary>
	/// <returns></returns>
	virtual int load(std::vector<std::string>& rawData)
	{
		return -1;
	}

#pragma endregion

#pragma region Methods
	/// <summary>
	/// Save data to database
	/// </summary>
	/// <returns></returns>
	int save()
	{
		std::vector<std::string> input = get_input_vector();
		if (std::strcmp(input[0].c_str(), "-1") == 0)
		{
			if (check_before_save() != 0) return 1;
			int insertC = _db->insert_row(&_tableStructure, input);
			_id = _db->get_last_ID(&_tableStructure);
		}
		else
		{
			_db->update_row(&_tableStructure, input);
		}

		return 0;
	}

	/// <summary>
	/// id of current item
	/// </summary>
	/// <returns></returns>
	const int& id() { return _id; }
	void set_id(int newID) { _id = newID; }

#pragma endregion

protected:
	IAM_Database* _db{ nullptr }; // database pointer to implementation
	AM_Database_TableStruct _tableStructure; // Table layout specified in the implementation
	int _id{ -1 }; // ID of item

	/// <summary>
	/// Obtains a vector of string with data obtained from the database
	/// as specified by the query generated on the get_load_string function.
	/// Afterwards the implementation should load the data implementing the 
	/// load function.
	/// </summary>
	std::vector<std::string> get_rawData()
	{
		return _db->get_row(&_tableStructure, get_load_string());
	}

	virtual int check_before_save() { return 0; }
};