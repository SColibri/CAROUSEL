#pragma once
#include "../../../interfaces/IAM_DBS.h"
/// <summary>
/// Implements IAM_DBS.h interface, this is a CALPHAD database
/// object structure (only stores name of the database).
/// </summary>
class DBS_CALPHADDatabase : public IAM_DBS
{
public:
	// [DBS_Parameter]
	int IDProject{ -1 };
	// [DBS_Parameter]
	std::string Thermodynamic{ "" };
	// [DBS_Parameter]
	std::string Physical{ "" };
	// [DBS_Parameter]
	std::string Mobility{ "" };

	DBS_CALPHADDatabase(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_CALPHADDatabase();
	}

	DBS_CALPHADDatabase(const DBS_CALPHADDatabase& toCopy): 
		IAM_DBS(toCopy._db)
	{
		_tableStructure = toCopy._tableStructure;
		IDProject = toCopy.IDProject;
		Thermodynamic = toCopy.Thermodynamic;
		Physical = toCopy.Physical;
		Mobility = toCopy.Mobility;
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDProject),
										Thermodynamic,
										Physical,
										Mobility };
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
		Thermodynamic = rawData[2];
		Physical = rawData[3];
		Mobility = rawData[4];

		return 0;
	}

#pragma endregion

private:

};