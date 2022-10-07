#include <functional>
#include "../include/AM_Database_Framework.h"
#include "../include/Database_implementations/Database_Factory.h"

AM_Database_Framework::AM_Database_Framework(AM_Config* configuration):
	_configuration(configuration)
{
	set_observedObject(_configuration);
	update_variables();
}

AM_Database_Framework::~AM_Database_Framework()
{
	if (_database != nullptr) delete _database;
	//if (_dataController != nullptr) delete _dataController;
}