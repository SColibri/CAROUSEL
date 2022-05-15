#include "../include/API_matcalc_lib.h"

#pragma region Cons_des
API_matcalc_lib::API_matcalc_lib(AM_Config* configuration)
{
	_configuration = configuration;
	this->load_dll(configuration);
};

API_matcalc_lib::~API_matcalc_lib()
{
	// close mcc socket
	dispose();
}
#pragma endregion

#pragma region Methods
void API_matcalc_lib::load_dll(AM_Config* configuration)
{
	// get the dll source folder
	std::filesystem::path fileP = configuration->get_apiExternal_path();

	// Add path of dll to the searching patterns
	SetDllDirectoryA(TEXT(fileP.parent_path().string().c_str()));

	// load library
	_library = LoadLibrary(TEXT(configuration->get_apiExternal_path().c_str()));

	AM_MCC_LIB_LOAD_ALL(_library);
}

int API_matcalc_lib::MCC_script_read(std::string script_filename)
{
	if (NULL == _library ||
		std::filesystem::exists(script_filename) == false) {
		return -1;
	}

	// TODO: if matcalc answers about the API error continue this path.
	return run_mcc_executable(script_filename);
}

int API_matcalc_lib::MCCEXE_script_read(std::string script_filename)
{
	if (std::filesystem::exists(script_filename) == false) {
		return -1;
	}

	return run_mcc_executable(script_filename);
}

int API_matcalc_lib::run_mcc_executable(std::string script_filename)
{
	STARTUPINFO si;
	PROCESS_INFORMATION pi;
	DWORD exitCode = -1;

	memset(&si, 0, sizeof(si));
	memset(&pi, 0, sizeof(pi));

	std::string run_file = "C:/Program Files/MatCalc 6/mcc.exe -x " + script_filename;
	if (CreateProcessA(0, run_file.data(), 0, 0, 0, 0, 0, 0, &si, &pi))
	{
		WaitForSingleObject(pi.hProcess, INFINITE);
		CloseHandle(pi.hProcess);
		CloseHandle(pi.hThread);
		GetExitCodeProcess(pi.hProcess, &exitCode);
	}

	return exitCode;
}

std::string API_matcalc_lib::MCRCommand(std::string commandLine)
{
	if (!_mccSokcet_active) start_mcc_socket_async();

	STARTUPINFO si;
	PROCESS_INFORMATION pi;
	DWORD exitCode = -1;
	std::string out{"MCRCommand did not output anything"};

	memset(&si, 0, sizeof(si));
	memset(&pi, 0, sizeof(pi));

	std::string run_file = "C:/Program Files/MatCalc 6/mcr.exe " + std::to_string(_mccPort) + " " + commandLine;
	if (CreateProcessA(0, run_file.data(), 0, 0, 0, 0, 0, 0, &si, &pi))
	{
		WaitForSingleObject(pi.hProcess, INFINITE);
		CloseHandle(pi.hProcess);
		CloseHandle(pi.hThread);
		GetExitCodeProcess(pi.hProcess, &exitCode);
	}

	return out;
}
#pragma endregion

#pragma region JunkCode
/*
#pragma region Lib_Functions
extern "C" static int AM_MCC_ScriptRead(HINSTANCE hLib, std::string script_filename) {

	AM_CHAR_FUNC run_script = (AM_CHAR_FUNC)GetProcAddress(hLib, "MCCOL_ProcessCommandLineInput");
	AM_CHAR_FUNC thread_run = (AM_CHAR_FUNC)GetProcAddress(hLib, "MCCOL_SetAllowThreads");
	AM_BOOL_FUNC num_elements = (AM_BOOL_FUNC)GetProcAddress(hLib, "MCC_GetNumDBElements");
	AM_BOOL_FUNC initializeK = (AM_BOOL_FUNC)GetProcAddress(hLib, "MCC_InitializeKernelPathConst");
	AM_CHAR_FUNC set_app_dir = (AM_CHAR_FUNC)GetProcAddress(hLib, "MCC_SetApplicationDirectoryChar");
	AM_CONSTCHAR_FUNC set_working_dir = (AM_CONSTCHAR_FUNC)GetProcAddress(hLib, "MCC_SetWorkingDirectoryChar");
	AM_RETUR_CHAR_FUNC get_working_dir = (AM_RETUR_CHAR_FUNC)GetProcAddress(hLib, "MCC_GetWorkingDirectoryChar");

	if (NULL != run_script && NULL != set_working_dir)
	{
		int thr_run = thread_run(0);
		bool response = initializeK(false);
		std::string workDir = std::filesystem::path(script_filename.c_str()).parent_path().string() + "/";
		set_working_dir(workDir.c_str());
		int resp = set_app_dir("C:/Users/drogo/Documents/MatCalcUserData");

		char* workDirInMC = get_working_dir();
		std::fstream fs;
		fs.open(script_filename);


		std::string fileContent;
		while (!fs.eof())
		{
			std::getline(fs, fileContent);
			int resInt = run_script(fileContent.data());

			fileContent.clear();
		}

		std::string scriptCommand = "run-script-file C:/Users/drogo/Desktop/Homless/NewScript.mcs ";
		int test = run_script(scriptCommand.data());

		char* scriptChar = fileContent.data();

		//int test = run_script(scriptChar);
		workDirInMC = get_working_dir();

		return 0;
	}
	else {
		throw "Error: matcalc library not available!";
	}
}

//char* licenseInfo = MCC_GetLicenseInformation(false);
		//MCC_ListLicenses();
		std::string dblic("license_mc6_tu_muenchen.mcl");
		std::string dbname("C:/Program Files/MatCalc 6/database/thermodynamic/mc_al.tdb");
		//std::string dbname("C:/Users/drogo/Documents/MatCalcUserData/database/thermodynamic/ME-Al1.2.tdb");
		std::string strnji("Al");
		char* const test = "C:/Users/drogo/Desktop/Homless/GM02_main_loop.mcs";
		bool wdir = MCC_SetWorkingDirectoryChar("C:/Users/drogo/Desktop/Homless");
		const char* strElement = "Al";
		bool kernelInit = MCC_InitializeKernel(true);
		bool dtbs = MCC_OpenDatabaseFileChar(dbname.data(), DBOC_EQUILIBRIUM, false);
		char* opndb = MCC_GetOpenDatabaseFileChar(DBOC_EQUILIBRIUM);
		char const* const elements[] = { "Al", "Si", "Mg", NULL };
		int stre = MCC_LoadLicenseFile(dblic.data());
		int validL = MCC_LicenseValid(false);
		int stro = MCC_GetElementIndex(elements[0]);
		// int out = run_mcc_executable(script_filename);
		// int testy = MCCOL_RunScriptFileChar(test);

#pragma endregion
*/
#pragma endregion