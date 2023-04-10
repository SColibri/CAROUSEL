#pragma once
#include <vector>
#include "COMMAND_abstract.h"
#include "COMMAND_exception.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_HeatTreatment.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_HeatTreatmentSegment.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_PrecipitationDomain.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

class COMMAND_heat_treatment_configuration : COMMAND_abstract
{
public:
	// constructor
	COMMAND_heat_treatment_configuration(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, DBS_HeatTreatment* heatTreatment,
		std::vector<DBS_HeatTreatmentSegment*>& listSegments, DBS_PrecipitationDomain* precipitationDomain) :
		COMMAND_abstract(mccComm, configuration)
	{
		// create heat treatment object in matcalc
		_command = "create-calc-state new-state-name=" + heatTreatment->Name + "\n" +
				    "append - tmt - segment " + heatTreatment->Name + "\n" +
					"edit-tmt-segment tm-treatment-name=" + heatTreatment->Name + " tm-treatment-segment=. precipitation-domain=" + precipitationDomain->Name + "\n" +
					"edit-tmt-segment tm-treatment-name=" + heatTreatment->Name + " tm-treatment-segment=. segment-start-temperature=" + std::to_string(heatTreatment->StartTemperature) + "\n";

		// add all segments
		int index{ 0 };
		for (auto& item : listSegments) 
		{
			// cooling rate
			if (item->TemperatureGradient > 0)
			{
				_command += "edit-tmt-segment tm-treatment-name=" + heatTreatment->Name + " tm-treatment-segment=. T_end+T_dot segment-end-temperature=" + std::to_string(item->EndTemperature) +
							" temperature-gradient=" + std::to_string(item->TemperatureGradient) + "\n";

			}

			// aging
			else if (item->Duration > 0)
			{
				_command += "edit-tmt-segment tm-treatment-name=" + heatTreatment->Name + " tm-treatment-segment=. T_end+delta_t segment-end-temperature=" + std::to_string(item->EndTemperature) +
							" segment-delta-time=" + std::to_string(item->Duration) + "\n";
			}

			// create new segment
			if (index != listSegments.size() - 1)
			{
				_command += "append-tmt-segment " + heatTreatment->Name + "\n";
				index++;
			}
		}

		// heat treatment parameters
		_command += "set-simulation-parameter temperature-control tm-treatment-name=" + heatTreatment->Name + "\n";
		_command += "set-simulation-parameter max-temperature-step=" + std::to_string(heatTreatment->MaxTemperatureStep) + "\n";

		// script content from base class
		_scriptContent = _command;
	}

	virtual std::string DoAction() override
	{
		return send_command(_command);
	}

private:
	std::string _command{ "" };

};