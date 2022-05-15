#pragma once
// AMCore.cpp : Defines the entry point for the application.
#include <iostream>
#include <fstream>
#include <string>
#include <Windows.h>
#include <vector>


#include "AMCore.h"
#include "../AMLib/include/AM_Config.h"
#include "../AMLib/include/AM_FileManagement.h"
#include "../AMLib/include/AM_Database.h"

#include "include/MenuOption_Main.h"
#include "include/HelpOptions.h"
#include "../AMLib/interfaces/IAM_API.h"
#include "include/API_controll.h"
#include "../AMLib/include/AM_lua_interpreter.h"


using namespace std;

int main(int argc, char* argv[])
{
	AM_Config config01;
	config01.set_apiExternal_path("C:/Program Files/MatCalc 6/mc_core.dll");

	API_controll api01(config01);

	HelpOptions Options(argc, argv);
	if (Options.get_help() == "TRUE") { Options.Show_help(); }
	else if (Options.get_terminal() == "TRUE")
	{
		system("cls");
		MenuOption_Main menuOptionMain;
		menuOptionMain.load_available_commands(api01.get_implementation()->get_declared_functions());
		menuOptionMain.init();
	}
	else if (Options.get_configuration() != "EMPTY")
	{
		system("cls");
		config01.load(Options.get_configuration());
		config01.save();
		std::string out = "configuration has been updated";

		MenuOption_Main menuOptionMain;
		menuOptionMain.load_available_commands(api01.get_implementation()->get_declared_functions());
		menuOptionMain.set_output(out);
		menuOptionMain.init();
	}
	else if (Options.get_luafile() != "EMPTY")
	{
		system("cls");
		std::string out = "This is some output that the script returned";//api01.get_implementation()->run_lua_script(Options.get_luafile());
		MenuOption_Main menuOptionMain;
		menuOptionMain.set_output(out);
		menuOptionMain.init();
	}
	else if (Options.get_script() != "EMPTY")
	{
		system("cls");
		
		if(std::filesystem::exists(Options.get_script()))
		{
			std::vector<std::string> parameters_{ Options.get_script() };
			std::string out = api01.get_implementation()->run_lua_command("run_script", parameters_);
			MenuOption_Main menuOptionMain;
			menuOptionMain.set_output(out);
			menuOptionMain.init();
		}
		else
		{
			Options.set_error("File was not found!");
			Options.Show_help();
		}
		
	}

	
	std::vector<std::string> vecpar{ "C:/Users/drogo/Desktop/Homless/GM02_main_loop.mcs" };
	std::vector<std::string> runCom{ "open-thermodyn-database C:/Users/drogo/Documents/MatCalcUserData/database/thermodynamic/ME-Al1.2.tdb" };
	std::vector<std::string> runCom2{ "exit" };
	api01.get_implementation()->helloApi();
	std::string outHere = api01.get_implementation()->run_lua_command("hello_world");
	std::string outHereother = ""; // api01.get_implementation()->run_lua_command("run_script", vecpar);
	std::string mcrThing = api01.get_implementation()->run_lua_command("run_command", runCom);
	std::cout << "Lua is doing this: " << outHere << " and this too? " << outHereother << " and mcd: " << mcrThing << endl;

	std::ofstream class_file;
	std::string Save01 = config01.get_save_string();

	class_file.open("testFile.txt");
	class_file << Save01;
	class_file.close();

	std::ifstream filey;
	filey.open("testFile.txt");
	AM_Config config02;
	config02.load_string(filey);

	char Namey[] = "Test";


	AM_Database db01;
	db01.connect("newTestOk.db");
	
	AM_Database_TableStruct TB01;
	TB01.tableName = "Compound_01";

	TB01.columnNames.push_back("column_01");
	TB01.columnNames.push_back("column_02");
	TB01.columnNames.push_back("column_03");
	TB01.columnNames.push_back("column_04");

	TB01.columnDataType.push_back("CHAR[150]");
	TB01.columnDataType.push_back("CHAR[150]");
	TB01.columnDataType.push_back("CHAR[150]");
	TB01.columnDataType.push_back("CHAR[150]");

	db01.add_table(&TB01);

	std::vector<std::string> TBNames = db01.get_tableNames();

	for each (std::string Table in TBNames)
	{
		cout << "Table content: " << Table << "\n";
	}

	cout << "Save is done " << endl;

	//MenuOption_Main MO;
	//MO.init();

	// HelpOptions HO;

	return 0;
}
