#pragma once
#include <string>
#include <vector>
#include "../../interfaces/IAM_Observer.h"
#include "../../interfaces/IAM_Observed.h"
#include "ObservedObject.h"

/// <summary>
/// This class only observes one object at a time
/// 
/// When using this model please remember to guarantee the lifespan of the observed object while observer
/// exists.
/// </summary>
class  ObserverObject : public IAM_Observer
{
public:
	// Constructors
	ObserverObject(){}
	~ObserverObject()
	{
		remove_me_from_ObserverList();
	}

	/// <summary>
	/// Interface implementation for the update method
	/// </summary>
	/// <param name="ObserverTypeName"></param>
	virtual void update(std::string& ObserverTypeName) override
	{
		//do nothing, could stay virtual 
	}

	/// <summary>
	/// set observed object
	/// </summary>
	void set_observedObject(ObservedObject* OObject) 
	{
		remove_me_from_ObserverList();
		_observed = OObject;
		_observed->add_observer(this);
	}

private:
	/// <summary>
	/// Pointer to observed function, we hold a pointer for removing this observer from
	/// the observed list
	/// </summary>
	IAM_Observed* _observed = nullptr;

	/// <summary>
	/// Removes current object from the observed object list
	/// </summary>
	void remove_me_from_ObserverList() 
	{
		if (_observed != nullptr)
		{
			_observed->remove_observer(this);
		}
	}

};
