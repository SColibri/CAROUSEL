#pragma once

#include <string>

class IAM_Command
{
public:
	/// <summary>
	/// command name
	/// </summary>
	/// <returns></returns>
	virtual std::string Name() = 0;
	
	/// <summary>
	/// Does action and returns output
	/// </summary>
	/// <returns>response of action</returns>
	virtual std::string DoAction() = 0;

	/// <summary>
	/// Obtains step-by step command script
	/// </summary>
	/// <returns></returns>
	virtual std::string Get_Script_text() = 0;

};