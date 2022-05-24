#pragma once
#include "../../../interfaces/IAM_DBS.h"
/// <summary>
/// Implements IAM_DBS.h interface, this is a phase
/// object structure.
/// </summary>
class DBS_ElementComposition : public IAM_DBS
{
public:
	int IDCase{ -1 };
	int IDElement{ -1 };
	std::string TypeComposition{ "" };
	double Value{ -1 };

	DBS_ElementComposition(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_ElementComposition();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDCase),
										std::to_string(IDElement),
										TypeComposition,
										std::to_string(Value) };
		return input;
	}

	virtual std::string get_load_string() override
	{
		return std::string(" ID = \'" + std::to_string(_id) + " \' ");
	}

	virtual int load() override
	{
		std::vector<std::string> rawData = get_rawData();
		if (rawData.size() < 5) return 1;

		IDCase = std::stoi(rawData[1]);
		IDElement = std::stoi(rawData[2]);
		TypeComposition = rawData[3];
		Value = std::stod(rawData[4]);
		return 0;
	}

#pragma endregion

private:

};