#pragma once
#include <string>
#include <fstream>
#include <ctime>
#include <vector>
#include <mutex>
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../AMLib/x_Helpers/string_manipulators.h"
#include "../../../AMLib/include/AM_Project.h"

namespace API_Scripting
{
#pragma region Helpers
	inline static std::mutex _uniqueNumber_mutex;
	inline static size_t _uniqueNumber = 0;
	static size_t get_uniqueNumber()
	{
		size_t out;

		_uniqueNumber_mutex.lock();
		out = _uniqueNumber;
		_uniqueNumber += 1;
		_uniqueNumber_mutex.unlock();

		return out;
	}
#pragma endregion

	std::vector<std::string> static Script_initialize(AM_Config* configuration) 
	{
		
		std::string Oldval = " ";
		std::string Newval = " ";

		std::string ThermoPath = configuration->get_ThermodynamicDatabase_path();

		std::vector<std::string> out
		{
			"use-module core",
			"set-working-directory \"" + configuration->get_working_directory() + "\"",
			"open-thermodyn-database " + configuration->get_ThermodynamicDatabase_path() + "",
			"set-variable-value npc 25"
		};


		return out;
	}

	std::vector<std::string> static script_get_thermodynamic_database(AM_Config* configuration)
	{
		std::vector<std::string> out; //= Script_initialize(configuration);
		out.push_back("open-thermodyn-database \"" + configuration->get_ThermodynamicDatabase_path() + "\"");
		out.push_back("list-database-contents equi-database-contents");
		return out;
	}

#pragma region single_commands
	std::string static script_initialize_core() 
	{
		return "use-module core";
	}

	std::string static script_runScript(std::string scriptFilenam)
	{
		return "run-script-file \"" + scriptFilenam + "\"";
	}

	std::string static script_set_working_directory(AM_Config* configuration)
	{
		return "set-working-directory \"" + configuration->get_working_directory() + "\"";
	}

	std::string static script_set_thermodynamic_database(std::string parameters)
	{
		return "open-thermodynamic-database \"" + parameters + "\"";
	}

	std::string static script_set_physical_database(std::string parameters)
	{
		return "read-mobility-database \"" + parameters + "\"";
	}

	std::string static script_set_mobility_database(std::string parameters)
	{
		return "read-physical-database \"" + parameters + "\"";
	}

	std::string static Script_selectElements(std::vector<std::string> Elements) 
	{
		std::string out = "select-elements ";

		for(std::string elementy : Elements)
		{
			out += elementy + " ";
		}

		out += "\n";

		return out;
	}

	std::string static Script_setReferenceElement(std::string referenceElement)
	{
		std::string out = "set-reference-element " + referenceElement + "\n";

		return out;
	}

	std::string static Script_setStepOptions(std::string stepOptions)
	{
		std::string out = "set-step-option " + stepOptions ;

		return out;
	}
	std::string static Script_stepEquilibrium()
	{
		std::string out = "step-equilibrium";

		return out;
	}

	std::string static Script_setComposition_weight(std::vector<std::string> Elements, 
													std::vector<std::string> Values)
	{
		std::string buildComp{ "enter-composition type=weight-percent composition=\"" };
		for (int n1 = 1; n1 < Elements.size(); n1++)
		{
			buildComp += Elements[n1] + "=" + Values[n1] + " ";
		}
		buildComp += "\"";
		return buildComp;
	}

	std::string static Script_setTemperature_Celcius(double temperature)
	{
		std::string out = "set-temperature-celcius " + std::to_string(temperature) + "\n";

		return out;
	}

	std::string static Script_setStartValues()
	{
		std::string out = "set-start-values";

		return out;
	}

	std::string static Script_calculateEquilibrium()
	{
		std::string out = "calculate-equilibrium";

		return out;
	}

	std::string static Script_selectPhases(std::vector<std::string> Phases)
	{
		std::string out = "select-phases ";

		for (std::string& elementy : Phases)
		{
			out += string_manipulators::trim_whiteSpace(elementy) + " ";
		}

		out += "";

		return out;
	}


	std::string static Script_readThermodynamicDatabase()
	{
		std::string out = "read-thermodynamic-database \n";
		return out;
	}

	std::string static Script_openThermodynamicDatabase(AM_Config* configuration)
	{
		std::string out = "open-thermodynamic-database \"" + configuration->get_ThermodynamicDatabase_path() + "\"" + "\n";
		return out;
	}

