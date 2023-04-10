#pragma once
#include "COMMAND_abstract.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_remove_phase : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_remove_phase(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, string PhaseName) :
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