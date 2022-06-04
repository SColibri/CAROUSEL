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
	int IDGroup{ 0 };
	std::string Name{ "" };
	std::string Script{ "" };
	std::string Date{ "" };
	double PosX{ 0 };
	double PosY{ 0 };
	double PosZ{ 0 };

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
										std::to_string(IDGroup),
										Name,
										Script,
										Date,
										std::to_string(PosX),
										std::to_string(PosY),
										std::to_string(PosZ)};
		return input;
	}

	virtual std::string get_load_string() override
	{
		return std::string(" ID = \'" + std::to_string(_id) + " \' ");
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
		IDProject = std::stoi(rawData[1]);
		IDGroup = std::stoi(rawData[2]);
		Name = rawData[3];
		Script = rawData[4];
		Date = rawData[5];
		PosX = std::stold(rawData[6]);
		PosY = std::stold(rawData[7]);
		PosZ = std::stold(rawData[8]);
		return 0;
	}

#pragma endregion

private:

};