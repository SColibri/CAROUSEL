#include <catch2/catch_test_macros.hpp>
#include <string>
#include <vector>
#include "../../../include/Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "../../../AMLib/include/AM_pixel_parameters.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/include/AM_Project.h"


namespace main_setup
{
	static AM_Config configuration;
	static AM_Database_Framework* _dbF{ nullptr };
	static IAM_Database* _db{ nullptr };
	static AM_Project* project{ nullptr };

	static void init()
	{
		configuration.set_working_directory("");
		Database_Factory::set_schema("pixelParameter");

		//Create a new database file
		std::string filename = configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "/" +
			Database_Factory::get_schema() + ".db";
		if (std::filesystem::exists(filename))
			std::filesystem::remove(std::filesystem::path(filename));

		_dbF = new AM_Database_Framework(&configuration);
		_db = _dbF->get_database();

		// create new test project
		project = new AM_Project(_db, &configuration, -1);
		project->set_project_name("projecty", configuration.get_api_path());
	}
}

TEST_CASE("AM_pixel_parameters", "[classic]")
{
	// initialize all variables needed for this test case
	SECTION("Test initialize")
	{
		main_setup::init();
		REQUIRE(main_setup::_db != nullptr);

		// Add phases for testing (Mock data)
		DBS_Phase tempPhase(main_setup::_db, -1);
		tempPhase.Name = "LIQUID";
		tempPhase.save();
		REQUIRE(tempPhase.id() != -1);

		tempPhase.set_id(-1);
		tempPhase.Name = "BCC";
		tempPhase.save();
		REQUIRE(tempPhase.id() != -1);

		tempPhase.set_id(-1);
		tempPhase.Name = "FCC";
		tempPhase.save();
		REQUIRE(tempPhase.id() != -1);

		// Add elements
		DBS_Element tempElement(main_setup::_db, -1);
		tempElement.Name = "AL";
		tempElement.save();
		REQUIRE(tempElement.id() != -1);

		tempElement.set_id(-1);
		tempElement.Name = "ZR";
		tempElement.save();
		REQUIRE(tempElement.id() != -1);

		// Add project selected elements
		main_setup::project->set_selected_elements_ByName(std::vector<std::string>{"AL", "ZR"});

	}

	SECTION("create new pixel parameter")
	{
		int IDpixel = AM_pixel_parameters::create_new_pixel(main_setup::_db,
										&main_setup::configuration,
										main_setup::project->get_project_ID(), "Casey");
		DBS_Project tempProject(main_setup::_db, main_setup::project->get_project_ID());
		AM_pixel_parameters pixelP(main_setup::_db, &tempProject, IDpixel);

		REQUIRE(pixelP.get_caseID() != -1);
		REQUIRE(pixelP.get_CaseName().compare("Casey") == 0);
		REQUIRE(pixelP.get_composition_double().size() == main_setup::project->get_selected_elements_ByName().size());
		REQUIRE(pixelP.get_selected_phases_ByName().size() == 0);

		REQUIRE(pixelP.get_calphad()->IDCase == pixelP.get_caseID());
		REQUIRE(pixelP.get_calphad_thermodynamic_database().compare(main_setup::configuration.get_ThermodynamicDatabase_path()) == 0);
		REQUIRE(pixelP.get_calphad_mobility_database().compare(main_setup::configuration.get_MobilityDatabase_path()) == 0);
		REQUIRE(pixelP.get_calphad_physical_database().compare(main_setup::configuration.get_PhysicalDatabase_path()) == 0);

		REQUIRE(pixelP.get_EquilibriumConfiguration()->IDCase == pixelP.get_caseID());

		REQUIRE(pixelP.get_ScheilConfiguration()->IDCase == pixelP.get_caseID());
		REQUIRE(pixelP.get_scheil_config_dependentPhaseName().compare("LIQUID") == 0);
		REQUIRE(pixelP.get_scheil_config_minimumLiquidFraction() == 0.01);
	}

	SECTION("load existing case")
	{
		DBS_Project tempProject(main_setup::_db, main_setup::project->get_project_ID());
		AM_pixel_parameters pixelP(main_setup::_db, &tempProject, 1);
		REQUIRE(pixelP.get_caseID() == 1);	
	}

	SECTION("Set configuration for Equilibrium")
	{
		int IDpixel = AM_pixel_parameters::create_new_pixel(main_setup::_db,
			&main_setup::configuration,
			main_setup::project->get_project_ID(), "Casey");
		DBS_Project tempProject(main_setup::_db, main_setup::project->get_project_ID());
		AM_pixel_parameters pixelP(main_setup::_db, &tempProject, IDpixel);

		// check that values are set
		pixelP.set_equilibrium_config_endTemperature(100);
		pixelP.set_equilibrium_config_startTemperature(10);
		pixelP.set_equilibrium_config_stepSize(25); // TODO: this is not yet implemented, step size is just a dummy function
		
		REQUIRE(pixelP.get_equilibrium_config_endTemperature() == 100);
		REQUIRE(pixelP.get_equilibrium_config_startTemperature() == 10);
		pixelP.save();

		// check that values are saved
		AM_pixel_parameters reloadPixel(main_setup::_db, &tempProject, IDpixel);
		REQUIRE(reloadPixel.get_equilibrium_config_endTemperature() == 100);
		REQUIRE(reloadPixel.get_equilibrium_config_startTemperature() == 10);

		// check that the function load all reloads all data properly
		pixelP.set_equilibrium_config_endTemperature(200);
		pixelP.set_equilibrium_config_startTemperature(20);
		pixelP.save();

		reloadPixel.load_all();
		REQUIRE(reloadPixel.get_equilibrium_config_endTemperature() == 200);
		REQUIRE(reloadPixel.get_equilibrium_config_startTemperature() == 20);
	}

	SECTION("Set configuration for Scheil")
	{
		int IDpixel = AM_pixel_parameters::create_new_pixel(main_setup::_db,
			&main_setup::configuration,
			main_setup::project->get_project_ID(), "Casey");
		DBS_Project tempProject(main_setup::_db, main_setup::project->get_project_ID());
		AM_pixel_parameters pixelP(main_setup::_db, &tempProject, IDpixel);

		// check that values are set
		pixelP.set_scheil_config_startTemperature(100);
		pixelP.set_scheil_config_endTemperature(10);
		pixelP.set_scheil_config_stepSize(25);
		pixelP.set_scheil_config_dependentPhase("LIQUID");
		pixelP.set_scheil_config_minimumLiquidFraction(0.05);

		REQUIRE(pixelP.get_scheil_config_endTemperature() == 10);
		REQUIRE(pixelP.get_scheil_config_startTemperature() == 100);
		REQUIRE(pixelP.get_scheil_config_StepSize() == 25);
		REQUIRE(pixelP.get_scheil_config_minimumLiquidFraction() == 0.05);
		REQUIRE(pixelP.get_scheil_config_dependentPhaseName().compare("LIQUID") == 0);
		pixelP.save();

		// check that values are saved
		AM_pixel_parameters reloadPixel(main_setup::_db, &tempProject, IDpixel);
		REQUIRE(reloadPixel.get_scheil_config_endTemperature() == 10);
		REQUIRE(reloadPixel.get_scheil_config_startTemperature() == 100);
		REQUIRE(reloadPixel.get_scheil_config_StepSize() == 25);
		REQUIRE(reloadPixel.get_scheil_config_minimumLiquidFraction() == 0.05);
		REQUIRE(reloadPixel.get_scheil_config_dependentPhaseName().compare("LIQUID") == 0);

		// check that the function load all reloads all data properly
		pixelP.set_scheil_config_startTemperature(200);
		pixelP.set_scheil_config_endTemperature(20);
		pixelP.set_scheil_config_stepSize(50);
		pixelP.set_scheil_config_dependentPhase("LIQUID");
		pixelP.set_scheil_config_minimumLiquidFraction(0.01);
		pixelP.save();

		reloadPixel.load_all();
		REQUIRE(reloadPixel.get_scheil_config_endTemperature() == 20);
		REQUIRE(reloadPixel.get_scheil_config_startTemperature() == 200);
		REQUIRE(reloadPixel.get_scheil_config_StepSize() == 50);
		REQUIRE(reloadPixel.get_scheil_config_minimumLiquidFraction() == 0.01);
		REQUIRE(reloadPixel.get_scheil_config_dependentPhaseName().compare("LIQUID") == 0);
	}

	SECTION("select phases")
	{
		int IDpixel = AM_pixel_parameters::create_new_pixel(main_setup::_db,
			&main_setup::configuration,
			main_setup::project->get_project_ID(), "Casey");
		DBS_Project tempProject(main_setup::_db, main_setup::project->get_project_ID());
		AM_pixel_parameters pixelP(main_setup::_db, &tempProject, IDpixel);

		// add phases by group
		pixelP.select_phases(std::vector<std::string> {"LIQUID", "BCC", "FCC"});
		REQUIRE(pixelP.get_selected_phases_ByID().size() == 3);

		pixelP.select_phases(std::vector<std::string> {"LIQUID", "BCC", "FCC"});
		REQUIRE(pixelP.get_selected_phases_ByID().size() == 3);

		// add an existing phase
		int responseAdd = pixelP.add_selectedPhase("BCC");
		REQUIRE(responseAdd == 2);

		// remove phase by name
		REQUIRE(pixelP.remove_selectedPhase("LIQUID") == 0);
		REQUIRE(pixelP.get_selected_phases_ByID().size() == 2);

		// check if phase is selected
		int selPhase_1 = pixelP.check_if_phase_is_selected(1);
		int selPhase_2 = pixelP.check_if_phase_is_selected(2);
		REQUIRE(selPhase_1 == 0);
		REQUIRE(selPhase_2 == 1);

		// Remove all phases
		pixelP.remove_all_seleccted_phase();
		REQUIRE(pixelP.get_selected_phases_ByID().size() == 0);

		// add phases
		REQUIRE(pixelP.add_selectedPhase("BCC") == 0);
		REQUIRE(pixelP.add_selectedPhase("FCC") == 0);
		REQUIRE(pixelP.add_selectedPhase("LIQUID") == 0);
		REQUIRE(pixelP.add_selectedPhase("BCC") == 2);
		REQUIRE(pixelP.add_selectedPhase("FCC") == 2);
		REQUIRE(pixelP.add_selectedPhase("LIQUID") == 2);
		REQUIRE(pixelP.get_selected_phases_ByID().size() == 3);
	}

	SECTION("Set composition values")
	{
		int IDpixel = AM_pixel_parameters::create_new_pixel(main_setup::_db,
			&main_setup::configuration,
			main_setup::project->get_project_ID(), "Casey");
		DBS_Project tempProject(main_setup::_db, main_setup::project->get_project_ID());
		AM_pixel_parameters pixelP(main_setup::_db, &tempProject, IDpixel);

		// test for setting the ccomposition to valid and invalid options
		REQUIRE(pixelP.set_composition("AL",90) == 0);
		REQUIRE(pixelP.set_composition("ZR", 5) == 0);
		REQUIRE(pixelP.set_composition("MN", 5) == 1);

		REQUIRE(pixelP.get_composition_double()[0] == 90.0);
		REQUIRE(pixelP.get_composition_double()[1] == 5.0);

		//Save and load
		pixelP.save();

		AM_pixel_parameters savedPixelP(main_setup::_db, &tempProject, IDpixel);
		REQUIRE(savedPixelP.get_composition_double()[0] == 90.0);
		REQUIRE(savedPixelP.get_composition_double()[1] == 5.0);

		REQUIRE(savedPixelP.set_composition("AL", 30) == 0);
		REQUIRE(savedPixelP.set_composition("ZR", 70) == 0);
		REQUIRE(savedPixelP.set_composition("MN", 5) == 1);
		savedPixelP.save();

		// check if it re-loads properly
		pixelP.load_all();
		REQUIRE(pixelP.get_composition_double()[0] == 30.0);
		REQUIRE(pixelP.get_composition_double()[1] == 70.0);
	}
}
