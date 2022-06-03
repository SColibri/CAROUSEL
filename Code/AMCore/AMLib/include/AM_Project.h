#pragma once
#include <vector>
#include "AM_pixel_parameters.h"
#include "../interfaces/IAM_Database.h"
#include "Database_implementations/Database_scheme_content.h"
#include "AM_Database_Datatable.h"

class AM_Project 
{
public:
	AM_Project(IAM_Database* database, int id)
	{
		_db = database;
		_project = new DBS_Project(database, id);
	}

	~AM_Project()
	{
		if (_project != nullptr) delete _project;
	}

	std::vector<AM_pixel_parameters>& get_singlePixel_Cases()
	{
		return _singlePixel_cases;
	}

	void load_singlePixel_Cases() 
	{
		AM_Database_Datatable CaseTables(_db, &AMLIB::TN_Case());
		CaseTables.load_data(AMLIB::TN_Case().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\' AND " +
							 AMLIB::TN_Case().columnNames[2] + " = \'0\'");

		if(CaseTables.row_count() > 0)
		{
			for(int n1 = 0; n1 < CaseTables.row_count(); n1++)
			{
			
			}
		}
	}

private:
	IAM_Database* _db;
	DBS_Project* _project{nullptr};
	std::vector<AM_pixel_parameters> _singlePixel_cases;

};