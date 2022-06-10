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
	double Temperature{ 700 };
	double StartTemperature{ 700 };
	double EndTemperature{ 700 };
	std::string TemperatureType{"C"};
	double Pressure{ 101325 };

	DBS_EquilibriumConfiguration(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_EquilibriumConfiguration();
	}

	DBS_EquilibriumConfiguration(DBS_EquilibriumConfiguration &toCopy) :
		IAM_DBS(toCopy._db)
	{
		_tableStructure = toCopy._tableStructure;
		IDCase = toCopy.IDCase;
		Temperature = toCopy.Temperature;
		StartTemperature = toCopy.StartTemperature;
		EndTemperature = toCopy.EndTemperature;
		TemperatureType = toCopy.TemperatureType;
		Pressure = toCopy.Pressure;
	}

	static int remove_equilibrium_data(IAM_Database* database, int CaseID)
	{
		std::string query = AMLIB::TN_EquilibriumPhaseFractions().columnNames[0] +
			" = " + std::to_string(CaseID);

		return database->remove_row(&AMLIB::TN_EquilibriumPhaseFractions(),query);
	}
#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDCase),
										std::to_string(Temperature),
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
		Temperature = std::stod(rawData[2]);
		StartTemperature = std::stod(rawData[3]);
		EndTemperature = std::stod(rawData[4]);
		TemperatureType = rawData[5];
		Pressure = std::stod(rawData[6]);
		return 0;
	}

#pragma endregion

private:

};

