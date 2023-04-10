#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_set_reference_element : public COMMAND_abstract
{
public: 
	// constructor
	COMMAND_set_reference_element(AMFramework::Interfaces::IAM_Communication* mccComm,AM_Config* configuration, std::string reference_element):
		COMMAND_abstract(mccComm, configuration), _refElement(reference_element)
	{
		_scriptContent = _command + " " + _refElement + "\n";
	}

	virtual std::string DoAction() override
	{
		if (_refElement.length() == 0) throw new COMMAND_exception("No reference element selected");
		return send_command(_scriptContent);
	}

private:
	std::string _command{"set-reference-element"};
	std::string _refElement{ "" };

};