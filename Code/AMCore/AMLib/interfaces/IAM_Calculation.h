#pragma once

class IAM_Calculation 
{
public:

	/// <summary>
	/// Realizes the calculation, returns the response after calculations
	/// </summary>
	/// <returns></returns>
	virtual std::string Calculate() = 0;

	/// <summary>
	/// obtains a step by step list of all commands used
	/// </summary>
	/// <returns></returns>
	virtual std::string Get_script_text() = 0;

	/// <summary>
	/// Returns execution output
	/// </summary>
	/// <returns></returns>
	virtual std::string& Get_output() = 0;

	/// <summary>
	/// Actions to do before calculation
	/// </summary>
	virtual void BeforeCalculation() = 0;

	/// <summary>
	/// Actions to do after calculation
	/// </summary>
	virtual void AfterCalculation() = 0;

private:

};