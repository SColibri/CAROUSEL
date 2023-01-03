#pragma once
#include "../../../interfaces/IAM_DBS.h"

namespace TRIGGERS
{
	class DBSTrigger_Case 
	{
	private:
		/// <summary>
		/// Static case
		/// </summary>
		DBSTrigger_Case() {};

		/// <summary>
		/// Removes a case row entry
		/// </summary>
		/// <returns></returns>
		static int remove_case_object(IAM_Database* database, int projectID)
		{
			std::string query = AMLIB::TN_Case().columnNames[1] +
				" = " + std::to_string(projectID);

			return database->remove_row(&AMLIB::TN_Case(), query);
		}

	public:
		/// <summary>
		/// Data trigger that removes data based on the project ID
		/// </summary>
		/// <param name="database">Database pointer</param>
		/// <param name="projectID">Project ID</param>
		/// <returns></returns>
		static int remove_project_data(IAM_Database* database, int projectID)
		{
			// Load all cases and delete related data
			std::string queryCase = AMLIB::TN_Case().columnNames[1] +
				" = " + std::to_string(projectID);
			AM_Database_Datatable caseList(database, &AMLIB::TN_Case());
			caseList.load_data(queryCase);

			for (int n1 = 0; n1 < caseList.row_count(); n1++)
			{
				remove_case_data(database, std::stoi(caseList(0, n1)));
			}

			remove_case_object(database, projectID);
		}

		/// <summary>
		/// Data trigger that removes dependent data based on case ID
		/// </summary>
		/// <param name="database">Database pointer</param>
		/// <param name="caseID">Case ID</param>
		/// <returns></returns>
		static int remove_case_data(IAM_Database* database, int caseID)
		{
		
		}

	};
}