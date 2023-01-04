#pragma once
#include "../../../interfaces/IAM_DBS.h"
#include "DBSTrigger_EquilibriumConfiguration.h"
#include "DBSTrigger_EquilibriumPhaseFraction.h"
#include "DBSTrigger_ScheilConfiguration.h"
#include "DBSTrigger_ScheilPhaseFraction.h"
#include "DBSTrigger_ElementComposition.h"
#include "DBSTrigger_HeatTreatment.h"
#include "DBSTrigger_PrecipitationPhase.h"
#include "DBSTrigger_SelectedPhases.h"

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

			// Remove case objects
			remove_case_object(database, projectID);
			return 0;
		}

		/// <summary>
		/// Data trigger that removes dependent data based on case ID
		/// </summary>
		/// <param name="database">Database pointer</param>
		/// <param name="caseID">Case ID</param>
		/// <returns></returns>
		static int remove_case_data(IAM_Database* database, int caseID)
		{
			// Case configurations
			DBSTrigger_EquilibriumConfiguration::remove_case_data(database, caseID);
			DBSTriggers_EquilibriumPhaseFraction::remove_case_data(database, caseID);
			DBSTriggers_ElementComposition::remove_case_data(database, caseID);
			DBSTriggers_SelectedPhases::remove_case_data(database, caseID);
			DBSTriggers_PrecipitationDomain::remove_case_data(database, caseID);

			// Solidification simulations
			DBSTriggers_ScheilConfiguration::remove_case_data(database, caseID);
			DBSTriggers_ScheilPhaseFraction::remove_case_data(database, caseID);

			// Heat treatment and precipitation simulations
			DBSTriggers_HeatTreatment::remove_case_data(database, caseID);
			DBSTriggers_PrecipitationPhase::remove_case_data(database, caseID);
		}

	};
}