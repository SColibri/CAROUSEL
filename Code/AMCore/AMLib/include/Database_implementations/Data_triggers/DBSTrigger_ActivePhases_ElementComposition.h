#pragma once
#include "../../../interfaces/IAM_DBS.h"

namespace TRIGGERS
{
	class DBSTrigger_ActivePhases_ElementComposition
	{
	private:
		/// <summary>
		/// Static class
		/// </summary>
		DBSTrigger_ActivePhases_ElementComposition() {};

	public:
		/// <summary>
		/// Data trigger that removes data based on the project ID
		/// </summary>
		/// <param name="database">database pointer</param>
		/// <param name="projectID">Project ID</param>
		/// <returns></returns>
		static int remove_project_data(IAM_Database* database, int projectID)
		{
			std::string query = AMLIB::TN_ActivePhases_ElementComposition().columnNames[1] +
				" = " + std::to_string(projectID);

			return database->remove_row(&AMLIB::TN_ActivePhases_ElementComposition(), query);
		}

	};
}

