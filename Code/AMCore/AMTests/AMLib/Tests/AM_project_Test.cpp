#include <catch2/catch_test_macros.hpp>

#include "../../../AMLib/include/AM_Project.h"
#include "../../../AMLib/include/AM_Config.h"
#include "../../../AMLib/include/AM_Database_Framework.h"
#include "../../../AMLib/include/Database_implementations/Database_Factory.h"

namespace main_setup
{
	static AM_Config configuration;
	static AM_Database_Framework* _dbF{nullptr};
	static IAM_Database* _db {nullptr};

	static void init() 
	{
		configuration.set_working_directory("");
		Database_Factory::set_schema("projectDB");

		//Create a new database file
		std::string filename = configuration.get_directory_path(AM_FileManagement::FILEPATH::DATABASE) + "\\" +
			Database_Factory::get_schema() + ".db";
		if (std::filesystem::exists(filename))
			std::filesystem::remove(std::filesystem::path(filename));

		_dbF = new AM_Database_Framework(&configuration);
		_db = _dbF->get_database();
	}
}

TEST_CASE("AM_project", "[classic]")
{
	// initialize all variables needed for this test case
	SECTION("Test initialize")
	{
		main_setup::init();
		REQUIRE(main_setup::_db != nullptr);
	}
	
	// create an object of type project
	SECTION("Constructor")
	{
		// Test project creation
		AM_Project project(main_setup::_db, &main_setup::configuration, -1);
		REQUIRE(project.get_project_ID() == -1);

		//Test create new project
		project.set_project_name("projecty", main_setup::configuration.get_api_path(), main_setup::configuration.get_apiExternal_path());
		REQUIRE(project.get_project_ID() != -1);
	}

	// Test project loading related data
	SECTION("Loading project")
	{
		//Test load by ID
		AM_Project tempProject(main_setup::_db, &main_setup::configuration, 1);
		REQUIRE(tempProject.get_project_ID() == 1);
		REQUIRE(std::strcmp(tempProject.get_project_name().c_str(), "projecty") == 0);

		//Test load by name
		AM_Project tempProject_2(main_setup::_db, &main_setup::configuration, "projecty");
		REQUIRE(tempProject_2.get_project_ID() == 1);
		REQUIRE(std::strcmp(tempProject_2.get_project_name().c_str(), "projecty") == 0);
	}

	// select elements that define the project setup
	SECTION("Set project elements")
	{
		AM_Project tempProject(main_setup::_db, &main_setup::configuration, 1);
		
		//add elements into the database as mock
		DBS_Element tempElements(main_setup::_db,-1);
		tempElements.Name = "AL"; 
		tempElements.save();
		tempElements.set_id(-1);

		tempElements.Name = "ZR";
		tempElements.save();
		tempElements.set_id(-1);

		tempElements.Name = "MN";
		tempElements.save();
		tempElements.set_id(-1);

		//adding an invalid element
		std::string outAddElements_invalid = tempProject.set_selected_elements_ByName(std::vector<std::string>{"ALU", "zr", "mN"});
		REQUIRE(tempProject.get_selected_elements_ByName().size() == 0);

		// adding a valid elements
		std::string outAddElements = tempProject.set_selected_elements_ByName(std::vector<std::string>{"AL","zr","mN"});
		REQUIRE(tempProject.get_selected_elements_ByName().size() == 3);

	}

	// select elements that define the project setup
	SECTION("Set reference element")
	{
		AM_Project tempProject(main_setup::_db, &main_setup::configuration, 1);

		// set reference element by name, valid
		int outAddElements = tempProject.set_reference_element("al");
		REQUIRE(outAddElements == 0);

		// set invalid reference by name
		outAddElements = tempProject.set_reference_element("ALU");
		REQUIRE(outAddElements != 0);
		REQUIRE(tempProject.get_reference_element_ByName().compare("AL") == 0);

		// set valid reference by name
		outAddElements = tempProject.set_reference_element("zR");
		REQUIRE(outAddElements == 0);

		// set valid reference by name
		outAddElements = tempProject.set_reference_element("mN");
		REQUIRE(outAddElements == 0);

	}

	// Check if template is created correctly
	SECTION("Create case template")
	{
		AM_Project tempProject(main_setup::_db, &main_setup::configuration, 1);
		tempProject.create_case_template("Test Case template");
		REQUIRE(tempProject.get_case_template() != nullptr);
		REQUIRE(tempProject.get_case_template()->get_CaseName().compare("Test Case template") == 0);
	}

	// select phases for a template by Name
	SECTION("set selected phases in template case")
	{
		AM_Project tempProject(main_setup::_db, &main_setup::configuration, 1);

		//add phases into thhe database as mock
		DBS_Phase tempPhase(main_setup::_db, -1);
		tempPhase.Name = "LIQUID";
		tempPhase.save();
		tempPhase.set_id(-1);

		tempPhase.Name = "FCC";
		tempPhase.save();
		tempPhase.set_id(-1);

		tempPhase.Name = "BCC";
		tempPhase.save();
		tempPhase.set_id(-1);

		// add phases
		tempProject.create_case_template("Test Case template");
		tempProject.get_case_template()->select_phases(std::vector<std::string>{"LIQUID","fcc","BcC"});
		REQUIRE(tempProject.get_case_template()->get_selected_phases_ByID().size() == 3);
		REQUIRE(tempProject.get_case_template()->get_selected_phases_ByName().size() == 3);
		REQUIRE(tempProject.get_case_template()->get_selected_phases_ByName()[0].compare("LIQUID") == 0);
		REQUIRE(tempProject.get_case_template()->get_selected_phases_ByName()[1].compare("FCC") == 0);
		REQUIRE(tempProject.get_case_template()->get_selected_phases_ByName()[2].compare("BCC") == 0);

		//invalid phase
		tempProject.get_case_template()->select_phases(std::vector<std::string>{"NOT a phase"});
		REQUIRE(tempProject.get_case_template()->get_selected_phases_ByID().size() == 0);
	}

	// create cases based on the case template varying the concentrations
	// of selected elements. also test for undefined elements
	SECTION("Create variant by composition using template")
	{
		AM_Project tempProject(main_setup::_db, &main_setup::configuration, 1);

		// add phases
		tempProject.create_case_template("Test Case template");
		tempProject.get_case_template()->select_phases(std::vector<std::string>{"LIQUID", "FCC", "BCC"});
		
		// create cases by varying composition
		tempProject.create_cases_vary_concentration(std::vector<int> {1}, 
													std::vector<double>{10.0},
													std::vector<double>{10.0},
													std::vector<double>{90.0, 5.0, 5.0});
		REQUIRE(tempProject.get_singlePixel_Cases().size() == 10);

		// create cases by varying composition
		tempProject.create_cases_vary_concentration(std::vector<int> {1,2,3},
													std::vector<double>{10.0, 10.0, 10.0},
													std::vector<double>{10.0, 5.0, 2.0},
													std::vector<double>{90.0, 5.0, 5.0});
		REQUIRE(tempProject.get_singlePixel_Cases().size() == 110);

	}

	// should load all cases related to the project
	SECTION("Load project with cases")
	{
		AM_Project tempProject(main_setup::_db, &main_setup::configuration, 1);
		REQUIRE(tempProject.get_singlePixel_Cases().size() == 110);
		REQUIRE(tempProject.get_pixelCase(1) != nullptr);
		REQUIRE(tempProject.get_pixelCase(1)->get_caseID() == 1);
	}

	// Changing the selected elements on a project should
	// remove all cases stored in it.
	SECTION("Select new elements in project")
	{
		// invalid elements should not remove all data
		AM_Project tempProject(main_setup::_db, &main_setup::configuration, 1);
		tempProject.set_selected_elements_ByName(std::vector<std::string>{"AL", "NMH"});
		REQUIRE(tempProject.get_singlePixel_Cases().size() == 110);
		
		// valid elements remove all cases and project object has to get updated
		tempProject.set_selected_elements_ByName(std::vector<std::string>{"al", "Zr"});
		REQUIRE(tempProject.get_singlePixel_Cases().size() == 0);

	}

}
