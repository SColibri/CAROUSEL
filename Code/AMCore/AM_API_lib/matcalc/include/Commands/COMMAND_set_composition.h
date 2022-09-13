#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_set_composition : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_set_composition(IPC_winapi* mccComm, AM_Config* configuration, std::vector<std::string> Elements, std::vector<double> Composition) :
		COMMAND_abstract(mccComm, configuration)
	{
		if(Elements.size() != Composition.size()) return;

		_scriptContent = _command;
		for (int n1 = 1; n1 < Elements.size(); n1++)
		{
			_scriptContent += Elements[n1] + "=" + std::to_string(Composition[n1]) + " ";
		}
		_scriptContent += "\"";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "enter-composition type=weight-percent composition=\"" };

};