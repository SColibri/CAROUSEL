#pragma once
#include <vector>
#include "CALCULATIONS_decorator.h"
#include "CALCULATION_scheilPrecipitation_distribution.h"
#include "../../../interfaces/IAM_DBS.h"
#include "../../../include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"

namespace matcalc
{
	class CALCULATION_scheilPrecipitation_heatTreatment : public CALCULATION_decorator
	{
	public:

		CALCULATION_scheilPrecipitation_heatTreatment(IAM_Database* db, IPC_winapi* mccComm, AM_Config* configuration, AM_Project* project, 
			AM_pixel_parameters* pixel_parameters, DBS_HeatTreatment* heatTreatment) : _db(db)
		{
			// decorator object
			_calculation = new CALCULATION_scheilPrecipitation_distribution(db, mccComm, configuration, pixel_parameters->get_ScheilConfiguration(), project, pixel_parameters);

			// Load heat treatments

			// Precipitation Domain
			DBS_PrecipitationDomain pDomain(db, heatTreatment->IDPrecipitationDomain);
			pDomain.load();

			DBS_Phase tempPhase(db, pDomain.IDPhase);
			tempPhase.load();

			_commandList.push_back(new COMMAND_create_precipitate_domain(mccComm, configuration, pDomain.Name));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "thermodynamic-matrix-phase=" + tempPhase.Name));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "microstructure-evolution vacancies vacancy-evolution-model=" + pDomain.VacancyEvolutionModel));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "microstructure-evolution vacancies vacancy-evolution-parameters excess-vacancy-efficiency-in-diffusion=" + std::to_string(pDomain.ExcessVacancyEfficiency)));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "initial-grain-diameter=" + std::to_string(pDomain.InitialGrainDiameter)));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "equilibrium-dislocation-density=" + std::to_string(pDomain.EquilibriumDiDe)));

		}

		~CALCULATION_scheilPrecipitation_heatTreatment()
		{
			
		}

		virtual void AfterCalculation() override { }

		virtual void AfterDecoratorCalculation() override
		{
			CALCULATION_scheil* tempPointer = (CALCULATION_scheil*)_calculation;
			_precipitationTemperature = tempPointer->Save_to_database(_db, _pixel_parameters);
		}


	private:
		IAM_Database* _db;
		AM_pixel_parameters* _pixel_parameters;


	};
}