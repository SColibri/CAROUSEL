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

		enum FILEPATH{NONE, GENERAL};
		std::string TestName{ "Namey" };

		//Methods
		//-----------------------------------------------
		
		/// <summary>
		/// Save content in FILEPATH as filename
		/// </summary>
		/// <param name="option_"></param>
		/// <param name="filename"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		std::string save_file(FILEPATH option_, std::string filename, std::string content);

		//-----------------------------------------------
		//END

		//getters and setters
		//-----------------------------------------------

		/// <summary>
		/// Gets filepath for option_ as FILEPATH
		/// </summary>
		/// <param name="option_"></param>
		/// <returns></returns>
		std::string get_filePath(FILEPATH option_);

		//-----------------------------------------------
		//END

		// Interfaces
		// --------------------------------------------

		// IStringify
		std::string get_save_string();
		void load_string(std::ifstream& save_string);

		// --------------------------------------------
		//END

	private:
		
		/// <summary>
		/// You can set a main path if you don't want to use the current working directory 
		/// </summary>
		std::string _mainPath{""};
		//std::string _filepath_names[] = {}; // String of FILEPATH enum

};
/** @}*/