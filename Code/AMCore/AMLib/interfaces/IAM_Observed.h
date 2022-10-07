#pragma once

#include <vector>
#include "IAM_Observer.h"

/// <summary>
/// Generic virtual Observed class
/// </summary>
class IAM_Observed 
{
public:

	virtual void add_observer(IAM_Observer* observer) = 0;

	virtual void remove_observer(IAM_Observer* observer) = 0;

	virtual void notify_observers(std::string ObserverTypeName) = 0;


};