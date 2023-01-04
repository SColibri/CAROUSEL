#pragma once
#include "../../../interfaces/IAM_DBS.h"


/* ------------------------------------------------------
File generated by cmake script. This is a standard

template and will only be generated if the file does

not exist, feel free to edit.

*/// ----------------------------------------------------

namespace TRIGGERS
{

	class DBSTriggers_SelectedElements
	{
	private:
		/// <summary>
		/// Static class
		/// <summary>
		DBSTriggers_SelectedElements() {};
	public:
		/// <summary>
		/// Data trigger that removes data based on the project ID
		/// </summary>
		/// <param name="database">Database pointer</param>
		/// <param name="projectID">Project ID</param>
		/// <returns></returns>
		static int remove_project_data(IAM_Database* database, int projectID)
		{
			std::string query = AMLIB::TN_SelectedElements().columnNames[1] +
				" = " + std::to_string(projectID);

			return database->remove_row(&AMLIB::TN_SelectedElements(), query);
		}
	};
}
