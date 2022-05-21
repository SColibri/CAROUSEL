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
	double StartTemperature{ -1 };
	double EndTemperature{ -1 };
	double StepSize{ -1 };
	int DependentPhase{ "" };
	double minimumLiquidFraction{ -1 };

	DBS_ElementComposition(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_ScheilConfiguration();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDCase),
										std::to_string(StartTemperature),
										std::to_string(EndTemperature),
										std::to_string(StepSize),
										std::to_string(DependentPhase),
										std::to_string(minimumLiquidFraction) };
		return input;
	}

	virtual std::string get_load_string() override
	{
		return std::string(" ID = \'" + std::to_string(_id) + " \' ");
	}

	virtual int load() override
	{
		std::vector<std::string> rawData = get_rawData();
		if (rawData.size() < _tableStructure.columnNames.size()) return 1;

		IDCase = std::stoi(rawData[1]);
		StartTemperature = std::stod(rawData[2]);
		EndTemperature = std::stod(rawData[3]);
		StepSize = std::stod(rawData[4]);
		DependentPhase = std::stoi(rawData[5]);
		minimumLiquidFraction = std::stod(rawData[6]);
		return 0;
	}

#pragma endregion

private:

};