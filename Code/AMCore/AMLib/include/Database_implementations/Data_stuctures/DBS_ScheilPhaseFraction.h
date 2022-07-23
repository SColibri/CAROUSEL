#pragma once
#include "../../../interfaces/IAM_DBS.h"
/// <summary>
/// Implements IAM_DBS.h interface, this is a phase
/// object structure.
/// </summary>
class DBS_ScheilPhaseFraction : public IAM_DBS
{
public:
	int IDCase{ -1 };
	int IDPhase{ -1 };
	std::string TypeComposition{ "" };
	double Temperature {0.0};
	double Value{ -1.0 };

	DBS_ScheilPhaseFraction(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_ScheilPhaseFraction();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::ostringstream decimalString;
		decimalString << std::fixed;
		decimalString << std::setprecision(16);
		decimalString << Value;

		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDCase),
										std::to_string(IDPhase),
										TypeComposition,
										std::to_string(Temperature),
										decimalString.str()};
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
		_id = -1;
		if (rawData.size() < _tableStructure.columnNames.size()) return 1;

		set_id(std::stoi(rawData[0]));
		IDCase = std::stoi(rawData[1]);
		IDPhase = std::stod(rawData[2]);
		TypeComposition = rawData[3];
		Temperature = std::stod(rawData[4]);
		Value = std::stod(rawData[5]);
		return 0;
	}

#pragma endregion

private:

};
