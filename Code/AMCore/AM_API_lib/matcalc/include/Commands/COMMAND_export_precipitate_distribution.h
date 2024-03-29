#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_export_precipitate_distribution : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_export_precipitate_distribution(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, std::string phaseName, std::string filename) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + phaseName + " file-name=\"" + filename + "\"\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "export-precipitate-distribution precipitate-name=" };

};