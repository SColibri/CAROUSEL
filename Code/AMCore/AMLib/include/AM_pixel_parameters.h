#pragma once
#include <string>
#include <vector>
#include "Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "Database_implementations/Data_stuctures/DBS_Project.h"
#include "Database_implementations/Data_stuctures/DBS_Case.h"
#include "Database_implementations/Data_stuctures/DBS_ElementComposition.h"
#include "Database_implementations/Data_stuctures/DBS_ScheilConfiguration.h"
#include "Database_implementations/Data_stuctures/DBS_SelectedPhases.h"
#include "Database_implementations/Data_stuctures/DBS_ScheilPhaseFraction.h"
#include "Database_implementations/Data_stuctures/DBS_CALPHADDatabase.h"
#include "Database_implementations/Data_stuctures/DBS_EquilibriumConfiguration.h"
#include "Database_implementations/Data_stuctures/DBS_EquilibriumPhaseFractions.h"
#include "Database_implementations/Data_stuctures/DBS_SelectedElements.h"
#include "Database_implementations/Data_Controller.h"

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Block parameters
/// </summary>
class AM_pixel_parameters {

public:
	AM_pixel_parameters(IAM_Database* database,
		DBS_Project* project,
		int IDCase);

	AM_pixel_parameters(const AM_pixel_parameters& toCopy);

	

#pragma region object
	/// <summary>
	/// saves current case and related data
	/// </summary>
	void save();

	/// <summary>
	/// reloads composition from a the related project to this case.
	/// </summary>
	void reload_element_compositions();

	/// <summary>
	/// create new case pixel
	/// </summary>
	/// <param name="db"></param>
	/// <param name="configuration"></param>
	/// <param name="projectID"></param>
	/// <param name="newName"></param>
	/// <returns></returns>
	int static create_new_pixel(IAM_Database* db,
								AM_Config* configuration,
								int projectID,
								std::string newName);

#pragma region Object_methods
	/// <summary>
	/// creates new cases based on this current object, modifying the concentrattion
	/// configuration based on the input
	/// </summary>
	/// <param name="elementID"></param>
	/// <param name="stepSize"></param>
	/// <param name="steps"></param>
	/// <param name="currentValues"></param>
	/// <param name="rIndex"></param>
	/// <returns></returns>
	std::string create_cases_vary_concentration(std::vector<int>& elementID,
		std::vector<double>& stepSize,
		std::vector<double>& steps,
		std::vector<double>& currentValues,
		int rIndex = 0);
	
	/// <summary>
	/// returns true if the IDphase is selected in this case
	/// </summary>
	/// <param name="idPhase"></param>
	/// <returns></returns>
	bool check_if_phase_is_selected(int idPhase);

	/// <summary>
	/// If contained it returns the index position of the elements composition ByID
	/// </summary>
	/// <param name="IDElement"></param>
	/// <returns></returns>
	int get_element_index(int IDElement);

	/// <summary>
	/// If contained it returns the index position of the elements composition ByName
	/// </summary>
	/// <param name="ElementName"></param>
	/// <returns></returns>
	int get_element_index(std::string ElementName);

	/// <summary>
	/// If specified, returns the reference element ID
	/// </summary>
	/// <returns></returns>
	int get_reference_elementID();

	/// <summary>
	/// removes all equilibrium phase fractions
	/// </summary>
	void reset_equilibrium();

	/// <summary>
	/// removes all scheil phase fractions
	/// </summary>
	void reset_scheil();
#pragma endregion

#pragma region Object_getters_setters
	/// <summary>
	/// Flag that locks the object for saving. true - thsi object can be saved 
	/// </summary>
	/// <param name="allowsave"></param>
	void set_AllowSave(bool allowsave);

	/// <summary>
	/// returns case ID
	/// </summary>
	/// <returns></returns>
	const int& get_caseID();

	/// <summary>
	/// returns case name
	/// </summary>
	/// <returns></returns>
	const std::string& get_CaseName();

	/// <summary>
	/// sets case name
	/// </summary>
	/// <param name="newName"></param>
	void set_CaseName(std::string newName);

	/// <summary>
	/// get vector of composition values in string type
	/// </summary>
	/// <returns></returns>
	std::vector<std::string> get_composition_string();

