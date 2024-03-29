#pragma once
#include "../../../interfaces/IAM_DBS.h"



/* ------------------------------------------------------
File generated by cmake script. This is a standard

template and will only be generated if the file does

not exist, feel free to edit.

*/// ----------------------------------------------------

namespace TRIGGERS
{

	class DBSTriggers_PrecipitationDomain
	{
	private:
		/// <summary>
		/// Static class
		/// <summary>
		DBSTriggers_PrecipitationDomain() {};
	public:
		/// <summary>
		/// Data trigger that removes data based on the case ID
		/// </summary>
		/// <returns></returns>
		static int remove_case_data(IAM_Database* database, int caseID)
		{
			std::string query = AMLIB::TN_PrecipitationDomain().columnNames[1] +
				" = " + std::to_string(caseID);

			return database->remove_row(&AMLIB::TN_PrecipitationDomain(), query);
		}
	};
}
