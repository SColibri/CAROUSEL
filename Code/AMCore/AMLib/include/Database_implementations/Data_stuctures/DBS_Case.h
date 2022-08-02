#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "DBS_ScheilConfiguration.h"
#include "DBS_EquilibriumConfiguration.h"
#include "DBS_PrecipitationDomain.h"

/// <summary>
/// Implements IAM_DBS.h interface, this is a phase
/// object structure.
/// </summary>
class DBS_Case : public IAM_DBS
{
public:
	int IDProject{ -1 };
	int IDGroup{ 0 };
	std::string Name{ "" };
	std::string Script{ "" };
	std::string Date{ "" };
	double PosX{ 0 };
	double PosY{ 0 };
	double PosZ{ 0 };

	DBS_Case(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_Case();
	}

	DBS_Case(const DBS_Case& toCopy):
		IAM_DBS(toCopy._db)
	{
		_tableStructure = toCopy._tableStructure;
		IDProject = toCopy.IDProject;
		IDGroup = toCopy.IDGroup;
		Name = toCopy.Name;
		Script = toCopy.Script;
		Date = toCopy.Date;
		PosX = toCopy.PosX;
		PosY = toCopy.PosY;
		PosZ = toCopy.PosZ;
	}

	static int remove_case_data(IAM_Database* database, int CaseID)
	{
		std::string query = AMLIB::TN_SelectedElements().columnNames[0] +
			" = " + std::to_string(CaseID);

		int results[11];
		results[0] = database->remove_row(&AMLIB::TN_EquilibriumConfiguration(), 
						AMLIB::TN_EquilibriumConfiguration().columnNames[1] +
						" = " + std::to_string(CaseID));
		results[1] = database->remove_row(&AMLIB::TN_EquilibriumPhaseFractions(),
						AMLIB::TN_EquilibriumPhaseFractions().columnNames[1] +
						" = " + std::to_string(CaseID));
		results[2] = database->remove_row(&AMLIB::TN_ScheilConfiguration(),
						AMLIB::TN_ScheilConfiguration().columnNames[1] +
						" = " + std::to_string(CaseID));
		results[3] = database->remove_row(&AMLIB::TN_ScheilPhaseFraction(),
						AMLIB::TN_ScheilPhaseFraction().columnNames[1] +
						" = " + std::to_string(CaseID));
		results[4] = database->remove_row(&AMLIB::TN_SelectedPhases(),
						AMLIB::TN_SelectedPhases().columnNames[1] +
						" = " + std::to_string(CaseID));
		results[5] = database->remove_row(&AMLIB::TN_CALPHADDatabase(),
						AMLIB::TN_CALPHADDatabase().columnNames[1] +
						" = " + std::to_string(CaseID));
		results[6] = database->remove_row(&AMLIB::TN_ElementComposition(),
						AMLIB::TN_ElementComposition().columnNames[1] +
						" = " + std::to_string(CaseID));
		results[7] = database->remove_row(&AMLIB::TN_Case(),
						AMLIB::TN_Case().columnNames[0] +
						" = " + std::to_string(CaseID));
		results[8] = database->remove_row(&AMLIB::TN_PrecipitationPhase(),
			AMLIB::TN_PrecipitationPhase().columnNames[0] +
			" = " + std::to_string(CaseID));
		results[9] = database->remove_row(&AMLIB::TN_PrecipitationDomain(),
			AMLIB::TN_PrecipitationDomain().columnNames[0] +
			" = " + std::to_string(CaseID));

		return 0;
	}

	static int create_case_data(IAM_Database* database, int CaseID) 
	{
		DBS_ScheilConfiguration new001(database,-1);
		DBS_EquilibriumConfiguration new002(database, -1);

		new001.IDCase = CaseID;
		new002.IDCase = CaseID;

		new001.save();
		new002.save();

		return 1;
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDProject),
										std::to_string(IDGroup),
										Name,
										Script,
										Date,
										std::to_string(PosX),
										std::to_string(PosY),
										std::to_string(PosZ)};
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
		IDGroup = std::stoi(rawData[2]);
		Name = rawData[3];
		Script = rawData[4];
		Date = rawData[5];
		PosX = std::stold(rawData[6]);
		PosY = std::stold(rawData[7]);
		PosZ = std::stold(rawData[8]);
		return 0;
	}

	virtual int remove_dependent_data() override 
	{
		//Nothing to do
		if (_id == -1) return 1;
		remove_case_data(_db, id());

		return 0;
	}

	virtual int create_dependent_data() override
	{
		//Nothing to do
		if (_id == -1) return 1;
		create_case_data(_db, id());

		return 0;
	}
#pragma endregion

private:

};