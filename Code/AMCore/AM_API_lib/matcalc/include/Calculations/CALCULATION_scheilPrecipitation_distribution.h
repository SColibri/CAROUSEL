#pragma once
#include <vector>
#include <filesystem>
#include "CALCULATIONS_decorator.h"
#include "CALCULATION_scheil.h"
#include "../../../../AMLib/interfaces/IAM_DBS.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

namespace matcalc
{
	class CALCULATION_scheilPrecipitation_distribution : public CALCULATION_decorator
	{
	public:

		CALCULATION_scheilPrecipitation_distribution(IAM_Database* db, AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, AM_Project* project, AM_pixel_parameters* pixel_parameters): _db(db), _pixel_parameters(pixel_parameters)
		{
			// decorator object
			_calculation = new CALCULATION_scheil(db, mccComm, configuration, pixel_parameters->get_ScheilConfiguration(), project, pixel_parameters);
			((CALCULATION_scheil*)_calculation)->set_auto_save();

			// Check if data was generated before
			std::vector<DBS_PrecipitationPhase*> precipitationPhases = pixel_parameters->get_precipitation_phases();
			bool DataContained = true;
			for (auto& item : precipitationPhases)
			{
				if (string_manipulators::find_index_of_keyword(item->Name, "P0") != std::string::npos)
				{
					_precipitationPhases.push_back(item);
					if (string_manipulators::find_index_of_keyword(_precipitationPhases.back()->PrecipitateDistribution, "_") != std::string::npos) { DataContained = false; }
				}
			}
			

			if(DataContained)
			{
				for(auto& item:_precipitationPhases)
				{
					_filenames.push_back(configuration->get_directory_path(AM_FileManagement::FILEPATH::TEMP) + "\\" + std::to_string(COMMAND_export_variables::get_uniqueNumber()) + "_" + item->Name + ".AMFramework");
					string_manipulators::write_to_file(_filenames.back(), item->PrecipitateDistribution);
				}
			}
			else
			{
				for (auto& item : _precipitationPhases)
				{
					DBS_Phase tempPhase(db, item->IDPhase);
					tempPhase.load();

					_commandList.push_back(new COMMAND_create_new_phase(mccComm, configuration, tempPhase.Name, tempPhase.Name + "_S", "precipitate", "(primary)"));
					_commandList.push_back(new COMMAND_set_precipitation_parameter(mccComm, configuration, item->Name, "nucleation-sites=none"));

				}

				//_commandList.push_back(new COMMAND_set_start_values(mccComm, configuration));
				_commandList.push_back(new COMMAND_step_equilibrium(mccComm, configuration));
				_commandList.push_back(new COMMAND_load_buffer_state(mccComm, configuration, -1));
				Add_calculation_commands(db, mccComm, configuration, pixel_parameters->get_ScheilConfiguration(), project, pixel_parameters);
			}

		}

		~CALCULATION_scheilPrecipitation_distribution()
		{
			// Remove all created files
			for(auto& fname : _filenames)
			{
				if (std::filesystem::exists(fname)) std::remove(fname.c_str());
			}

			delete _calculation;
		}

		virtual void AfterCalculation() override { Save_to_database(_db); }

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

		const std::string Get_filename_from_phasename(std::string phasename) 
		{
			for (auto& item : _filenames)
			{
				if (string_manipulators::find_index_of_keyword(item, phasename) != std::string::npos)
				{
					return item;
				}
			}

			return "";
		}

		const double& precipitationTemperature() 
		{
			return _precipitationTemperature;
		}

	private:
		IAM_Database* _db;
		AM_pixel_parameters* _pixel_parameters;
		std::vector<DBS_PrecipitationPhase*> _precipitationPhases;

		std::vector<std::string> _filenames;
		double _precipitationTemperature{ -1 };

		std::string Read_distribution(std::string phaseName) 
		{
			std::string Result{ "" };
			for(auto& item : _filenames)
			{
				if(string_manipulators::find_index_of_keyword(item,phaseName) != std::string::npos)
				{
					if (!std::filesystem::exists(item)) break;
					Result = string_manipulators::read_file_content(item);
					break;
				}
			}

			return Result;
		}

		void Add_calculation_commands(IAM_Database* db, AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, DBS_ScheilConfiguration* scheilConfig, AM_Project* project, AM_pixel_parameters* pixel_parameters)
		{			
			for (auto& item : _precipitationPhases)
			{
				DBS_Phase tempPhase(db, item->IDPhase);
				tempPhase.load();

				_commandList.push_back(new COMMAND_generate_precipitate_distribution(mccComm, configuration, item->Name, item->CalcType,
					item->MinRadius, item->MeanRadius, item->MaxRadius, item->StdDev));
			}

			for (auto& item : _precipitationPhases)
			{
				DBS_Phase tempPhase(db, item->IDPhase);
				tempPhase.load();

				_filenames.push_back(configuration->get_directory_path(AM_FileManagement::FILEPATH::TEMP) + "\\" + std::to_string(COMMAND_export_variables::get_uniqueNumber()) + "_" + item->Name + ".AMFramework");
				_commandList.push_back(new COMMAND_export_precipitate_distribution(mccComm, configuration, item->Name, _filenames[_filenames.size() - 1]));
			}
		}

	};
}