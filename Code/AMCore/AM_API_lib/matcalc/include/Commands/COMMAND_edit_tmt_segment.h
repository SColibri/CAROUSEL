#pragma once
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"

class COMMAND_edit_tmt_segment : public COMMAND_abstract
{
public:
	// constructor
	COMMAND_edit_tmt_segment(IPC_winapi* mccComm, AM_Config* configuration, std::string htName, std::string treatemtSegment, std::string startTemperature) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " tm-treatment-name=" + htName + " tm-treatment-segment=." + treatemtSegment + " segment-start-temperature=" + startTemperature + "\n";
	}

	COMMAND_edit_tmt_segment(IPC_winapi* mccComm, AM_Config* configuration, std::string htName, std::string precipitationDomainName) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command + " tm-treatment-name=" + htName + " tm-treatment-segment=." + " precipitation-domain=" + precipitationDomainName + "\n";
	}

	COMMAND_edit_tmt_segment(IPC_winapi* mccComm, AM_Config* configuration, std::string htName, DBS_HeatTreatmentSegment* segment) :
		COMMAND_abstract(mccComm, configuration)
	{
		_scriptContent = _command;

		if (segment->TemperatureGradient > 0)
		{
			_scriptContent += " tm-treatment-name=" + htName + " tm-treatment-segment=. T_end+T_dot segment-end-temperature=" + 
							    std::to_string(segment->EndTemperature) + " temperature-gradient=" + std::to_string(segment->TemperatureGradient) + "\n";
		}
		else if (segment->Duration > 0)
		{
			_scriptContent += " tm-treatment-name=" + htName + " tm-treatment-segment=. T_end+delta_t segment-end-temperature=" +
				std::to_string(segment->EndTemperature) + " segment-delta-time=" + std::to_string(segment->Duration) + "\n";
		}
	}

	virtual std::string DoAction() override
	{
		return send_command(_scriptContent);
	}

private:
	std::string _command{ "edit-tmt-segment" };

};