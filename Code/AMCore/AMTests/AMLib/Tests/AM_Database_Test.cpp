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
		std::string filename = config01.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "\\" +
			Database_Factory::get_schema() + ".db";
		std::filesystem::remove(std::filesystem::path(filename));

		AM_Database_Framework FM01(&config01);

		IAM_Database* db01 = (IAM_Database*) new Database_Sqlite3(&config01);
		REQUIRE(db01->connect() == 0);


		DBS_CALPHADDatabase CalDB(db01, -1);
		CalDB.IDProject = 1;
		CalDB.Thermodynamic = "New Name Calphad";
		CalDB.save();
		REQUIRE(CalDB.id() > -1);

		DBS_CALPHADDatabase CalDBL(db01, CalDB.id());
		CalDBL.load();
		REQUIRE(CalDBL.id() > -1);
		REQUIRE(CalDBL.IDProject == CalDB.IDProject);
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

		DBS_PrecipitationPhase precipPhase(db01, -1);
		precipPhase.IDCase = 1;
		precipPhase.IDPhase = 1;
		precipPhase.Name = "_P0";
		precipPhase.save();
		REQUIRE(precipPhase.id() > -1);

		DBS_PrecipitationPhase precipPhaseDBL(db01, precipPhase.id());
		precipPhaseDBL.load();
		REQUIRE(precipPhaseDBL.id() == precipPhase.id());
		REQUIRE(precipPhaseDBL.IDCase == 1);
		REQUIRE(precipPhaseDBL.IDPhase == 1);
		REQUIRE(precipPhaseDBL.Name.compare("_P0") == 0);
		precipPhaseDBL.set_id(-1);
		precipPhaseDBL.save();

		AM_Database_Datatable Pecipph(db01, &AMLIB::TN_PrecipitationPhase());
		Pecipph.load_data("Name = \'" + precipPhaseDBL.Name + "\'");
		REQUIRE(Pecipph.row_count() == 1);


		DBS_PrecipitationDomain PD(db01, -1);
		PD.ConsiderExVa = 1;
		PD.EquilibriumDiDe = 1;
		PD.ExcessVacancyEfficiency = 1;
		PD.IDCase = 1;
		PD.InitialGrainDiameter = 0.00000001;
		PD.Name = "DomainName";
		PD.VacancyEvolutionModel = "EvolModel";
		PD.save();
		REQUIRE(PD.id() > -1);

		DBS_PrecipitationDomain PD_load(db01, PD.id());
		PD_load.load();
		REQUIRE(PD_load.id() == PD.id());
		REQUIRE(PD_load.ConsiderExVa == PD.ConsiderExVa);
		REQUIRE(PD_load.EquilibriumDiDe == PD.EquilibriumDiDe);
		REQUIRE(PD_load.ExcessVacancyEfficiency == PD.ExcessVacancyEfficiency);
		REQUIRE(PD_load.IDCase == PD.IDCase);
		REQUIRE(PD_load.InitialGrainDiameter == PD.InitialGrainDiameter);
		REQUIRE(PD_load.Name.compare(PD.Name) == 0);
		REQUIRE(PD_load.VacancyEvolutionModel.compare(PD.VacancyEvolutionModel) == 0);
		PD_load.set_id(-1);
		PD_load.save();

		AM_Database_Datatable PecipDom(db01, &AMLIB::TN_PrecipitationDomain());
		PecipDom.load_data("Name = \'" + PD.Name + "\'");
		REQUIRE(PecipDom.row_count() == 1);

		DBS_HeatTreatment HT(db01, -1);
		HT.Name = "HT01";
		HT.IDCase = 151;
		HT.IDPrecipitationDomain = 1;
		HT.MaxTemperatureStep = 25;
		HT.StartTemperature = 356.7;
		HT.save();
		REQUIRE(HT.Name.compare("HT01") == 0);
		REQUIRE(HT.id() > -1);

		DBS_HeatTreatment HT_load(db01, HT.id());
		HT_load.load();
		REQUIRE(HT_load.id() > -1);
		REQUIRE(HT_load.Name.compare("HT01") == 0);
		REQUIRE(HT_load.IDPrecipitationDomain == HT.IDPrecipitationDomain);
		REQUIRE(HT_load.MaxTemperatureStep == HT.MaxTemperatureStep);
		REQUIRE(HT_load.StartTemperature == HT.StartTemperature);

		DBS_HeatTreatment HT_loadBN(db01, HT.id());
		HT_loadBN.load_by_name("HT01");
		REQUIRE(HT_loadBN.id() > -1);
		REQUIRE(HT_loadBN.Name.compare("HT01") == 0);
		REQUIRE(HT_loadBN.IDPrecipitationDomain == HT.IDPrecipitationDomain);
		REQUIRE(HT_loadBN.MaxTemperatureStep == HT.MaxTemperatureStep);
		REQUIRE(HT_loadBN.StartTemperature == HT.StartTemperature);

		DBS_HeatTreatment HT_New(db01, -1);
		HT_New.Name = "HT01";
		HT_New.IDCase = 151;
		HT_New.save();

		AM_Database_Datatable HeatTreat(db01, &AMLIB::TN_HeatTreatment());
		HeatTreat.load_data("Name = \'HT01\'");
		REQUIRE(HeatTreat.row_count() == 1);

		DBS_HeatTreatmentSegment HS(db01, -1);
		HS.Duration = 60 * 60 * 6;
		HS.EndTemperature = 25;
		HS.stepIndex = 1;
		HS.save();
		REQUIRE(HS.id() > -1);

		DBS_HeatTreatmentSegment HS_load(db01, HS.id());
		HS_load.load();
		REQUIRE(HS_load.Duration == HS.Duration);
		REQUIRE(HS_load.EndTemperature == HS.EndTemperature);
		REQUIRE(HS_load.stepIndex == HS.stepIndex);

		DBS_PrecipitateSimulationData PPF(db01, -1);
		PPF.IDPrecipitationPhase = 1;
		PPF.PhaseFraction = 0.0000001;
		PPF.MeanRadius = 0.0000001;
		PPF.NumberDensity = 0.0000001;
		PPF.Time = 1e-10;
		PPF.save();
		REQUIRE(PPF.id() > -1);

		DBS_PrecipitateSimulationData PPF_load(db01, PPF.id());
		PPF_load.load();
		REQUIRE(PPF_load.id() > -1);
		REQUIRE(PPF_load.IDPrecipitationPhase == PPF.IDPrecipitationPhase);
		REQUIRE(PPF_load.PhaseFraction == PPF.PhaseFraction);
		REQUIRE(PPF_load.MeanRadius == PPF.MeanRadius);
		REQUIRE(PPF_load.NumberDensity == PPF.NumberDensity);
		REQUIRE(PPF_load.Time == PPF.Time);

		DBS_HeatTreatmentProfile HP(db01, -1);
		HP.IDHeatTreatment = 1;
		HP.Temperature = 350.0019;
		HP.Time = 0.000001;
		HP.save();
		REQUIRE(HP.id() > -1);

		DBS_HeatTreatmentProfile HP_load(db01, HP.id());
		HP_load.load();
		REQUIRE(HP_load.id() == HP.id());
		REQUIRE(HP_load.IDHeatTreatment == HP.IDHeatTreatment);
		REQUIRE(HP_load.Temperature == HP.Temperature);
		REQUIRE(HP_load.Time == HP.Time);



	}

}
