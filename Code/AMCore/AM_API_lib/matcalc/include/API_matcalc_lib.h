#pragma once

#include <Windows.h>
#include <string>
#include <filesystem>
#include <fstream>
#include <iostream>
#include <thread>
#include "../../../AMLib/include/AM_Config.h"
#include "API_matcalc_definitions.h"

/** \addtogroup AM_API_lib
  *  @{
  */

#pragma region FunctionTypes
typedef int(__cdecl* AM_CONSTCHAR_FUNC)(const char*);
typedef int(__cdecl* AM_CHAR_FUNC)(char*);
typedef int(__cdecl* AM_BOOL_FUNC)(bool);
typedef char* (__cdecl* AM_RETUR_CHAR_FUNC)();

#pragma endregion


/// <summary>
/// This class handles the dynamic connection with the library and
/// also maps the needed functions.
/// </summary>
class API_matcalc_lib
{
public:

	API_matcalc_lib(AM_Config* configuration);
	~API_matcalc_lib();
	
	/// <summary>
	/// loads external matcalc library
	/// </summary>
	/// <param name="configuration"></param>
	void load_dll(AM_Config* configuration);

	/// <summary>
	/// Use matcalc API for reading scripts
	/// </summary>
	/// <param name="script_filename"></param>
	/// <returns></returns>
	int MCC_script_read(std::string script_filename);

	/// <summary>
	/// Use matcalc executable to run scripts
	/// </summary>
	/// <param name="script_filename"></param>
	/// <returns></returns>
	int MCCEXE_script_read(std::string script_filename);

	/// <summary>
	/// send a command to mcc console using mcc.exe and
	/// mcr.exe that matcalc provides.
	/// </summary>
	/// <param name="commandLine"></param>
	/// <returns></returns>
	std::string MCRCommand(std::string commandLine);

private:
	HINSTANCE _library{NULL}; // Matcalc library
	AM_Config* _configuration{nullptr}; // configuration file
	bool _mccSokcet_active{false}; // MCR commuincation needs mcc socket communication
	std::thread _sockThread;
	int _mccPort{7890};

	/// <summary>
	/// call mcc.exe to run mcs scripts
	/// </summary>
	/// <param name="script_filename"></param>
	/// <returns></returns>
	int run_mcc_executable(std::string script_filename);

	/// <summary>
	/// set-up socket communication with mcc defined on port=_mccPort
	/// Async
	/// </summary>
	/// <param name="portNumber"></param>
	void start_mcc_socket_async()
	{
		if (_mccSokcet_active) return;
		_mccSokcet_active = true;

		memset(&si, 0, sizeof(si));
		memset(&pi, 0, sizeof(pi));

		std::filesystem::path mccPath(_configuration->get_apiExternal_path());
		std::string run_file = mccPath.parent_path().string() + "/mcc.exe -o " + std::to_string(_mccPort);
		if (CreateProcessA(0, run_file.data(), 0, 0, 0, CREATE_NO_WINDOW, 0, 0, &si, &pi))
		{
			_sockThread = std::thread([this] {this->mcc_socketHandles(); });
		}
		std::this_thread::sleep_for(std::chrono::milliseconds(250)); // wait for mcc to finish set-up

	}

	STARTUPINFO si;
	PROCESS_INFORMATION pi;
	DWORD exitCode = -1;
	/// <summary>
	/// set-up socket communication with mcc defined on port=_mccPort
	/// </summary>
	/// <param name="portNumber"></param>
	void mcc_socketHandles()
	{
		WaitForSingleObject(pi.hProcess, INFINITE);
		CloseHandle(pi.hProcess);
		CloseHandle(pi.hThread);
		GetExitCodeProcess(pi.hProcess, &exitCode);
		_mccSokcet_active = false;
	}

	/// <summary>
	/// closes socket and waits for the console thread to finish.
	/// </summary>
	void dispose()
	{
		if (_mccSokcet_active)
		{	
			while(_mccSokcet_active)
			{
				// for some reason the exit command does not
				// always exit at the first try, matcalc pipe errors?
				MCRCommand("exit");
			}
			
			_sockThread.join();
		}
	}
};
/** @}*/


