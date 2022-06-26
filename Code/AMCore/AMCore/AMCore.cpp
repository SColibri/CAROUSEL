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
	CHAR chBuf[BUFSIZE]{'\0'};
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
	config01.set_working_directory("C:/Users/drogo/Desktop/Homless");
	config01.set_ThermodynamicDatabase_path("C:/Users/drogo/Documents/MatCalcUserData/database/thermodynamic/ME-Al1.2.tdb");
	//config01.load();

	API_controll api01(config01);
	config01.add_observer((IAM_Observer*) &api01);

	HelpOptions Options(argc, argv);
	if (Options.get_help() == "TRUE") { Options.Show_help(); }
	else if (Options.get_terminal() == "EMPTY")
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

	

	return 0;
}
