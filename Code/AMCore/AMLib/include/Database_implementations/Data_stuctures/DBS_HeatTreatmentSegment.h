#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "../../../x_Helpers/string_manipulators.h"

/// <summary>
/// Implements IAM_DBS.h interface, this is a Element
/// object structure.
/// </summary>
class DBS_HeatTreatmentSegment : public IAM_DBS
{
private:
	std::string _loadString{ "" };

public:
	int stepIndex{ -1 };
	long int IDHeatTreatment{ -1 };
	long int IDPrecipitationDomain{ -1 };
	double EndTemperature{ 25 };
	double TemperatureGradient{ 0 };
	double Duration{ 0.0 };

	DBS_HeatTreatmentSegment(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_HeatTreatmentSegments();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
									std::to_string(stepIndex),
									std::to_string(IDHeatTreatment),
									std::to_string(IDPrecipitationDomain),
									std::to_string(EndTemperature),
									std::to_string(TemperatureGradient),
									std::to_string(Duration)};
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
		stepIndex = std::stoi(rawData[1]);
		IDHeatTreatment = std::stoi(rawData[2]);
		IDPrecipitationDomain = std::stoi(rawData[3]);
		EndTemperature = std::stod(rawData[4]);
		TemperatureGradient = std::stod(rawData[5]);
		Duration = std::stod(rawData[6]);
		return 0;
	}

	virtual int check_before_save() override
	{
		return 0;
	}

#pragma endregion

};