#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_set_physical_database : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_set_physical_database(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " \"" + configuration->get_PhysicalDatabase_path() + "\"\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "read-physical-database" };

};