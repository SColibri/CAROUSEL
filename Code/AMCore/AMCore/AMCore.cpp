﻿// AMCore.cpp : Defines the entry point for the application.
//
#include "AMCore.h"
#include "../AMLib/include/AM_Config.h"
#include "../AMLib/include/AM_FileManagement.h"
#include "../AMLib/include/AM_Database.h"
#include <iostream>
#include <fstream>
#include <string>
#include <Windows.h>
#include <vector>

extern "C" {
// #include "API/matcalc/include/dll_import.h"

}

using namespace std;

struct Test {
	int TesInt{ 1 };
	std::string Name{ "" };
};

typedef double(__cdecl* MYPROC)();
typedef void(__cdecl* MYPROC_2)();
typedef bool(__cdecl* MYPROC_3)(bool);
typedef bool(__cdecl* MYPROC_4)(char*);

extern "C" double MCC_GetTemperature(HINSTANCE hLib) {
	MYPROC ProcAdd = (MYPROC)GetProcAddress(hLib, "MCC_GetTemperature");

	if (NULL != ProcAdd)
	{
		return ProcAdd();
	}

	return 0.0;
}

extern "C" void MCC_ListLicenseFiles(HINSTANCE hLib) {
	MYPROC_2 ProcAdd = (MYPROC_2)GetProcAddress(hLib, "MCC_PrintNodeAndUserId");
	if (NULL != ProcAdd)
	{
		ProcAdd();
	}
	else {
		cout << "This was not found." << endl;
	}
	
}

extern "C" void MCC_LicenseValid(HINSTANCE hLib) {
	MYPROC_3 ProcAdd = (MYPROC_3)GetProcAddress(hLib, "MCC_LicenseValid");
	if (NULL != ProcAdd)
	{
		cout << "The license is: " << ProcAdd(false) << " - This is true: " << true << "\n";
	}
	else {
		cout << "This was not found." << endl;
	}
}

extern "C" void MCC_ScriptRead(HINSTANCE hLib) {
	MYPROC_4 ProcAdd = (MYPROC_4)GetProcAddress(hLib, "MCCOL_ProcessCommandLineInput");
	if (NULL != ProcAdd)
	{
		std::string filename = "D:/Documents/matcalc/T13.mcs";
		int textLength(filename.length());
		char* filenameChar = NULL; 
		filenameChar = "new-workspace save-workspace \"D:\\Documents\\matcalc\\test.mcw\""; //new char[textLength + 1];
		//strcpy(filenameChar, filename.c_str());

		ProcAdd(filenameChar);
		cout << "The license is: did the script run? - This is true: " << true << "\n";

		//delete[] filenameChar;
	}
	else {
		cout << "This was not found." << endl;
	}
}

// C:/Program Files/MatCalc 6/mc_core.dll
int main(int argc, char* argv[])
{
	HINSTANCE hinstLib;
	hinstLib = LoadLibrary(TEXT("C:/Program Files/MatCalc 6/mc_core.dll"));

	if (hinstLib == NULL) {
		cout << "Library was not loaded." << endl;
	}
	else {
		cout << "Library was loaded. Yupee" << endl;

		double Testy = MCC_GetTemperature(hinstLib);

		if (Testy != NULL) {
			cout << " Tempy: Got the core " << Testy << endl;
		}

		MCC_ListLicenseFiles(hinstLib);
		MCC_LicenseValid(hinstLib);
		MCC_ScriptRead(hinstLib);
	}

	cout << "Hello CMake." << endl;
	
	AM_Config config01;

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
	return 0;
}
