#pragma once
#include <string>
#include "../../../interfaces/IAM_Database.h"
#include "../Database_scheme_content.h"
#include "../../AM_Database_Datatable.h"
#include "../../AM_Database_TableStruct.h"

class DBS_Project
{
public:
	int ID{-1};
	std::string Name{""};
	std::string APIName{""};

	DBS_Project(IAM_Database* database)
	{
		_db = database;
	}

	int save()
	{
		AM_Database_TableStruct TableStructure = AMLIB::TN_Projects();
		std::vector<std::string> Input{std::to_string(ID), 
									    Name, 
										APIName};

		if(ID == -1)
		{
			_db->insert_row(&TableStructure, Input);
			ID = _db->get_last_ID(&TableStructure);
		}
		else 
		{
			_db->update_row(&TableStructure, Input);
		}

		return 0;
	}

private:
	IAM_Database* _db{nullptr};

};