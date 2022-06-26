#pragma once
#include <vector>
#include <filesystem>
#include "AM_pixel_parameters.h"
#include "../interfaces/IAM_Database.h"
#include "Database_implementations/Database_scheme_content.h"
#include "AM_Database_Datatable.h"

/// <summary>
/// Controller class that manages all project functions.
/// </summary>
class AM_Project 
{
public:

	AM_Project(IAM_Database* database, AM_Config* configuration, int id);
	AM_Project(IAM_Database* database, AM_Config* configuration, std::string projectName);
	~AM_Project();

	#pragma region setters_getters
	/// <summary>
	/// Set project name, if ID of the project is -1, it will
	/// generate a new id for the current project.
	/// </summary>
	/// <param name="newName"></param>
	/// <param name="apiPath"></param>
	void set_project_name(std::string newName, std::string apiPath, std::string externalAPI_Path);
	
	/// <summary>
	/// get project name
	/// </summary>
	/// <returns></returns>
	const std::string& get_project_name();

	/// <summary>
	/// get API name used in this project. This API corresponds to the
	/// AMFramework API and not the external api (e.g matcalc)
	/// </summary>
	/// <returns></returns>
	const std::string& get_project_APIName();

	/// <summary>
	/// get external API Name (e.g. Matcalc + version)
	/// </summary>
	/// <returns></returns>
	const std::string& get_project_external_APIName();

	/// <summary>
	/// returns current project id, unsaved projects have an id of -1
	/// </summary>
	/// <returns></returns>
	const int& get_project_ID();

	/// <summary>
	/// returns project IAM_DBS input vector in csv format
	/// </summary>
	/// <returns></returns>
	std::string get_project_data_csv();

	/// <summary>
	/// returns all cases contained in this project
	/// </summary>
	/// <returns></returns>
	std::vector<AM_pixel_parameters*>& get_singlePixel_Cases();

	/// <summary>
	/// Selects a new set of elements by name, this will remove all existing data since
	/// modifying the selection would also mean a different project.
	/// </summary>
	/// <param name="newSelection"></param>
	/// <returns></returns>
	std::string set_selected_elements_ByName(std::vector<std::string> newSelection);

	/// <summary>
	/// returns vector with selected elements by name
	/// </summary>
	/// <returns></returns>
	std::vector<std::string> get_selected_elements_ByName();

	/// <summary>
	/// set reference element, this allows modifying the composition of
	/// other elements and adjusts the reference element so it fits 
	/// the configuration e.g. (%weight sum of all elements = 100%)
	/// </summary>
	/// <param name="ElementName"></param>
	/// <returns></returns>
	int set_reference_element(std::string ElementName);

	/// <summary>
	/// returns reference element by name
	/// </summary>
	/// <returns></returns>
	std::string get_reference_element_ByName();


	/// <summary>
	/// returns reference element by ID
	/// </summary>
	/// <returns></returns>
	int get_reference_element_ByID();

#pragma endregion

#pragma region Project_data
	/// <summary>
	/// clears all data related to each case contained in this project
	/// </summary>
	void clear_project_data();

	/// <summary>
	/// reloads information from the database
	/// </summary>
	void refresh_data();
#pragma endregion

#pragma region Checks
	/// <summary>
	/// returns if the project has a valid configuration and id > -1
	/// </summary>
	/// <returns></returns>
	bool project_is_valid();

