#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_select_elements : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_select_elements(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, std::vector<std::string> Elements) :
		COMMAND_abstract(mccComm, configuration)
	{
		_command = "select-elements ";

		for (std::string& elementy : Elements)
		{
			_command += elementy + " ";
		}

		_command += "\n";
		_scriptContent = _command;
	}

	virtual std::string DoAction() override
	{
		return send_command(_command);
	}

private:
	std::string _command{ "" };

};