	/// <summary>
	/// get cvector of composition values in double type
	/// </summary>
	/// <returns></returns>
	std::vector<double> get_composition_double();

	/// <summary>
	/// set composition value. Find element by ID.
	/// return 0 when successful
	/// </summary>
	/// <param name="IDElement"></param>
	/// <param name="newValue"></param>
	/// <returns></returns>
	int set_composition(int IDElement, double newValue);

	/// <summary>
	/// set composition value. Find element by Name.
	/// return 0 when successful
	/// </summary>
	/// <param name="ElementName"></param>
	/// <param name="newValue"></param>
	/// <returns></returns>
	int set_composition(std::string ElementName, double newValue);

	/// <summary>
	/// update reference composition.
	/// </summary>
	void update_referenceComposition();

	/// <summary>
	/// checks if composition is valid, in this case weight percent
	/// </summary>
	/// <returns></returns>
	bool check_composition();

	/// <summary>
	/// retunrs vector with names of all selected phases
	/// </summary>
	/// <returns></returns>
	std::vector<std::string> get_selected_phases_ByName();

	/// <summary>
	/// returns vector with ID of all selected phases
	/// </summary>
	/// <returns></returns>
	std::vector<int> get_selected_phases_ByID();

	/// <summary>
	/// Adds phase to selected phases by name
	/// </summary>
	/// <param name="phaseName"></param>
	/// <returns></returns>
	int add_selectedPhase(std::string phaseName);

	/// <summary>
	/// removes phase from selected phases by name
	/// </summary>
	/// <param name="phaseName"></param>
	/// <returns></returns>
	int remove_selectedPhase(std::string phaseName);

	/// <summary>
	/// set selected phases by input vector with name of phases
	/// </summary>
	/// <param name="phasesSelect"></param>
	/// <returns></returns>
	int select_phases(std::vector<std::string> phasesSelect);

	/// <summary>
	/// removes all selected phases
	/// </summary>
	void remove_all_seleccted_phase();
#pragma endregion

#pragma endregion

#pragma region Data
	/// <summary>
	/// loads all related data to this case.
	/// </summary>
	void load_all();

	/// <summary>
	/// loads calphad database used for this project
	/// </summary>
	/// <param name="IDCase"></param>
	void load_DBS_CALPHADDB(int IDCase);

	/// <summary>
	/// loads scheil configuration for step calculations
	/// </summary>
	/// <param name="IDCase"></param>
	void load_DBS_ScheilConfig(int IDCase);

	/// <summary>
	/// loads equilibrium step configuration
	/// </summary>
	/// <param name="IDCase"></param>
	void load_DBS_EquilibriumConfig(int IDCase);

	/// <summary>
	/// loads all phase fractions calculated from
	/// equilibrium
	/// </summary>
	void load_DBS_EquilibriumPhaseFraction();

	/// <summary>
	/// loads all phase fractions calculated from Scheil
	/// </summary>
	void load_DBS_ScheilPhaseFraction();

	/// <summary>
	/// loads element composition for the current case
	/// </summary>
	void load_DBS_elementComposition();
	
	/// <summary>
	/// loads all selected phases
	/// </summary>
	void load_DBS_selectedPhases();

#pragma region csv
	/// <summary>
	/// gets in csv format name of element and composition
	/// </summary>
	/// <returns></returns>
	std::string csv_composition_ByName();
#pragma endregion

#pragma endregion



	

#pragma region external_objects

#pragma region Equilibrium
	DBS_EquilibriumConfiguration* get_EquilibriumConfiguration()
	{
		return _equilibriumConfiguration; //TODO remove this
	}

	/// <summary>
	/// set start temperature for step calculations
	/// </summary>
	/// <param name="newvalue"></param>
	void set_equilibrium_config_startTemperature(double newvalue);

	/// <summary>
	/// returns start temperature for equilibrium configuration
	/// </summary>
	/// <param name="newvalue"></param>
	double get_equilibrium_config_startTemperature();


	/// <summary>
	/// set end temperature for step calculations
	/// </summary>
	/// <param name="newvalue"></param>
	void set_equilibrium_config_endTemperature(double newvalue);

