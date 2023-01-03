#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "../../../x_Helpers/string_manipulators.h"

class DBS_HeatTreatmentProfile : public IAM_DBS
{
public:
	//General
	// [DBS_Parameter]
	int IDHeatTreatment{ -1 };
	// [DBS_Parameter]
	double Time{ 0.0 };
	// [DBS_Parameter]
	double Temperature{ 0.0 };

	DBS_HeatTreatmentProfile(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_HeatTreatmentProfile();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDHeatTreatment),
										string_manipulators::double_to_string(Time),
										string_manipulators::double_to_string(Temperature) };
		return input;
	}

	virtual std::string get_load_string() override
	{
		return std::string(" ID = " + std::to_string(_id) + "  ");
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

		int index = 0;
		set_id(std::stoi(rawData[index++]));
		IDHeatTreatment = std::stoi(rawData[index++]);
		Time = std::stod(rawData[index++]);
		Temperature = std::stod(rawData[index++]);

		return 0;
	}

#pragma endregion

};