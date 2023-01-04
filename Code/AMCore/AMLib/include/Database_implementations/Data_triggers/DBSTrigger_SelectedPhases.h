#pragma once
#include "../../../interfaces/IAM_DBS.h"



/* ------------------------------------------------------
File generated by cmake script. This is a standard

template and will only be generated if the file does

not exist, feel free to edit.

*/// ----------------------------------------------------

namespace TRIGGERS
{

	class DBSTriggers_SelectedPhases
	{
	private:
		/// <summary>
		/// Static class
		/// <summary>
		DBSTriggers_SelectedPhases() {};
	public:
		/// <summary>
		/// Removes a case row entry
		/// </summary>
		/// <returns></returns>
		static int remove_case_data(IAM_Database* database, int caseID)
		{
			std::string query = AMLIB::TN_SelectedPhases().columnNames[1] +
				" = " + std::to_string(projectID);

			return database->remove_row(&AMLIB::TN_SelectedPhases(), query);
		}
	};
}
