#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_append_tmt_segment : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_append_tmt_segment(IPC_winapi* mccComm, AM_Config* configuration, std::string htName) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + htName + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "append-tmt-segment" };

};