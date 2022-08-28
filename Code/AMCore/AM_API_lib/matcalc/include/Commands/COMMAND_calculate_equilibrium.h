#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_calculate_equilibrium : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_calculate_equilibrium(IPC_winapi* mccComm, AM_Config* configuration, double Temperature) :
		COMMAND_abstract(mccComm, configuration)
	{
		_command = "set-temperature-celsius " + std::to_string(Temperature) + " \n calculate-equilibrium";
		_scriptContent = _command;
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "" };

};