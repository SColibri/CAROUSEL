#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_scheil_precipitation : COMMAND_abstract
{
public:
	// constructor
	COMMAND_scheil_precipitation(IPC_winapi* mccComm, AM_Config* configuration) :
		COMMAND_abstract(mccComm, configuration)
	{
		_command = "step-equilibrium \n";
		_scriptContent = _command;
	}

	virtual std::string DoAction() override
	{
		return send_command(_command);
	}

private:
	std::string _command{ "" };

};