	std::string static Script_readMobilityDatabase(AM_Config* configuration)
	{
		std::string out = "read-mobility-database \"" + configuration->get_MobilityDatabase_path() + "\"" + "\n";
		return out;
	}

	std::string static Script_readPhysicalDatabase(AM_Config* configuration)
	{
		std::string out = "read-physical-database \"" + configuration->get_PhysicalDatabase_path() + "\"" + "\n";
		return out;
	}

	std::string static Script_set_number_of_precipitate_classes(int precipClasses)
	{
		std::string out = "set-variable-value npc " + std::to_string(precipClasses) + "\n";
		return out;
	}

#pragma region precipitation
	std::string static Script_create_precipitation_domain(std::string DomainP, std::string PhaseName)
	{
		std::string out = "set-precipitation-parameter precipitate-or-domain-name=" + DomainP + " thermodynamic-matrix-phase=" + PhaseName + "\n";
		return out;
	}

	std::string static Script_precipitation_domain_set_vacancy_evolution_parameters(std::string DomainP, std::string vacancyParameter)
	{
		std::string out = "set-precipitation-parameter " + DomainP + " microstructure-evolution vacancies vacancy-evolution-parameters " + vacancyParameter + "\n";
		return out;
	}

	std::string static Script_precipitation_domain_set_initial_grain_diameter(std::string DomainP, std::string grainParameter)
	{
		std::string out = "set-precipitation-parameter " + DomainP + " initial-grain-diameter=" + grainParameter + "\n";
		return out;
	}

	std::string static Script_precipitation_domain_set_equilibrium_dislocation_density(std::string DomainP, std::string dislocDensity)
	{
		std::string out = "set-precipitation-parameter " + DomainP + " equilibrium-dislocation-density=" + dislocDensity + "\n";
		return out;
	}

	std::string static Script_create_new_phase_fromScheil(std::string PhaseName)
	{
		std::string out = "create-new-phase parent-phase=" + PhaseName + "_S precipitate " + PhaseName + "(primary)\n";
		return out;
	}

	std::string static Script_create_new_phase(std::string PhaseName)
	{
		std::string out = "create-new-phase parent-phase=" + PhaseName + " precipitate " + PhaseName + "(primary)\n";
		return out;
	}

	std::string static Script_create_new_phase_secondary(std::string PhaseName)
	{
		std::string out = "create-new-phase parent-phase=" + PhaseName + " precipitate " + PhaseName + "(sec)\n";
		return out;
	}

	std::string static Script_set_precipitation_parameter(std::string PhaseName, std::string ParameterPrecipitation)
	{
		std::string out = "set-precipitation-parameter " + PhaseName + "_P0 " + ParameterPrecipitation + "\n";
		return out;
	}

	std::string static Script_precipitate_distribution(std::string PhaseName, std::string filename)
	{
		std::string out = "set-precipitation-parameter " + PhaseName + "_P0 " + filename + "\n";
		return out;
	}

	std::string static Script_generate_precipitate_distribution(std::string PhaseName, std::string calculationType, 
		double minRadius, double meanRadius, double maxRadius, double stdDev)
	{
		std::string out = "generate-precipitate-distribution phase-name=" + PhaseName + "_P0 calculation-type=" + calculationType + 
						  " min-radius=" + std::to_string(minRadius) + " mean-radius=" + std::to_string(meanRadius) + " max-radius=" + std::to_string(maxRadius) +
						  " standard-deviation=" +std::to_string(stdDev) + "\n";
		return out;
	}

	std::string static Script_export_precipitate_distribution(std::string PhaseName, std::string filename)
	{
		std::string out = "export-precipitate-distribution precipitate-name=" + PhaseName + "_P0 file-name=" + filename + "\n";
		return out;
	}

#pragma region Kinetics

#pragma region HeatTreatment
	
#pragma endregion

	std::string static Script_set_simulation_parameters(std::string kineticParameters)
	{
		std::string out = "set-precipitation-parameter " + kineticParameters;
		return out;
	}

	std::string static Script_start_precipitate_simulation(std::string kineticParameters)
	{
		std::string out = "start-precipitate-simulation " + kineticParameters;
		return out;
	}

	std::string static Script_restrict_precipitation_nucleation(std::string DomainP, std::string PhaseName)
	{
		std::string out = "set-precipitation-parameter precipitate-or-domain-name=" + PhaseName + " restrict-nucleation-to-precipitation-domain=" + DomainP + "\n";
		return out;
	}

