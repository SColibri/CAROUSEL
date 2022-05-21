#pragma once
#include "../../../interfaces/IAM_DBS.h"

/// <summary>
/// Implements IAM_DBS.h interface, this is a Element
/// object structure.
/// </summary>
class DBS_Element : public IAM_DBS
{
public:
	std::string Name{ "" };

	DBS_Element(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_Element();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										Name };
		return input;
	}

	virtual std::string get_load_string() override
	{
		return std::string(" ID = \'" + std::to_string(_id) + " \' ");
	}

	virtual int load() override
	{
		std::vector<std::string> rawData = get_rawData();
		if (rawData.size() < 2) return 1;

		Name = rawData[1];
		return 0;
	}

#pragma endregion

private:

};