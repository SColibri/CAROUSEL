#pragma once

#include <string>

class IAM_Command
{
public:
	virtual std::string Name() = 0;
	
	// ---------------------------->
	virtual void DoAction() = 0;


};