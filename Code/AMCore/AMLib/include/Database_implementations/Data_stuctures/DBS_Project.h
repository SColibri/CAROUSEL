#pragma once
#include "../../../interfaces/IAM_DBS.h"
/// <summary>
/// Implements IAM_DBS.h interface, this is a project
/// object structure.
/// </summary>
class DBS_Project: public IAM_DBS
{
public:
	std::string Name{""};
	std::string APIName{""};

	DBS_Project(IAM_Database* database, int id): 
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_Projects();
	}

	int load_ByName(std::string name) 
	{
		AM_Database_Datatable tempData(_db, &_tableStructure);
		tempData.load_data(_tableStructure.columnNames[1] + " = \'" + name + "\'");

		if (tempData.row_count() != 1) return 1; // No unique value was found!
		_id = std::stoi(tempData(0, 0));
		load();

		return 0;
	}

	static int remove_project_data(IAM_Database* database, int projectID)
	{
		std::string query = AMLIB::TN_SelectedElements().columnNames[1] +
			" = " + std::to_string(projectID);

		std::string queryCase = AMLIB::TN_Case().columnNames[1] +
			" = " + std::to_string(projectID);
		AM_Database_Datatable caseList(database, &AMLIB::TN_Case());
		caseList.load_data(queryCase);

		for(int n1 = 0; n1 < caseList.row_count(); n1++)
		{
			DBS_Case::remove_case_data(database, std::stoi(caseList(0,n1)));
		}

		return database->remove_row(&AMLIB::TN_SelectedElements(), query);
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										Name,
										APIName };
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
		Name = rawData[1];
		APIName = rawData[2];
		
		return 0;
	}

#pragma endregion

private:

};