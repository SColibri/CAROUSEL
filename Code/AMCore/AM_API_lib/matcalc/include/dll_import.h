#ifndef INCLUDE_DLL_IMPORT_H
#define INCLUDE_DLL_IMPORT_H

#if !defined(BUILDING_MCR)

#include "mc_defines.h"
#include "mc_types.h"

#include "shared_types.h"

class CMCGrainSizeClass; // must be changed in new API

#ifdef MY_QT
#include "dll_import_internal.h"
#endif

//******************************************************************************
// GUI
//******************************************************************************
int MCC_DoCalcKinetic(void *gibbs, int *IsBusy, int flags);

//******************************************************************************
// Coline ...
//******************************************************************************
DECL_DLL_EXPORT void MCCOL_UseModul(int active_modul, int last_modul);
DECL_DLL_EXPORT int MCCOL_ProcessCommandLineInputNewColine(char *ptr_msg_string);
DECL_DLL_EXPORT int MCCOL_ProcessCommandLineInputNewColineConst(const char *ptr_msg_string);
DECL_DLL_EXPORT int MCCOL_ProcessCommandLineInput(char *command_string);
DECL_DLL_EXPORT int MCCOL_ProcessCommandLineInputConst(const char *command_string);
DECL_DLL_EXPORT int MCCOL_RunScriptFileChar(char *script_file_string);
DECL_DLL_EXPORT int MCCOL_SetAllowThreads(int Allow);
DECL_DLL_EXPORT int MCCOL_GetMCCalcVariable(char *var_desc, double *destination);
DECL_DLL_EXPORT int MCC_RedirectInputAndOutput( int (*console_out_func)(const char * out_string), int (*console_in_func)(char * buffer, const char * message_text));


//******************************************************************************
// Variables ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumVariableCategories();
DECL_DLL_EXPORT int MCC_GetVariableCategory(int index, int &code, const char *&text);
DECL_DLL_EXPORT int MCC_GetCategoryVariables(int category_code, int &num_vars, char **&cat_vars);
DECL_DLL_EXPORT int MCC_ExpandVariableWildcards(char *var, int wildcard_index, int &num_vars, char **&exp_vars);

// **
// **********
// ******************
// **************************
// **********************************
//      *************************************
DECL_DLL_EXPORT const char* MCC_VariableName(int code);
DECL_DLL_EXPORT         int MCC_VariableCode(const char* name);
//                              ***************************************
//                                      ****************************************

//******************************************************************************
// Licensing ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_ReadLicenseInformation();
// DECL_DLL_EXPORT char * MCC_GetLicenseInformationDetail(int info_type);
DECL_DLL_EXPORT char *MCC_GetLicenseInformation(bool html_format);

// bool MCC_IsFullVersion();
DECL_DLL_EXPORT bool MCC_LicenseValid(bool echo_expired);
// DECL_DLL_EXPORT bool MCC_IsModuleEnabled(int module_flags);
// DECL_DLL_EXPORT bool MCC_CheckLicenseAndMaxSelectedElements(int echo_error);

// filepath
// DECL_DLL_EXPORT char * MCC_GetLicenseFilePathChar();
// DECL_DLL_EXPORT bool MCC_SetLicenseFilePathChar(char * file_path);


//******************************************************************************
// speichern und laden ...
//******************************************************************************

DECL_DLL_EXPORT const char *MCC_GetSettingsValue(char const *category, char const *param_name);
DECL_DLL_EXPORT int MCC_SetSettingsValue(char const *category, char const *param_name, char const *new_value);


//******************************************************************************
// Binary file operation ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_WriteBinaryFileChar(const char *filename);
DECL_DLL_EXPORT int MCC_ReadBinaryFileChar(const char *filename);


//******************************************************************************
// Testing operations ...
//******************************************************************************

DECL_DLL_EXPORT bool MCC_TestExistFile(char const *FileName, int Type);
DECL_DLL_EXPORT bool MCC_TestValidExpression(char const *expression);
DECL_DLL_EXPORT void MCC_SetTest( bool true_false );


//******************************************************************************
// Initialisierung ...
//******************************************************************************

DECL_DLL_EXPORT bool MCC_InitializeKernel(bool echoVersion = false);
DECL_DLL_EXPORT bool MCC_InitializeKernelPathConst(const char *app_file_path, bool echoVersion = false);
DECL_DLL_EXPORT bool MCC_InitializeKernelPathChar(char *application_file_path, bool echoVersion = false);
DECL_DLL_EXPORT bool MCC_SetApplicationDirectoryChar(char *appl_dir);
DECL_DLL_EXPORT bool MCC_SetWorkingDirectoryChar(char *work_dir);
DECL_DLL_EXPORT char *MCC_GetApplicationDirectoryChar();
DECL_DLL_EXPORT char *MCC_GetWorkingDirectoryChar();

DECL_DLL_EXPORT bool MCC_InitializeExternalChar(char *app_file_path, bool echoVersion = false);
DECL_DLL_EXPORT bool MCC_InitializeExternalConstChar(const char *app_file_path, bool echoVersion = false);

DECL_DLL_EXPORT std::vector<const char *> *MCC_GetWriteOutBuffer();
DECL_DLL_EXPORT const char *MCC_GetLastStatus();
DECL_DLL_EXPORT void MCC_SetWriteOutMode(bool);

DECL_DLL_EXPORT bool MCC_SetEcho(int new_IsEcho);
DECL_DLL_EXPORT bool MCC_IsEcho();
DECL_DLL_EXPORT bool MCC_IsFlagSet(int flag);
DECL_DLL_EXPORT int MCC_SetFlag(int flag, bool Set);
DECL_DLL_EXPORT bool MCC_IsBusy();
DECL_DLL_EXPORT bool MCC_StopCurrentActivity();
DECL_DLL_EXPORT bool MCC_IsStopActivity();
DECL_DLL_EXPORT bool MCC_ClearStopActivity();
DECL_DLL_EXPORT bool MCC_IsPaused();
DECL_DLL_EXPORT int MCC_PauseThreads();
DECL_DLL_EXPORT int MCC_ResumeThreads();

DECL_DLL_EXPORT int MCC_SetRemoteControlMode(bool IsRemoteControl);


//******************************************************************************
// Knotenmanagment
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfNodeGroups();
DECL_DLL_EXPORT int MCC_GetNumberOfNodesInGroup(int NodeGrpIndex);
DECL_DLL_EXPORT MC_HANDLE MCC_GetNodeHandle(int NodeGrpIndex, int NodeIndex);
DECL_DLL_EXPORT int MCC_GetNodeGroupIndexFromHandle(MC_HANDLE NodeHandle);
DECL_DLL_EXPORT int MCC_SetSelectedNode(MC_HANDLE NodeHandle);
DECL_DLL_EXPORT MC_HANDLE MCC_GetSelectedNodeHandle();
DECL_DLL_EXPORT int MCC_GetNodeID(MC_HANDLE NodeHandle);
DECL_DLL_EXPORT char *MCC_GetNodeName(MC_HANDLE NodeHandle);
DECL_DLL_EXPORT MC_HANDLE MCC_GetNodeByName(int NodeGroupIndex, char *NodeName);
DECL_DLL_EXPORT bool MCC_IsValidNodeHandle(MC_HANDLE NodeHandle);
DECL_DLL_EXPORT bool MCC_IsValidNodeGroup(int node_group_index);
DECL_DLL_EXPORT bool MCC_IsValidNode(int node_group_index, int node_index);

DECL_DLL_EXPORT int MCC_CreateNewNodeGroup(const char *group_name);
DECL_DLL_EXPORT const char *MCC_GetNodeGroupName(int GroupIndex);
DECL_DLL_EXPORT int MCC_GetNodeGroupIndex(const char *group_name);
DECL_DLL_EXPORT int MCC_ProtectNodeGroup(int GroupIndex, bool Protect);
DECL_DLL_EXPORT int MCC_GetNodeGroupProtect(int GroupIndex);
DECL_DLL_EXPORT MC_HANDLE MCC_CreateNewNode(int GroupIndex, char *name);
DECL_DLL_EXPORT int MCC_DuplicateGibbsProps(MC_HANDLE TargetNodeHandle, MC_HANDLE SourceNodeHandle);
// for simulation module
DECL_DLL_EXPORT int MCC_DuplicatePrecDomainSettings(MC_HANDLE TargetNodeHandle, MC_HANDLE SourceNodeHandle);

DECL_DLL_EXPORT int MCC_RemoveNodeGroup(int GroupIndex);
DECL_DLL_EXPORT int MCC_RemoveNode(MC_HANDLE NodeHandle);


//******************************************************************************
// Knoteneigenschaften ...
//******************************************************************************

// ein integer wert frei definierbar. wird nicht gespeichert !!!
DECL_DLL_EXPORT int MCC_SetNodeUserData(MC_HANDLE NodeHandle, int data);
DECL_DLL_EXPORT int MCC_GetNodeUserData(MC_HANDLE NodeHandle);
DECL_DLL_EXPORT int MCC_SetNodeGroupUserData(int GroupIndex, int data);
DECL_DLL_EXPORT int MCC_GetNodeGroupUserData(int GroupIndex);

// Knotendaten, die gespeichert werden ...
DECL_DLL_EXPORT void *MCC_GetNodeExtraDataPtr(MC_HANDLE NodeHandle);


