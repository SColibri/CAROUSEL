#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_ScheilConfiguration.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_Phase.h"

class COMMAND_scheil_configuration : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_scheil_configuration(IPC_winapi* mccComm, AM_Config* configuration, DBS_ScheilConfiguration* scheilConfig, std::string dependentPhase) :
		COMMAND_abstract(mccComm, configuration)
	{

		_command = "set-step-option type=scheil  \n \
					set-step-option range start=" + std::to_string(scheilConfig->StartTemperature) + " stop=" + std::to_string(scheilConfig->EndTemperature) + " step-width=" + std::to_string(scheilConfig->StepSize) + " \n" +
				    "set-step-option scheil-dependent-phase=" + dependentPhase + " \n" + 
					"set-step-option scheil-minimum-liquid-fraction= " + std::to_string(scheilConfig->minimumLiquidFraction) + " \n" +
					"set-step-option scheil-create-phases-automatically= yes \n ";
		
		_scriptContent = _command;	
	}

	virtual std::string DoAction() override
	{
		return send_command(_command);
	}

private:
	std::string _command{ "" };

};