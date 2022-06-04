#pragma once
#include "../../../interfaces/IAM_DBS.h"

class DBS_SelectedPhases : public IAM_DBS 
{
public:
	int IDCase{ -1 };
	int IDPhase{ -1 };

	DBS_SelectedPhases(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_SelectedPhases();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDCase),
										std::to_string(IDPhase) };
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
		IDPhase = std::stoi(rawData[2]);
		return 0;
	}

#pragma endregion

};