	/// <summary>
	/// Returns true if the element is contained, checks by ID.
	/// </summary>
	/// <param name="IDElement"></param>
	/// <returns></returns>
	bool element_is_contained(int IDElement);


#pragma endregion

	
#pragma region Project
	std::string csv_list_cases_singlePixel() 
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_Case());
		dataT.load_data(AMLIB::TN_Case().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\' AND " + 
						AMLIB::TN_Case().columnNames[2] + " = \'0\' ");
		return dataT.get_csv();
	}

	std::string csv_list_cases_Object()
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_Case());
		dataT.load_data(AMLIB::TN_Case().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\' AND " +
			AMLIB::TN_Case().columnNames[2] + " != 0 ");
		return dataT.get_csv();
	}

	std::string csv_list_SelectedElements()
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_SelectedElements());
		dataT.load_data(AMLIB::TN_SelectedElements().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\'");
		return IAM_Database::csv_join_row(dataT.get_column_data(2), IAM_Database::Delimiter);
	}

	std::string csv_list_selectedPhases(int IDCase)
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_SelectedPhases());
		dataT.load_data(AMLIB::TN_SelectedPhases().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");
		return IAM_Database::csv_join_row(dataT.get_column_data(2), IAM_Database::Delimiter);
	}

	std::string csv_list_equilibriumConfiguration(int IDCase)
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_EquilibriumConfiguration());
		dataT.load_data(AMLIB::TN_EquilibriumConfiguration().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");
		return dataT.get_csv();
	}

	std::string csv_list_scheilConfiguration(int IDCase)
	{
		AM_Database_Datatable dataT(_db, &AMLIB::TN_ScheilConfiguration());
		dataT.load_data(AMLIB::TN_ScheilConfiguration().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");
		return dataT.get_csv();
	}

#pragma endregion

#pragma region Cases
	/// <summary>
	/// if pixelcase is ccontained in thsi project it will return 
	/// a pointer to the object
	/// </summary>
	/// <param name="IDCase"></param>
	/// <returns></returns>
	AM_pixel_parameters* get_pixelCase(int IDCase);

#pragma region SinglePixel_Cases
	/// <summary>
	/// Creates a new singlepixel case and adds it to the project.
	/// This is used for simulating one pixelcase only.
	/// if the project gets saved using 
	/// </summary>
	/// <param name="newName"></param>
	/// <returns></returns>
	int new_singlePixel_Case(std::string newName);

#pragma endregion

#pragma region Template_Case
	/// <summary>
	/// creates a new template case, this replaces any old template.
	/// Template cases are not stored in the database and only function
	/// as a temporal case configuration.
	/// </summary>
	/// <param name="nameCase"></param>
	void create_case_template(const std::string& nameCase);

	/// <summary>
	/// returns current case template. If no case is created it returns
	/// a nullpointer.
	/// </summary>
	/// <returns></returns>
	AM_pixel_parameters* get_case_template();

	/// <summary>
	/// set composition by name in case template, returns 0 is successfull
	/// </summary>
	/// <param name="ElementName"></param>
	/// <param name="newValue"></param>
	/// <returns></returns>
	int template_set_composition(std::string ElementName, double newValue);

	/// <summary>
	/// set composition by ID in case template, returns 0 is successfull
	/// </summary>
	/// <param name="ElementID"></param>
	/// <param name="newValue"></param>
	/// <returns></returns>
	int template_set_composition(int ElementID, double newValue);

	/// <summary>
	/// returns string with csv format contaning the elements by name
	/// with its composition value.
	/// </summary>
	/// <param name="ElementID"></param>
	/// <param name="newValue"></param>
	/// <returns></returns>
	std::string template_get_composition(int ElementID, double newValue);

	/// <summary>
	/// using the configurations loaded into the template case, create
	/// cases with varying concentrations defined by the input.
	/// </summary>
	/// <param name="elementID"></param>
	/// <param name="stepSize">concentration delta</param>
	/// <param name="steps">Steps to take per element</param>
	/// <param name="currentValues"></param>
	/// <returns></returns>
	std::string create_cases_vary_concentration(std::vector<int>& elementID,
		std::vector<double>& stepSize,
		std::vector<double>& steps,
		std::vector<double>& currentValues);
#pragma endregion

#pragma endregion

private:
	
	IAM_Database* const _db;
	AM_Config* const _configuration;

	DBS_Project* _project{nullptr}; //project object connected to database
	AM_pixel_parameters* _tempPixel {nullptr};
	DBS_CALPHADDatabase* _calphadDatabases {nullptr};
	std::vector<AM_pixel_parameters*> _singlePixel_cases; // single cases
	std::vector<DBS_SelectedElements*> _selectedElements; // list of selected elements

#pragma region Loaders
	/// <summary>
	/// loads all single cases related to this project ID
	/// </summary>
	void load_singlePixel_Cases();

	/// <summary>
	/// loads selected elements for this project
	/// </summary>
	void load_DBS_selectedElements();

	void load_DBS_CALPHAD();
#pragma endregion

#pragma region pointerRemove

	/// <summary>
	/// removes all pointers of selected elements
	/// </summary>
	void clear_selectedElements()
	{
		for (auto* SE : _selectedElements) { delete SE; }
		_selectedElements.clear();
	}

	/// <summary>
	/// removes all pointers to single pixel cases
	/// </summary>
	void clear_singlePixel_cases()
	{
		for (auto* SE : _singlePixel_cases) { delete SE; }
		_singlePixel_cases.clear();
	}

#pragma endregion
};
