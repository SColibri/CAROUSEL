#pragma once
#include <string>

class IAM_Observer 
{
public:
	virtual void update(std::string& ObserverTypeName) = 0;
};