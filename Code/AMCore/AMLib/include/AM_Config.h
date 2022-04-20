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
	
	// De/Con-structors ---------------------------- 
	
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

	// ---------------------------------------------
	// END 
	
	// Methods
	// ---------------------------------------------

	/// <summary>
	/// Save configuration as JSON file
	/// </summary>
	void save();

	/// <summary>
	/// Load configuration from filename
	/// </summary>
	/// <param name="filename"></param>
	void load(std::string filename);

	// --------------------------------------------
	// END

	// setters-getters
	// ---------------------------------------------

	std::string get_filename();


	AM_FileManagement& get_fileManagement(); 
	void set_mainPath(std::string mainPath); 
	void set_config_name(std::string newName); // sets a new name for the config file
	
	// --------------------------------------------
	// END

	// Interfaces
	// --------------------------------------------
	
	// IStringify - IMPLEMENTATION
	std::string get_save_string();
	void load_string(std::ifstream& save_string);

	// --------------------------------------------
	//END

private:

	AM_FileManagement _fileManagement{}; // config file management 
	std::string Name{"New Config"};

};
/** @}*/