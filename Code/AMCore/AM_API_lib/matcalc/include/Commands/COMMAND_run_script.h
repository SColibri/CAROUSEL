#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_run_script : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_run_script(IPC_winapi* mccComm, AM_Config* configuration, std::string filename) :
		COMMAND_abstract(mccComm, configuration), _filename(filename)
	{
		_scriptContent = _command + " \"" + _filename + "\" \n";
	}

	virtual std::string DoAction() override
	{
		if (_filename.length() == 0) throw new COMMAND_exception("No filename!");

		return send_command(_scriptContent);
	}

private:
	std::string _command{ "run-script-file" };
	std::string _filename{ "" };

};