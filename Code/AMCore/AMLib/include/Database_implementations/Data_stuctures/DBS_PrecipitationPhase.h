#pragma once
#include "../../../interfaces/IAM_DBS.h"

class DBS_PrecipitationPhase : public IAM_DBS
{
public:
	//General
	int IDCase{ -1 }; // ID case
	int IDPhase{ -1 }; // ID phase
	int NumberSizeClasses = 25; // number of size classes  or precipitation classes
	std::string Name{ "" }; // precipitation name (usually phaseName_p0)
	std::string NucleationSites{ "none" };
	int IDPrecipitationDomain{ -1 }; // precipitation domain

	//calculation parameters
	std::string CalcType{ "normal" };
	double MinRadius{ 0.000001 };
	double MeanRadius{ 0.000002 };
	double MaxRadius{ 0.000003 }; 
	double StdDev{ 0.05 }; // standard deviation
	std::string PrecipitateDistribution{ "" };

	DBS_PrecipitationPhase(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_PrecipitationPhase();
	}

	static int remove_selection_data(IAM_Database* database, long int CaseID)
	{
		std::string query = AMLIB::TN_PrecipitationPhase().columnNames[0] +
			" = " + std::to_string(CaseID);

		return database->remove_row(&AMLIB::TN_PrecipitationPhase(), query);
	}

	static int remove_case_data(IAM_Database* database, long int projectID)
	{
		std::string query = AMLIB::TN_PrecipitationPhase().columnNames[1] +
			" = " + std::to_string(projectID);

		return database->remove_row(&AMLIB::TN_PrecipitationPhase(), query);
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDCase),
										std::to_string(IDPhase),
										std::to_string(NumberSizeClasses),
										Name,
										NucleationSites,
										std::to_string(IDPrecipitationDomain),
										CalcType,
										std::to_string(MinRadius),
										std::to_string(MeanRadius),
										std::to_string(MaxRadius),
										std::to_string(StdDev),
										PrecipitateDistribution};
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

		set_id(std::stoi(rawData[0]));
		IDCase = std::stoi(rawData[1]);
		IDPhase = std::stoi(rawData[2]);
		NumberSizeClasses = std::stoi(rawData[3]);
		Name = rawData[4];
		NucleationSites = rawData[5];
		IDPrecipitationDomain = std::stoi(rawData[6]);
		CalcType = rawData[7];
		MinRadius = std::stold(rawData[8]);
		MeanRadius = std::stold(rawData[9]);
		MaxRadius = std::stold(rawData[10]);
		StdDev = std::stold(rawData[11]);
		PrecipitateDistribution = rawData[12];

		return 0;
	}

	virtual int check_before_save() override
	{
		// check if is unique
		if (_id == -1)
		{
			AM_Database_Datatable tempTable(_db, &AMLIB::TN_PrecipitationPhase());
			tempTable.load_data("IDCase = " + std::to_string(IDCase) + " AND Name = \'" + Name + "\'");
			if (tempTable.row_count() > 0)
			{
				this->set_id(std::stoi(tempTable(0, 0)));
				save();

				return 1;
			}
		}

		return 0;
	}

#pragma endregion
};