#include "../include/AM_Database_Framework.h"
#include "../include/Database_implementations/Database_Factory.h"

AM_Database_Framework::AM_Database_Framework(AM_Config* configuration):
	_fileManagement(AM_FileManagement(configuration->get_working_directory())),
	_configuration(configuration)
{
	_database = Database_Factory::get_database(configuration);
	create_database();
}