#pragma once
#include "../../../interfaces/IAM_DBS.h"
/// <summary>
/// Implements IAM_DBS.h interface, this is a phase
/// object structure.
/// </summary>
class DBS_ScheilConfiguration : public IAM_DBS
{
public:
	int IDCase{ -1 };
	double StartTemperature{ 700 };
	double EndTemperature{ 700 };
	double StepSize{ -25 };
	int DependentPhase{ -1 };
	double minimumLiquidFraction{ 0.01 };

	DBS_ScheilConfiguration(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_ScheilConfiguration();
	}

	static int remove_scheil_data(IAM_Database* database, int CaseID)
	{
		std::string query = AMLIB::TN_ScheilConfiguration().columnNames[0] +
			" = " + std::to_string(CaseID);

		return database->remove_row(&AMLIB::TN_ScheilConfiguration(), query);
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
		return load(rawData);
	}

	virtual int load(std::vector<std::string>& rawData) override
	{
		if (rawData.size() < _tableStructure.columnNames.size()) return 1;

		set_id(std::stoi(rawData[0]));
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