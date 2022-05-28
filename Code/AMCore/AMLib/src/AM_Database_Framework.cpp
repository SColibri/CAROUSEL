#include "../include/AM_Database_Framework.h"
#include "../include/Database_implementations/Database_Factory.h"

AM_Database_Framework::AM_Database_Framework(AM_Config* configuration):
	_fileManagement(AM_FileManagement(configuration->get_working_directory())),
	_configuration(configuration)
{
	_database = Database_Factory::get_database(configuration);
	_database->connect();
	_dataController = new Data_Controller(_database, configuration, -1);
	create_database();
}

AM_Database_Framework::~AM_Database_Framework()
{
	if (_database != nullptr) delete _database;
	if (_dataController != nullptr) delete _dataController;
}