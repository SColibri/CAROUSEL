#pragma once
#include "../../../interfaces/IAM_DBS.h"

class DBS_PrecipitationDomain : public IAM_DBS
{
public:
	//General
	int IDCase{ -1 }; // ID case
	std::string Name{ "" }; // domain name
	int IDPhase {-1}; // Thermodynamic matrix phase
	double InitialGrainDiameter{ 0.00005 }; // Initial grain diameter
	double EquilibriumDiDe{ 100000000000 }; // Equilibrium dislocation density

	// Vacancies
	std::string VacancyEvolutionModel{ "" }; // Vacancy evolution model
	int ConsiderExVa{ 0 }; // Consider excess vacancy
	double ExcessVacancyEfficiency{ 0.0 }; // Excess vacancy efficiency
	
	DBS_PrecipitationDomain(IAM_Database* database, int id) :
		IAM_DBS(database)
	{
		_id = id;
		_tableStructure = AMLIB::TN_SelectedElements();
	}

#pragma region implementation

	virtual std::vector<std::string> get_input_vector() override
	{
		std::vector<std::string> input{ std::to_string(_id),
										Name,
										std::to_string(IDPhase),
										std::to_string(InitialGrainDiameter),
										std::to_string(EquilibriumDiDe),
										VacancyEvolutionModel,
										std::to_string(ConsiderExVa),
										std::to_string(ExcessVacancyEfficiency) };
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
		IDPhase = std::stoi(rawData[2]);
		InitialGrainDiameter = std::stold(rawData[3]);
		EquilibriumDiDe = std::stold(rawData[4]);
		VacancyEvolutionModel = rawData[5];
		ConsiderExVa = std::stoi(rawData[6]);
		ExcessVacancyEfficiency = std::stold(rawData[7]);
		return 0;
	}

#pragma endregion

};