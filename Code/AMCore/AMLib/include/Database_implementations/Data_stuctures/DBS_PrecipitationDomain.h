#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "../../../x_Helpers/string_manipulators.h"

class DBS_PrecipitationDomain : public IAM_DBS
{
public:
	//General
	int IDCase{ -1 }; // ID case
	std::string Name{ "" }; // domain name
	int IDPhase {-1}; // Thermodynamic matrix phase
	double InitialGrainDiameter{ 100.0e-6 }; // Initial grain diameter
	double EquilibriumDiDe{ 100000000000 }; // Equilibrium dislocation density

	// Vacancies
	std::string VacancyEvolutionModel{ "FSAK-dynamics" }; // Vacancy evolution model
	int ConsiderExVa{ 0 }; // Consider excess vacancy
	double ExcessVacancyEfficiency{ 1.0 }; // Excess vacancy efficiency
	
	DBS_PrecipitationDomain(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_PrecipitationDomain();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										std::to_string(IDCase),
										Name,
										std::to_string(IDPhase),
										string_manipulators::double_to_string(InitialGrainDiameter),
										std::to_string(EquilibriumDiDe),
										VacancyEvolutionModel,
										std::to_string(ConsiderExVa),
										string_manipulators::double_to_string(ExcessVacancyEfficiency) };
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

		int index = 0;
		set_id(std::stoi(rawData[index++]));
		IDCase = std::stoi(rawData[index++]);
		Name = rawData[index++];
		IDPhase = std::stoi(rawData[index++]);
		InitialGrainDiameter = std::stold(rawData[index++]);
		EquilibriumDiDe = std::stold(rawData[index++]);
		VacancyEvolutionModel = rawData[index++];
		ConsiderExVa = std::stoi(rawData[index++]);
		ExcessVacancyEfficiency = std::stold(rawData[index++]);
		return 0;
	}

	virtual int check_before_save() override
	{
		if (Name.length() == 0) return 1;

		//check if this already exists
		if (_id == -1)
		{
			AM_Database_Datatable tempTable(_db, &AMLIB::TN_PrecipitationDomain());
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