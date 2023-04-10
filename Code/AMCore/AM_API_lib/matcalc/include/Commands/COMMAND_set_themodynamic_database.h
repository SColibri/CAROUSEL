#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_set_thermodynamic_database : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_set_thermodynamic_database(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " \"" + configuration->get_ThermodynamicDatabase_path() + "\"\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "open-thermodynamic-database" };

};