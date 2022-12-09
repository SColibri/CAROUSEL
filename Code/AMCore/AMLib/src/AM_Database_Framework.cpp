#include <functional>
#include "../include/AM_Database_Framework.h"
#include "../include/Database_implementations/Database_Factory.h"

AM_Database_Framework::AM_Database_Framework(AM_Config* configuration):
	_configuration(configuration)
{
	set_observedObject(_configuration);
	_database = Database_Factory::get_database(_configuration);
	update_variables();
}

AM_Database_Framework::~AM_Database_Framework()
{
	database_delete();
}