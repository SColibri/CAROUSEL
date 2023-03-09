#pragma once
TEST_CASE("Database", "[classic]")
{
	SECTION("Create a sqlite database")
	{
		AM_Config config01;
		Database_Sqlite3 db(&config01);
		int Response = db.connect();
		REQUIRE(Response == 0);
	}

	SECTION("Save and Load")
	{
// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_ActivePhases.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_ActivePhases_Configuration.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_ActivePhases_ElementComposition.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_CALPHADDatabase.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_Case.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_Element.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_ElementComposition.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_EquilibriumConfiguration.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_EquilibriumPhaseFractions.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_HeatTreatment.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_HeatTreatmentProfile.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_HeatTreatmentSegment.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_Phase.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_PrecipitateSimulationData.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_PrecipitationDomain.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_PrecipitationPhase.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_Project.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_ScheilConfiguration.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_ScheilCumulativeFraction.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_ScheilPhaseFraction.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_SelectedElements.h


// C:/Users/drogo/Documents/TUM/Thesis/Framework/AMFramework/Code/AMCore/AMLib/include/Database_implementations/Data_stuctures/DBS_SelectedPhases.h


	}
}
