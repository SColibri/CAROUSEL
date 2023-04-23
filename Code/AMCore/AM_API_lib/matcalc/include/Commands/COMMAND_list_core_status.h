#pragma once

#include <string>

#include "COMMAND_abstract.h"

namespace APIMatcalc 
{
	namespace COMMANDS 
	{
		/// <summary>
		/// Command lists the core status (contains phase fractions, elements and phases)
		/// </summary>
		class COMMAND_list_core_status : public COMMAND_abstract
		{
		// list-core-status

		public:
			// constructor
			COMMAND_list_core_status(AMFramework::Interfaces::IAM_Communication* mccComm, AM_Config* configuration) :
				COMMAND_abstract(mccComm, configuration)
			{
				_scriptContent = _command + "\n";
			}

			virtual std::string DoAction() override
			{
				return send_command(_scriptContent);
			}

		private:
			std::string _command{ "list-core-status" };
		};
	}
}