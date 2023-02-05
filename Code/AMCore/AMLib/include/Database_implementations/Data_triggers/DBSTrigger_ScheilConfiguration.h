#pragma once
#include "../../../interfaces/IAM_DBS.h"



/* ------------------------------------------------------
File generated by cmake script. This is a standard

template and will only be generated if the file does

not exist, feel free to edit.

*/// ----------------------------------------------------

namespace TRIGGERS
{

	class DBSTriggers_ScheilConfiguration
	{
	private:
		/// <summary>
		/// Static class
		/// <summary>
		DBSTriggers_ScheilConfiguration() {};
	public:
		/// <summary>
		/// Removes a case row entry
		/// </summary>B
		/// <returns></returns>
		static int remove_case_data(IAM_Database* database, int caseID)
		{
			std::string query = AMLIB::TN_ScheilConfiguration().columnNames[1] +
				" = " + std::to_string(caseID);

			return database->remove_row(&AMLIB::TN_ScheilConfiguration(), query);
		}
	};
}