//******************************************************************************
// Datenbankfunktionen ...
//******************************************************************************

DECL_DLL_EXPORT bool MCC_OpenDatabaseFileChar(char *FileName, int Type, bool StartAsThread);
DECL_DLL_EXPORT char *MCC_GetOpenDatabaseFileChar(int Type);

DECL_DLL_EXPORT bool MCC_SelectDBElement(int Index, bool Select);
DECL_DLL_EXPORT bool MCC_SelectDBPhase(int Index, bool Select);
DECL_DLL_EXPORT bool MCC_ReadDatabaseFile(int Type, bool CreateCompSetPhases, bool StartAsThread);

DECL_DLL_EXPORT bool MCC_RemoveDiffusionData();
DECL_DLL_EXPORT bool MCC_RemovePhysicalData();


//******************************************************************************
// Neural Networking functions ...
//******************************************************************************

DECL_DLL_EXPORT bool MCC_ReadNNDatabaseFile(char *FileName);
DECL_DLL_EXPORT bool MCC_RemoveNNData();
DECL_DLL_EXPORT bool MCC_HasNNData();

// from list
DECL_DLL_EXPORT int MCC_GetNumberOfNNAliases();
DECL_DLL_EXPORT char *MCC_GetNNAliasPhase(int alias_index);
DECL_DLL_EXPORT char *MCC_GetNNAliasParent(int alias_index);

// from phase
DECL_DLL_EXPORT int MCC_GetNNAliasIndexFromPhaseHandle(MC_HANDLE phase_handle);
DECL_DLL_EXPORT int MCC_SetNNAliasIndexFromPhaseHandle(MC_HANDLE phase_handle, int alias_index);


//******************************************************************************
// Gibbs-funktionen bearbeiten ...
//******************************************************************************

DECL_DLL_EXPORT MC_HANDLE MCC_GetThermFktMgrHandle(int Type, MC_HANDLE PhaseHandle, int ElementIndex);
DECL_DLL_EXPORT int MCC_GetTFM_NumFunctions(MC_HANDLE ThermFktHandle);
DECL_DLL_EXPORT char *MCC_GetTFM_FunctionData(MC_HANDLE ThermFktHandle, int FuncIndex, int get_flags);
DECL_DLL_EXPORT bool
MCC_SetTFM_FunctionData(MC_HANDLE ThermFktHandle, int FuncIndex, const char *new_data, int set_flags);
DECL_DLL_EXPORT int
MCC_LookupTFM_Function(MC_HANDLE ThermFktHandle, char *name, const char *constituents, int element, int digits);
DECL_DLL_EXPORT int MCC_AppendTFM_Parameter(char *param_string, int database_type, MC_HANDLE &ThermFktHandle);


//******************************************************************************
// Physikalische daten bearbeiten ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumPhysicalParameters();
DECL_DLL_EXPORT int MCC_GetPhysicalParameterIndex(char *param_code);
DECL_DLL_EXPORT char *MCC_GetPhysicalParameterCode(int ParameterIndex);


//******************************************************************************
// Datenbankinformationen ...
//******************************************************************************

DECL_DLL_EXPORT bool MCC_CheckDBReload();
DECL_DLL_EXPORT int MCC_GetNumDBElements(bool SelectedOnly);
DECL_DLL_EXPORT int MCC_GetNumDBPhases(bool SelectedOnly);
DECL_DLL_EXPORT TDMGRElementData *MCC_GetDBElement(int Index);
DECL_DLL_EXPORT TDMGRPhaseData *MCC_GetDBPhase(int Index);
DECL_DLL_EXPORT int MCC_GetDBElementIndex(char *Element);
DECL_DLL_EXPORT int MCC_GetDBPhaseIndex(char *Phase);

//******************************************************************************
// Globale Informationen / Variablen ...
//******************************************************************************

DECL_DLL_EXPORT void MCC_WriteOutString(char *text, bool AppendCR = false, bool SendSignal = false);
DECL_DLL_EXPORT bool
MCC_SendInfoOn(MC_HANDLE hWndSendTo, int Topic, void *str_vars, void *str_vars1, int num_vars, MC_INDEX var1);
DECL_DLL_EXPORT bool MCC_SetGlobalParameter(int Which, int Value);
DECL_DLL_EXPORT bool MCC_SetGlobalDoubleParameter(int Which, double Value);
DECL_DLL_EXPORT bool MCC_SetGlobalHandleParameter(int Which, const MC_HANDLE Value);
DECL_DLL_EXPORT bool MCC_SetGlobalIndexParameter(int Which, MC_INDEX Value);
DECL_DLL_EXPORT bool MCC_SetGlobalFunctionParameter(int Which, char *func_expression);
DECL_DLL_EXPORT bool MCC_GetGlobalParameter(int Which, int *Buff);
DECL_DLL_EXPORT bool MCC_GetGlobalDoubleParameter(int Which, double *Buff);
DECL_DLL_EXPORT bool MCC_GetGlobalHandleParameter(int Which, MC_HANDLE *Buff);
DECL_DLL_EXPORT bool MCC_GetGlobalIndexParameter(int Which, MC_INDEX *Buff);
DECL_DLL_EXPORT bool MCC_GetGlobalFunctionParameter(int Which, const char *&func_expression);
DECL_DLL_EXPORT int MCC_GetElementIndex(const char *element_name);
DECL_DLL_EXPORT bool MCC_SetTotalAmountOfElement(int GBSIndex, double NewVal, bool InWeightPercent, int AdjustType);
DECL_DLL_EXPORT bool MCC_GetTotalAmountOfElement(int GBSIndex, double *buff, bool InWeightPercent);
/*
bool MCC_SetMatrixAmountOfElement(int GBSIndex,
                                  double NewVal,
                                  bool InWeightPercent,
                                  nt AdjustType);
bool MCC_GetMatrixAmountOfElement(int GBSIndex,
                                  double * buff,
                                  bool InWeightPercent);
*/
DECL_DLL_EXPORT bool MCC_SetTotalUFractionOfElement(int GBSIndex, double NewVal, int AdjustType);
DECL_DLL_EXPORT bool MCC_GetTotalUFractionOfElement(int GBSIndex, double *buff);
/*
bool MCC_SetMatrixUFractionOfElement(int GBSIndex,
                                     double NewVal,
                                     int AdjustType);
bool MCC_GetMatrixUFractionOfElement(int GBSIndex, double * buff);
*/
DECL_DLL_EXPORT double MCC_GetSubstitutionalElementFraction();

DECL_DLL_EXPORT int MCC_GetElementCompositionControl(int GBSIndex, const char *&func_expresssion);
DECL_DLL_EXPORT int MCC_SetElementCompositionControl(int GBSIndex, char const *func_expression);

DECL_DLL_EXPORT bool MCC_CreateNewCompositionTable(const char * comp_table_name);
DECL_DLL_EXPORT int MCC_GetNumberOfCompositionTables();
DECL_DLL_EXPORT bool MCC_RemoveCompositionTable(MC_INDEX index);
DECL_DLL_EXPORT bool MCC_SetActiveCompositionTable(MC_INDEX index, bool apply_comp=true);
DECL_DLL_EXPORT MC_INDEX MCC_GetActiveCompositionTableIndex();
DECL_DLL_EXPORT MC_INDEX MCC_GetCompositionTableIndex(const char * comp_table_name);
DECL_DLL_EXPORT bool MCC_SetCompositionTableProperty(MC_INDEX index, int prop_code, int int_buff, double double_buff, void *void_ptr);
DECL_DLL_EXPORT bool MCC_GetCompositionTableProperty(MC_INDEX index, int prop_code, int &int_buff, double &double_buff, void *&void_ptr);

DECL_DLL_EXPORT bool
MCC_GetCalcVariable(const char *VariableDesc, double *destination, bool AllowOnlyOneVariable, bool NoErrorWarning);
DECL_DLL_EXPORT double MCC_GetMCVariable(char *VariableDesc);
DECL_DLL_EXPORT bool
MCC_GetCalcVariableHelpText(char *VariableDesc, const char *&destination, const char *&unit_qual_dest);
DECL_DLL_EXPORT bool MCC_GetCalcVariableFromNodeHandle(MC_HANDLE NodeHandle, char *VariableDesc, double *destination);
DECL_DLL_EXPORT bool
MCC_GetParsedCalcVarFromNodeHandle(MC_HANDLE NodeHandle, MC_HANDLE PVar_Handle, double *destination);
DECL_DLL_EXPORT bool
MCC_GetBufferVariables(char **VariableDesc, double **destination, int NumVars, MC_INDEX BufferHandle);
DECL_DLL_EXPORT bool
MCC_GetParticleArrayVariables(char **VariableDesc, double **destination, int NumVars);
DECL_DLL_EXPORT bool MCC_GetGrainArrayVariables(char **VariableDesc, double **destination, int NumVars);


DECL_DLL_EXPORT bool MCC_GetParticleDistribution(
  MC_HANDLE ph_handle, double **ptrDoubleData, int NumSizeClasses, double FixedLowerValue, double FixedUpperValue,
  bool ScaledRadius, int ScaleFrequency, int histogram_type, bool Logarithmic = false);


