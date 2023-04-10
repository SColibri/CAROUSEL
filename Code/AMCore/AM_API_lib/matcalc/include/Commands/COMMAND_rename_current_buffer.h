#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_rename_current_buffer : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_rename_current_buffer(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, std::string bufferName) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " " + bufferName + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "rename-current-buffer" };

};