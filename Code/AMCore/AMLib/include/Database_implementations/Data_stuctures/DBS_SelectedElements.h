#pragma once
#include "../../../interfaces/IAM_DBS.h"

class DBS_SelectedElements : public IAM_DBS
{
public:
	int IDProject{ -1 };
	int IDElement{ -1 };

	DBS_SelectedElements(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_SelectedElements();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDProject),
										std::to_string(IDElement) };
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
		IDProject = std::stoi(rawData[1]);
		IDElement = std::stoi(rawData[2]);
		return 0;
	}

#pragma endregion

};
