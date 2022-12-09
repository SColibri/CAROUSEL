#pragma once
#include "../../interfaces/IAM_Observed.h"

class ObservedObject : public IAM_Observed
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

	void notify_observers(std::string ObserverTypeName)
	{
		for (IAM_Observer* observer : _observers)
		{
			if (observer == nullptr) continue;
			observer->update(ObserverTypeName);
		}
	}

protected:
	std::vector<IAM_Observer*> _observers;

};