#pragma once

#include "../../../interfaces/IAM_DBS.h"

class DBS_ActivePhases_Configuration : public IAM_DBS
{
public:
	int IDProject{ -1 };
	int StartTemp{ 700 };
	int EndTemp{ 20 };
	int StepSize{ 25 };

	DBS_ActivePhases_Configuration(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_ActivePhases_Configuration();
	}

	static int remove_selection_data(IAM_Database* database, int CaseID)
	{
		std::string query = AMLIB::TN_ActivePhases_Configuration().columnNames[0] +
			" = " + std::to_string(CaseID);

		return database->remove_row(&AMLIB::TN_ActivePhases_Configuration(), query);
	}

	static int remove_project_data(IAM_Database* database, int ProjectID)
	{
		std::string query = AMLIB::TN_ActivePhases().columnNames[1] +
			" = " + std::to_string(ProjectID);

		return database->remove_row(&AMLIB::TN_ActivePhases(), query);
	}
#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDProject),
										std::to_string(StartTemp),
										std::to_string(EndTemp),
										std::to_string(StepSize) };
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
		IDProject = std::stoi(rawData[1]);
		StartTemp = std::stoi(rawData[2]);
		EndTemp = std::stoi(rawData[3]);
		StepSize = std::stoi(rawData[4]);
		return 0;
	}
#pragma endregion

};
