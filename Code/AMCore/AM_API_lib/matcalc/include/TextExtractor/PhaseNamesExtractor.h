#pragma once

// c++
#include <string>
#include <regex>

// core
#include "../../../../AMLib/interfaces/IAM_Communication.h"
#include "../../../../AMLib/interfaces/ITextExtractor.h"
#include "../../../../AMLib/include/callbackFunctions/ErrorCallback.h"

// local
#include "../Commands/COMMAND_list_core_status.h"

namespace APIMatcalc
{
	namespace Extractors
	{
		/// <summary>
		/// PhaseName Extractor will extract the phase names from text and add them to the database
		/// if non existent. phases with the '#' char will be added as type "not contained" this means
		/// that these phases are not self contained in the database because these are phases with the 
		/// same name but different constituents.
		/// 
		/// Note:
		/// Constituents are not added into the database, for now this is not relevant to the thesis
		/// however if needed you can extract this data using the command "get phase status"
		/// </summary>
		class PhaseNameExtractor : public AMFramework::Interfaces::ITextExtractor
		{	
		public:

			/// <summary>
			/// Empty constructor
			/// </summary>
			PhaseNameExtractor() 
			{
				// Empty
			};

			/// <summary>
			/// Constructor with text input
			/// </summary>
			/// <param name="text"></param>
			PhaseNameExtractor(std::string text)
			{
				_text = text;
			};

#pragma region Implementation ITextExtractor
			/// <summary>
			/// extract from defined text, if not defined returns empty list
			/// </summary>
			/// <returns></returns>
			virtual std::vector<std::string> extract() override
			{
				return try_parse();
			}

			/// <summary>
			/// Extract phases from text
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			virtual std::vector<std::string> extract(std::string text) override
			{
				// Get phases from string 
				_text = text;
				return extract();
			}

			/// <summary>
			/// Extract data using matcalc core status
			/// </summary>
			/// <param name="comm"></param>
			/// <returns></returns>
			virtual std::vector<std::string> extract(AMFramework::Interfaces::IAM_Communication* comm) override
			{
				// Get phases from command
				APIMatcalc::COMMANDS::COMMAND_list_core_status commandList(comm, (AM_Config*)nullptr);
				std::string outString = commandList.DoAction();

				return extract(outString);
			}
#pragma endregion

		private:
			/// <summary>
			/// Parses through text
			/// </summary>
			/// <returns></returns>
			std::vector<std::string> try_parse() 
			{
				// output
				std::vector<std::string> result;

				// check using core status format
				result = result.size() == 0 ? parse_core_status_output_text() : result;

				return result;
			}

			/// <summary>
			/// Parses matcalc output text
			/// </summary>
			/// <returns></returns>
			std::vector<std::string> parse_core_status_output_text() 
			{
				// output
				std::vector<std::string> result;

				// Find the keyword phases: if not found return empty list
				int indexStart = string_manipulators::find_index_of_keyword(_text, "phases:");
				if (indexStart < 1 && indexStart + 7 < _text.size()) return result;
				
				// remove the keyword "phases:"
				indexStart = indexStart + 7;

				// extract section from string
				std::string neededSection = _text.substr(indexStart, _text.size() - 1);

				// Gets all phase names contained in the list
				std::regex pattern("\\b[\\w#]+\\b");
				std::smatch matchValue;

				while (std::regex_search(neededSection, matchValue, pattern)) {
					result.push_back(matchValue[0]);
					neededSection = matchValue.suffix().str();
				}

				return result;
			}

		};
	}
}