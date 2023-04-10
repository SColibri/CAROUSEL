#pragma once
#include "../../../../AMLib/interfaces/IAM_Command.h"
#include "../../../../AMLib/x_Helpers/IPC_winapi.h"
#include "../../../../AMLib/include/AM_Config.h"
#include "../../../../AMLib/x_Helpers/string_manipulators.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"
#include "../../../../AMLib/include/callbackFunctions/ErrorCallback.h"

class COMMAND_abstract : public IAM_Command
{
public:
	// constructor
	COMMAND_abstract(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration) :
		_communication(mccComm), _configuration(configuration)
	{
		if (!_communication->isRunning())
		{
			std::string errMessage = "COMMAND_abstract - Command API: There is no open communication channel! Check if the path to the external CALPHAD software is correct";
			AMFramework::Callback::ErrorCallback::TriggerCallback(&errMessage[0]);
			throw new exception(&errMessage[0]);
		}
	}

	virtual std::string DoAction() override = 0;

	virtual std::string Get_Script_text() override 
	{
		return _scriptContent;
	}

	virtual std::string Name() override
	{
		return _name;
	}

protected:
	std::string _name{ "Generic command" };
	AMFramework::Interfaces::IAM_Communication* _communication;
	AM_Config* _configuration;
	std::string _scriptContent{ "" };

	std::string send_command(std::string commandLine)
	{
		std::string outResult{""};

		try
		{
			std::vector<std::string> commList = string_manipulators::split_text(commandLine, "\n");

			for (auto& item : commList)
			{
				outResult += _communication->send_command(item + "\r\n");
			}

		}
		catch (const std::exception& e)
		{
			outResult = e.what();
		}
		
		return outResult;
	}

};