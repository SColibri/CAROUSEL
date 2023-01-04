#pragma once
#include "../../../interfaces/IAM_DBS.h"

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
		/// <summary>
		/// Removes all project data
		/// </summary>
		/// <param name="database">database pointer</param>
		/// <param name="projectID">project ID</param>
		/// <returns>returns 0 when completed</returns>
		static int remove_project_data(IAM_Database* database, int projectID)
		{
			// Remove active phases
			TRIGGERS::DBSTriggers_ActivePhases::remove_project_data(database, projectID);
			TRIGGERS::DBSTrigger_ActivePhases_ElementComposition::remove_project_data(database, projectID);
			TRIGGERS::DBSTriggers_ActivePhases_Configuration::remove_project_data(database, projectID);

			// Remove selected Elements from project
			TRIGGERS::DBSTriggers_SelectedElements::remove_project_data(database, projectID);
			
			// Remove CALPHAD information
			TRIGGERS::DBSTriggers_CALPHADDatabase::remove_project_data(database, projectID);

			// Remove all cases
			TRIGGERS::DBSTrigger_Case::remove_project_data(database, projectID);
			
			return 0;
		}

	};
}