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
		try
		{
			if (rawData.size() < 3) return 1;
			set_id(std::stoi(rawData[0]));
			Name = rawData[1];
			APIName = rawData[2];
		}
		catch (const std::exception&)
		{
			return 1;
		}
		
		return 0;
	}

#pragma endregion

private:

};