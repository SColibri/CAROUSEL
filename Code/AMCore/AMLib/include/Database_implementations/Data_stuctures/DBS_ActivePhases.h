#pragma once

#include "../../../interfaces/IAM_DBS.h"

class DBS_ActivePhases : public IAM_DBS
{
public:
	long int IDProject{ -1 };
	long int IDPhase{ -1 };

	DBS_ActivePhases(IAM_Database* database, long int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_ActivePhases();
	}

	static int remove_selection_data(IAM_Database* database, long int CaseID)
	{
		std::string query = AMLIB::TN_ActivePhases().columnNames[0] +
			" = " + std::to_string(CaseID);

		return database->remove_row(&AMLIB::TN_ActivePhases(), query);
	}

	static int remove_project_data(IAM_Database* database, long int projectID)
	{
		std::string query = AMLIB::TN_ActivePhases().columnNames[1] +
			" = " + std::to_string(projectID);

		return database->remove_row(&AMLIB::TN_ActivePhases(), query);
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDProject),
										std::to_string(IDPhase)};
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
		IDPhase = std::stoi(rawData[2]);
		return 0;
	}

#pragma endregion

};
