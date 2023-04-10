#pragma once

// c++
#include <string>

// Core
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_EquilibriumConfiguration.h"
#include "../../../../AMLib/include/Database_implementations/Data_stuctures/DBS_Phase.h"
#include "../../../../AMLib/interfaces/IAM_Communication.h"

// Local
#include "COMMAND_exception.h"
#include "COMMAND_abstract.h"

namespace APIMatcalc
{
	namespace Commands
	{
		class COMMAND_equilibrium_configuration : public COMMAND_abstract
		{
		private:


		public:
			/// <summary>
			/// Equilibrium phase precipitation step options configurations
			/// </summary>
			/// <param name="mccComm"></param>
			/// <param name="configuration"></param>
			/// <param name="equilibriumConfig"></param>
			COMMAND_equilibrium_configuration(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration, DBS_EquilibriumConfiguration* equilibriumConfig) :
				COMMAND_abstract(mccComm, configuration)
			{
				_command = "set-step-option type=temperature  \n \
					set-step-option range start=" + std::to_string(equilibriumConfig->StartTemperature) + " stop=" + std::to_string(equilibriumConfig->EndTemperature) + " scale=lin step-width=" + std::to_string(equilibriumConfig->StepSize) + " \n";

				_scriptContent = _command;
			}

			virtual std::string DoAction() override
			{
				return send_command(_command);
			}

		private:
			std::string _command{ "" };

		};
	}
}