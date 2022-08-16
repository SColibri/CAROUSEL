#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "../../../x_Helpers/string_manipulators.h"

/// <summary>
/// Implements IAM_DBS.h interface, this is a Element
/// object structure.
/// </summary>
class DBS_HeatTreatment : public IAM_DBS
{
private:
	std::string _loadString{ "" };

public:
	long int IDCase{ -1 };
	std::string Name{ "" };
	int MaxTemperatureStep{ 10 };
	long int IDPrecipitationDomain{ -1 };
	double StartTemperature{ -1 };

	DBS_HeatTreatment(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_HeatTreatment();
	}

#pragma region methods
	void load_by_name(std::string treatName)
	{
		_loadString = " " + _tableStructure.columnNames[2] + " = \'" + treatName + "\' ";
		load();
	}
#pragma endregion

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
									std::to_string(IDCase),
									Name,
									std::to_string(MaxTemperatureStep),
									std::to_string(IDPrecipitationDomain),
									std::to_string(StartTemperature) };
		return input;
	}

	virtual std::string get_load_string() override
	{
		if (_loadString.length() == 0) _loadString = " ID = \'" + std::to_string(_id) + " \' ";
		return _loadString;
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
		Name = rawData[2];
		MaxTemperatureStep = std::stoi(rawData[3]);
		IDPrecipitationDomain = std::stoi(rawData[4]);
		StartTemperature = std::stod(rawData[5]);
		return 0;
	}

	virtual int check_before_save() override
	{
		if (Name.length() == 0) return 1;

		//check if this already exists
		if (_id == -1) 
		{
			AM_Database_Datatable tempTable(_db, &AMLIB::TN_HeatTreatment());
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

	virtual int remove_dependent_data() override 
	{
		// Remove segments
		std::string query = AMLIB::TN_HeatTreatmentSegments().columnNames[2] +
			" = " + std::to_string(id());

		_db->remove_row(&AMLIB::TN_HeatTreatmentSegments(), query);
		
		// Remove temperature profile
		query = AMLIB::TN_HeatTreatmentProfile().columnNames[1] +
			" = " + std::to_string(id());

		return _db->remove_row(&AMLIB::TN_HeatTreatmentSegments(), query);
	}

#pragma endregion

};