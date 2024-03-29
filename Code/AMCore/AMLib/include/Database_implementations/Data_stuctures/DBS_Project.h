#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "DBS_Case.h"
#include "DBS_ActivePhases_ElementComposition.h"
#include "DBS_ActivePhases.h"
#include "../../callbackFunctions/MessageCallBack.h"

/// <summary>
/// Implements IAM_DBS.h interface, this is a project
/// object structure.
/// </summary>
class DBS_Project : public IAM_DBS
{
public:
#pragma region Fields

#pragma endregion

#pragma region Properties
	// [DBS_Parameter] Project Name
	std::string Name{ "" };
	// [DBS_Parameter] Name API used by the framework to communicate with the external software (e.g. AM_API_matcalc)
	std::string APIName{ "" };
	// [DBS_Parameter] name of External software API (e.g. Matcalc )
	std::string External_APIName{ "" };
#pragma endregion

#pragma region Constructor
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="database">IAM_Database object</param>
	/// <param name="id">Identifier, use -1 if new</param>
	DBS_Project(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_Projects();
	}
#pragma endregion

#pragma region Methods
	/// <summary>
	/// Loads project by name
	/// </summary>
	/// <param name="name"></param>
	/// <returns>Returns 0 when successful</returns>
	int load_ByName(std::string name)
	{
		AM_Database_Datatable tempData(_db, &_tableStructure);
		tempData.load_data(_tableStructure.columnNames[1] + " = \'" + name + "\'");

		if (tempData.row_count() != 1) return 1; // No unique value was found!
		_id = std::stoi(tempData(0, 0));
		load();

		return 0;
	}

	/// <summary>
	/// Removes project from database, including all relational data
	/// </summary>
	/// <param name="database">IAM_Database object</param>
	/// <param name="projectID">Id</param>
	/// <returns>Returns 0 when successful</returns>
	static int remove_project_data(IAM_Database* database, int projectID)
	{
		// Remove active phases element composition
		DBS_ActivePhases_ElementComposition::remove_project_data(database, projectID);
		DBS_ActivePhases::remove_project_data(database, projectID);

		// get selected Elements from project
		std::string query = AMLIB::TN_SelectedElements().columnNames[1] +
			" = " + std::to_string(projectID);

		// Load all cases and delete related data
		std::string queryCase = AMLIB::TN_Case().columnNames[1] +
			" = " + std::to_string(projectID);
		AM_Database_Datatable caseList(database, &AMLIB::TN_Case());
		caseList.load_data(queryCase);

		for (int n1 = 0; n1 < caseList.row_count(); n1++)
		{
			DBS_Case::remove_case_data(database, std::stoi(caseList(0, n1)));
		}

		// Remove selected elements
		return database->remove_row(&AMLIB::TN_SelectedElements(), query);
	}
#pragma endregion

#pragma region IAM_DBS implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										Name,
										APIName,
										External_APIName,};
		return input;
	}

	virtual std::string get_load_string() override
	{
		return std::string(" ID = " + std::to_string(_id) + " ");
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
		Name = rawData[1];
		APIName = rawData[2];
		External_APIName = rawData[3];

		return 0;
	}

#pragma endregion

};