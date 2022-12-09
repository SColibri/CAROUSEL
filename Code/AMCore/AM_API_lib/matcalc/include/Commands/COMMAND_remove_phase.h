#pragma once
#include "COMMAND_abstract.h"

class COMMAND_remove_phase : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_remove_phase(IPC_winapi* mccComm, AM_Config* configuration, string PhaseName) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " " + PhaseName + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "remove-phase" };

};