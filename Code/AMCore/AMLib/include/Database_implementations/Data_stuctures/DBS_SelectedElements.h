#pragma once
#include "../../../interfaces/IAM_DBS.h"

class DBS_SelectedElements : public IAM_DBS
{
public:
	// [DBS_Parameter]
	int IDProject{ -1 };
	// [DBS_Parameter]
	int IDElement{ -1 };
	// [DBS_Parameter]
	int isReferenceElement{ 0 }; // 0 - false || 1 - True

	DBS_SelectedElements(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_SelectedElements();
	}

	static int remove_project_data(IAM_Database* database, int projectID)
	{
		std::string query = AMLIB::TN_SelectedElements().columnNames[1] +
			" = " + std::to_string(projectID);

		return database->remove_row(&AMLIB::TN_SelectedElements(), query);
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDProject),
										std::to_string(IDElement),
										std::to_string(isReferenceElement)};
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
		IDElement = std::stoi(rawData[2]);
		isReferenceElement = std::stoi(rawData[3]);
		return 0;
	}

#pragma endregion

};
