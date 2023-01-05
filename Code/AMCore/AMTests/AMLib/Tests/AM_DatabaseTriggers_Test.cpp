#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/include/Database_implementations/Database_Sqlite3.h"
#include "../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../AMLib/include/Database_implementations/Data_triggers/DBSTrigger_ALL.h"
#include <string>
#include <catch2/catch_test_macros.hpp>
#include <filesystem>

namespace TriggerSetup
{

	static AM_Config configuration;
	static AM_Database_Framework _dbF(&configuration);
	static IAM_Database* _db{nullptr};

	static void add_data();
	static void init()
	{
		configuration.set_working_directory("");
		Database_Factory::set_schema("projectDB");

		//Create a new database file
		std::string filename = configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "\\" +
			Database_Factory::get_schema() + ".db";

		if (std::filesystem::exists(filename)) 
		{
			std::filesystem::remove(std::filesystem::path(filename));
		}

		AM_Database_Framework dbF(&configuration);

		_db = (Database_Factory::get_database(&configuration));
		add_data();
	}

	static void add_data()
	{
		DBS_Project dbsProject(_db, -1);
		dbsProject.Name = "New project";
		dbsProject.save();

		// Project has to have and ID > -1
		REQUIRE(dbsProject.id() > -1);

		DBS_ActivePhases_Configuration dbsActiveC(_db, -1);
		dbsActiveC.IDProject = dbsProject.id();
		dbsActiveC.save();

		DBS_CALPHADDatabase dbsCalphad(_db, -1);
		dbsCalphad.IDProject = dbsProject.id();
		dbsCalphad.save();

		for (size_t i = 0; i < 1; i++)
		{
			DBS_ActivePhases dbsActiveC(_db, -1);
			dbsActiveC.IDProject = dbsProject.id();
			dbsActiveC.IDPhase = 100;
			dbsActiveC.save();

			DBS_ActivePhases_ElementComposition dbsActiveE(_db, -1);
			dbsActiveE.IDProject = dbsProject.id();
			dbsActiveE.IDElement = 100;
			dbsActiveE.save();
		}

		for (size_t i = 0; i < 1; i++)
		{
			DBS_SelectedElements dbsElements(_db, -1);
			dbsElements.IDProject = dbsProject.id();
			dbsElements.IDElement = 100;
			dbsElements.save();

			DBS_Case dbsCase(_db, -1);
			dbsCase.Name = "New case";
			dbsCase.IDProject = dbsProject.id();
			dbsCase.save();

			REQUIRE(dbsCase.id() > -1);
			DBS_EquilibriumConfiguration dbsEConfig(_db, -1);
			dbsEConfig.IDCase = dbsCase.id();
			dbsEConfig.save();

			DBS_ScheilConfiguration dbsSConfig(_db, -1);
			dbsSConfig.IDCase = dbsCase.id();
			dbsSConfig.save();

			for (size_t i = 0; i < 10; i++)
			{
				DBS_EquilibriumPhaseFraction dbsEFrac(_db, -1);
				dbsEFrac.IDCase = dbsCase.id();
				dbsEFrac.save();

				DBS_ScheilPhaseFraction dbsSFrac(_db, -1);
				dbsSFrac.IDCase = dbsCase.id();
				dbsSFrac.save();

				DBS_PrecipitationPhase dbsPPhase(_db, -1);
				dbsPPhase.IDCase = dbsCase.id();
				dbsPPhase.IDPhase = 100;
				dbsPPhase.save();
				REQUIRE(dbsPPhase.id() > -1);

				DBS_PrecipitationDomain dbsPDomain(_db, -1);
				dbsPDomain.IDCase = dbsCase.id();
				dbsPDomain.IDPhase = 100;
				dbsPDomain.Name = "pDomain";
				dbsPDomain.Name.append(std::to_string(i));
				dbsPDomain.save();

				DBS_SelectedPhases dbsSPhase(_db, -1);
				dbsSPhase.IDCase = dbsCase.id();
				dbsSPhase.IDPhase = 100;
				dbsSPhase.save();

				DBS_HeatTreatment dbsHeat(_db, -1);
				dbsHeat.IDCase = dbsCase.id();
				dbsHeat.Name = "Testy ";
				dbsHeat.Name.append(std::to_string(i));
				dbsHeat.save();

				REQUIRE(dbsHeat.id() > -1);
				for (size_t i = 0; i < 5; i++)
				{
					DBS_HeatTreatmentSegment dbsHSegment(_db, -1);
					dbsHSegment.IDHeatTreatment = dbsHeat.id();
					dbsHSegment.save();
				}

				for (size_t i = 0; i < 100; i++)
				{
					DBS_HeatTreatmentProfile dbsProfile(_db, -1);
					dbsProfile.IDHeatTreatment = dbsHeat.id();
					dbsProfile.save();

					DBS_PrecipitateSimulationData dbsPrecip(_db, -1);
					dbsPrecip.IDHeatTreatment = dbsHeat.id();
					dbsPrecip.IDPrecipitationPhase = dbsPPhase.id();
					dbsPrecip.save();
				}
				
			}
		}


	}

}

