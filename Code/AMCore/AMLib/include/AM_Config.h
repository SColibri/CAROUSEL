#pragma once
#include <string>
#include <filesystem>
#include "AM_FileManagement.h"
#include "../interfaces/IStrigify.h"
#include "../interfaces/IAM_Observed.h"


/** \defgroup AMLib
 *  @{
 *    This library contains all methodsand classes used in AMCoreand AM_API_lib.
 *  @}
 */

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Configuration options used by the framework
/// </summary>
class AM_Config : public IStringify, public IAM_Observed{

public:
	
	// Constructors, destructors and other
	// ---------------------------------------------
#pragma region Cons_Des
	/// <summary>
	/// Load config file if available
	/// </summary>
	AM_Config();

	/// <summary>
	/// Load config file from specified filename, config is copied to working directory
	/// </summary>
	/// <param name="filename"></param>
	AM_Config(std::string filename); // Load specified config file


	~AM_Config();

#pragma endregion Cons_Des
	
	// Methods
	// ---------------------------------------------
#pragma region Methods
	/// <summary>
	/// Save configuration as JSON file
	/// </summary>
	void save();

	/// <summary>
	/// Load configuration from filename
	/// </summary>
	/// <param name="filename"></param>
	void load(std::string filename);

	/// <summary>
	/// Loads by default name
	/// </summary>
	void load();
#pragma endregion Methods

	// setters-getters
	// ---------------------------------------------
#pragma region Getters_Setters
	std::string get_filename(); // gets full path of config file
	std::string get_directory_path(AM_FileManagement::FILEPATH fmanage); // gets full path of config file
	void set_config_name(std::string newName); // sets a new name for the config file

	const std::string get_working_directory();
	void set_working_directory(std::string mainPath); 
	
	void set_workingDirectory_option(AM_FileManagement::FILEPATH newOption);
	AM_FileManagement::FILEPATH get_workingDirectory_option();

	const std::string& get_api_path(); // gets AM_API library path
	void set_api_path(std::string filename); // sets AM_API library path

	const std::string& get_apiExternal_path(); // gets path for external libraries (e.g. matcalc dll)
	void set_apiExternal_path(std::string filename); // sets path for external libraries (e.g. matcalc dll)

	const std::string& get_ThermodynamicDatabase_path(); // gets path for external libraries (e.g. matcalc dll)
	void set_ThermodynamicDatabase_path(std::string filename); // sets path for external libraries (e.g. matcalc dll)

	const std::string& get_PhysicalDatabase_path(); // gets path for external libraries (e.g. matcalc dll)
	void set_PhysicalDatabase_path(std::string filename); // sets path for external libraries (e.g. matcalc dll)

	const std::string& get_MobilityDatabase_path(); // gets path for external libraries (e.g. matcalc dll)
	void set_MobilityDatabase_path(std::string filename); // sets path for external libraries (e.g. matcalc dll)

	const int& get_max_thread_number(); // gets the max number of threads to use for calculations 
	void set_max_thread_number(int maxNumber); // sets the max number of threads to be used for calculations
#pragma endregion Getters_Setters

	// Interfaces
	// --------------------------------------------
#pragma region Interfaces
	
	// IStringify - IMPLEMENTATION
	std::string get_save_string();
	void load_string(std::ifstream& save_string);

#pragma endregion Interfaces


private:

	AM_FileManagement _fileManagement{}; // config file management 
	std::string Name{"New Config"}; // Name of the configuration file
	std::string _thermodynamic_database_path{};
	std::string _physical_database_path{};
	std::string _mobility_database_path{};
	std::string _workingDirectory{};
	int _maxThreads{2};

	/// <summary>
	/// The framework uses a defined directory structure on which the config file can be found
	/// at the "general" location, namely at the top node of the working directory 
	/// </summary>
	AM_FileManagement::FILEPATH workingDirectoryOption = AM_FileManagement::FILEPATH::GENERAL;

	/// <summary>
	/// Library to implementation of IAM_API
	/// </summary>
	std::string _apiPath{std::filesystem::current_path().string() + "\\..\\AM_API_lib\\matcalc\\AM_MATCALC_Lib.dll"};
	
	/// <summary>
	/// If the software provider gives an api implementation and a dll file use this
	/// parameter to specify the path. For example, matcalc offers and api library that can be used,
	/// in this case, the AM_API_lib implements the external api and obtains the path by
	/// receiving and AM_Config parameter. 
	/// </summary>
	std::string _apiExternalPath{};
};
/** @}*/
