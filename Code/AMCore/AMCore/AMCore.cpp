// AMCore.cpp : Defines the entry point for the application.
#include "AMCore.h"
#include "../AMLib/include/AM_Config.h"
#include "../AMLib/include/AM_FileManagement.h"
#include "../AMLib/include/AM_Database.h"
#include <iostream>
#include <fstream>
#include <string>
#include <Windows.h>
#include <vector>
#include "include/MenuOption_Main.h"
#include "include/HelpOptions.h"
#include "../AMLib/interfaces/IAM_API.h"
#include "include/API_controll.h"
#include "../AMLib/include/AM_lua_interpreter.h"


using namespace std;
// typedef void (HelpOptions::* Some_fnc_ptr)();

int main(int argc, char* argv[])
{
	AM_Config config01;
	API_controll api01(config01);

	HelpOptions Options(argc, argv);
	if (Options.get_help() == "TRUE") { Options.Show_help(); }
	else if (Options.get_terminal() == "TRUE")
	{
		MenuOption_Main menuOptionMain;
		menuOptionMain.load_available_commands(api01.get_implementation()->get_declared_functions());
		menuOptionMain.init();
	}

	
	api01.get_implementation()->helloApi();
	std::string outHere = api01.get_implementation()->run_lua_command("hello_world");
	std::cout << "Lua is doing this: " << outHere << "" << endl;

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

	HelpOptions HO;

	return 0;
}
