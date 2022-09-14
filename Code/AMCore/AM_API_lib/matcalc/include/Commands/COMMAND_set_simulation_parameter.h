#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_set_simulation_parameter : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_set_simulation_parameter(IPC_winapi* mccComm, AM_Config* configuration, std::string parameter) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " " + parameter + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "set-simulation-parameter" };

};