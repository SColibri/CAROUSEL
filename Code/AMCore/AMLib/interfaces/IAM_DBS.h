#pragma once
#include <string>
#include <vector>
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
	/// Constructs a vector that contains all variables in an ordered fashion and
	/// as declared under the database_scheme_content.h 
	/// </summary>
	/// <returns></returns>
	virtual std::vector<std::string> get_input_vector()
	{
		return std::vector<std::string>();
	}

	/// <summary>
	/// Returns a string for building the query sent to the load function.
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
	/// Loads data and updates all variable contents
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
	/// 
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
			// if implemented check before save
			if (check_before_save() != 0) return 1;

			// Store in the database
			int insertC = _db->insert_row(&_tableStructure, input);
			_id = _db->get_last_ID(&_tableStructure);

			// If implemented Create all dependent data
			if(_id > -1) create_dependent_data();
		}
		else
		{
			return _db->update_row(&_tableStructure, input);
		}

		return 0;
	}

	/// <summary>
	/// Batch save of DBS objects. A faster way to save multiple objects in one go. This functions
	/// does not implement the function calls to create dependent data, this you have to do manually. This
	/// function is normally not used for items that have dependent data (e.g. Phase fractions) making it fast to save
	/// a lot of data.
	/// </summary>
	/// <param name="vectorSave"></param>
	/// <returns></returns>
	static int save(std::vector<IAM_DBS*>& vectorSave)
	{
		if (vectorSave.size() == 0) return 1;
		std::string stringBuild{ "BEGIN TRANSACTION;\n" }; //TODO: this is specific to SQLIite 3

		for(int n1 = 0; n1 < vectorSave.size(); n1++)
		{
			stringBuild += vectorSave[n1]->_db->get_insert_query(&vectorSave[n1]->_tableStructure, vectorSave[n1]->get_input_vector()) + "\n";
		}

		stringBuild += "COMMIT;";
		
		return vectorSave[0]->_db->execute_query(stringBuild);
	}

	/// <summary>
	/// Save uninitialized item using as input a csv formatted vector.
	/// If the object is new it will create a new ID for the object.
	/// </summary>
	/// <param name="DBSObject"></param>
	/// <param name="csvF"></param>
	/// <returns>ID of initialized object</returns>
	static int save(IAM_DBS* DBSObject, std::vector<std::string>& csvF)
	{
		if (DBSObject == nullptr) return -1;

		// create new config object if non existent
		if (std::stoi(csvF[0]) == -1)
		{
			DBSObject->load(csvF);
			DBSObject->save();
			csvF[0] = std::to_string(DBSObject->id());
		}

		DBSObject->load(csvF);
		DBSObject->save();

		return DBSObject->id();
	}

	/// <summary>
	/// Create dependent data for new items
	/// </summary>
	/// <returns></returns>
	virtual int create_dependent_data() { return 0; }

	/// <summary>
	/// Removes initialized objects (has a unique id).
	/// </summary>
	/// <returns></returns>
	virtual int remove() 
	{
		if (_id == -1) return 1;
		return _db->remove_row(&_tableStructure, _tableStructure.columnNames[0] + " = \'" + std::to_string(_id) + "\'");
	}

	/// <summary>
	/// Should remove all data that depends on this item. After removing the item
	/// the dependence is lost and therefore useless.
	/// </summary>
	/// <returns></returns>
	virtual int remove_dependent_data() { return 0; }

	/// <summary>
	/// Remove Item by ID, this function calls the remove_dependent_data which
	/// removes all data that points to this object.
	/// </summary>
	/// <param name="DBSObject">Uninitialized objecct pointer</param>
	/// <param name="csvF">data in csv format</param>
	/// <returns></returns>
	static int remove_byID(IAM_DBS* DBSObject, std::vector<std::string>& csvF)
	{
		int result{ -1 };

		// remove existing ID
		if (std::stoi(csvF[0]) > -1)
		{
			DBSObject->remove_dependent_data();
			result = DBSObject->_db->remove_row(&DBSObject->_tableStructure, "ID=" + csvF[0]);
		}

		return result;
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