	std::string static Script_import_precipitate_distribution(std::string filename, std::string PhaseName)
	{
		std::string out = "import-precipitate-distribution " + PhaseName + " " + filename + "\n";
		return out;
	}
#pragma endregion
#pragma endregion


#pragma region MatcalcBUFFER
	std::string static script_buffer_listContent()
	{
		return "list-buffer-contents";
	}

	std::string static script_buffer_loadState(int stateNumber)
	{
		return "load-buffer-state line-index=" + std::to_string(stateNumber);
	}

	std::string static script_buffer_getPhaseStatus(std::string phaseName)
	{
		string_manipulators::toCaps(phaseName);
		return "list-phase-status phase=" + phaseName;
	}

	std::string static script_buffer_clear(std::string bufferName)
	{
		return "list-phase-status phase=" + bufferName;
	}
#pragma endregion
#pragma region MatcalcVariables
	/// <summary>
	/// 
	/// </summary>
	/// <param name="stringVar"></param>
	/// <param name="format"></param>
	/// <param name="variableNames"></param>
	/// <returns></returns>
	std::string static script_format_variable_string(const std::string& stringVar ,
													 const std::string& format, 
													 std::vector<std::string>& variableNames) 
	{
		std::string out{ "format-variable-string variable=" + 
						 stringVar + 
					    " format-string=\"%g," + format + "\" T " +
						IAM_Database::csv_join_row(variableNames," ")};

		return out;
	}

	std::vector<std::string> static script_get_phase_equilibrium_variable_name(std::vector<std::string>& Phases)
	{
		std::vector<std::string> out;

		for (std::string phaseN : Phases)
		{
			out.push_back("F$" + phaseN);
		}

		return out;
	}

	std::vector<std::string> static script_get_phase_equilibrium_scheil_variable_name(std::vector<std::string>& Phases)
	{
		std::vector<std::string> out;

		for (std::string phaseN : Phases)
		{
			if(string_manipulators::find_index_of_keyword(phaseN, "LIQUID") == std::string::npos)
			{
				out.push_back("F$" + string_manipulators::trim_whiteSpace(phaseN) + "_S");
			}
			else
			{
				out.push_back("F$" + string_manipulators::trim_whiteSpace(phaseN));
			}
			
		}

		return out;
	}

	std::string static print_variable_to_console(std::string stringVar)
	{
		std::string out{"send-console-string string=#" + stringVar};
		return out;
	}
#pragma endregion

#pragma endregion

#pragma region generic
	static void GenericScript_Database(IAM_Database* db, AM_Config* configuration, int projectID, int caseID, std::vector<std::string>& outVector)
	{
		AM_Project tempProj(db, configuration, projectID);

		outVector.push_back(script_set_thermodynamic_database(configuration->get_ThermodynamicDatabase_path()));
		outVector.push_back(Script_selectElements(tempProj.get_selected_elements_ByName()));

		AM_pixel_parameters* tempPixel = tempProj.get_pixelCase(caseID);
		if (tempPixel == nullptr) return;

		std::vector<std::string> Phases = tempPixel->get_selected_phases_ByName();
		if (Phases.size() < 11)
		{
			outVector.push_back(Script_selectPhases(Phases));
		}
		else
		{
			int Index = 0;
			for (int n1 = Index; n1 < Phases.size(); n1++)
			{
				outVector.push_back(Script_selectPhases(std::vector<std::string>{Phases[n1]}));
			}
		}

		outVector.push_back(Script_readThermodynamicDatabase());
		outVector.push_back(Script_readMobilityDatabase(configuration));
		outVector.push_back(Script_readPhysicalDatabase(configuration));
		outVector.push_back(Script_setReferenceElement(tempProj.get_reference_element_ByName()));
	}
#pragma endregion

	std::string static Script_setupScheilGulliver()
	{
		std::string out = "set-step-option type=scheil  \n \
						   set-step-option range start=700 stop=25 step-width=1 \n \
						   set-step-option scheil-dependent-phase=LIQUID \n \
						   set-step-option scheil-minimum-liquid-fraction=0.01 \n \
						   set-step-option scheil-create-phases-automatically=yes \n \
						   step-equilibrium";

		return out;
	}

