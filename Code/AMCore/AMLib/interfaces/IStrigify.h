#pragma once
#include <string>
#include <vector>
#include <sstream>

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Interface type used for loading and saving class objects.
/// The class object has to return a save string and it has to be able to load
/// data from teh save string variable.
/// </summary>
class IStringify {
public:

	static inline std::string separatorChar{"|"}; // character that will be used to split data

	/// <summary>
	/// gets a string with all data from the class that implements this interface
	/// </summary>
	/// <returns></returns>
	virtual std::string get_save_string() = 0;

	/// <summary>
	/// Loads data to the class that implements it
	/// </summary>
	/// <param name="save_string"></param>
	virtual void load_string(std::ifstream& save_string) = 0;

	std::vector<std::string> split(const std::string& s, char delimiter)
	{
		std::vector<std::string> tokens;
		std::string token;
		std::istringstream tokenStream(s);
		while (std::getline(tokenStream, token, delimiter))
		{
			tokens.push_back(token);
		}
		return tokens;
	}
};

/** @}*/
