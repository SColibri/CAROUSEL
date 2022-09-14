#pragma once

#include "../../../../AMLib/include/AM_Config.h"
#include "../../../../AMLib/include/AM_Project.h"
#include "../../../../AMLib/interfaces/IAM_Calculation.h"
#include "../../../../AMLib/interfaces/IAM_Database.h"
#include "../../../../AMLib/x_Helpers/IPC_winapi.h"
#include "../Commands/COMMAND_ALL.h"
#include "CALCULATIONS_abstract.h"


namespace matcalc
{
	class CALCULATION_decorator : public CALCULATION_abstract
	{
	public:
		CALCULATION_decorator() {}
		CALCULATION_decorator(CALCULATION_abstract* calc) { Set_Calculation(calc); }

		virtual void BeforeCalculation() override 
		{ 
			_output += _calculation->Calculate();
			AfterDecoratorCalculation();
		}

		virtual void AfterCalculation() override = 0;

		virtual void AfterDecoratorCalculation() = 0;

		virtual std::string Get_script_text() override
		{

			std::string output{""};

			if(_calculation != nullptr)
			{
				output = _calculation->Get_script_text();
			}

			for (auto& comm : _commandList)
			{
				output += comm->Get_Script_text();
			}

			return output;
		}

	protected:
		CALCULATION_abstract* _calculation;
		

		void Set_Calculation(CALCULATION_abstract* calc)
		{
			_calculation = calc;
		}

	};
}
