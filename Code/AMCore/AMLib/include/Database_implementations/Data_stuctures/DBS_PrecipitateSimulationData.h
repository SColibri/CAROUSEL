#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "../../../x_Helpers/string_manipulators.h"

/// <summary>
/// Implements IAM_DBS.h interface, this is a phase fraction obtained from a
/// heat treatment simulation
/// object structure.
/// </summary>
class DBS_PrecipitateSimulationData : public IAM_DBS
{
private:
	std::string _loadString{ "" };

public:
	int IDPrecipitationPhase{ -1 };
	int IDHeatTreatment{ -1 };
	double Time{ 0 };
	double PhaseFraction{ 0 };
	double NumberDensity{ 0 };
	double MeanRadius{ 0 };

	DBS_PrecipitateSimulationData(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_PrecipitateSimulationData();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDPrecipitationPhase),
										std::to_string(IDHeatTreatment),
										string_manipulators::double_to_string(Time),
										string_manipulators::double_to_string(PhaseFraction),
										string_manipulators::double_to_string(NumberDensity),
										string_manipulators::double_to_string(MeanRadius) };
		return input;
	}

	virtual std::string get_load_string() override
	{
		if (_loadString.length() == 0) _loadString = " ID = \'" + std::to_string(_id) + " \' ";
		return _loadString;
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
		IDPrecipitationPhase = std::stoi(rawData[1]);
		IDHeatTreatment = std::stoi(rawData[2]);
		Time = std::stod(rawData[3]);
		PhaseFraction = std::stod(rawData[4]);
		NumberDensity = std::stod(rawData[5]);
		MeanRadius = std::stod(rawData[6]);
		return 0;
	}

	virtual int check_before_save() override
	{
		return 0;
	}

#pragma endregion

};