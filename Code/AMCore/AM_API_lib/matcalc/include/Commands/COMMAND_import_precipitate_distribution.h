#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_import_precipitate_distribution : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_import_precipitate_distribution(IPC_winapi* mccComm, AM_Config* configuration, std::string phasename, std::string filename) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " " + phasename + " " + filename + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "import-precipitate-distribution" };

};