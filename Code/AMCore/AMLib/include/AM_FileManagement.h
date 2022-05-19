#pragma once
#include <string>
#include "../interfaces/IStrigify.h"

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// File management
/// </summary>
class AM_FileManagement:IStringify {
	public:
		enum class FILEPATH{NONE, 
							GENERAL, 
							PROJECTS, 
							SCRIPTS, 
							DATABASE};

		std::string filepath_names[5] = {"None",
										"General",
										"Projects",
										"Scripts", 
										"Database"};

	// Constructors, destructors and other
	// -------------------------------------------------------------------------------------
	#pragma region Cons_Des
		AM_FileManagement();
		AM_FileManagement(std::string newWorkingDirectory);
		~AM_FileManagement();
	#pragma endregion Cons_Des

	//Methods
	// -------------------------------------------------------------------------------------
	#pragma region Methods

		/// <summary>
		/// Save content in FILEPATH as filename
		/// </summary>
		/// <param name="option_"></param>
		/// <param name="filename"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		std::string save_file(FILEPATH option_, std::string filename, std::string content);

		/// <summary>
		/// Gets filepath for option_ as FILEPATH
		/// </summary>
		/// <param name="option_"></param>
		/// <returns></returns>
		std::string get_filePath(FILEPATH option_);

	#pragma endregion Methods

	//getters and setters
	// -------------------------------------------------------------------------------------
	#pragma region Getters_Setters
		
		void set_workingDirectory(std::string workDirectory);

	#pragma endregion Getters_Setters

	// Interfaces
	// -------------------------------------------------------------------------------------
	#pragma region Interfaces
		// IStringify
		std::string get_save_string();
		void load_string(std::ifstream& save_string);
	#pragma endregion Interfaces


	private:
		
		/// <summary>
		/// You can set a main path if you don't want to use the current working directory 
		/// </summary>
		std::string _workingDirectory{""};

};
/** @}*/