TEST_CASE("Database", "[classic]")
{
	SECTION("Initialize")
	{
		TriggerSetup::init();
	}

	SECTION("Check if testing data was created correctly")
	{
		// projects
		AM_Database_Datatable dTProject(TriggerSetup::_db, &AMLIB::TN_Projects());
		dTProject.load_data();
		REQUIRE(dTProject.row_count() > 0);

		// cases
		AM_Database_Datatable dTCase(TriggerSetup::_db, &AMLIB::TN_Case());
		dTCase.load_data();
		REQUIRE(dTCase.row_count() > 0);

		// Acctive phases
		AM_Database_Datatable dTAPhase(TriggerSetup::_db, &AMLIB::TN_ActivePhases());
		dTAPhase.load_data();
		REQUIRE(dTAPhase.row_count() > 0);

		// Active phases configurations
		AM_Database_Datatable dTAPhaseC(TriggerSetup::_db, &AMLIB::TN_ActivePhases_Configuration());
		dTAPhaseC.load_data();
		REQUIRE(dTAPhaseC.row_count() > 0);

		// Active phases element composition
		AM_Database_Datatable dTAPhaseE(TriggerSetup::_db, &AMLIB::TN_ActivePhases_ElementComposition());
		dTAPhaseE.load_data();
		REQUIRE(dTAPhaseE.row_count() > 0);

		// Solidification simulation
		AM_Database_Datatable dTEConf(TriggerSetup::_db, &AMLIB::TN_EquilibriumConfiguration());
		dTEConf.load_data();
		REQUIRE(dTEConf.row_count() > 0);

		AM_Database_Datatable dTEFrac(TriggerSetup::_db, &AMLIB::TN_EquilibriumPhaseFractions());
		dTEFrac.load_data();
		REQUIRE(dTEFrac.row_count() > 0);

		AM_Database_Datatable dTSConf(TriggerSetup::_db, &AMLIB::TN_ScheilConfiguration());
		dTSConf.load_data();
		REQUIRE(dTSConf.row_count() > 0);

		AM_Database_Datatable dTSPFrac(TriggerSetup::_db, &AMLIB::TN_ScheilPhaseFraction());
		dTSPFrac.load_data();
		REQUIRE(dTSPFrac.row_count() > 0);

		// Precipitation
		AM_Database_Datatable dTPrecipDom(TriggerSetup::_db, &AMLIB::TN_PrecipitationDomain());
		dTPrecipDom.load_data();
		REQUIRE(dTPrecipDom.row_count() > 0);

		AM_Database_Datatable dTPrecipPhase(TriggerSetup::_db, &AMLIB::TN_PrecipitationPhase());
		dTPrecipPhase.load_data();
		REQUIRE(dTPrecipPhase.row_count() > 0);

		AM_Database_Datatable dTPrecipSim(TriggerSetup::_db, &AMLIB::TN_PrecipitateSimulationData());
		dTPrecipSim.load_data();
		REQUIRE(dTPrecipSim.row_count() > 0);

		// Heat treatment
		AM_Database_Datatable dTht(TriggerSetup::_db, &AMLIB::TN_HeatTreatment());
		dTht.load_data();
		REQUIRE(dTht.row_count() > 0);

		AM_Database_Datatable dThtseg(TriggerSetup::_db, &AMLIB::TN_HeatTreatmentSegments());
		dThtseg.load_data();
		REQUIRE(dThtseg.row_count() > 0);

		AM_Database_Datatable dThtprof(TriggerSetup::_db, &AMLIB::TN_HeatTreatmentProfile());
		dThtprof.load_data();
		REQUIRE(dThtprof.row_count() > 0);

	}

	SECTION("Verify that project level is removed correctly")
	{
		// Verify that data is there
		AM_Database_Datatable dT(TriggerSetup::_db, &AMLIB::TN_Projects());
		dT.load_data("ID = 1");
		REQUIRE(dT.row_count() > 0);

		// Remove project
		TRIGGERS::DBSTrigger_Project::remove_project_data(TriggerSetup::_db, 1);
		
		// Verify data
		dT.load_data("ID = 1");
		REQUIRE(dT.row_count() == 0);

		AM_Database_Datatable dCase(TriggerSetup::_db, &AMLIB::TN_Case());
		dCase.load_data("IDProject = 1");
		REQUIRE(dCase.row_count() == 0);

	}
}