#pragma once
#include <vector>
#include <mutex>
#include <iostream>
#include <fstream>
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "COMMAND_run_script.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_export_variables : public COMMAND_abstract
{
public:
	/// <summary>
	/// 
	/// </summary>
	/// <param name="mccComm"></param>
	/// <param name="configuration"></param>
	/// <param name="filename"></param>
	/// <param name="FormattedString"></param>
	/// <param name="Variables"></param>
	/// <param name="Header"></param>
	COMMAND_export_variables(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, std::string filename, std::string FormattedString, std::string Variables, std::string Header) :
		COMMAND_abstract(mccComm, configuration), _filename(filename)
	{
		// create script
		_script = "export-open-file file-name=\"" + _filename + "\" \n";

		// Add Header if added
		if (Header.length() > 0)
		{
			_script += "export-file-variables format-string=" + Header + "\n";
		}

		_script += "export-file-buffer format-string=\"" + FormattedString + "\" variable-name=" + Variables + "\n";
		_script += "export-close-file \n";

		// create script filename for later
		std::string tempName = "TempFile" + std::to_string(std::rand() + get_uniqueNumber());
		_scriptFilename = configuration->get_directory_path(AM_FileManagement::FILEPATH::SCRIPTS) + "\\" + tempName + ".mcs";

		// Additional commands
		_commandList.push_back(new COMMAND_run_script(_communication, _configuration, _scriptFilename));

		// script content from base class
		_scriptContent = _script;
	}

	~COMMAND_export_variables() 
	{
		for (auto& item:_commandList)
		{
			delete item;
		}

		_commandList.clear();
	}

	virtual std::string DoAction() override
	{
		std::string output{""};

		std::ofstream tempFile(_scriptFilename);
		tempFile << _script << std::endl;
		tempFile.close();

		for (auto& item : _commandList) 
		{
			output += item->DoAction() + "\n";
		}

		return output;
	}

	/// <summary>
	/// Get a unique number, useful for parallel operations
	/// </summary>
	/// <returns></returns>
	static size_t get_uniqueNumber()
	{
		size_t out;

		_uniqueNumber_mutex.lock();
		out = _uniqueNumber;
		_uniqueNumber += 1;
		_uniqueNumber_mutex.unlock();

		return out;
	}

	/// <summary>
	/// returns the output data in vector form. only use for small data sets.
	/// 
	/// Note: header is included if file contains a header at row index = 0
	/// </summary>
	/// <param name="filename">path to file with data structure</param>
	/// <returns></returns>
	static std::vector<std::vector<std::string>> Get_data_from_file(std::string filename) 
	{
		std::vector<std::vector<std::string>> output;
		
		// get data from file
		std::vector<std::string> rowEntries = string_manipulators::read_file_to_end(filename);

		// retrieve data form file
		for (auto& row : rowEntries)
		{
			output.push_back(string_manipulators::split_text(row, " "));
		}

		return output;
	}


private:
	std::string _command{ "" };
	std::string _filename{ "" };
	std::string _script{ "" };
	std::string _scriptFilename{ "" };
	std::vector<IAM_Command*> _commandList;
	inline static mutex _uniqueNumber_mutex;
	inline static int _uniqueNumber = 0;
};