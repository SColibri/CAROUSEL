#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_start_precipitate_simulation : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_start_precipitate_simulation(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "start-precipitate-simulation" };

};