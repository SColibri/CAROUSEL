#pragma once

#include <vector>
#include "IAM_Observer.h"

/// <summary>
/// Generic virtual Observed class
/// </summary>
class IAM_Observed 
{
public:

	void add_observer(IAM_Observer* observer)
	{	
		_observers.push_back(observer);
	}

	void remove_observer(IAM_Observer* observer) 
	{
		_observers.erase(std::remove_if(_observers.begin(), 
										_observers.end(), 
										[&](IAM_Observer* observy) {return observy == observer; }),
										_observers.end());
	}

	void notify_observers() 
	{
		for(IAM_Observer* observer : _observers)
		{
			if (observer == nullptr) continue;
			observer->update();
		}
	}

private:
	std::vector<IAM_Observer*> _observers;


};