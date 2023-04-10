#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_load_buffer_state : public COMMAND_abstract
{
public:

	COMMAND_load_buffer_state(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, int bufferIndex) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " " + std::to_string(bufferIndex) + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "load-buffer-state" };

};