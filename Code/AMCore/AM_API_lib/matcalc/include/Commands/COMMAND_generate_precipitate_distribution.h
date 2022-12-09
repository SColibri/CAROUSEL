#pragma once
#include <string>
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"

class COMMAND_generate_precipitate_distribution : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_generate_precipitate_distribution(IPC_winapi* mccComm, AM_Config* configuration, std::string PhaseName, 
		std::string calculationType, double minRadius, double meanRadius, double maxRadius, double stdDev) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = "generate-precipitate-distribution phase-name=" + PhaseName + " calculation-type=" + calculationType +
			" min-radius=" + std::to_string(minRadius) + " mean-radius=" + std::to_string(meanRadius) + " max-radius=" + std::to_string(maxRadius) +
			" standard-deviation=" + std::to_string(stdDev) + "\n";
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "" };

};