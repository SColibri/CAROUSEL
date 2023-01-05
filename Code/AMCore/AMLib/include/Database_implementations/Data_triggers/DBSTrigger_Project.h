#pragma once
#include "../../../interfaces/IAM_DBS.h"
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

		/// <summary>
		/// Removes a project row entry
		/// </summary>
		/// <returns></returns>
		static int remove_project_object(IAM_Database* database, int projectID)
		{
			std::string query = AMLIB::TN_Projects().columnNames[0] +
				" = " + std::to_string(projectID);

			return database->remove_row(&AMLIB::TN_Projects(), query);
		}
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
			DBSTriggers_ActivePhases::remove_project_data(database, projectID);
			DBSTrigger_ActivePhases_ElementComposition::remove_project_data(database, projectID);
			DBSTriggers_ActivePhases_Configuration::remove_project_data(database, projectID);

			// Remove selected Elements from project
			DBSTriggers_SelectedElements::remove_project_data(database, projectID);
			
			// Remove CALPHAD information
			DBSTriggers_CALPHADDatabase::remove_project_data(database, projectID);

			// Remove all cases
			DBSTrigger_Case::remove_project_data(database, projectID);
			
			// Remove this
			remove_project_object(database, projectID);
			return 0;
		}

	};
}