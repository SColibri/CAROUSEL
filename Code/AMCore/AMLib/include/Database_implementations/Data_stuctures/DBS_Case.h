#pragma once
#include "../../../interfaces/IAM_DBS.h"
/// <summary>
/// Implements IAM_DBS.h interface, this is a phase
/// object structure.
/// </summary>
class DBS_Case : public IAM_DBS
{
public:
	int IDProject{ -1 };
	std::string ScriptName{ "" };
	std::string Date{ "" };

	DBS_Case(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_Case();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDProject),
										ScriptName,
										Date};
		return input;
	}

	virtual std::string get_load_string() override
	{
		return std::string(" ID = \'" + std::to_string(_id) + " \' ");
	}

	virtual int load() override
	{
		std::vector<std::string> rawData = get_rawData();
		if (rawData.size() < 4) return 1;

		IDProject = std::stoi(rawData[1]);
		ScriptName = rawData[2];
		Date = RawData[3];
		return 0;
	}

#pragma endregion

private:

};