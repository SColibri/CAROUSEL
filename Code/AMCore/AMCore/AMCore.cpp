#pragma once
// AMCore.cpp : Defines the entry point for the application.
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include <iostream>
#include <fstream>
#include <string>
#include <vector>


#include "AMCore.h"
#include "../AMLib/include/AM_Config.h"
#include "../AMLib/include/AM_FileManagement.h"
#include "../AMLib/include/Database_implementations/Database_Sqlite3.h"
#include "../AMLib/interfaces/IAM_Database.h"


#include "include/MenuOption_Main.h"
#include "include/HelpOptions.h"
#include "../AMLib/interfaces/IAM_API.h"
#include "include/API_controll.h"
#include "../AMLib/include/AM_lua_interpreter.h"
#include "../AMLib/include/AM_Database_Framework.h"
#include "../AMLib/include/Database_implementations/Data_stuctures/DBS_Project.h"
#include "../AMLib/include/Database_implementations/Data_stuctures/DBS_Element.h"
#include "../AMLib/include/Database_implementations/Data_stuctures/DBS_ElementComposition.h"
#include "../AMLib/include/AM_Server.h"

using namespace std;

#pragma region testsocket
#include <stdio.h>
#define BUFSIZE 4096 

int ChildNode()
{
	CHAR chBuf[BUFSIZE];
	DWORD dwRead, dwWritten;
	HANDLE hStdin, hStdout;
	BOOL bSuccess;

	hStdout = GetStdHandle(STD_OUTPUT_HANDLE);
	hStdin = GetStdHandle(STD_INPUT_HANDLE);
	if (
		(hStdout == INVALID_HANDLE_VALUE) ||
		(hStdin == INVALID_HANDLE_VALUE)
		)
		ExitProcess(1);

	// Send something to this process's stdout using printf.
	printf("\n ** This is a message from the child process. ** \n");

	// This simple algorithm uses the existence of the pipes to control execution.
	// It relies on the pipe buffers to ensure that no data is lost.
	// Larger applications would use more advanced process control.

	for (;;)
	{
		// Read from standard input and stop on error or no data.
		bSuccess = ReadFile(hStdin, chBuf, BUFSIZE, &dwRead, NULL);

		if (!bSuccess || dwRead == 0)
			break;

		// Write to standard output and stop on error.
		bSuccess = WriteFile(hStdout, chBuf, dwRead, &dwWritten, NULL);

		if (!bSuccess)
			break;
	}
	return 0;
}

#pragma endregion

int main(int argc, char* argv[])
{
	AM_Config config01;
	config01.set_apiExternal_path("C:/Program Files/MatCalc 6/mc_core.dll");

	API_controll api01(config01);
	config01.set_working_directory("C:/Users/drogo/Desktop/Homless");
	config01.set_ThermodynamicDatabase_path("C:/Users/drogo/Documents/MatCalcUserData/database/thermodynamic/ME-Al1.2.tdb");

	HelpOptions Options(argc, argv);
	if (Options.get_help() == "TRUE") { Options.Show_help(); }
	else if (Options.get_terminal() == "FALSE")
	{
		system("cls");
		MenuOption_Main menuOptionMain;
		menuOptionMain.set_luaInterpreter(api01.get_implementation());
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
		std::string out = api01.get_implementation()->run_lua_script(Options.get_luafile());
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
	else if (Options.get_socket() == "EMPTY")
	{
		AM_Server servy(api01.get_implementation());
		servy.init();
	}

	std::string outtest = api01.get_implementation()->run_lua_script("C:/Users/drogo/Desktop/Homless/TestLua.lua");

	std::vector<std::string> vecpar{ "C:/Users/drogo/Desktop/Homless/GM02_main_loop.mcs" };
	std::vector<std::string> runCom{ "open-thermodyn-database C:/Users/drogo/Documents/MatCalcUserData/database/thermodynamic/ME-Al1.2.tdb" };
	std::vector<std::string> runCom2{ "exit" };
	api01.get_implementation()->helloApi();
	//std::string outHere = api01.get_implementation()->run_lua_command("hello_world");
	std::string outHereother = ""; // api01.get_implementation()->run_lua_command("run_script", vecpar);
	//std::string mcrThing = api01.get_implementation()->run_lua_command("run_command", runCom);
	//std::string mcrThing_2 = api01.get_implementation()->run_lua_command("initialize_core");
	//std::cout << "Lua is doing this: " << outHere << " and this too? " << outHereother << " and mcd: " << mcrThing << endl;

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


	AM_Database_Framework amF(&config01);

	IAM_Database* db01 = (IAM_Database*) new Database_Sqlite3(&config01);
	db01 -> connect();
	
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

	db01->add_table(&TB01);

	std::vector<std::string> TBNames = db01->get_tableNames();

	for each (std::string Table in TBNames)
	{
		cout << "Table content: " << Table << "\n";
	}

	cout << "Save is done " << endl;

	DBS_Project NewDB(db01, -1);
	NewDB.APIName = "New name";
	NewDB.Name = "Cool project";
	NewDB.save();

	DBS_Project savedDB(db01, NewDB.id());
	savedDB.load();

	DBS_Element NewDBE(db01, -1);
	NewDBE.Name = "Cool project";
	NewDBE.save();
	NewDBE.load();

	DBS_ElementComposition NewEly(db01, -1);
	NewEly.IDElement = 1;
	NewEly.Value = 0.5;
	NewEly.TypeComposition = "weight";
	NewEly.save();

	DBS_ElementComposition NewElyL(db01, NewEly.id());
	NewElyL.load();

	AM_Database_TableStruct prj = AMLIB::TN_Projects();
	std::vector<std::vector<std::string>> testTable = db01->get_tableRows(&prj);
	bool stopHere{true};
	//MenuOption_Main MO;
	//MO.init();

	// HelpOptions HO;

	return 0;
}