//
DECL_DLL_EXPORT double MCC_GetTemperature();
DECL_DLL_EXPORT double MCC_SetTemperature(double NewTemp, bool ReCalcGibbsEnergy);
DECL_DLL_EXPORT double MCC_GetBaseTemperature();
DECL_DLL_EXPORT double MCC_GetExcessTemperature();
DECL_DLL_EXPORT void MCC_SetBaseTemperature(double newT);
DECL_DLL_EXPORT void MCC_SetExcessTemperature(double newT);

DECL_DLL_EXPORT double MCC_GetEpsDotValue();

DECL_DLL_EXPORT double MCC_GetPressure();
DECL_DLL_EXPORT double MCC_SetPressure(double NewTemp, bool ReCalcGibbsEnergy);
DECL_DLL_EXPORT double MCC_GetSimulationTime();
DECL_DLL_EXPORT void MCC_SetSimulationTime(double NewTime);
DECL_DLL_EXPORT double MCC_GetMolarGibbsEnergy();

DECL_DLL_EXPORT double MCC_GetPhaseAmount(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT double MCC_GetUPhaseAmount(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT double MCC_GetFixedUPhaseAmount(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT bool MCC_SetPhaseAmount(MC_HANDLE PhaseHandle, double new_amount);
DECL_DLL_EXPORT bool MCC_SetUPhaseAmount(MC_HANDLE PhaseHandle, double new_amount);
DECL_DLL_EXPORT bool MCC_SetFixedUPhaseAmount(MC_HANDLE PhaseHandle, double new_amount);
DECL_DLL_EXPORT double MCC_GetAmountOfElementInPhase(MC_HANDLE PhaseHandle, int ElementIndex, bool InWeightPercent);
DECL_DLL_EXPORT double MCC_GetUFractionOfElementInPhase(MC_HANDLE PhaseHandle, int ElementIndex);

DECL_DLL_EXPORT double MCC_GetStructuralDefectFraction(MC_HANDLE PhaseHandle, const char *&func_expression);
DECL_DLL_EXPORT int MCC_SetStructuralDefectFraction(MC_HANDLE PhaseHandle, char const*new_expression);


DECL_DLL_EXPORT double MCC_GetMolarMass();
DECL_DLL_EXPORT double MCC_GetMolarMassOfPhase(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT double MCC_GetMolarVolume();
DECL_DLL_EXPORT double MCC_GetMolarVolumeOfPhase(MC_HANDLE PhaseHandle);

DECL_DLL_EXPORT double MCC_GetEquilibriumVacancyConcentration(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT double MCC_GetLatticeVacancyConcentration(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT int MCC_SetLatticeVacancyConcentration(MC_HANDLE PhaseHandle, double value);

// warning: this function must be used with care, since it is not yet
// working correctly for complex phases!!!
// Function takes yx-values !!!
DECL_DLL_EXPORT int
MCC_TransferCompositionIntoPhase(MC_HANDLE PhaseHandle, double *yx_composition_array, int num_elements_in_array);

/*
double MCC_GetTracerDiffusionCoefficient(MC_HANDLE GibbsNodeHandle,
                                         int ElementIndex);
double MCC_GetChemicalDiffusionCoefficient(MC_HANDLE GibbsNodeHandle,
                                           int LineIndex,
                                           int RowIndex);
double MCC_GetL1kjCoefficient(MC_HANDLE GibbsNodeHandle,
                              int LineIndex,
                              int RowIndex);
double MCC_GetChemicalPotential(MC_HANDLE GibbsNodeHandle, int ElementIndex);
double MCC_GetExtrapolatedChemPot(MC_HANDLE GibbsNodeHandle,
                                  int ElementIndex,
                                  double new_X_value);
double MCC_GetExtrapolatedChemPotInPhase(MC_HANDLE PhaseHandle,
                                         int ElementIndex,
                                         double new_X_value);
double MCC_GetExtrapolatedX(MC_HANDLE GibbsNodeHandle,
                            int ElementIndex,
                            double new_Potential);
double MCC_GetReducedChemicalPotential(MC_HANDLE GibbsNodeHandle,
                                       int ElementIndex);
double MCC_GetExtrapolatedReducedChemPot(MC_HANDLE GibbsNodeHandle,
                                         int ElementIndex,
                                         double new_X_value);
double MCC_GetExtrapolatedReducedChemPotInPhase(MC_HANDLE PhaseHandle,
                                                int ElementIndex,
                                                double new_X_value);
double MCC_GetExtrapolatedXReduced(MC_HANDLE GibbsNodeHandle,
                                   int ElementIndex,
                                   double new_Potential);
double MCC_GetDeriveChemPot(MC_HANDLE GibbsNodeHandle,
                            int LineIndex,
                            int RowIndex);
double MCC_GetDeriveChemPot_2(MC_HANDLE GibbsNodeHandle,
                              int RootIndex,
                              int DeriveIndex);
double MCC_GetReducedDeriveChemPot(MC_HANDLE GibbsNodeHandle,
                                   int LineIndex,
                                   int RowIndex);
double MCC_GetIntrinsicDiffusionCoefficient(MC_HANDLE GibbsNodeHandle,
                                            int LineIndex,
                                            int RowIndex);
*/

DECL_DLL_EXPORT double MCC_GetDiffusionMobility(MC_HANDLE PhaseHandle, int ElementIndex);
DECL_DLL_EXPORT double MCC_GetDiffusionCoefficient(MC_HANDLE PhaseHandle, int ElementIndex1, int ElementIndex2);
DECL_DLL_EXPORT double MCC_GetTracerDiffusionCoefficient(int ElementIndex);
DECL_DLL_EXPORT double MCC_GetChemicalDiffusionCoefficient(int LineIndex, int RowIndex);
DECL_DLL_EXPORT double MCC_GetIntrinsicDiffusionCoefficient(int LineIndex, int RowIndex);
DECL_DLL_EXPORT double MCC_GetL1kjCoefficient(int LineIndex, int RowIndex);


DECL_DLL_EXPORT double MCC_GetGlobalChemicalPotential(int ElementIndex);
DECL_DLL_EXPORT double MCC_GetChemicalPotentialInPhase(MC_HANDLE PhaseHandle, int ElementIndex);
DECL_DLL_EXPORT double MCC_GetGlobalReducedChemicalPotential(int ElementIndex);
DECL_DLL_EXPORT double MCC_GetExtrapolatedChemPot(int ElementIndex, double new_X_value);
DECL_DLL_EXPORT double MCC_GetExtrapolatedChemPotInPhase(MC_HANDLE PhaseHandle, int ElementIndex, double new_X_value);
DECL_DLL_EXPORT double MCC_GetExtrapolatedX(int ElementIndex, double new_Potential);
DECL_DLL_EXPORT double MCC_GetExtrapolatedReducedChemPot(int ElementIndex, double new_X_value);
DECL_DLL_EXPORT double
MCC_GetExtrapolatedReducedChemPotInPhase(MC_HANDLE PhaseHandle, int ElementIndex, double new_X_value);
DECL_DLL_EXPORT double MCC_GetExtrapolatedXReduced(int ElementIndex, double new_Potential);
DECL_DLL_EXPORT double MCC_GetDeriveChemPot(int LineIndex, int RowIndex);
DECL_DLL_EXPORT double MCC_GetDeriveChemPot_2(int RootIndex, int DeriveIndex);
DECL_DLL_EXPORT double MCC_GetReducedDeriveChemPot(int LineIndex, int RowIndex);
// int MCC_GetNumberOfDiffElementsInMatrix();


//******************************************************************************
// some helpers with functionality for CAFE module

DECL_DLL_EXPORT double
MCC_GetCalculatedPlanarSharpInterfaceEnergy(MC_HANDLE MatrixHandle, MC_HANDLE PrecHandle, int force_recalc);

//******************************************************************************
// some helpers with functionality for Monte module
/*
int MCC_GetCompositionDependentEnergyOfPhase(MC_HANDLE PhaseHandle,
                                             double * energy,
                                             double * yx_composition_array,
                                             int num_elements_in_array,
                                             int type);
*/

DECL_DLL_EXPORT int
MCC_SetPhaseComposition(MC_HANDLE PhaseHandle, double *yx_composition_array, int num_elements_in_array);

DECL_DLL_EXPORT double MCC_GetAtomicInteractionEnergy(
  MC_HANDLE PhaseHandle,
  /*double * interaction_energy,*/
  int element_index1, int element_index2, int force_recalc);


//******************************************************************************
// Verification API ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_ClearVerificationRecords();
DECL_DLL_EXPORT int MCC_GetNumVerificationRecords();
DECL_DLL_EXPORT int MCC_AppendVerificationBufferRecord(
  char const*table_name, char const*var_name, char const*title, int threshold_type, double threshold_value_1,
  double threshold_value_2);
DECL_DLL_EXPORT int MCC_AppendVerificationCalcStateRecord(
  int num_vars, char const**var_names, double *values, char const*title, int threshold_type, double threshold_value_1,
  double threshold_value_2);
DECL_DLL_EXPORT int MCC_RemoveVerificationRecord(int index);
DECL_DLL_EXPORT int MCC_EvaluateVerificationRecord(int index, int echo_type);


//******************************************************************************
// Berechnungen ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_CalcEquilibrium(bool SupressStartAsThread, int flags);
DECL_DLL_EXPORT int MCC_CalcEquilibriumStep(bool SupressStartAsThread);
DECL_DLL_EXPORT int MCC_CalcSolubilityTemperature(bool SupressStartAsThread);
DECL_DLL_EXPORT int MCC_CalcT0Temperature(bool SupressStartAsThread);
DECL_DLL_EXPORT int MCC_CalcSolubilityElementContent(bool SupressStartAsThread);
DECL_DLL_EXPORT int MCC_CalcScheilReaction(bool SupressStartAsThread);
DECL_DLL_EXPORT int MCC_CalcPartitionRatio();

DECL_DLL_EXPORT int MCC_GetCalcStatus();

// special
DECL_DLL_EXPORT int MCC_SetSiteFractionsByChemPot(bool SupressStartAsThread);
DECL_DLL_EXPORT char *MCC_WriteOutStepInfo();


//******************************************************************************
// diverse Hilfsfunktionen ...
//******************************************************************************

DECL_DLL_EXPORT bool MCC_UpdateComposition(int Direction);
// missing unicode support here
DECL_DLL_EXPORT bool MCC_SaveComposition(char *FileName);
DECL_DLL_EXPORT bool MCC_ReadComposition(char *FileName);
DECL_DLL_EXPORT bool MCC_ViewCompositionFile(char *FileName);

DECL_DLL_EXPORT bool MCC_IsPhasePossible(char *PhaseName, char *ConstituentString);

DECL_DLL_EXPORT int MCC_SetDefaultReferenceElement(char *element);
DECL_DLL_EXPORT int MCC_GetNumElements();
DECL_DLL_EXPORT TDMGRElementData *MCC_GetElement(MC_INDEX Index);
DECL_DLL_EXPORT int MCC_GetReferenceElementIndex();
DECL_DLL_EXPORT void MCC_SetReferenceElement(int Index);
DECL_DLL_EXPORT int MCC_GetVacancyIndex();
DECL_DLL_EXPORT int MCC_IsSubstitutionalElement(int Index);
DECL_DLL_EXPORT bool MCC_SetAllStartValues(int flags);
DECL_DLL_EXPORT void MCC_NormalizePhaseAmounts();
DECL_DLL_EXPORT void MCC_AdjustAmountOfRefPhase();
DECL_DLL_EXPORT int MCC_SetCompositionFromParent(MC_HANDLE ParentPhaseHandle, MC_HANDLE ChildPhaseHandle, int flags);

// must provide array (mole fraction) with GBS_Elements.GetSize() elements
DECL_DLL_EXPORT int MCC_SetGlobalCompositionFromArray(double *composition);
// must provide array (mole fraction) with GBS_Elements.GetSize() elements
DECL_DLL_EXPORT int MCC_GetGlobalCompositionIntoArray(double *composition);

DECL_DLL_EXPORT bool MCC_SaveDefaultComposition();


//******************************************************************************
// variables ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfVariables();
DECL_DLL_EXPORT char *MCC_GetVariableName(int index);
DECL_DLL_EXPORT int MCC_GetVariableIndex(char *name);
DECL_DLL_EXPORT int MCC_RenameVariable(char *OldName, char *NewName);
DECL_DLL_EXPORT int MCC_GetVariableValue(char const*var_name, double &value);
DECL_DLL_EXPORT int MCC_SetVariableValue(const char *var_name, double value);
DECL_DLL_EXPORT int MCC_RemoveVariable(char *var_name);

DECL_DLL_EXPORT int MCC_GetNumberOfStringVariables();
DECL_DLL_EXPORT char *MCC_GetStringVariableName(int index);
DECL_DLL_EXPORT int MCC_GetStringVariableIndex(char *name);
DECL_DLL_EXPORT int MCC_RenameStringVariable(char *OldName, char *NewName);
DECL_DLL_EXPORT int MCC_GetVariableString(char *var_name, const char *&value);
DECL_DLL_EXPORT int MCC_SetVariableString(char *var_name, const char *value);
DECL_DLL_EXPORT int MCC_GetNumVarArgs(char *format_string, int &num_args);
DECL_DLL_EXPORT int MCC_FormatVariableString(char *format_string, char **var_ptr_array, char *var_name);
DECL_DLL_EXPORT int MCC_RemoveStringVariable(char *var_name);
DECL_DLL_EXPORT int MCC_IsValidStringVariableName(const char *var_name);
DECL_DLL_EXPORT const char *MCC_StringVariableRegExp();


//******************************************************************************
// functions ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfFunctions();
DECL_DLL_EXPORT char *MCC_GetFunctionName(int index);
DECL_DLL_EXPORT int MCC_GetFunctionIndex(char *name);

DECL_DLL_EXPORT int MCC_RenameFunction(char *OldName, char *NewName);

DECL_DLL_EXPORT int MCC_GetFunctionDataIndex(int index, char *&name, const char *&function_data);

DECL_DLL_EXPORT int MCC_GetFunctionData(const char *name, const char *&function_data, bool EchoError);

DECL_DLL_EXPORT int MCC_GetFunctionValue(int index, double &value);

DECL_DLL_EXPORT int MCC_SetFunctionData(char *name, char *function_data);

DECL_DLL_EXPORT int MCC_CreateNewFunction(char *name, const char *function_data);

DECL_DLL_EXPORT int MCC_RemoveFunction(const char *name);


//******************************************************************************
// Alle moeglichen Aktionen fuer Phasen im Gibbs-Modul...
//******************************************************************************

DECL_DLL_EXPORT int MCC_IsValidPhaseHandle(MC_HANDLE td_phase_handle);
DECL_DLL_EXPORT MC_HANDLE MCC_GetNextPhaseHandle(MC_HANDLE PreviousHandle);
DECL_DLL_EXPORT MC_HANDLE MCC_GetParentPhaseHandle(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT MC_HANDLE MCC_GetPhaseHandleByName(const char *PhaseName);
DECL_DLL_EXPORT MC_HANDLE MCC_GetPhaseHandleByIndex(MC_INDEX index);

DECL_DLL_EXPORT bool MCC_SetPhaseAsReferencePhase(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT MC_HANDLE MCC_GetReferencePhaseHandle();
DECL_DLL_EXPORT char *MCC_GetPhaseName(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT char *MCC_GetPhaseNameFromIndex(MC_INDEX PhaseHandle);
DECL_DLL_EXPORT int MCC_GetPhaseFlags(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT int MCC_SetPhaseFlags(MC_HANDLE PhaseHandle, int flags, bool Set);

DECL_DLL_EXPORT MC_HANDLE MCC_CreateNewPhaseFrom(MC_HANDLE PhaseHandle, int FixedSolidStatus, int ExtraIndex = 0);
DECL_DLL_EXPORT MC_HANDLE
MCC_CreateNewCompositionSetFrom(MC_HANDLE PhaseHandle, char const *constituents, char const *new_name);
DECL_DLL_EXPORT MC_HANDLE
MCC_CreateNewPrecipitateFrom(MC_HANDLE PhaseHandle, int &result, char const *alias_name = nullptr);
DECL_DLL_EXPORT int MCC_RemovePhase(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT char *MCC_GetPhaseConstituents(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT char *MCC_GetPhaseMajorConstituents(MC_HANDLE PhaseHandle, int type);
DECL_DLL_EXPORT bool MCC_SetPhaseMajorConstituents(MC_HANDLE PhaseHandle, char const*MajConstString);
DECL_DLL_EXPORT bool MCC_SetPhaseStartValues(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT int MCC_GetNumOfSiteFractionVars(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT int MCC_GetNumOfSublattices(MC_HANDLE PhaseHandle);
DECL_DLL_EXPORT int MCC_GetNumOfElementsOnSublattice(MC_HANDLE PhaseHandle, int SL);
DECL_DLL_EXPORT int MCC_GetSublatticeAndPosFromSFVar(MC_HANDLE PhaseHandle, int var_pos, int &SL, int &pos);
DECL_DLL_EXPORT int MCC_GetElementIndexOnSiteFractionPos(MC_HANDLE PhaseHandle, int SL, int pos);
DECL_DLL_EXPORT int MCC_GetElementIndexOfSiteFractionVar(MC_HANDLE PhaseHandle, int var_pos);
DECL_DLL_EXPORT int MCC_ExistsElementOnSublattice(MC_HANDLE PhaseHandle, char *Element, int SL);
DECL_DLL_EXPORT int MCC_ExistsElementInPhase(MC_HANDLE PhaseHandle, char *Element);

DECL_DLL_EXPORT int
MCC_GetElementStatusInPhase(MC_HANDLE PhaseHandle, int ElementIndex, int &condition_type, double &condition_value);
DECL_DLL_EXPORT int
MCC_SetElementStatusInPhase(MC_HANDLE PhaseHandle, int ElementIndex, int condition_type, double condition_value);

DECL_DLL_EXPORT int MCC_GetElementSublatticeTypeInPhase(MC_HANDLE PhaseHandle, int ElementIndex, int &type);


//******************************************************************************
// Resultate, Condition-lines, buffer ...
//******************************************************************************

DECL_DLL_EXPORT MC_INDEX MCC_CreateNewState(char *CL_name);
DECL_DLL_EXPORT int MCC_RemoveState(MC_INDEX LineHandle);
DECL_DLL_EXPORT int MCC_SelectState(MC_INDEX LineHandle);
DECL_DLL_EXPORT MC_INDEX MCC_GetActiveStateHandle();
DECL_DLL_EXPORT MC_INDEX MCC_GetStateHandle(char const*CL_name, bool exact = false);
DECL_DLL_EXPORT char *MCC_GetStateName(MC_INDEX LineHandle);
DECL_DLL_EXPORT int MCC_SetStateName(MC_INDEX LineHandle, char *new_CL_name);
DECL_DLL_EXPORT int MCC_GetNumStates();
DECL_DLL_EXPORT int MCC_StoreState(MC_INDEX LineHandle);
DECL_DLL_EXPORT int MCC_LoadState(MC_INDEX LineHandle);
DECL_DLL_EXPORT int MCC_LoadStateFromNode(MC_HANDLE DestNodeHandle, MC_HANDLE SourceNodeHandle, MC_HANDLE LineHandle);

DECL_DLL_EXPORT MC_INDEX MCC_CreateNewBuffer(const char *B_name);
DECL_DLL_EXPORT int MCC_RemoveBuffer(MC_INDEX BufferHandle);
DECL_DLL_EXPORT MC_INDEX MCC_GetBufferHandle(const char *B_name, bool exact = true);
DECL_DLL_EXPORT const char *MCC_GetBufferName(MC_INDEX BufferHandle);
DECL_DLL_EXPORT int MCC_SetBufferName(MC_INDEX BufferHandle, char *new_B_name);
DECL_DLL_EXPORT int MCC_GetNumBuffers();
DECL_DLL_EXPORT int MCC_GetNumLinesInBuffer(MC_INDEX BufferHandle);
DECL_DLL_EXPORT int MCC_GetBufferType(MC_INDEX BufferHandle);
DECL_DLL_EXPORT int MCC_SetBufferType(MC_INDEX BufferHandle, int StepType);
DECL_DLL_EXPORT int MCC_ClearBufferContent(MC_INDEX BufferHandle);
DECL_DLL_EXPORT int MCC_ClearBufferRecords(MC_INDEX BufferHandle, double FromValaue, double ToValue);
DECL_DLL_EXPORT int MCC_ClearBufferRecordsIndex(MC_INDEX BufferHandle, int FromIndex, int ToIndex);
DECL_DLL_EXPORT int MCC_RemoveBufferRecord(MC_INDEX BufferHandle, MC_INDEX LineHandle);
DECL_DLL_EXPORT int MCC_ReduceBufferRecords(MC_INDEX BufferHandle, double FromStepVal, double ToStepVal);
DECL_DLL_EXPORT MC_INDEX MCC_GetActiveBufferHandle();
DECL_DLL_EXPORT int MCC_SelectBuffer(MC_INDEX BufferHandle);

DECL_DLL_EXPORT double MCC_GetBufferLineStepVal(MC_INDEX BufferHandle, MC_INDEX LineHandle);
DECL_DLL_EXPORT double MCC_GetBufferLineTemperature(MC_INDEX BufferHandle, MC_INDEX LineHandle);
DECL_DLL_EXPORT char *MCC_GetBufferLineComment(MC_INDEX BufferHandle, MC_INDEX LineHandle);
DECL_DLL_EXPORT double MCC_GetLastBufferLineStepValue(MC_INDEX BufferHandle);
DECL_DLL_EXPORT int MCC_LoadStateFromBufferLine(MC_INDEX BufferHandle, MC_INDEX LineHandle);
DECL_DLL_EXPORT int
MCC_StoreStateInBufferLine(MC_INDEX BufferHandle, double AktStepVal, const char *comment, bool Append);


//******************************************************************************
// Export ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_OpenExportFileChar(char *filename);
DECL_DLL_EXPORT char *MCC_GetExportFileNameChar();
DECL_DLL_EXPORT char *MCC_GetExportFilePathChar();
DECL_DLL_EXPORT int MCC_CloseExportFile();
DECL_DLL_EXPORT int MCC_ClearExportFile();
// int MCC_FileExportData(char * data);
DECL_DLL_EXPORT int MCC_FileExportVariables(char *format_string, char **var_ptr_array);
DECL_DLL_EXPORT int MCC_FileExportBufferVariables(char *format_string, char **var_ptr_array, int module=MC_MODULE_CORE);

DECL_DLL_EXPORT int MCC_ImportPrecipitateDistribution(MC_HANDLE precipitate_handle, char *filename);
DECL_DLL_EXPORT int MCC_ExportPrecipitateDistribution(MC_HANDLE precipitate_handle, char *filename);

DECL_DLL_EXPORT int MCC_ExportGrainSizeDistributionFromHandle(
        MC_HANDLE precipitate_handle, char const * path, int verbose);
DECL_DLL_EXPORT int MCC_ExportGrainSizeDistributionFromIndex(
        MC_INDEX precipitate_index, char const * path, int verbose);
DECL_DLL_EXPORT int MCC_ImportGrainSizeDistributionToHandle(
        MC_HANDLE precipitate_handle, char const * path);
DECL_DLL_EXPORT int MCC_ImportGrainSizeDistributionToIndex(
        MC_INDEX precipitate_index, char const * path);

//******************************************************************************
// Transformation ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfTransformations();

DECL_DLL_EXPORT MC_HANDLE MCC_GetTransformationHandle(int index);
DECL_DLL_EXPORT MC_HANDLE MCC_GetTransformationHandleFromName(char const*name);

DECL_DLL_EXPORT char *MCC_GetTransformationName(MC_HANDLE sp_handle);
DECL_DLL_EXPORT char *MCC_GetTransformationOptionDescription(MC_HANDLE sp_handle);

DECL_DLL_EXPORT int MCC_IsTransformationActive(MC_HANDLE sp_handle);
DECL_DLL_EXPORT int MCC_SetTransformationActive(MC_HANDLE sp_handle, bool active);

DECL_DLL_EXPORT int MCC_SetTransformationName(MC_HANDLE sp_handle, char *new_desc);

DECL_DLL_EXPORT MC_HANDLE MCC_CreateNewTransformationRecord(char const*name);

DECL_DLL_EXPORT int MCC_DeleteTransformationRecord(MC_HANDLE sp_handle);

DECL_DLL_EXPORT int MCC_SetTransformationParameters(
  MC_HANDLE sp_handle, int Type, int IsActive, char *FromPhaseName, char *ToPhaseName, char *EquiPhaseName,
  bool set_only_phase_fraction, double upper_T, double lower_T, int TempInC, double Avrami_k, double Avrami_n,
  double Koistinen_Marburger_n, double max_phase_fraction, int table_index);

DECL_DLL_EXPORT int MCC_GetTransformationParameters(
  MC_HANDLE sp_handle, int &Type, int &IsActive, char *&FromPhaseName, char *&ToPhaseName, char *&EquiPhaseName,
  bool &set_only_phase_fraction, double &upper_T, double &lower_T, int &TempInC, double &Avrami_k, double &Avrami_n,
  double &Koistinen_Marburger_n, double &max_phase_fraction, int &table_index);

/*
int MCC_SetTransformationParameters(MC_HANDLE sp_handle,
                                    int Type,
                                    char * FromPhaseName,
                                    char * ToPhaseName,
                                    char * EquiPhaseName,
                                    int num_entries,
                                    double * T_array,
                                    double * ratio_array);

int MCC_GetNumberOfTransformationRecordEntries(MC_HANDLE sp_handle);

int MCC_GetTransformationParameters(MC_HANDLE sp_handle,
                                    int & Type,
                                    char * FromPhaseName,
                                    char * ToPhaseName,
                                    char * EquiPhaseName,
                                    int & max_entries,
                                    double * T_array,
                                    double * ratio_array);
*/


//******************************************************************************
// Tables ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfTables();
DECL_DLL_EXPORT MC_HANDLE MCC_GetTableHandle(int index);
DECL_DLL_EXPORT int MCC_CreateNewTable(char const*table_name);
DECL_DLL_EXPORT int MCC_RemoveTable(MC_HANDLE handle);

DECL_DLL_EXPORT int MCC_GetTableIndex(char *table_name);
DECL_DLL_EXPORT char *MCC_GetTableName(MC_HANDLE table_handle);
DECL_DLL_EXPORT int MCC_SetTableName(MC_HANDLE table_handle, char *new_name);

DECL_DLL_EXPORT int MCC_RemoveTableContents(MC_HANDLE table_handle);
DECL_DLL_EXPORT int MCC_GetNumberOfTableRecords(MC_HANDLE table_handle);
DECL_DLL_EXPORT int MCC_GetTableRecord(MC_HANDLE table_handle, int index, double &x_value, double &y_value);
DECL_DLL_EXPORT int MCC_AddTableRecord(MC_HANDLE table_handle, double x_value, double y_value, bool over_write_same);
// gets table value from x-value
DECL_DLL_EXPORT double MCC_GetTableValueAtX(MC_HANDLE table_handle, double x_value);
// gets user defined variable as x-value: e.g.: table(time)
DECL_DLL_EXPORT double MCC_GetTableValueAt(MC_HANDLE table_handle);
DECL_DLL_EXPORT double MCC_GetTableDXToNextPinPoint(MC_HANDLE table_handle, double x_value);

DECL_DLL_EXPORT int MCC_ImportGlobalTable(MC_HANDLE table_handle, char *filename);
DECL_DLL_EXPORT int MCC_ExportGlobalTable(MC_HANDLE table_handle, char *filename);


//******************************************************************************
// Arrays ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfGlobalArrays();
DECL_DLL_EXPORT MC_HANDLE MCC_GetGlobalArrayHandle(int index);
DECL_DLL_EXPORT char *MCC_GetGlobalArrayName(MC_HANDLE array_handle);
DECL_DLL_EXPORT int MCC_GetGlobalArrayIndex(char *array_name);
DECL_DLL_EXPORT int MCC_GetGlobalArrayCount();
DECL_DLL_EXPORT int MCC_CreateNewGlobalArray(char const*table_name);
DECL_DLL_EXPORT int MCC_RemoveGlobalArray(MC_HANDLE handle);
DECL_DLL_EXPORT int MCC_GetNumberOfGlobalArrayRows(MC_HANDLE handle);
DECL_DLL_EXPORT int MCC_GetNumberOfGlobalArrayCols(MC_HANDLE handle);
DECL_DLL_EXPORT double MCC_GetGlobalArrayValue(MC_HANDLE array_handle, int row, int col);
DECL_DLL_EXPORT int MCC_SetGlobalArrayName(MC_HANDLE array_handle, char *new_name);

/*
DECL_DLL_EXPORT int MCC_SetTableName(MC_HANDLE table_handle, char * new_name);

DECL_DLL_EXPORT int MCC_RemoveTableContents(MC_HANDLE table_handle);
DECL_DLL_EXPORT int MCC_GetNumberOfTableRecords(MC_HANDLE table_handle);
DECL_DLL_EXPORT int MCC_GetTableRecord(MC_HANDLE table_handle,
                                       int index,
                                       double &x_value,
                                       double &y_value);
DECL_DLL_EXPORT int MCC_AddTableRecord(MC_HANDLE table_handle,
                                       double x_value,
                                       double y_value,
                                       bool over_write_same);
//gets table value from x-value
DECL_DLL_EXPORT double MCC_GetTableValueAtX(MC_HANDLE table_handle,
                                            double x_value);
//gets user defined variable as x-value: e.g.: table(time)
DECL_DLL_EXPORT double MCC_GetTableValueAt(MC_HANDLE table_handle);
*/

DECL_DLL_EXPORT int MCC_SetGlobalArraySize(MC_HANDLE array_handle, int num_rows, int num_cols);
DECL_DLL_EXPORT int MCC_SetGlobalArrayValue(MC_HANDLE array_handle, int row, int col, double value);

DECL_DLL_EXPORT int MCC_ImportGlobalArray(MC_HANDLE array_handle, char *filename);
DECL_DLL_EXPORT int MCC_ExportGlobalArray(MC_HANDLE array_handle, char *filename);


//******************************************************************************
// precipitation domains ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfPrecipitationDomains();
DECL_DLL_EXPORT int MCC_IsValidPrecipitationDomainIndex(MC_INDEX domain_index);
DECL_DLL_EXPORT MC_INDEX MCC_GetPrecipitationDomainIndexFromName(const char *name);
DECL_DLL_EXPORT MC_INDEX MCC_CreatePrecipitationDomain(char *name);
//DECL_DLL_EXPORT MC_INDEX MCC_GetPrecipitationDomainIndexFromIndex(MC_INDEX index);
//DECL_DLL_EXPORT MC_HANDLE MCC_GetPrecipitationDomainHandleFromIndex(MC_INDEX index);
//DECL_DLL_EXPORT MC_HANDLE MCC_GetPrecipitationDomainHandleFromName(const char *name);
DECL_DLL_EXPORT int MCC_RemovePrecipitationDomain(MC_INDEX domain_index);
DECL_DLL_EXPORT char *MCC_GetPrecipitationDomainName(MC_INDEX domain_index);
DECL_DLL_EXPORT int MCC_SetPrecipitationDomainName(MC_INDEX domain_index, char *new_name);
// int MCC_GetPrecipitationDomainType(MC_HANDLE domain_handle);
DECL_DLL_EXPORT MC_INDEX MCC_GetActivePrecDomain();
DECL_DLL_EXPORT bool MCC_SetActivePrecDomain(MC_INDEX domain_index);

DECL_DLL_EXPORT int MCC_SetPrecDomainMatrixPhase(MC_INDEX domain_index, MC_HANDLE matrix_handle);
DECL_DLL_EXPORT MC_HANDLE MCC_GetPrecDomainMatrixPhase(MC_INDEX domain_index);

DECL_DLL_EXPORT int MCC_GetNumberOfPrecipitatesInPrecDomain(MC_INDEX domain_index);
DECL_DLL_EXPORT MC_HANDLE MCC_GetPrecipitateInPrecDomainFromIndex(MC_INDEX domain_index, int index);
// obsolete
// DECL_DLL_EXPORT int MCC_AttachPrecipitateToPrecipitationDomain(
//        MC_HANDLE domain_handle, MC_HANDLE phase_handle);
// obsolete
// DECL_DLL_EXPORT int MCC_DetachPrecipitateFromPrecipitationDomain(
//        MC_HANDLE domain_handle, MC_HANDLE phase_handle);

// DECL_DLL_EXPORT int MCC_ResetSoluteTrappingPhaseComposition(
//        MC_INDEX domain_handle);

DECL_DLL_EXPORT char *MCC_WriteOutPrecipitationDomainInfo(MC_INDEX domain_index);

DECL_DLL_EXPORT bool MCC_SetPrecipitationDomainAmountOfElement(
  MC_INDEX domain_index, int GBSIndex, double NewVal, bool InWeightPercent, int AdjustType);
DECL_DLL_EXPORT bool
MCC_GetPrecipitationDomainAmountOfElement(MC_INDEX domain_index, int GBSIndex, double *buff, bool InWeightPercent);
DECL_DLL_EXPORT bool
MCC_SetPrecipitationDomainUFractionOfElement(MC_INDEX domain_index, int GBSIndex, double NewVal, int AdjustType);
DECL_DLL_EXPORT bool MCC_GetPrecipitationDomainUFractionOfElement(MC_INDEX domain_index, int GBSIndex, double *buff);

// format: GBS_Elements.x0[]
DECL_DLL_EXPORT bool MCC_SetPrecipitationDomainGlobalComposition(MC_INDEX domain_index, double *global_composition);
// format: GBS_Elements.x0[]
DECL_DLL_EXPORT bool MCC_GetPrecipitationDomainGlobalComposition(MC_INDEX domain_index, double *global_composition);
// format: GBS_Elements.x0[]
DECL_DLL_EXPORT bool MCC_GetPrecipitationDomainMatrixComposition(MC_INDEX domain_index, double *matrix_composition);

// Grain size distribution
DECL_DLL_EXPORT int MCC_InitializeGrainSizeDistributionFromHandle(MC_HANDLE ph_matrix, int num_size_classes);
DECL_DLL_EXPORT int MCC_InitializeGrainSizeDistributionFromIndex(MC_INDEX domain_index, int num_size_classes);

DECL_DLL_EXPORT int
MCC_GetGrainSizeDistributionEntryFromHandle(MC_HANDLE ph_matrix, int row, CMCGrainSizeClass &destination);
DECL_DLL_EXPORT int
MCC_GetGrainSizeDistributionEntryFromIndex(MC_INDEX pd_index, int row, CMCGrainSizeClass &destination);

DECL_DLL_EXPORT int
MCC_GetGrainSizeDistributionEntriesFromHandle(MC_HANDLE ph_matrix, CMCGrainSizeClass **&destination);
DECL_DLL_EXPORT int MCC_GetGrainSizeDistributionEntriesFromIndex(MC_INDEX pd_index, CMCGrainSizeClass **&destination);

DECL_DLL_EXPORT int MCC_SetGrainSizeDistributionEntryFromHandle(MC_INDEX ph_matrix, int row, CMCGrainSizeClass &source);
DECL_DLL_EXPORT int MCC_SetGrainSizeDistributionEntryFromIndex(MC_INDEX pd_index, int row, CMCGrainSizeClass &source);
DECL_DLL_EXPORT int MCC_SetGrainSizeDistributionEntriesFromHandle(MC_HANDLE ph_matrix, CMCGrainSizeClass **&source);
DECL_DLL_EXPORT int MCC_SetGrainSizeDistributionEntriesFromIndex(MC_INDEX pd_index, CMCGrainSizeClass **&source);
DECL_DLL_EXPORT int MCC_GenerateGrainSizeDistributionFromHandle(
  MC_HANDLE ph_matrix, int type, double fraction, double min_radius, double mean_radius, double max_radius,
  double standard_deviation);

DECL_DLL_EXPORT int MCC_GetGrainSizeDistributionHistogramFromIndex(
  MC_INDEX pd_index, double **ptrDoubleData, int numSizeClasses, double fixedLowerValue, double fixedUpperValue,
  bool doScaleRadius, int scaleFrequency, int histogramType, bool isLogarithmic);

DECL_DLL_EXPORT int MCC_GetGrainOrientationFromHandle(MC_HANDLE ph_matrix, int orientation, int projection_type, int *num, double ***ptrDoubleData);
DECL_DLL_EXPORT int MCC_GetGrainOrientationFromIndex(MC_INDEX pd_index, int orientation, int projection_type, int *num, double ***ptrDoubleData);

// randomization of precipitation paramaters
DECL_DLL_EXPORT int MCC_RandomizePrecipitationParameters(
  MC_HANDLE phase_handle, int param_code, int randomize_type, double param1, double param2, double param3,
  double param4, double param5);

DECL_DLL_EXPORT int
MCC_GetRandomizedPrecipitationParameters(MC_HANDLE phase_handle, struct_randomize_prec_params **&params);


// slip systems
DECL_DLL_EXPORT int MCC_GetSlipSystemEntryFromHandle(MC_HANDLE ph_matrix, int row, CMCSlipSystem_Data *destination);
DECL_DLL_EXPORT int MCC_GetSlipSystemEntryFromIndex(MC_INDEX pd_index, int row, CMCSlipSystem_Data *destination);
DECL_DLL_EXPORT int MCC_GetSlipSystemEntriesFromHandle(MC_HANDLE ph_matrix, CMCSlipSystem_Data ***destination);
DECL_DLL_EXPORT int MCC_GetSlipSystemEntriesFromIndex(MC_INDEX pd_index, CMCSlipSystem_Data ***destination);
DECL_DLL_EXPORT int MCC_SetSlipSystemEntryFromHandle(MC_INDEX ph_matrix, int row, CMCSlipSystem_Data *source);
DECL_DLL_EXPORT int MCC_SetSlipSystemFromIndex(MC_INDEX pd_index, int row, CMCSlipSystem_Data *source);
DECL_DLL_EXPORT int MCC_SetSlipSystemEntriesFromHandle(MC_HANDLE ph_matrix, CMCSlipSystem_Data ***source);
DECL_DLL_EXPORT int MCC_SetSlipSystemEntriesFromIndex(MC_INDEX pd_index, CMCSlipSystem_Data ***source);
DECL_DLL_EXPORT bool MCC_ExportSlipSystemEntriesFromHandle(MC_HANDLE ph_matrix, const char *path);
DECL_DLL_EXPORT bool MCC_ExportSlipSystemEntriesFromIndex(MC_INDEX pd_index, const char *path);
DECL_DLL_EXPORT bool MCC_ExportSlipSystemEntries(const char *path, CMCSlipSystem_Data ***source);
DECL_DLL_EXPORT bool MCC_ImportSlipSystemEntriesToHandle(MC_HANDLE ph_matrix, const char *path);
DECL_DLL_EXPORT bool MCC_ImportSlipSystemEntriesToIndex(MC_INDEX pd_index, const char *path);
DECL_DLL_EXPORT bool MCC_ImportSlipSystemEntries(const char *path, CMCSlipSystem_Data ***destination);


//******************************************************************************
// precipitation domain interactions ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfPrecDomainInteractions();
DECL_DLL_EXPORT int MCC_AddPrecDomainInteraction(MC_HANDLE domain_handle_alpha, MC_HANDLE domain_handle_beta);
DECL_DLL_EXPORT int MCC_RemovePrecDomainInteraction(int index);
DECL_DLL_EXPORT int
MCC_GetPrecDomainInteraction(int index, MC_HANDLE &domain_handle_alpha, MC_HANDLE &domain_handle_beta);


//******************************************************************************
// precipitation domain solute traps ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfPrecDomainSoluteTraps(MC_INDEX domain_handle);
DECL_DLL_EXPORT int MCC_AddPrecDomainSoluteTrap(MC_INDEX domain_handle);
DECL_DLL_EXPORT int MCC_RemovePrecDomainSoluteTrap(MC_INDEX domain_handle, MC_INDEX trap_handle);
DECL_DLL_EXPORT int MCC_IsValidPrecDomainSoluteTrap(MC_INDEX domain_handle, MC_INDEX trap_handle);
DECL_DLL_EXPORT int MCC_GetPrecDomainSoluteTrapProperty(
  MC_INDEX domain_handle, MC_INDEX trap_handle, int prop_code, int &int_buff, double &double_buff, void *&void_ptr);
DECL_DLL_EXPORT int MCC_SetPrecDomainSoluteTrapProperty(
  MC_INDEX domain_handle, MC_INDEX trap_handle, int prop_code, int int_buff, double double_buff, void *void_ptr);
DECL_DLL_EXPORT int MCC_InitializeKineticTrappingParameters(MC_INDEX domain_handle);


//******************************************************************************
// precipitation domain couples&pairs ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumberOfPrecDomainCouplesPairs(MC_INDEX domain_handle);
DECL_DLL_EXPORT int MCC_AddPrecDomainCouplePair(MC_INDEX domain_handle);
DECL_DLL_EXPORT int MCC_RemovePrecDomainCouplePair(MC_INDEX domain_handle, MC_INDEX cp_handle);
DECL_DLL_EXPORT int MCC_IsValidPrecDomainCouplePair(MC_INDEX domain_handle, MC_INDEX cp_handle);
DECL_DLL_EXPORT MC_HANDLE MCC_GetCouplePairHandle(MC_INDEX domain_handle, MC_INDEX cp_handle);
DECL_DLL_EXPORT int MCC_GetPrecDomainCouplePairProperty(
  MC_INDEX domain_handle, MC_INDEX cp_handle, int prop_code, int &int_buff, double &double_buff, void *&void_ptr);
DECL_DLL_EXPORT int MCC_SetPrecDomainCouplePairProperty(
  MC_INDEX domain_handle, MC_INDEX cp_handle, int prop_code, int int_buff, double double_buff, void *void_ptr);


//******************************************************************************
// heat treatments ...
//******************************************************************************

DECL_DLL_EXPORT MC_HANDLE MCC_TrimHeatTreatment(MC_HANDLE ht_handle, double accuracy);
DECL_DLL_EXPORT int MCC_GetNumberOfHeatTreatments();
DECL_DLL_EXPORT MC_HANDLE MCC_GetHTHandleByIndex(int index);
DECL_DLL_EXPORT MC_HANDLE MCC_GetHTHandleByName(char const*name);
DECL_DLL_EXPORT int MCC_GetHTIndexByName(char const*name);
DECL_DLL_EXPORT int MCC_GetHTSegmentIndexByHandle(MC_HANDLE ht_handle, MC_HANDLE seg_handle);

DECL_DLL_EXPORT MC_HANDLE MCC_CreateNewHeatTreatment(char const*name);
DECL_DLL_EXPORT int MCC_SetHeatTreatmentName(MC_HANDLE handle, char *name);
DECL_DLL_EXPORT int MCC_RemoveHeatTreatment(MC_HANDLE ht_handle);

DECL_DLL_EXPORT char *MCC_GetHeatTreatmentName(MC_HANDLE ht_handle);
DECL_DLL_EXPORT int MCC_GetNumberOfHTSegments(MC_HANDLE ht_handle);
DECL_DLL_EXPORT MC_HANDLE MCC_GetHTSegmentHandleFromIndex(MC_HANDLE ht_handle, int index);
DECL_DLL_EXPORT MC_HANDLE MCC_GetHTSegmentHandleFromTime(MC_HANDLE ht_handle, double time);
DECL_DLL_EXPORT double MCC_GetHTTemperatureFromTime(MC_HANDLE ht_handle, double time);
DECL_DLL_EXPORT int MCC_AppendNewHTSegment(MC_HANDLE ht_handle);
DECL_DLL_EXPORT int MCC_InsertHTSegment(MC_HANDLE ht_handle, int index);
DECL_DLL_EXPORT int MCC_RemoveHTSegment(MC_HANDLE ht_handle, MC_HANDLE seg_handle);

DECL_DLL_EXPORT int
MCC_GetHTSegmentProperty(MC_HANDLE seg_handle, int prop_code, int &int_buff, double &double_buff, void *&void_ptr);
DECL_DLL_EXPORT int
MCC_SetHTSegmentProperty(MC_HANDLE seg_handle, int prop_code, int int_buff, double double_buff, void *void_ptr);
DECL_DLL_EXPORT int MCC_ImportHeatTreatment(MC_HANDLE ht_handle, char *filename);

DECL_DLL_EXPORT int MCC_ImportAndAppendHeatTreatment(MC_HANDLE ht_handle, char *filename);
DECL_DLL_EXPORT int MCC_ImportNewHeatTreatment(MC_HANDLE ht_handle, char *filename);

DECL_DLL_EXPORT int MCC_ExportHeatTreatment(MC_HANDLE ht_handle, char *filename);
DECL_DLL_EXPORT MC_INDEX
MCC_SetPrecDomainFromHeatTreatment(MC_HANDLE ht_handle, double time, MC_INDEX prev_domain_index);


//******************************************************************************
// Precipitate calculation ...
//******************************************************************************

DECL_DLL_EXPORT int MCC_CalcKineticStepEndTime(double end_time, bool SupressStartAsThread, int output_flags);

DECL_DLL_EXPORT int MCC_CalcKineticTTPDiagram(bool SupressStartAsThread, int output_flags);

DECL_DLL_EXPORT int MCC_CalcKineticStep(bool SupressStartAsThread, int output_flags);

// DECL_DLL_EXPORT int MCC_CalcNucleusComposition(MC_HANDLE phase_handle,
//                                               int DoInit,
//                                               int output_flags);
DECL_DLL_EXPORT int MCC_CalcNucleusCompositions(MC_HANDLE phase_handle, int DoInit, int output_flags);
DECL_DLL_EXPORT int MCC_SetNucleusComposition(MC_HANDLE ph_handle, int num_vars, char **new_values);
DECL_DLL_EXPORT int MCC_GetNucleusComposition(MC_HANDLE ph_handle, int var_index, const char *&func_string);

DECL_DLL_EXPORT int MCC_SetRelaxedPrecipitateState(MC_HANDLE ph_handle);

DECL_DLL_EXPORT int MCC_GetKineticPropertyFromHandle(
  MC_HANDLE ph_handle, int type_code, int &int_var, double &double_var, void *&extra_ptr);
DECL_DLL_EXPORT int
MCC_SetKineticPropertyFromHandle(MC_HANDLE ph_handle, int type_code, int int_var, double double_var, void const*extra_ptr);

DECL_DLL_EXPORT int MCC_InitializeKineticCalc(MC_HANDLE ph_handle, int num_size_classes);
DECL_DLL_EXPORT int MCC_GeneratePrecipitateDistributionFromHandle(
  MC_HANDLE ph_matrix, int type, double min_radius, double mean_radius, double max_radius, double standard_deviation);

DECL_DLL_EXPORT int MCC_GetPrecArrayEntry(MC_HANDLE ph_handle, int row, int col, double *destination);
DECL_DLL_EXPORT int MCC_SetPrecArrayEntry(MC_HANDLE ph_handle, int row, int col, double new_value);

DECL_DLL_EXPORT int MCC_SetCompositionFromPrecArray(MC_HANDLE ph_handle);

DECL_DLL_EXPORT int MCC_IsPhaseKineticPrecipitate(MC_HANDLE phase_handle);
DECL_DLL_EXPORT int MCC_IsPhaseKineticParent(MC_HANDLE phase_handle);
DECL_DLL_EXPORT int MCC_IsPhaseKineticMatrix(MC_HANDLE phase_handle);
DECL_DLL_EXPORT int MCC_IsPhaseKineticTrapping(MC_HANDLE phase_handle);

DECL_DLL_EXPORT char *MCC_WriteOutKineticInfoForPhase(MC_HANDLE phase_handle);
DECL_DLL_EXPORT char *MCC_WriteOutPrecDistributionForPhase(MC_HANDLE phase_handle);
DECL_DLL_EXPORT char *MCC_WriteOutKineticInfoForSimulation();
DECL_DLL_EXPORT char *MCC_WriteOutKineticInfoForTTPSimulation();


//******************************************************************************
// TTP-Buffer manipulation
//******************************************************************************

DECL_DLL_EXPORT int MCC_GetNumRecordsInTTPBuffer();
DECL_DLL_EXPORT int MCC_GetTTPRecordData(int rec_index, int &num_entries, int &T_type, double &step_value);
DECL_DLL_EXPORT int MCC_ClearTTPBuffer();
DECL_DLL_EXPORT int MCC_RemoveTTPRecord(int rec_index);
DECL_DLL_EXPORT int MCC_AppendBufferToTTPBuffer(MC_INDEX BufferHandle, int T_type, double step_value);

DECL_DLL_EXPORT int MCC_GetKineticTTPLine(
  char *phase_names, double line_fract, int ttp_line_type, double ttp_min_fraction, double *x_data, double *y_data);
DECL_DLL_EXPORT int
MCC_GetKineticTTPTemperatureLine(double step_value, int &num_points, double *&x_data, double *&y_data);

DECL_DLL_EXPORT char *MCC_WriteOutTTPBufferContents();


// LICENSES {
DECL_DLL_EXPORT int MCC_LoadLicenseFile(char const* const license_file_name);
DECL_DLL_EXPORT int MCC_ActivateLicenseFile(char const *const license_file_name);
DECL_DLL_EXPORT int MCC_DeactivateLicenseFile(char const *const license_file_name);
DECL_DLL_EXPORT int MCC_RetrieveNetworkLicense();
DECL_DLL_EXPORT int MCC_RetrieveNetworkLicenseFrom(unsigned long int ipv4_address_as_integer); //< QHostAddress.toInt()!
DECL_DLL_EXPORT void MCC_ListLicenseFiles();
DECL_DLL_EXPORT void MCC_ListLicenseFilesDetails();
DECL_DLL_EXPORT void MCC_ListLicenses();
DECL_DLL_EXPORT void MCC_ListLicensesOf(char const *const license_file_name);

DECL_DLL_EXPORT void MCC_PrintNodeAndUserId();

// } LICENSES


//******************************************************************************
// temporary
//******************************************************************************


DECL_DLL_EXPORT double MCC_GetNucleationFreeEnergy(
  MC_HANDLE PhaseHandle, double radius, bool with_di, bool with_ie, bool with_ve, bool with_het);

DECL_DLL_EXPORT double MCC_GetSDSFGXXA(int index);
DECL_DLL_EXPORT double MCC_GetSDSFGXXA2(int index);
DECL_DLL_EXPORT double MCC_GetSDSFGXXA3(int index);
DECL_DLL_EXPORT double MCC_GetSDSFGBIS(double dmubt);
DECL_DLL_EXPORT int MCC_GetNumSDSFGPoints();
DECL_DLL_EXPORT bool MCC_UpdateSoluteDrag();



//******************************************************************************
// SimpleMSE
//******************************************************************************
DECL_DLL_EXPORT int MCC_SimpleMseInitialize();
DECL_DLL_EXPORT int MCC_SimpleMseReset();
DECL_DLL_EXPORT int MCC_SimpleMsePerformCalcStep(double delta_t, int heat_treatment_index, bool append);
DECL_DLL_EXPORT int MCC_SimpleMseListParams();
DECL_DLL_EXPORT int MCC_SimpleMseResultAt_v1(int index, CMCSimpleMSE_Data_v1 *result);
DECL_DLL_EXPORT int MCC_SimpleMseResults_v1(CMCSimpleMSE_Data_v1 **results);
DECL_DLL_EXPORT int MCC_SimpleMseRegisterDataResetCallback(
        void *user_data,
        void (*callback)(void *user_data),
        int *id);
DECL_DLL_EXPORT int MCC_SimpleMseRegisterDataChangedCallback(
        void *user_data,
        void (*callback)(void *user_data, int at, CMCSimpleMSE_Data_v1 data),
        int *id);
DECL_DLL_EXPORT int MCC_SimpleMseRegisterDataInsertedCallback(
        void *user_data,
        void (*callback)(void *user_data, int at, CMCSimpleMSE_Data_v1 data),
        int *id);
DECL_DLL_EXPORT int MCC_SimpleMseRegisterDataRemovedCallback(
        void *user_data,
        void (*callback)(void *user_data, int begin, int end),
        int *id);
DECL_DLL_EXPORT int MCC_SimpleMseRegisterDoneCallback(
        void *user_data,
        void (*callback)(void *user_data),
        int *id);
DECL_DLL_EXPORT int MCC_SimpleMseUnregisterCallback(void *user_data, int id);
DECL_DLL_EXPORT int MCC_SimpleMseBlockCallbacks(bool block);

DECL_DLL_EXPORT int MCC_SimpleMseVariableCategories(int *size, int** categories);
DECL_DLL_EXPORT int MCC_SimpleMseVariableCategories_destroy(int size, int* categories);
DECL_DLL_EXPORT int MCC_SimpleMseVariableCategoryName(int category, char const** name);
DECL_DLL_EXPORT int MCC_SimpleMseVariables(
        int *size,
        char const*** names,
        char const*** descriptions,
        char*** descriptors,
        int category);
DECL_DLL_EXPORT int MCC_SimpleMseVariables_destroy(
        int size,
        char const** names,
        char const** descriptions,
        char** descriptors);
DECL_DLL_EXPORT int MCC_SimpleMseVariableDescriptor(int code, char const** descriptor);
DECL_DLL_EXPORT int MCC_SimpleMseVariableDescriptorByName(char const* name, char const** descriptor);
DECL_DLL_EXPORT int MCC_SimpleMseVariableCode(char const* name, int *code);
DECL_DLL_EXPORT int MCC_SimpleMseVariableNames(int *size, char const*** names, int categories);
DECL_DLL_EXPORT int MCC_SimpleMseVariableNames_destroy(int size, char const** names);
DECL_DLL_EXPORT int MCC_SimpleMseVariableDescriptions(int *size, char const*** descriptions, int categories);
DECL_DLL_EXPORT int MCC_SimpleMseVariableDescriptions_destroy(int size, char const** descriptions);
DECL_DLL_EXPORT int MCC_SimpleMseVariableValue(int code, CMCSimpleMSE_VariableData *variable);
DECL_DLL_EXPORT int MCC_SimpleMseSetVariableDescriptor(int code, char const* descriptor);


DECL_DLL_EXPORT int MCC_ColineOutputSinkCount( int* out_value );
DECL_DLL_EXPORT int MCC_ColineOutputSinkName( int sink_idx, char const** out_ptr );
DECL_DLL_EXPORT int MCC_ColineOutputSinkEnable( int sink_idx );
DECL_DLL_EXPORT int MCC_ColineOutputSinkDisable( int sink_idx );
DECL_DLL_EXPORT int MCC_ColineOutputSinkGetCategory( int sink_idx, int* out_value );
DECL_DLL_EXPORT int MCC_ColineOutputSinkSetCategory( int sink_idx, int category_value );
DECL_DLL_EXPORT int MCC_ColineOutputSinkAdd( char const* name_of_sink, int* out_sink_idx );
DECL_DLL_EXPORT int MCC_ColineOutputSinkDelete( int sink_idx );



#endif // MCR
#endif // INCLUDE_DLL_IMPORT_H