	/// <summary>
	/// returns end temperature for equilibrium step calculations
	/// </summary>
	/// <returns></returns>
	double get_equilibrium_config_endTemperature();

	/// <summary>
	/// set step size for step calculations
	/// </summary>
	/// <param name="newvalue"></param>
	void set_equilibrium_config_stepSize(double newvalue);

	/// <summary>
	/// returns step size for equilibrium step calculations
	/// </summary>
	/// <returns></returns>
	double get_equilibrium_config_stepSize();
#pragma endregion

#pragma region Scheil
	DBS_ScheilConfiguration* get_ScheilConfiguration()
	{
		return _scheilConfiguration; //TODO remove this
	}

	/// <summary>
	/// set start temperature for step calculations
	/// </summary>
	/// <param name="newvalue"></param>
	void set_scheil_config_startTemperature(double newvalue);

	/// <summary>
	/// get configuration for start temperature for step calculations
	/// </summary>
	/// <returns></returns>
	double get_scheil_config_startTemperature();

	/// <summary>
	/// set end temperature for step calculations
	/// </summary>
	/// <param name="newvalue"></param>
	void set_scheil_config_endTemperature(double newvalue);

	/// <summary>
	/// get configuration for end temperature for step calculations
	/// </summary>
	/// <returns></returns>
	double get_scheil_config_endTemperature();

	/// <summary>
	/// get step size for step calculation
	/// </summary>
	/// <param name="newvalue"></param>
	void set_scheil_config_stepSize(double newvalue);

	/// <summary>
	/// get step size for step calculation
	/// </summary>
	/// <returns></returns>
	double get_scheil_config_StepSize();

	/// <summary>
	/// select dependent phase by ID. default "LIQUID"
	/// </summary>
	/// <param name="newvalue"></param>
	/// <returns></returns>
	int set_scheil_config_dependentPhase(int newvalue);

	/// <summary>
	/// select dependent phase by Name. default "LIQUID"
	/// </summary>
	/// <param name="phaseName"></param>
	/// <returns></returns>
	int set_scheil_config_dependentPhase(std::string phaseName);

	/// <summary>
	/// returns ID of dependent phase
	/// </summary>
	/// <returns></returns>
	int get_scheil_config_dependentPhaseID();

	/// <summary>
	/// returns dependent phase by name
	/// </summary>
	/// <returns></returns>
	std::string get_scheil_config_dependentPhaseName();

	/// <summary>
	/// set minimum liquidfraction for scheil calculations
	/// default 0.01
	/// </summary>
	/// <param name="newvalue"></param>
	void set_scheil_config_minimumLiquidFraction(double newvalue);

	/// <summary>
	/// get minimul liquid fraction for scheil calculations
	/// default 0.01
	/// </summary>
	/// <returns></returns>
	double get_scheil_config_minimumLiquidFraction();
#pragma endregion

#pragma region CALPHAD
	DBS_CALPHADDatabase* get_calphad() 
	{
		return _CALPHAD_DB;
	}

	const int& get_calphad_id()
	{
		return _CALPHAD_DB->id();
	}

	const std::string& get_calphad_thermodynamic_database()
	{
		return _CALPHAD_DB->Thermodynamic;
	}
	
	const std::string& get_calphad_mobility_database()
	{
		return _CALPHAD_DB->Mobility;
	}

	const std::string& get_calphad_physical_database()
	{
		return _CALPHAD_DB->Physical;
	}
#pragma endregion

#pragma endregion



private:
	//Flags
	bool _allowSave{ true };

	//Models
	IAM_Database* _db;
	DBS_Project* _project;
	DBS_CALPHADDatabase* _CALPHAD_DB;
	DBS_Case* _case;
	DBS_ScheilConfiguration* _scheilConfiguration;
	DBS_EquilibriumConfiguration* _equilibriumConfiguration;

	std::vector<DBS_EquilibriumPhaseFraction*> _equilibriumPhaseFractions;
	std::vector<DBS_ScheilPhaseFraction*> _scheilPhaseFractions;
	std::vector<DBS_ElementComposition*> _elementComposition;
	std::vector<DBS_SelectedPhases*> _selectedPhases;



};
/** @}*/