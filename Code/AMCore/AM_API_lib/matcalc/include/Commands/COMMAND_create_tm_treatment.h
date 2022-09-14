#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_create_tm_treatment : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_create_tm_treatment(IPC_winapi* mccComm, AM_Config* configuration, std::string htName) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " tm-treatment-name=" + htName + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "create-tm-treatment" };

};