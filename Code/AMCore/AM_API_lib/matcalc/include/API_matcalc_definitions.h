#pragma once
#include <windows.h>
#include "mc_defines.h"
#include "mc_types.h"
#include "shared_types.h"

/** \addtogroup AM_API_lib
  *  @{
  */

#pragma region FunctionTypes
//
//typedef bool(__cdecl* AM_CIB_B_FUNC)(char*, int, bool);
//typedef bool(__cdecl* AM_IB_B_FUNC)(int, bool);
//typedef bool(__cdecl* AM_IBB_B_FUNC)(int, bool, bool);
//typedef bool(__cdecl* AM_B_B_FUNC)(bool);
//typedef bool(__cdecl* AM_CcB_B_FUNC)(const char*, bool);
//typedef bool(__cdecl* AM_CB_B_FUNC)(char*, bool);
//typedef bool(__cdecl* AM_C_B_FUNC)(char*);
//typedef bool(__cdecl* AM__B_FUNC)();
//
//typedef int(__cdecl* AM_B_I_FUNC)(bool);
//typedef int(__cdecl* AM_I_I_FUNC)(int);
//typedef int(__cdecl* AM_C_I_FUNC)(char*);
//typedef int(__cdecl* AM_CD_I_FUNC)(char*, double*);
//typedef int(__cdecl* AM_Cc_I_FUNC)(const char*);
//typedef int(__cdecl* AM_Ccc_I_FUNC)(char const* const);
//typedef int(__cdecl* AM_II_I_FUNC)(int, int);
//typedef int(__cdecl* AM_Iul_I_FUNC)(unsigned long int);
//typedef int(__cdecl* AM__I_FUNC)();
//
//typedef char*(__cdecl* AM_I_C_FUNC)(int);
//typedef char* (__cdecl* AM_B_C_FUNC)(bool);
//typedef char* (__cdecl* AM_CB_C_FUNC)(char*, bool);
//typedef char* (__cdecl* AM__C_FUNC)();
//
//typedef void(__cdecl* AM_I__FUNC)(int);
//typedef void(__cdecl* AM_II__FUNC)(int, int);
//typedef void(__cdecl* AM____FUNC)();
//#pragma endregion
//
//#pragma region Lib_Functions
//
////******************************************************************************
//// Coline ...
////******************************************************************************
//DECL_DLL_EXPORT static AM_II__FUNC MCCOL_UseModul;
//DECL_DLL_EXPORT static AM_C_I_FUNC MCCOL_ProcessCommandLineInputNewColine;
//DECL_DLL_EXPORT static AM_Cc_I_FUNC MCCOL_ProcessCommandLineInputNewColineConst;
//DECL_DLL_EXPORT static AM_C_I_FUNC MCCOL_ProcessCommandLineInput;
//DECL_DLL_EXPORT static AM_Cc_I_FUNC MCCOL_ProcessCommandLineInputConst;
//DECL_DLL_EXPORT static AM_C_I_FUNC MCCOL_RunScriptFileChar;
//DECL_DLL_EXPORT static AM_I_I_FUNC MCCOL_SetAllowThreads;
//DECL_DLL_EXPORT static AM_CD_I_FUNC MCCOL_GetMCCalcVariable;
//DECL_DLL_EXPORT static AM_II_I_FUNC MCC_RedirectInputAndOutput;
//
////******************************************************************************
//// Datenbankfunktionen ...
////******************************************************************************
//DECL_DLL_EXPORT static AM_CIB_B_FUNC MCC_OpenDatabaseFileChar; //(char *FileName, int Type, bool StartAsThread)
//DECL_DLL_EXPORT static AM_I_C_FUNC MCC_GetOpenDatabaseFileChar; // int Type
//DECL_DLL_EXPORT static AM_IB_B_FUNC MCC_SelectDBElement;
//DECL_DLL_EXPORT static AM_IB_B_FUNC MCC_SelectDBPhase;
//DECL_DLL_EXPORT static AM_IBB_B_FUNC MCC_ReadDatabaseFile;
//DECL_DLL_EXPORT static AM__B_FUNC MCC_RemoveDiffusionData;
//DECL_DLL_EXPORT static AM__B_FUNC MCC_RemovePhysicalData;
//
////******************************************************************************
//// Licensing ...
////******************************************************************************
//DECL_DLL_EXPORT static AM__I_FUNC MCC_ReadLicenseInformation;
//DECL_DLL_EXPORT static AM_B_C_FUNC MCC_GetLicenseInformation; // (bool html format)
//DECL_DLL_EXPORT static AM_B_B_FUNC MCC_LicenseValid; // (bool echo_expired)
//DECL_DLL_EXPORT static AM_Ccc_I_FUNC MCC_LoadLicenseFile; // (char const* const license_file_name);
//DECL_DLL_EXPORT static AM_Ccc_I_FUNC MCC_ActivateLicenseFile; // (char const* const license_file_name);
//DECL_DLL_EXPORT static AM_Ccc_I_FUNC MCC_DeactivateLicenseFile; // (char const* const license_file_name);
//DECL_DLL_EXPORT static AM__I_FUNC MCC_RetrieveNetworkLicense;
//DECL_DLL_EXPORT static AM_Iul_I_FUNC MCC_RetrieveNetworkLicenseFrom; // (unsigned long int ipv4_address_as_integer); //< QHostAddress.toInt()!
//DECL_DLL_EXPORT static AM____FUNC MCC_ListLicenseFiles;
//DECL_DLL_EXPORT static AM____FUNC MCC_ListLicenseFilesDetails;
//DECL_DLL_EXPORT static AM____FUNC MCC_ListLicenses;
//DECL_DLL_EXPORT static AM_Ccc_I_FUNC MCC_ListLicensesOf; // (char const* const license_file_name);
//DECL_DLL_EXPORT static AM____FUNC MCC_PrintNodeAndUserId;
//
//
////******************************************************************************
//// Globale Informationen / Variablen ...
////******************************************************************************
//DECL_DLL_EXPORT static AM_Cc_I_FUNC MCC_GetElementIndex;//(const char* element_name);
//
//
////******************************************************************************
//// Initialisierung ...
////******************************************************************************
//
//DECL_DLL_EXPORT static AM_B_B_FUNC MCC_InitializeKernel; // (bool echoVersion = false);
//DECL_DLL_EXPORT static AM_CcB_B_FUNC MCC_InitializeKernelPathConst; // (const char* app_file_path, bool echoVersion = false);
//DECL_DLL_EXPORT static AM_CB_B_FUNC MCC_InitializeKernelPathChar; // (char* application_file_path, bool echoVersion = false);
//DECL_DLL_EXPORT static AM_C_B_FUNC MCC_SetApplicationDirectoryChar; // (char* appl_dir);
//DECL_DLL_EXPORT static AM_C_B_FUNC MCC_SetWorkingDirectoryChar; // (char* work_dir);
//DECL_DLL_EXPORT static AM__C_FUNC MCC_GetApplicationDirectoryChar;
//DECL_DLL_EXPORT static AM__C_FUNC MCC_GetWorkingDirectoryChar;
//DECL_DLL_EXPORT static AM_CB_C_FUNC MCC_InitializeExternalChar; // (char* app_file_path, bool echoVersion = false);
//DECL_DLL_EXPORT static AM_CcB_B_FUNC MCC_InitializeExternalConstChar; // (const char* app_file_path, bool echoVersion = false);
//
////******************************************************************************
//// common ...
////******************************************************************************
//
//extern "C" static void AM_MCC_LIB_LOAD_ALL(HINSTANCE hLib)
//{
//	//******************************************************************************
//	// Coline ...
//	//******************************************************************************
//	MCC_OpenDatabaseFileChar = (AM_CIB_B_FUNC)GetProcAddress(hLib, "MCC_OpenDatabaseFileChar");
//	MCC_GetOpenDatabaseFileChar = (AM_I_C_FUNC)GetProcAddress(hLib, "MCC_GetOpenDatabaseFileChar");
//	MCC_SelectDBElement = (AM_IB_B_FUNC)GetProcAddress(hLib, "MCC_SelectDBElement");
//	MCC_SelectDBPhase = (AM_IB_B_FUNC)GetProcAddress(hLib, "MCC_SelectDBPhase");
//	MCC_ReadDatabaseFile = (AM_IBB_B_FUNC)GetProcAddress(hLib, "MCC_ReadDatabaseFile");
//	MCC_RemoveDiffusionData = (AM__B_FUNC)GetProcAddress(hLib, "MCC_RemoveDiffusionData");
//	MCC_RemovePhysicalData = (AM__B_FUNC)GetProcAddress(hLib, "MCC_RemovePhysicalData");
//
//	//******************************************************************************
//	// Datenbankfunktionen ...
//	//******************************************************************************
//	MCCOL_UseModul = (AM_II__FUNC)GetProcAddress(hLib, "MCCOL_UseModul");
//	MCCOL_ProcessCommandLineInputNewColine = (AM_C_I_FUNC)GetProcAddress(hLib, "MCCOL_ProcessCommandLineInputNewColine");
//	MCCOL_ProcessCommandLineInputNewColineConst = (AM_Cc_I_FUNC)GetProcAddress(hLib, "MCCOL_ProcessCommandLineInputNewColineConst");
//	MCCOL_ProcessCommandLineInput = (AM_C_I_FUNC)GetProcAddress(hLib, "MCCOL_ProcessCommandLineInput");
//	MCCOL_ProcessCommandLineInputConst = (AM_Cc_I_FUNC)GetProcAddress(hLib, "MCCOL_ProcessCommandLineInputConst");
//	MCCOL_RunScriptFileChar = (AM_C_I_FUNC)GetProcAddress(hLib, "MCCOL_RunScriptFileChar");
//	MCCOL_SetAllowThreads = (AM_I_I_FUNC)GetProcAddress(hLib, "MCCOL_SetAllowThreads");
//	MCCOL_GetMCCalcVariable = (AM_CD_I_FUNC)GetProcAddress(hLib, "MCCOL_GetMCCalcVariable");
//	MCC_RedirectInputAndOutput = (AM_II_I_FUNC)GetProcAddress(hLib, "MCC_RedirectInputAndOutput");
//
//	//******************************************************************************
//	// Licensing ...
//	//******************************************************************************
//	MCC_ReadLicenseInformation = (AM__I_FUNC)GetProcAddress(hLib, "MCC_ReadLicenseInformation");
//	MCC_GetLicenseInformation = (AM_B_C_FUNC)GetProcAddress(hLib, "MCC_GetLicenseInformation");
//	MCC_LicenseValid = (AM_B_B_FUNC)GetProcAddress(hLib, "MCC_LicenseValid");
//	MCC_LoadLicenseFile = (AM_Ccc_I_FUNC)GetProcAddress(hLib, "MCC_LoadLicenseFile");
//	MCC_ActivateLicenseFile = (AM_Ccc_I_FUNC)GetProcAddress(hLib, "MCC_ActivateLicenseFile");
//	MCC_DeactivateLicenseFile = (AM_Ccc_I_FUNC)GetProcAddress(hLib, "MCC_DeactivateLicenseFile");
//	MCC_RetrieveNetworkLicense = (AM__I_FUNC)GetProcAddress(hLib, "MCC_RetrieveNetworkLicense");
//	MCC_RetrieveNetworkLicenseFrom = (AM_Iul_I_FUNC)GetProcAddress(hLib, "MCC_RetrieveNetworkLicenseFrom");
//	MCC_ListLicenseFiles = (AM____FUNC)GetProcAddress(hLib, "MCC_ListLicenseFiles");
//	MCC_ListLicenseFilesDetails = (AM____FUNC)GetProcAddress(hLib, "MCC_ListLicenseFilesDetails");
//	MCC_ListLicenses = (AM____FUNC)GetProcAddress(hLib, "MCC_ListLicenses");
//	MCC_ListLicensesOf = (AM_Ccc_I_FUNC)GetProcAddress(hLib, "MCC_ListLicensesOf");
//	MCC_PrintNodeAndUserId = (AM____FUNC)GetProcAddress(hLib, "MCC_PrintNodeAndUserId");
//
//	//******************************************************************************
//	// Globale Informationen / Variablen ...
//	//******************************************************************************
//	MCC_GetElementIndex = (AM_Cc_I_FUNC)GetProcAddress(hLib, "MCC_GetElementIndex");
//
//	//******************************************************************************
//	// Initialisierung ...
//	//******************************************************************************
//	MCC_InitializeKernel = (AM_B_B_FUNC)GetProcAddress(hLib,"MCC_InitializeKernel");
//	MCC_InitializeKernelPathConst = (AM_CcB_B_FUNC)GetProcAddress(hLib, "MCC_InitializeKernelPathConst");
//	MCC_InitializeKernelPathChar = (AM_CB_B_FUNC)GetProcAddress(hLib, "MCC_InitializeKernelPathChar");
//	MCC_SetApplicationDirectoryChar = (AM_C_B_FUNC)GetProcAddress(hLib, "MCC_SetApplicationDirectoryChar");
//	MCC_SetWorkingDirectoryChar = (AM_C_B_FUNC)GetProcAddress(hLib, "MCC_SetWorkingDirectoryChar");
//	MCC_GetApplicationDirectoryChar = (AM__C_FUNC)GetProcAddress(hLib, "MCC_GetApplicationDirectoryChar");
//	MCC_GetWorkingDirectoryChar = (AM__C_FUNC)GetProcAddress(hLib, "MCC_GetWorkingDirectoryChar");
//	MCC_InitializeExternalChar = (AM_CB_C_FUNC)GetProcAddress(hLib, "MCC_InitializeExternalChar");
//	MCC_InitializeExternalConstChar = (AM_CcB_B_FUNC)GetProcAddress(hLib, "MCC_InitializeExternalConstChar");
//
//}

#pragma endregion
/** @}*/
