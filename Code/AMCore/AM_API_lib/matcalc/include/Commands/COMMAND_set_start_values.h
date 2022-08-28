#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_set_start_values : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_set_start_values(IPC_winapi* mccComm, AM_Config* configuration) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command;
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "set-start-values" };

};