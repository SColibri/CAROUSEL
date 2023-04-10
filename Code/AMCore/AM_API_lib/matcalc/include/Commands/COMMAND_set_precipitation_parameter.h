#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_set_precipitation_parameter : public COMMAND_abstract
{
public:

	COMMAND_set_precipitation_parameter(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, std::string PhaseName, std::string parameter) :
		COMMAND_abstract(mccComm, configuration)
	{

		_scriptContent = "set-precipitation-parameter " + PhaseName + " " + parameter + "\n";
		_command = _scriptContent;
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "" };

};