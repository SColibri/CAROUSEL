#pragma once

#include "../../../../AMLib/include/AM_Config.h"
#include "../../../../AMLib/include/AM_Project.h"
#include "../../../../AMLib/interfaces/IAM_Calculation.h"
#include "../../../../AMLib/interfaces/IAM_Database.h"
#include "../../../../AMLib/x_Helpers/IPC_winapi.h"
#include "../Commands/COMMAND_ALL.h"


namespace matcalc 
{
	class CALCULATION_abstract : public IAM_Calculation
	{
	public:
		
		// destructor :)
		~CALCULATION_abstract() 
		{
			for (auto& item : _commandList)
			{
				delete item;
			}

			_commandList.clear();
		}

		virtual std::string Calculate() override 
		{
			BeforeCalculation();

			for (auto& comm : _commandList) 
			{
				_output += comm->DoAction();
			}

			AMFramework::Callback::MessageCallBack::TriggerCallback(&_output[0]);
			AfterCalculation();
			return _output;
		}

		virtual std::string Get_script_text() override 
		{
			std::string output{""};

			for (auto& comm : _commandList) 
			{
				output += comm->Get_Script_text();
			}

			return output;
		}

		virtual std::string& Get_output() override 
		{
			return _output;
		}

		virtual void AfterCalculation() override = 0;
		virtual void BeforeCalculation() override = 0;

	protected:
		std::vector<IAM_Command*> _commandList;
		std::string _output{ "" };
	};
}
