#pragma once
#include "../../../interfaces/IAM_DBS.h"
/// <summary>
/// Implements IAM_DBS.h interface, this is a Equilibrium Phase fraction
/// object structure.
/// </summary>
class DBS_EquilibriumPhaseFraction : public IAM_DBS
{
public:
	int IDCase{ -1 };
	int IDPhase{ -1 };
	double Temperature{ 0.0 };
	double Value{ -1.0 };

	DBS_EquilibriumPhaseFraction(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_EquilibriumPhaseFractions();
	}

	static int remove_scheil_data(IAM_Database* database, int CaseID)
	{
		std::string query = AMLIB::TN_ScheilPhaseFraction().columnNames[0] +
			" = " + std::to_string(CaseID);

		return database->remove_row(&AMLIB::TN_ScheilPhaseFraction(), query);
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
		if (rawData.size() <= _tableStructure.columnNames.size()) return 1;
		set_id(std::stoi(rawData[0]));
		IDCase = std::stoi(rawData[1]);
		IDPhase = std::stoi(rawData[2]);
		Temperature = std::stod(rawData[3]);
		Value = std::stod(rawData[4]);
		return 0;
	}

#pragma endregion

private:

};