	std::string static Script_Header()
	{
		std::time_t today = std::time(0);
		std::string out = "\
			Script auto generated using AMFramework: " + std::string(std::ctime(&today)) + " \n \
			********************************************************************************************************************************************";

		return out;
	}

#pragma region Equilibrium_steps
	std::vector<std::string> static Script_run_stepEquilibrium(AM_Config* configuration, 
																double startTemperature,
																double endTemperature,
																std::vector<std::string>& Elements, 
																std::vector<std::string>& Compositions, 
																std::vector<std::string>& Phases)
	{
		std::vector<std::string> out;
		out.push_back(script_set_thermodynamic_database(configuration->get_ThermodynamicDatabase_path()));
		out.push_back(Script_selectElements(Elements));
		out.push_back(Script_selectPhases(Phases));
		out.push_back(Script_readThermodynamicDatabase());
		out.push_back(Script_readMobilityDatabase(configuration));
		out.push_back(Script_readPhysicalDatabase(configuration));
		out.push_back(Script_setReferenceElement(Elements[0])); // TODO let the user select the reference element, here default Index == 0
		out.push_back("set-temperature-celsius " + std::to_string((int)startTemperature));
		out.push_back(Script_setComposition_weight(Elements, Compositions)); //TODO let user define the composition type
		out.push_back(Script_setStartValues());
		out.push_back(Script_calculateEquilibrium());
		out.push_back(Script_setStepOptions("type=temperature")); // TODO add this parameter to eConfig
		out.push_back(Script_setStepOptions("temperature-in-celsius=yes")); // TODO this should be based on eConfig
		out.push_back(Script_setStepOptions("range start=" + std::to_string((int)startTemperature) +
												 " stop=" + std::to_string((int)endTemperature) + 
												 " scale=lin step-width=25"));
		out.push_back(Script_stepEquilibrium());

		return out;
	}

	std::vector<std::string> static Script_run_stepScheilEquilibrium(AM_Config* configuration,
		double startTemperature,
		double endTemperature,
		double stepWitdh,
		std::vector<std::string>& Elements,
		std::vector<std::string>& Compositions,
		std::vector<std::string>& Phases)
	{
		std::vector<std::string> out;
		out.push_back(script_set_thermodynamic_database(configuration->get_ThermodynamicDatabase_path()));
		out.push_back(Script_selectElements(Elements));
		
		if(Phases.size() < 11)
		{
			out.push_back(Script_selectPhases(Phases));
		}
		else
		{
			int Index = 0;
			for (int n1 = Index; n1 < Phases.size(); n1++)
			{
				out.push_back(Script_selectPhases(std::vector<std::string>{Phases[n1]}));
			}
			
		}
		
		
		out.push_back(Script_readThermodynamicDatabase());
		out.push_back(Script_readMobilityDatabase(configuration));
		out.push_back(Script_readPhysicalDatabase(configuration));
		out.push_back(Script_setReferenceElement(Elements[0])); // TODO let the user select the reference element, here default Index == 0
		out.push_back("set-temperature-celsius " + std::to_string((int)startTemperature));
		out.push_back(Script_setComposition_weight(Elements, Compositions)); //TODO let user define the composition type
		out.push_back(Script_setStartValues());
		out.push_back(Script_calculateEquilibrium());
		out.push_back(Script_setStepOptions("type=scheil")); // TODO add this parameter to eConfig
		out.push_back(Script_setStepOptions("range start=" + std::to_string((int)startTemperature) +
			" stop=" + std::to_string((int)endTemperature) +
			" step-width=" + std::to_string(stepWitdh)));
		out.push_back(Script_setStepOptions("scheil-dependent-phase=LIQUID")); // TODO let choose the phase
		out.push_back(Script_setStepOptions("scheil-minimum-liquid-fraction=0.01")); // TODO add parameter for this
		out.push_back(Script_setStepOptions("temperature-in-celsius=yes")); // TODO this should be based on eConfig
		out.push_back(Script_setStepOptions("scheil-create-phases-automatically=yes"));
		
		out.push_back(Script_stepEquilibrium());

		return out;
	}

