#pragma once
#include "../../../interfaces/IAM_DBS.h"
/// <summary>
/// Implements IAM_DBS.h interface, this is a phase
/// object structure.
///
/// Note:
///	DBType specifies if the phase is contained in a database or other. Thus only Type
///	0 can be passed as parameters to the external calphad software
/// </summary>
class DBS_Phase : public IAM_DBS
{
private:
	std::string _loadString{ "" };

public:
	// [DBS_Parameter]
	std::string Name{ "" };
	// [DBS_Parameter]
	int DBType{ 0 };


	DBS_Phase(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_Phase();
	}

#pragma region methods
	void load_by_name(std::string phaseName)
	{
		_loadString = " " + _tableStructure.columnNames[1] + " = \'" + phaseName + "\' ";
		load();
	}

	bool element_exists(std::string phaseName)
	{
		DBS_Phase testElement(_db, -1);
		testElement.load_by_name(phaseName);
		if (testElement.id() == -1) return false;
		return true;
	}
#pragma endregion

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										Name,
											std::to_string(DBType)};
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
		_id = -1;
		if (rawData.size() < 3) return 1;
		set_id(std::stoi(rawData[0]));
		Name = rawData[1];
		DBType = std::stoi(rawData[2]);
		return 0;
	}

	virtual int check_before_save() override
	{
		if (Name.length() == 0) return 1;
		//if (element_exists(Name) == true) return 1;

		return 0;
	}

#pragma endregion

private:

};