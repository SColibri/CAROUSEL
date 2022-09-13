#pragma once
#include <vector>
#include <filesystem>
#include "CALCULATIONS_decorator.h"
#include "CALCULATION_scheil.h"
#include "../../../interfaces/IAM_DBS.h"
#include "../../../include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"

namespace matcalc
{
	class CALCULATION_scheilPrecipitation_distribution : public CALCULATION_decorator
	{
	public:

		CALCULATION_scheilPrecipitation_distribution(IAM_Database* db, IPC_winapi* mccComm, AM_Config* configuration, DBS_ScheilConfiguration* scheilConfig, AM_Project* project, AM_pixel_parameters* pixel_parameters): _db(db), _pixel_parameters(pixel_parameters)
		{
			// decorator object
			_calculation = new CALCULATION_scheil(db, mccComm, configuration, scheilConfig, project, pixel_parameters);

			// selected phases with P0
			std::vector<DBS_PrecipitationPhase*> precipitationPhases = pixel_parameters->get_precipitation_phases();
			for (auto& item : precipitationPhases)
			{
				if (string_manipulators::find_index_of_keyword(item->Name, "P0") != std::string::npos)
				{
					_precipitationPhases.push_back(item);
				}
			}

			for (auto& item : _precipitationPhases)
			{
				DBS_Phase tempPhase(db, item->IDPhase);
				tempPhase.load();

				_commandList.push_back(new COMMAND_create_new_phase(mccComm, configuration, item, tempPhase.Name, "precipitate", "(primary)"));
				_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, item->Name, "nucleation-sites=none"));

			}

			_commandList.push_back(new COMMAND_set_start_values(mccComm, configuration));


			for (auto& item : _precipitationPhases)
			{
				DBS_Phase tempPhase(db, item->IDPhase);
				tempPhase.load();

				_commandList.push_back(new COMMAND_generate_precipitate_distribution(mccComm, configuration, item->Name, item->CalcType, 
					item->MinRadius, item-> MeanRadius, item->MaxRadius, item->StdDev));
			}

			for (auto& item : _precipitationPhases)
			{
				DBS_Phase tempPhase(db, item->IDPhase);
				tempPhase.load();

				_filenames.push_back(configuration->get_directory_path(AM_FileManagement::FILEPATH::TEMP) + "/" + std::to_string(COMMAND_export_variables::get_uniqueNumber()) + "_" + tempPhase.Name + ".AMFramework");
				_commandList.push_back(new COMMAND_export_precipitate_distribution(mccComm, configuration, item->Name, _filenames[_filenames.size() - 1]));
			}

		}

		~CALCULATION_scheilPrecipitation_distribution()
		{
			// Remove all created files
			for(auto& fname : _filenames)
			{
				if (std::filesystem::exists(fname)) std::remove(fname.c_str());
			}
		}

		virtual void AfterCalculation() override { }

		virtual void AfterDecoratorCalculation() override 
		{
			CALCULATION_scheil* tempPointer = (CALCULATION_scheil*)_calculation;
			_precipitationTemperature = tempPointer->Save_to_database(_db, _pixel_parameters);
		}

		void Save_to_database(IAM_Database* db)
		{
			for (auto& item : _precipitationPhases)
			{
				std::string pName = item->Name;
				item->PrecipitateDistribution = Read_distribution(item->Name);
				item->save();
			}
		}

		const std::vector<std::string>& Get_files() { return _filenames; }


	private:
		IAM_Database* _db;
		AM_pixel_parameters* _pixel_parameters;
		std::vector<DBS_PrecipitationPhase*> _precipitationPhases;

		std::vector<std::string> _filenames;
		int _precipitationTemperature{ -1 };

		std::string Read_distribution(std::string phaseName) 
		{
			std::string Result{ "" };
			for(auto& item : _filenames)
			{
				if(string_manipulators::find_index_of_keyword(item,phaseName) != std::string::npos)
				{
					if (!std::filesystem::exists(item)) break;
					Result = string_manipulators::read_file_to_end(item);
					break;
				}
			}

			return Result;
		}

	};
}