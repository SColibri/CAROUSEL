#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_select_phases : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_select_phases(IPC_winapi* mccComm, AM_Config* configuration, std::vector<std::string> Phases) :
		COMMAND_abstract(mccComm, configuration)
	{
		_command = "select-phases ";

		int index{ 0 };
		for (std::string& phase : Phases)
		{
			_command += phase + " ";

			// if we add too many phases in one line, matcalc truncates the command, thus
			// we have to split the command in smaller bits.
			index++;
			if (index > 10)
			{
				index = 0;
				_command += " \n ";
			}
		}

		_command += "\n";
		_scriptContent = _command;
	}

	virtual std::string DoAction() override
	{
		return send_command(_command);
	}

private:
	std::string _command{ "" };

};