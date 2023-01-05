#pragma once
#include "../../../interfaces/IAM_DBS.h"

namespace TRIGGERS
{
	class DBSTrigger_EquilibriumConfiguration
	{
	private:
		/// <summary>
		/// Static class
		/// </summary>
		DBSTrigger_EquilibriumConfiguration() {};
	public:
		/// <summary>
		/// Removes a case row entry
		/// </summary>
		/// <returns></returns>
		static int remove_case_data(IAM_Database* database, int caseID)
		{
			std::string query = AMLIB::TN_EquilibriumConfiguration().columnNames[1] +
				" = " + std::to_string(caseID);

			return database->remove_row(&AMLIB::TN_EquilibriumConfiguration(), query);
		}
	};
}