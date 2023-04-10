#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_create_precipitate_domain : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_create_precipitate_domain(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, std::string PrecipitationDomainName) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " " + PrecipitationDomainName + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "create-precipitation-domain" };

};