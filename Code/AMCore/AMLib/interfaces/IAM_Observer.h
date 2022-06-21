#pragma once

#include "IAM_Observed.h"
class IAM_Observer 
{
public:
	virtual void update() = 0;
	virtual void dispose(IAM_Observed* observed) = 0;

};