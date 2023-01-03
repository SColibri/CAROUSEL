#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "DBSTrigger_ActivePhases_ElementComposition.h"
#include "DBSTrigger_SelectedElements.h"

namespace TRIGGERS
{
	class DBSTrigger_Project
	{
	private:
		/// <summary>
		/// Static class
		/// </summary>
		DBSTrigger_Project() {}

	public:

		static int remove_project_data(IAM_Database* database, int projectID)
		{
			// Remove active phases
			TRIGGERS::DBSTrigger_ActivePhases_ElementComposition::remove_project_data(database, projectID);
			TRIGGERS::DBSTrigger_Project::remove_project_data(database, projectID);

			// get selected Elements from project
			TRIGGERS::DBSTrigger_SelectedElements::remove_project_data(database, projectID);

			// Load all cases and delete related data
			std::string queryCase = AMLIB::TN_Case().columnNames[1] +
				" = " + std::to_string(projectID);
			AM_Database_Datatable caseList(database, &AMLIB::TN_Case());
			caseList.load_data(queryCase);

			for (int n1 = 0; n1 < caseList.row_count(); n1++)
			{
				DBS_Case::remove_case_data(database, std::stoi(caseList(0, n1)));
			}

			// Remove selected elements
			return 1;
		}

	};
}