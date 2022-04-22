#pragma once
#include <string>
#include "AM_FileManagement.h"
#include "../interfaces/IStrigify.h"


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
class AM_Config : IStringify{

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
#pragma endregion Methods

	// setters-getters
	// ---------------------------------------------
#pragma region Getters_Setters
	std::string get_filename(); // gets full path of config file
	void set_config_name(std::string newName); // sets a new name for the config file

	const std::string get_working_directory();
	void set_working_directory(std::string mainPath); 
	
	void set_workingDirectory_option(AM_FileManagement::FILEPATH newOption);
	AM_FileManagement::FILEPATH get_workingDirectory_option();
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

	/// <summary>
	/// The framework uses a defined directory structure on which the config file can be found
	/// at the "general" location, namely at the top node of the working directory 
	/// </summary>
	AM_FileManagement::FILEPATH workingDirectoryOption = AM_FileManagement::FILEPATH::GENERAL;


};
/** @}*/
