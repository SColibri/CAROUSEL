#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_run_step_equilibrium : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_run_step_equilibrium(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration) :
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