	void static Script_run_ScheilPrecipitation(IAM_Database* db,
		std::vector<std::string>& stepScheilScript,
		std::vector<DBS_PrecipitationPhase*> precipitationPhases,
		std::string TempDirectoryPath)
	{
		
		std::vector<DBS_PrecipitationPhase*> tempPhases;
		for (auto& item : precipitationPhases)
		{
			if (string_manipulators::find_index_of_keyword(item->Name,"P0") != std::string::npos)
			{
				tempPhases.push_back(item);
			}
		}

		// Add precipitation phases
		// TODO: optimize! here we call at each loop the database to load the phase item, we can create a vector pointer for this :)
		for(auto& item: tempPhases)
		{
			DBS_Phase tempPhase(db,item->IDPhase);
			tempPhase.load();

			stepScheilScript.push_back(Script_create_new_phase_fromScheil(tempPhase.Name));
			stepScheilScript.push_back(Script_set_precipitation_parameter(tempPhase.Name, "nucleation-sites=none"));
		}
		
		stepScheilScript.push_back(Script_setStartValues());
		stepScheilScript.push_back(Script_stepEquilibrium());
		stepScheilScript.push_back(script_buffer_loadState(-1));

		for (auto& item : tempPhases)
		{
			DBS_Phase tempPhase(db, item->IDPhase);
			tempPhase.load();

			stepScheilScript.push_back(Script_generate_precipitate_distribution(tempPhase.Name, item->CalcType, item->MinRadius, item->MeanRadius, item->MaxRadius, item->StdDev));
		}

		for (auto& item : tempPhases)
		{
			DBS_Phase tempPhase(db, item->IDPhase);
			tempPhase.load();

			stepScheilScript.push_back(Script_export_precipitate_distribution(tempPhase.Name, TempDirectoryPath + "/" + std::to_string(item->id()) + "_" + tempPhase.Name + ".txt"));
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="db"></param>
	/// <param name="configuration"></param>
	/// <param name="outVector"></param>
	/// <param name="storedFiles"></param>
	void static Script_run_heat_treatment(IAM_Database* db, AM_Config* configuration, std::string heatTreatmentName,
		std::vector<std::string>& outVector, std::vector<std::string>& storedFiles)
	{
		// get reference to the heat treatment object
		DBS_HeatTreatment HeatTreatment(db, -1);
		HeatTreatment.load_by_name(heatTreatmentName);
		if (HeatTreatment.id() == -1 || HeatTreatment.IDPrecipitationDomain == -1) return;

		// We use the file management of the system to store temporal text files used for the calculations
		AM_FileManagement fman(configuration->get_working_directory());

		// Link the datbase from configuration template
		DBS_Case tempCase(db, HeatTreatment.IDCase);
		tempCase.load();
		GenericScript_Database(db, configuration, tempCase.IDProject, HeatTreatment.IDCase, outVector);

		AM_Project Project(db, configuration, tempCase.IDProject);
		DBS_Project tempProject(db, Project.get_project_ID());
		AM_pixel_parameters TempPixel(db, &tempProject, tempCase.id());
		outVector.push_back(Script_setComposition_weight(Project.get_selected_elements_ByName(), TempPixel.get_composition_string()));
		outVector.push_back(Script_setStartValues());
		// get precipitation domain that will be used
#pragma region Matcalc_precipitationDomain_declaration

		DBS_PrecipitationDomain PrecipitationDomain(db, HeatTreatment.IDPrecipitationDomain);
		PrecipitationDomain.load();
		
		DBS_Phase tempPhase(db, PrecipitationDomain.IDPhase);
		tempPhase.load();

		outVector.push_back("create-precipitation-domain " + PrecipitationDomain.Name);
		outVector.push_back("set-precipitation-parameter " + PrecipitationDomain.Name + " thermodynamic-matrix-phase=" + tempPhase.Name);
		outVector.push_back("set-precipitation-parameter " + PrecipitationDomain.Name + " microstructure-evolution vacancies vacancy-evolution-model=" + PrecipitationDomain.VacancyEvolutionModel);
		outVector.push_back("set-precipitation-parameter " + PrecipitationDomain.Name + " microstructure-evolution vacancies vacancy-evolution-parameters excess-vacancy-efficiency-in-diffusion=" + std::to_string(PrecipitationDomain.ExcessVacancyEfficiency));
		outVector.push_back("set-precipitation-parameter " + PrecipitationDomain.Name + " initial-grain-diameter=" + std::to_string(PrecipitationDomain.InitialGrainDiameter));
		outVector.push_back("set-precipitation-parameter " + PrecipitationDomain.Name + " equilibrium-dislocation-density=" + std::to_string(PrecipitationDomain.EquilibriumDiDe));

#pragma endregion
		

		// get all primary/secondary precipitation phases and load into matcalc
#pragma region Matcalc_precipitationPhases_creation

		AM_Database_Datatable precipitationPhases(db, &AMLIB::TN_PrecipitationPhase());
		precipitationPhases.load_data("IDCase = " + std::to_string(HeatTreatment.IDCase) + " AND Name LIKE \'%P0%\'");

		if (precipitationPhases.row_count() == 0) return; // there is nothing to calculate first define primary precipitates

		for (int n1 = 0; n1 < precipitationPhases.row_count(); n1++)
		{
			DBS_PrecipitationPhase tempRef(db, -1);
			tempRef.load(precipitationPhases.get_row_data(n1));

			DBS_Phase tempPhase(db, tempRef.IDPhase);
			tempPhase.load();

			outVector.push_back(Script_create_new_phase(tempPhase.Name));
			outVector.push_back(Script_set_simulation_parameters(tempRef.Name + " number-of-size-classes=" + std::to_string(tempRef.NumberSizeClasses)));
			outVector.push_back(Script_set_simulation_parameters(tempRef.Name + " nucleation-sites=" + tempRef.NucleationSites));
			outVector.push_back(Script_set_simulation_parameters(tempRef.Name + " restrict-nucleation-to-precipitation-domain=" + PrecipitationDomain.Name));

			storedFiles.push_back(fman.save_file(AM_FileManagement::FILEPATH::TEMP,tempRef.Name + ".txt", tempRef.PrecipitateDistribution));
			outVector.push_back(Script_import_precipitate_distribution(storedFiles[storedFiles.size() - 1], tempRef.Name));
		}

		// Create secondary precipitates
		precipitationPhases.load_data("IDCase = " + std::to_string(HeatTreatment.IDCase) + " AND Name LIKE \'%P1%\'");
		if (precipitationPhases.row_count() == 0) return;
		
		for (int n1 = 0; n1 < precipitationPhases.row_count(); n1++)
		{
			DBS_PrecipitationPhase tempRef(db, -1);
			tempRef.load(precipitationPhases.get_row_data(n1));

			DBS_Phase tempPhase(db, tempRef.IDPhase);
			tempPhase.load();

			outVector.push_back(Script_create_new_phase_secondary(tempPhase.Name));
			outVector.push_back(Script_set_simulation_parameters(tempRef.Name + " number-of-size-classes=" + std::to_string(tempRef.NumberSizeClasses)));
			outVector.push_back(Script_set_simulation_parameters(tempRef.Name + " nucleation-sites=" + tempRef.NucleationSites));
			outVector.push_back(Script_set_simulation_parameters(tempRef.Name + " restrict-nucleation-to-precipitation-domain=" + PrecipitationDomain.Name));
		}

#pragma endregion

		// set heat treatment segments
#pragma region Matcalc_HeatTreatmentSegments

		// declare new buffer name
		std::string bufferName = "HT_Buf";
		std::string newStateName = "NSN";
		outVector.push_back("create-calc-state new-state-name=" + newStateName);
		outVector.push_back("rename-current-buffer " + bufferName);
		outVector.push_back("create-tm-treatment tm-treatment-name=" + HeatTreatment.Name);

		// load segments
		AM_Database_Datatable HTSegments(db, &AMLIB::TN_HeatTreatmentSegments());
		HTSegments.load_data("IDHeatTreatment = " + std::to_string(HeatTreatment.id()) + " ORDER BY stepIndex");
		if (HTSegments.row_count() == 0) 
		{
			outVector.clear(); 
			outVector.push_back("There are no segments for this heat treatment!");
			return;
		}

		// first setup line
		outVector.push_back("append-tmt-segment " + HeatTreatment.Name);
		outVector.push_back("edit-tmt-segment tm-treatment-name=" + HeatTreatment.Name + " tm-treatment-segment=. precipitation-domain=" + PrecipitationDomain.Name);
		outVector.push_back("edit-tmt-segment tm-treatment-name=" + HeatTreatment.Name + " tm-treatment-segment=. segment-start-temperature=" + std::to_string(HeatTreatment.StartTemperature));

		for (int n2 = 0; n2 < HTSegments.row_count(); n2++)
		{
			DBS_HeatTreatmentSegment tempSeg(db, -1);
			tempSeg.load(HTSegments.get_row_data(n2));

			if (tempSeg.TemperatureGradient > 0)
			{
				outVector.push_back("edit-tmt-segment tm-treatment-name=" + HeatTreatment.Name + " tm-treatment-segment=. T_end+T_dot segment-end-temperature=" + std::to_string(tempSeg.EndTemperature) +
					" temperature-gradient=" + std::to_string(tempSeg.TemperatureGradient));

			}
			else if (tempSeg.Duration > 0)
			{
				outVector.push_back("edit-tmt-segment tm-treatment-name=" + HeatTreatment.Name + " tm-treatment-segment=. T_end+delta_t segment-end-temperature=" + std::to_string(tempSeg.EndTemperature) +
					" segment-delta-time=" + std::to_string(tempSeg.Duration));
			}

			if (n2 != HTSegments.row_count() - 1)
			{
				outVector.push_back("append-tmt-segment " + HeatTreatment.Name);
			}

		}

		outVector.push_back("set-simulation-parameter temperature-control tm-treatment-name=" + HeatTreatment.Name);
		outVector.push_back("set-simulation-parameter max-temperature-step=" + std::to_string(HeatTreatment.MaxTemperatureStep));

#pragma endregion

		outVector.push_back("set-simulation-parameter starting-conditions=" + newStateName); // use same buffername for data extraction
		outVector.push_back("start-precipitate-simulation");
	}


#pragma endregion


#pragma region Script_contents
	/// <summary>
	/// Stores in a file all buffer data specified in FORMATTEDSTRING
	/// </summary>
	/// <param name="BUFFERSIZE">Size of Matcalc buffer</param>
	/// <param name="FORMATTEDSTRING">string that has the formatted variable names</param>
	/// <param name="TEMPDATA">Name of the temp file</param>
	/// <param name="configuration">AM_config</param>
	/// <returns>filename where data is in CSV format</returns>
	std::string static Buffer_to_variable(std::string BUFFERSIZE, std::string FORMATTEDSTRING, std::string TEMPDATA, AM_Config* configuration)
	{
		// create filenames
		std::string tempName = "TempFile" + std::to_string(std::rand() + get_uniqueNumber());
		std::string matcalfFilename = configuration->get_directory_path(AM_FileManagement::FILEPATH::SCRIPTS) + "\\" + tempName + ".Framework";

		// create script
		std::string scriptTemplate = "export-open-file file-name=\"" + matcalfFilename + "\" \n";
		scriptTemplate += "for (i;1.." + BUFFERSIZE + ") \n@ load-buffer-state line-index=i \n";
		scriptTemplate += "@ " + FORMATTEDSTRING + " \n";
		scriptTemplate += "@ export-file-variables format-string=\"#" + TEMPDATA + "\" \n";
		scriptTemplate += "endfor \n";
		scriptTemplate += "export-close-file \n";

		// Save script
		std::string fileName = configuration->get_directory_path(AM_FileManagement::FILEPATH::SCRIPTS) +  "\\" + tempName + ".mcs";
		std::ofstream striptFile(fileName);
		striptFile << scriptTemplate.c_str() << std::endl;
		striptFile.close();

		return fileName;
	}

	std::string static Buffer_to_variable_V02(AM_Config* configuration, std::string FORMATTEDSTRING, std::string VARIABLES, std::string HEADER)
	{
		// create filenames
		std::string tempName = "TempFile" + std::to_string(std::rand() + get_uniqueNumber());
		std::string matcalfFilename = configuration->get_directory_path(AM_FileManagement::FILEPATH::SCRIPTS) + "\\" + tempName + ".Framework";

		// create script
		std::string scriptTemplate = "export-open-file file-name=\"" + matcalfFilename + "\" \n";

		// Add Header if added
		if (HEADER.length() > 0) 
		{
			scriptTemplate += "export-file-variables format-string=" + HEADER + "\n";
		}
		
		scriptTemplate += "export-file-buffer format-string=\"" + FORMATTEDSTRING + "\" variable-name=" + VARIABLES + "\n";
		scriptTemplate += "export-close-file \n";

		// Save script
		std::string fileName = configuration->get_directory_path(AM_FileManagement::FILEPATH::SCRIPTS) + "\\" + tempName + ".mcs";
		std::ofstream striptFile(fileName);
		striptFile << scriptTemplate.c_str() << std::endl;
		striptFile.close();

		return fileName;
	}
#pragma endregion



}
