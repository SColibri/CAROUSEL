#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/include/Database_implementations/Database_Sqlite3.h"
#include "../../../AMLib/include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include <catch2/catch_test_macros.hpp>
#include <filesystem>


TEST_CASE("Database", "[classic]")
{
	SECTION("Create a sqlite database")
	{
		AM_Config config01;
		Database_Sqlite3 db(&config01);
		int Response = db.connect();

		REQUIRE(Response == 0);
	}

	SECTION("test structures")
	{
		AM_Config config01;

		//Create a new database file
		std::string filename = config01.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "/" +
			Database_Factory::get_schema() + ".db";
		std::filesystem::remove(std::filesystem::path(filename));

		AM_Database_Framework FM01(&config01);

		IAM_Database* db01 = (IAM_Database*) new Database_Sqlite3(&config01);
		REQUIRE(db01->connect() == 0);


		DBS_CALPHADDatabase CalDB(db01, -1);
		CalDB.IDCase = 1;
		CalDB.Thermodynamic = "New Name Calphad";
		CalDB.save();
		REQUIRE(CalDB.id() > -1);

		DBS_CALPHADDatabase CalDBL(db01, CalDB.id());
		CalDBL.load();
		REQUIRE(CalDBL.id() > -1);
		REQUIRE(CalDBL.IDCase == CalDB.IDCase);
		REQUIRE(std::strcmp(CalDBL.Thermodynamic.c_str(), CalDB.Thermodynamic.c_str()) == 0);

		DBS_Case CaseDB(db01, -1);
		CaseDB.IDProject = 5;
		CaseDB.Script = "SomeScript.name";
		CaseDB.Date = "DD.MM.YYYY HH:mm:ss";
		CaseDB.save();
		REQUIRE(CaseDB.id() > -1);

		DBS_Case CaseDBL(db01, CaseDB.id());
		CaseDBL.load();
		REQUIRE(CaseDBL.id() > -1);
		REQUIRE(CaseDBL.IDProject == CaseDB.IDProject);
		REQUIRE(std::strcmp(CaseDBL.Script.c_str(), CaseDB.Script.c_str()) == 0);
		REQUIRE(std::strcmp(CaseDBL.Date.c_str(), CaseDB.Date.c_str()) == 0);

		DBS_Element ElemDB(db01, -1);
		ElemDB.Name = "New element";
		ElemDB.save();
		REQUIRE(ElemDB.id() > -1);

		DBS_Element ElemDBL(db01, ElemDB.id());
		ElemDBL.load();
		REQUIRE(ElemDBL.id() > -1);
		REQUIRE(std::strcmp(ElemDBL.Name.c_str(), ElemDB.Name.c_str()) == 0);

		DBS_ElementComposition ElemCDB(db01, -1);
		ElemCDB.IDElement = 1;
		ElemCDB.Value = 0.5;
		ElemCDB.TypeComposition = "weight";
		ElemCDB.save();
		REQUIRE(ElemCDB.id() > -1);

		DBS_ElementComposition ElemCDBL(db01, ElemCDB.id());
		ElemCDBL.load();
		REQUIRE(ElemCDBL.id() > -1);
		REQUIRE(ElemCDBL.IDElement == ElemCDB.IDElement);
		REQUIRE(ElemCDBL.Value == ElemCDB.Value);
		REQUIRE(std::strcmp(ElemCDBL.TypeComposition.c_str(),
							ElemCDB.TypeComposition.c_str()) == 0);

		DBS_Phase PhaseDB(db01, -1);
		PhaseDB.Name = "Phasey";
		PhaseDB.save();
		REQUIRE(PhaseDB.id() > -1);

		DBS_Phase PhaseDBL(db01, PhaseDB.id());
		PhaseDBL.load();
		REQUIRE(PhaseDBL.id() > -1);
		REQUIRE(std::strcmp(PhaseDBL.Name.c_str(),
							PhaseDBL.Name.c_str()) == 0);

		DBS_Project ProjectDB(db01, -1);
		ProjectDB.Name = "Phasey";
		ProjectDB.APIName = "matcalc";
		ProjectDB.save();
		REQUIRE(ProjectDB.id() > -1);

		DBS_Project ProjectDBL(db01, ProjectDB.id());
		ProjectDBL.load();
		REQUIRE(ProjectDBL.id() > -1);
		REQUIRE(std::strcmp(ProjectDBL.Name.c_str(),
							ProjectDB.Name.c_str()) == 0);
		REQUIRE(std::strcmp(ProjectDBL.APIName.c_str(),
							ProjectDB.APIName.c_str()) == 0);

		DBS_ScheilConfiguration ScheilConfigDB(db01, -1);
		ScheilConfigDB.DependentPhase = 1;
		ScheilConfigDB.EndTemperature = 100.0;
		ScheilConfigDB.minimumLiquidFraction = 0.005;
		ScheilConfigDB.StartTemperature = 200.00;
		ScheilConfigDB.StepSize = 0.0123;
		ScheilConfigDB.save();
		REQUIRE(ScheilConfigDB.id() > -1);

		DBS_ScheilConfiguration ScheilConfigDBL(db01, ScheilConfigDB.id());
		ScheilConfigDBL.load();
		REQUIRE(ScheilConfigDBL.id() > -1);
		REQUIRE(ScheilConfigDBL.DependentPhase == ScheilConfigDB.DependentPhase);
		REQUIRE(ScheilConfigDBL.EndTemperature == ScheilConfigDB.EndTemperature);
		REQUIRE(ScheilConfigDBL.minimumLiquidFraction == ScheilConfigDB.minimumLiquidFraction);
		REQUIRE(ScheilConfigDBL.StartTemperature == ScheilConfigDB.StartTemperature);
		REQUIRE(ScheilConfigDBL.StepSize == ScheilConfigDB.StepSize);

		DBS_ScheilPhaseFraction ScheilFracDB(db01, -1);
		ScheilFracDB.IDPhase = 1;
		ScheilFracDB.IDCase = 1;
		ScheilFracDB.Value = 0.000000001;
		ScheilFracDB.TypeComposition = "weight";
		ScheilFracDB.save();
		REQUIRE(ScheilFracDB.id() > -1);

		DBS_ScheilPhaseFraction ScheilFracDBL(db01, ScheilFracDB.id());
		ScheilFracDBL.load();
		REQUIRE(ScheilFracDBL.id() > -1);
		REQUIRE(std::strcmp(ScheilFracDB.TypeComposition.c_str(),
			ScheilFracDBL.TypeComposition.c_str()) == 0);
		REQUIRE(ScheilFracDBL.IDPhase == ScheilFracDB.IDPhase);
		REQUIRE(ScheilFracDBL.IDCase == ScheilFracDB.IDCase);
		REQUIRE(ScheilFracDBL.Value == ScheilFracDB.Value);


	}

}
