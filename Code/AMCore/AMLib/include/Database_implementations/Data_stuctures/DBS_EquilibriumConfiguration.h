#pragma once
#include "../../../interfaces/IAM_DBS.h"
/// <summary>
/// Implements IAM_DBS.h interface, this is a Equilibrium configuration
/// object structure.
/// </summary>
class DBS_EquilibriumConfiguration : public IAM_DBS
{
public:
	int IDCase{ -1 };
	double Temperatre{ 0.0 };
	double StartTemperature{ 0.0 };
	double EndTemperature{ 0.0 };
	std::string TemperatureType{"C"};
	double Pressure{ 101325 };

	DBS_EquilibriumConfiguration(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_EquilibriumConfiguration();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDCase),
										std::to_string(Temperatre),
										std::to_string(StartTemperature),
										std::to_string(EndTemperature),
										TemperatureType,
										std::to_string(Pressure)};
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
		if (rawData.size() <= _tableStructure.columnNames.size()) return 1;
		set_id(std::stoi(rawData[0]));
		IDCase = std::stoi(rawData[1]);
		Temperatre = std::stod(rawData[2]);
		StartTemperature = std::stod(rawData[3]);
		EndTemperature = std::stod(rawData[4]);
		TemperatureType = rawData[5];
		Pressure = std::stod(rawData[6]);
		return 0;
	}

#pragma endregion

private:

};

