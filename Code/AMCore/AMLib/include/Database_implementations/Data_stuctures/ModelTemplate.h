#pragma once

// c++
#include <string>
#include <vector>
#include <regex>

// Core
#include "../../../x_Helpers/string_manipulators.h"

namespace AMFramework
{
	template <typename T, typename = void>
	struct has_ostream_operator : std::false_type {};

	template <typename T>
	struct has_ostream_operator<T, decltype(std::cout << std::declval<T>(), void())> : std::true_type {};

	template<typename T>
	/// <summary>
	/// ModelTemplate for modelSchema objects
	/// </summary>
	class ModelTemplate : public T
	{
	public:
#pragma region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		ModelTemplate()
		{
			// Empty
		}
#pragma endregion

#pragma region Methods
		/// <summary>
		/// Returns data in CSV format
		/// </summary>
		/// <returns></returns>
		std::string GetCSV()
		{
			std::stringstream sstream;
			sstream << this;

			std::string data = sstream.str();
			
		}

		/// <summary>
		/// Gets all properties
		/// </summary>
		/// <returns></returns>
		std::vector<std::string> GetProperties()
		{

		}
#pragma endregion


	};
}