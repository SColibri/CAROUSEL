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
			AM_pixel_parameters* pixel_parameters, DBS_HeatTreatment* heatTreatment) : _db(db), _mccComm(mccComm), _configuration(configuration), 
																					   _heatTreatment(heatTreatment)
		{
			// decorator object
			_calculation = new CALCULATION_scheilPrecipitation_distribution(db, mccComm, configuration, pixel_parameters->get_ScheilConfiguration(), project, pixel_parameters);

			// Load heat treatments

			// Precipitation Domain
			DBS_PrecipitationDomain pDomain(db, heatTreatment->IDPrecipitationDomain);
			pDomain.load();
			_precipitationDomainName = pDomain.Name;

			DBS_Phase tempPhase(db, pDomain.IDPhase);
			tempPhase.load();

			_commandList.push_back(new COMMAND_create_precipitate_domain(mccComm, configuration, pDomain.Name));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "thermodynamic-matrix-phase=" + tempPhase.Name));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "microstructure-evolution vacancies vacancy-evolution-model=" + pDomain.VacancyEvolutionModel));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "microstructure-evolution vacancies vacancy-evolution-parameters excess-vacancy-efficiency-in-diffusion=" + std::to_string(pDomain.ExcessVacancyEfficiency)));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "initial-grain-diameter=" + std::to_string(pDomain.InitialGrainDiameter)));
			_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, pDomain.Name, "equilibrium-dislocation-density=" + std::to_string(pDomain.EquilibriumDiDe)));


			// load primary precipitate distributions and scondary phases into matcalc
#pragma region Load_precipitate_distributions
			AM_Database_Datatable precipitationPhases(db, &AMLIB::TN_PrecipitationPhase());
			precipitationPhases.load_data("IDCase = " + std::to_string(heatTreatment->IDCase) + " AND Name LIKE \'%P0%\'");

			if (precipitationPhases.row_count() == 0) return; // there is nothing to calculate first define primary precipitates
			for (int n1 = 0; n1 < precipitationPhases.row_count(); n1++)
			{
				DBS_PrecipitationPhase tempRef(db, -1);
				tempRef.load(precipitationPhases.get_row_data(n1));

				DBS_Phase tempPhase(db, tempRef.IDPhase);
				tempPhase.load();

				_commandList.push_back(new COMMAND_create_new_phase(mccComm, configuration, &tempRef, tempPhase.Name, "precipitate", "(primary)"));
				_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, tempRef.Name, " number-of-size-classes=" + std::to_string(tempRef.NumberSizeClasses)));
				_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, tempRef.Name, " nucleation-sites=" + tempRef.NucleationSites));
				_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, tempRef.Name, " restrict-nucleation-to-precipitation-domain=" + pDomain.Name));

				std::string filename = ((CALCULATION_scheilPrecipitation_distribution*)_calculation)->Get_filename_from_phasename(tempRef.Name);
				_commandList.push_back(new COMMAND_import_precipitate_distribution(mccComm, configuration, tempRef.Name, filename));
			}

			// secondary phases
			precipitationPhases.load_data("IDCase = " + std::to_string(heatTreatment->IDCase) + " AND Name LIKE \'%P1%\'");
			if (precipitationPhases.row_count() == 0) return;

			for (int n1 = 0; n1 < precipitationPhases.row_count(); n1++)
			{
				DBS_PrecipitationPhase tempRef(db, -1);
				tempRef.load(precipitationPhases.get_row_data(n1));

				DBS_Phase tempPhase(db, tempRef.IDPhase);
				tempPhase.load();

				_commandList.push_back(new COMMAND_create_new_phase(mccComm, configuration, &tempRef, tempPhase.Name, "precipitate", "(sec)"));
				_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, tempRef.Name, " number-of-size-classes=" + std::to_string(tempRef.NumberSizeClasses)));
				_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, tempRef.Name, " nucleation-sites=" + tempRef.NucleationSites));
				_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, tempRef.Name, " restrict-nucleation-to-precipitation-domain=" + pDomain.Name));

				std::string filename = ((CALCULATION_scheilPrecipitation_distribution*)_calculation)->Get_filename_from_phasename(tempRef.Name);
				_commandList.push_back(new COMMAND_import_precipitate_distribution(mccComm, configuration, tempRef.Name, filename));
			}

#pragma endregion

		}

		~CALCULATION_scheilPrecipitation_heatTreatment()
		{
			delete _calculation;
		}

		virtual void AfterCalculation() override { }

		virtual void AfterDecoratorCalculation() override
		{
			CALCULATION_scheilPrecipitation_distribution* tempPointer = (CALCULATION_scheilPrecipitation_distribution*)_calculation;
			
			// Add heat treatment using as starting temperature
#pragma region Heat_treatment
			// declare new buffer name
			std::string bufferName = "HT_Buf";
			std::string newStateName = "NSN";
			_commandList.push_back(new COMMAND_create_calc_state(_mccComm, _configuration, newStateName));
			_commandList.push_back(new COMMAND_rename_current_buffer(_mccComm, _configuration, bufferName));

			// create heat treatment
			_commandList.push_back(new COMMAND_create_tm_treatment(_mccComm, _configuration, _heatTreatment->Name));
			_commandList.push_back(new COMMAND_append_tmt_segment(_mccComm, _configuration, _heatTreatment->Name));
			_commandList.push_back(new COMMAND_edit_tmt_segment(_mccComm, _configuration, _heatTreatment->Name, _precipitationDomainName));
			_commandList.push_back(new COMMAND_edit_tmt_segment(_mccComm, _configuration, _heatTreatment->Name, "", std::to_string(tempPointer->precipitationTemperature())));

			AM_Database_Datatable HTSegments(_db, &AMLIB::TN_HeatTreatmentSegments());
			HTSegments.load_data("IDHeatTreatment = " + std::to_string(_heatTreatment->id()) + " ORDER BY stepIndex");
			
			if (HTSegments.row_count() == 0)
			{
				throw new exception("This case does not have any heat treatment segments");
			}

			for (int n2 = 0; n2 < HTSegments.row_count(); n2++)
			{
				DBS_HeatTreatmentSegment tempSeg(_db, -1);
				tempSeg.load(HTSegments.get_row_data(n2));

				_commandList.push_back(new COMMAND_edit_tmt_segment(_mccComm, _configuration, _heatTreatment->Name, &tempSeg));

				if (n2 != HTSegments.row_count() - 1)
				{
					_commandList.push_back(new COMMAND_append_tmt_segment(_mccComm, _configuration, _heatTreatment->Name));
				}
			}
			
			_commandList.push_back(new COMMAND_set_simulation_parameter(_mccComm, _configuration, "set-simulation-parameter temperature-control tm-treatment-name=" + _heatTreatment->Name));
			_commandList.push_back(new COMMAND_set_simulation_parameter(_mccComm, _configuration, "set-simulation-parameter max-temperature-step=" + std::to_string(_heatTreatment->MaxTemperatureStep)));
			_commandList.push_back(new COMMAND_set_simulation_parameter(_mccComm, _configuration, "set-simulation-parameter starting-conditions=" + newStateName));
			_commandList.push_back(new COMMAND_start_precipitate_simulation(_mccComm, _configuration));

#pragma endregion
		}


	private:
		IAM_Database* _db;
		IPC_winapi* _mccComm;
		AM_Config* _configuration;
		AM_pixel_parameters* _pixel_parameters;
		DBS_HeatTreatment* _heatTreatment;
		std::string _precipitationDomainName;

	};
}