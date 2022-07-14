#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "../../../x_Helpers/string_manipulators.h"

/// <summary>
/// Implements IAM_DBS.h interface, this is a Element
/// object structure.
/// </summary>
class DBS_Element : public IAM_DBS
{
private:
	std::string _loadString{ "" };

public:
	std::string Name{ "" };

	DBS_Element(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_Element();
	}

#pragma region methods
	void load_by_name(std::string elementName)
	{
		string_manipulators::toCaps(elementName);
		_loadString = " " + _tableStructure.columnNames[1] + " = \'" + elementName + "\' ";
		load();
	}

	bool element_exists(std::string elementName) 
	{
		DBS_Element testElement(_db, -1);
		testElement.load_by_name(elementName);
		if (testElement.id() == -1) return false;
		return true;
	}
#pragma endregion

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										Name };
		return input;
	}

	virtual std::string get_load_string() override
	{
		if (_loadString.length() == 0) _loadString = " ID = \'" + std::to_string(_id) + " \' ";
		return _loadString;
	}


	virtual int load() override
	{
		std::vector<std::string> rawData = get_rawData();
		return load(rawData);
	}

	virtual int load(std::vector<std::string>& rawData) override
	{
		if (rawData.size() < _tableStructure.columnNames.size()) return 1;
		set_id(std::stoi(rawData[0]));
		Name = rawData[1];
		return 0;
	}

	virtual int check_before_save() override 
	{
		if (Name.length() == 0) return 1;
		if (element_exists(Name) == true) return 1;

		return 0;
	}

#pragma endregion

};