//*****************************************************
//
// Dieses file enthaelt alle Definitionen, die
// von MatCalc 2.0 und der mc_core.dll verwendet
// werden.
//
// (C) Ernst Kozeschnik
//
//*****************************************************

//*****************************************************
// Uebergabestrukturen
//*****************************************************

#ifndef mc_defines_h
#define mc_defines_h

const double const_pi=3.1415926535897932384626433832795028841971693993751;
const double const_NA=6.02214199e23;
const double const_kB=1.3806503e-23;
const double const_Rg=const_NA*const_kB;
const double const_ec=1.602176565e-19;


//*****************************************************
// licensing

#define LT_FREE					0
#define LT_FULL					10


#define LT2_UNLIMITED			1
#define LT2_SINGLE_UNBOUND		10
#define LT2_HARDWARE_BOUND		50
#define LT2_FLOATING			100


#define LC_LICENSE_OK					1
#define LC_INVALID_HARDWARE_ID			10
#define LC_LICENSE_HAS_EXPIRED			20

//info
#define LIT_LICENCE_FILE_PATH				1
#define LIT_LICENCE_TYPE_1					10
#define LIT_LICENCE_TYPE_2					11
#define LIT_LICENSE_ID						50
#define LIT_USER_NAME						100
#define LIT_USER_ORGANIZATION				101
#define LIT_VALID_UNTIL						110
#define LIT_UPGRADE_UNTIL					115
#define LIT_MAINTENANCE_UNTIL				116
#define LIT_ENABLED_MODULES					200
#define LIT_ENABLED_MODULES_UNFORMATTED		201

//module definitions ...
#define MOD_CORE						0x80000000	//should be always true
#define MOD_MULTI_COMP_THERMODYNAMICS	0x40000000	//demo version gives zero, only full version
#define MOD_PRECIPITATION_KINETICS		0x00000001	//precipitation kinetics
#define MOD_CAFE_SIMULATION				0x00000010	//diffusion simulation
#define MOD_MONTE_CARLO					0x00000020	//monte carlo simulation
#define MOD_MICROSTRUCTURE_SIM          0x00100000  //MSE - central (new) module in MatCalc 6.x


//modules (is index in array !!!)
#define MC_MODULE_CORE		0
#define MC_MODULE_CAFE		1
#define MC_MODULE_MONTE		2
#define MC_MODULE_REGION	3

//Autosave
#define AST_NONE			0
#define AST_BINARY			1
#define AST_WORKSPACE		5

//*****************************************************
//kinetic store

#define KST_NONE		0
#define KST_LIN			1
#define KST_LOG			10
#define KST_COUNT		20
#define KST_AUTO		100
			

//*****************************************************
// Windows Messages, die an den Anwendungsrahmen
// geschickt werden.
//*****************************************************

//non-windows (e.g. Qt)
#ifdef MY_QT
#define WM_USER_MC QEvent::User
#else
#define WM_USER_MC 1024
#endif

#define MCCM_READY						WM_USER_MC + 10	//MCCORE ist fertig/bereit
#define MCCM_STATUSCHANGED				WM_USER_MC + 11	//irgendetwas ist anders
#define MCCM_SENDINGINFO				WM_USER_MC + 20	//wird gesendet, wenn auf die Informationsanfrage geantwortet wird
#define MCCM_MESSAGE_BEEP				WM_USER_MC + 21	//beep Signal

#define MCCM_CHECKCOMPOSITION			WM_USER_MC + 50	//Ueberpruefung beim Linken -> Zusammensetzung nicht eingegeben
#define MCCM_BUFFERDATACHANGED			WM_USER_MC + 51	//wird gesendet, wenn sich der Inhalt eines Buffers geaendert hat (MC_core)
#define MCCM_BUFFER_ADDED_REMOVED		WM_USER_MC + 52	//wird gesendet, wenn sich der Inhalt eines Buffers geaendert hat (MC_core)
#define MCCM_UPDATE_BUFFER_STATE_SEL	WM_USER_MC + 53	//wird gesendet, wenn Buffer/State selection geaendert (MC_core)
#define MCCM_SIMBUFFERDATACHANGED		WM_USER_MC + 60	//wird gesendet, wenn sich der Inhalt eines Buffers geaendert hat (MC_real)
#define MCCM_STATIONARYDATACHANGED		WM_USER_MC + 61	//wird gesendet, wenn sich der Inhalt nach einer stationary geaendert hat (MC_real)
#define MCCM_BUFFERDATAAPPEND			WM_USER_MC + 62	//wird gesendet, wenn sich der Inhalt eines Buffers geaendert hat (MC_core)
#define MCCM_SIMBUFFERDATAAPPEND		WM_USER_MC + 63	//wird gesendet, wenn sich der Inhalt eines Buffers geaendert hat (MC_real)
#define MCCM_CAFD_1D_DATACHANGED		WM_USER_MC + 65	//wird gesendet, wenn sich der Inhalt fuer 1-D CAFD Fenster geaendert hat (MC_fdtr)
#define MCCM_CAFD_2D_DATACHANGED		WM_USER_MC + 66	//wird gesendet, wenn sich der Inhalt fuer 2-D CAFD Fenster geaendert hat (MC_fdtr)
#define MCCM_SIMULATIONFINISHED			WM_USER_MC + 70	//wird gesendet, wenn sich der Inhalt eines Buffers geaendert hat (MC_real)
#define MCCM_STATIONARYCALCFINISHED		WM_USER_MC + 71	//wird gesendet, wenn sich der Inhalt eines Buffers geaendert hat (MC_real)
#define MCCM_CAFD_SIMULATIONFINISHED	WM_USER_MC + 72	//wird gesendet, wenn sich der Inhalt eines Buffers geaendert hat (MC_fdtr)
#define MCCM_TABLEDATACHANGED			WM_USER_MC + 80	//wird gesendet, wenn sich table geaendert hat (MC_core)
#define MCCM_REGION_BUFFERDATACHANGED	WM_USER_MC + 85	//wird gesendet, wenn sich region buffer geaendert hat (MC_region)
#define MCCM_REGION_BUFFERDATAAPPEND	WM_USER_MC + 86	//wird gesendet, wenn sich der Inhalt eines REgion Buffers geaendert hat (MC_core)

#define MCCM_WRITEOUT					WM_USER_MC + 100	//in Outputfenster schreiben
#define MCCM_WRITESTATUS				WM_USER_MC + 101	//in Statuszeile schreiben
#define MCCM_WRITEOUTCONSOLE			WM_USER_MC + 102	//in Konsolen-fenster schreiben
#define MCCM_READFROMCONSOLE			WM_USER_MC + 103	//von Konsolen-fenster lesen
#define MCCM_WRITEOUTPROJECT			WM_USER_MC + 105	//in Project-Outputfenster schreiben
#define MCCM_UPDATEPROGRESS				WM_USER_MC + 110	//Progress-Window update
#define MCCM_UPDATEMEMORYUSAGE			WM_USER_MC + 111	//Memory-Usage update
#define MCCM_RESETPROGRESS				WM_USER_MC + 112	//Progress-Window reset
#define MCCM_UPDATEPROGRESSINDICATOR	WM_USER_MC + 113	//Progress-Indicator update
#define MCCM_SETPROGRESS				WM_USER_MC + 114	//Progress-Window set
#define MCCM_UPDATEQACTIONSTATES		WM_USER_MC + 130	//alle QAction aktualisieren
#define MCCM_ECHOERROR					WM_USER_MC + 150	//Error aufgetreten ...
#define MCCM_WAITINGONEVENT				WM_USER_MC + 151	//SysCalcDebug ...
#define MCCM_ASKUSER					WM_USER_MC + 152	//User interaction ...

#define MCCM_WORKINGDIRCHANGED			WM_USER_MC + 160	//current directory has changed

#define MCCM_CONFIRMUSERBREAK			WM_USER_MC + 200	//soll der Userbreak wirklich durchgefuehrt werden?
#define MCCM_CONFIRMERRORBREAK			WM_USER_MC + 201	//Fehler bei Step: soll abgebrochen werden?
#define MCCM_ASKCHANGESTEPDIR			WM_USER_MC + 205	//in andere Richtung weiter-steppen?
#define MCCM_EDITNEXTITEM				WM_USER_MC + 210	//fahre mit edit fort bei naechstem item (GUI)
#define MCCM_PROCESS_COMMAND			WM_USER_MC + 215	//process command for script Verarbeitung aus Threads (GUI)
#define MCCM_RUN_SCRIPT_FINISHED		WM_USER_MC + 217	//script finished for script Verarbeitung aus Threads (GUI)

#define MCCM_OPENDATABASEFINISHED		WM_USER_MC + 1000	//DoOpenDatabase() Thread finished
#define MCCM_OPENDIFFDATAFINISHED		WM_USER_MC + 1001	//DoOpenDatabase() Thread finished
#define MCCM_OPENPHYSDATAFINISHED		WM_USER_MC + 1002	//DoOpenDatabase() Thread finished
#define MCCM_READDATABASEFINISHED		WM_USER_MC + 1003	//DoReadDatabase() Thread finished
#define MCCM_READDIFFDATAFINISHED		WM_USER_MC + 1004	//DoReadDatabase() Thread finished
#define MCCM_READPHYSDATAFINISHED		WM_USER_MC + 1005	//DoReadDatabase() Thread finished
#define MCCM_FILEREADFINISHED			WM_USER_MC + 1006	//DoReadFile() Thread finished
#define MCCM_CALCEQUILIBFINISHED		WM_USER_MC + 1020	//CalcEquilibrium() Thread finished
#define MCCM_STEPEQUILIBFINISHED		WM_USER_MC + 1021	//CalcEquilibriumStep() Thread finished
#define MCCM_CALCSOLTEMPFINISHED		WM_USER_MC + 1022	//CalcSolubilityTemperature() Thread finished
#define MCCM_CALCT0TEMPFINISHED			WM_USER_MC + 1023	//CalcT0Temperature() Thread finished
#define MCCM_CALCTIELINESFINISHED		WM_USER_MC + 1024	//CalcTieLines() Thread finished
#define MCCM_SCHEILCALCFINISHED			WM_USER_MC + 1025	//CalcScheilReaction() Thread finished
#define MCCM_CALCMULTIPARAMSTEPFINISHED	WM_USER_MC + 1026	//CalcMultiParamStep() Thread finished
#define MCCM_CALCKINETICSTEPFINISHED	WM_USER_MC + 1030	//CalcKineticTimeStep() Thread finished
#define MCCM_CALCKINETICTTPDIAGRAMFINISHED	WM_USER_MC + 1031	//CalcKineticTTPDiagram() Thread finished
#define MCCM_GETBUFFERVARSFINISHED		WM_USER_MC + 1050	//GetBufferVariables() Thread finished
#define MCCM_GETSIMBUFFERVARSFINISHED	WM_USER_MC + 1051	//GetBufferVariables() Thread finished

#define MCCM_REMOVE_PLOT_WITH_ID        WM_USER_MC + 1100   //remove plot (from RMB menu of plot)

//grid simulation ...
#define MCCM_STARTSIMULATIONFINISHED	WM_USER_MC + 2000	//StartSimulation() Thread finished

//monte carlo ...
#define MCCM_UPDATEMONTE				WM_USER_MC + 3000	//update monte windows

//*****************************************************
// Topics fuer Info-Anfragen
//*****************************************************

#define TOPIC_PHASES_GENERAL_INFO			10
#define TOPIC_PHASES_SUMMARY				11
#define TOPIC_PHASE_DATABASE_INFO			12
#define TOPIC_SYMBOL_DATABASE_INFO			13
#define TOPIC_CHEM_POTS_GENERAL				14
#define TOPIC_PHASE_NNDATABASE_INFO			15
#define TOPIC_VARIABLES						20
#define TOPIC_BUFFER_RESULTS				21
#define TOPIC_DIFFUSION_DATA				22
#define TOPIC_PARTICLE_DISTRIBUTION			30
#define TOPIC_GRAIN_DISTRIBUTION			31
#define TOPIC_PHASE_NUCLEATION_INFO			36
#define TOPIC_PHASE_KIN_ACCEL_INFO			37
#define TOPIC_KIN_ACCEL_SUMMARY 			38
#define TOPIC_FSK_TRAP_PART_TABLE			40
#define TOPIC_DEBUG_DX_ARRAYS				100
#define TOPIC_DEBUG_JACO_MATRIX				101
#define TOPIC_DEBUG_GE_DERIVATIVES			102
#define TOPIC_DEBUG_DFM_DERIVATIVES			103
#define TOPIC_DEBUG_MU_DERIVATIVES			104
#define TOPIC_DEBUG_FE_DERIVATIVES			105
#define TOPIC_DEBUG_KINETIC_MATRICES		110
#define TOPIC_DEBUG_SD_SFG                  120

//first CAFE topic starts at 300...


//*****************************************************
// Globale Variablen
//*****************************************************

#define MCGV_SMALL_VALUE						10			//double
#define MCGV_JACO_ANALYTICAL					20			//bool
#define MCGV_LSM_METHODE						21			//int
#define MCGV_REDUCE_PB							22			//bool
#define MCGV_REDUCE_SFB							23			//bool
#define MCGV_REDUCE_SFESCALE					24			//bool
#define MCGV_USE_EQUI_SYSTEM_FOR_CP				25			//bool
#define MCGV_USE_THERMOCALC_COMPATIBILITY		26			//bool
#define MCGV_COMPOSITION_TYPE					100			//int
#define MCGV_STEPGLOBALELEMENTCONTENT			101			//bool
#define MCGV_STEPPARAEQUILIBCONSTRAINT			102			//bool
#define MCGV_CURRENT_STEPSTART					200			//double
#define MCGV_CURRENT_STEPSTOP					201			//double
#define MCGV_CURRENT_STEPWIDTH					202			//double
#define MCGV_CURRENT_ELEMENT_IN_STEP			203			//int
#define MCGV_CURRENT_STEP_TYPE					204			//int
#define MCGV_ELEMENT_STEP_TEMPERATURE			205			//double
#define MCGV_CURRENT_SOLPHASE_IN_STEP			206			//int
#define MCGV_CURRENT_DEPT0PHASE_IN_STEP			207			//int
#define MCGV_CURRENT_PARENTT0PHASE_IN_STEP		208			//int
#define MCGV_CURRENT_STEPT0OFFSET_IN_STEP		209			//double -> function since 6.00.0.296
#define MCGV_MAX_DT_IN_SOLTEMP_STEP				210			//double
#define MCGV_CURRENT_STEP_FLAGS					215			//int
#define MCGV_SCHEIL_STEPSTART					300			//double
#define MCGV_SCHEIL_STEPSTOP					301			//double
#define MCGV_SCHEIL_STEPWIDTH					302			//double
#define MCGV_SCHEIL_DEPENDENT_PHASE				305			//handle
#define MCGV_SCHEIL_MIN_LIQUID_FRACTION			306			//double
#define MCGV_SCHEIL_BACK_DIFFUSION				310			//handle
#define MCGV_CURRENT_SEARCH_BOUNDARY_TYPE		400			//int
#define MCGV_CURRENT_SEARCH_TARGET_PHASE		405			//int
#define MCGV_CURRENT_SEARCH_PARENT_PHASE		406			//int
#define MCGV_CURRENT_SEARCH_ELEMENT				410			//int
#define MCGV_CURRENT_SEARCH_TEMPERATURE		411			//double
#define MCGV_CURRENT_SEARCH_T_IN_C				412			//int
#define MCGV_CURRENT_SEARCH_DFM_OFFSET			420			//double
#define MCGV_CURRENT_SEARCH_FORCE_COMP			421			//int

//Kinetik
#define MCGV_KIN_SIM_END_TIME					1000			//double
#define MCGV_KIN_SIM_END_TEMP					1001			//double
#define MCGV_KIN_MANUAL_TIME_STEP_WIDTH         1005			//double
#define MCGV_KIN_TIME_STEP_CONTROL_TYPE         1006			//int
#define MCGV_KIN_INITIAL_TIME_STEP				1007			//double
#define MCGV_KIN_MAXIMUM_TIME_STEP				1008			//double
#define MCGV_KIN_T_CONTROL_TYPE					1010			//int
#define MCGV_KIN_ISOTHERMAL_T					1015			//double
#define MCGV_KIN_T_HEAT_TREATMENT_INDEX         1016			//int
#define MCGV_KIN_T_HEAT_TREATMENT_HANDLE        1017			//handle
#define MCGV_KIN_GLOBAL_EPS_DOT					1018			//handle
#define MCGV_KIN_GLOBAL_STRESS_LOAD				1019			//handle
#define MCGV_KIN_UPDATE_EVERY					1020			//int
#define MCGV_KIN_STORE_EVERY					1021			//int
#define MCGV_KIN_LIN_STORE_EVERY				1022			//double
#define MCGV_KIN_LOG_STORE_EVERY				1023			//double
#define MCGV_KIN_STORE_TYPE						1024			//int
#define MCGV_KIN_T_IN_C							1025			//int
#define MCGV_KINETIC_FLAGS						1030			//int
#define MCGV_KIN_LOAD_STATE						1040			//int
#define MCGV_KIN_MANUAL_TIME_STEP_INCREASE		1041			//double
#define MCGV_KIN_CONTI_T_DOT					1042			//double
#define MCGV_KIN_MAX_T_STEP_WIDTH				1045			//double
//#define MCGV_KIN_THERMODYNAMIC_MODEL			1050			//int
#define MCGV_NUCLEATION_ENHANCEMENT				1060			//double
//#define MCGV_KIN_TIME_INTEGR_MODEL				1065			//int

#define MCGV_KIN_MASS_BALANCE_TOLERANCE		1070			//double
#define MCGV_KIN_STOP_NUCLEATION_FRACTION		1071			//double

#define MCGV_FIRST_MAX_NUCL_DENSITY				1100			//double
#define MCGV_REL_MATRIX_CHANGE_NUCL				1101			//double
#define MCGV_NUCL_DENSITY_INC_FACT				1102			//double
#define MCGV_MAX_DFM_CHANGE_IS_FACT				1105			//double
#define MCGV_MIN_DFM_FOR_GROWTH_CORR			1106			//double
#define MCGV_MAX_VOL_FRACTION_ON_DISSOLVE		1107			//double
#define MCGV_REL_RADIUS_CHANGE_GROWTH			1110			//double
#define MCGV_REL_RADIUS_CHANGE_SHRINK			1111			//double
#define MCGV_REL_VOL_CHANGE_GROWTH				1114			//double
#define MCGV_REL_VOL_CHANGE_SHRINK				1115			//double
#define MCGV_REL_VA_SITE_FRACTION_CHANGE		1116			//double
#define MCGV_REL_ACC_EPS_CHANGE                 1117			//double
#define MCGV_REL_DISLOCATION_DENSITY_CHANGE		1118			//double
#define MCGV_REL_DISLOCATION_DENSITY_CHANGE_MC	1119			//double
#define MCGV_REL_SUBGRAIN_DIAMETER_CHANGE		1120			//double
#define MCGV_REL_GRAIN_DIAMETER_CHANGE_SC		1121			//double
#define MCGV_REL_GRAIN_DIAMETER_GROWTH_MC		1122			//double
#define MCGV_REL_GRAIN_DIAMETER_SHRINK_MC		1123			//double
#define MCGV_REL_REXX_FRACTION_CHANGE			1125			//double
#define MCGV_REL_REXX_VOL_CHANGE_NUCL			1126			//double
#define MCGV_REXX_MAX_INIT_NUCL_DENSITY 		1127			//double
#define MCGV_REXX_NUCL_DENSITY_INC_FACT 		1128			//double
#define MCGV_REL_COMP_CHANGE_IN_PREC			1129			//double
#define MCGV_REL_COMP_CHANGE_IN_MATRIX			1130			//double
#define MCGV_REL_COMP_CHANGE_BY_COMP_CONTROL	1131			//double
#define MCGV_REL_PART_RATIO_CHANGE				1133			//double
#define MCGV_MAX_STD_DEVIATION_CHANGE			1134			//double
#define MCGV_REL_DFM_CHANGE						1135			//double
#define MCGV_MIN_DFM_FOR_MCOMP_ITER				1136			//double
#define MCGV_DIFF_RED_FOR_NEGATIVE_DFM			1137			//double
#define MCGV_REL_STRESS_LOAD_CHANGE_RELAX  			1138			//double
#define MCGV_REL_STRESS_LOAD_CHANGE_DISPL  			1139			//double
//#define MCGV_USE_INDEPENDENT_VARIABLES			1140			//int
#define MCGV_KINETIC_PRECISION					1141			//double
//#define MCGV_ADVANCED_VARIABLE_INTEGRATION		1145			//int
#define MCGV_DFM_COMPENSATION_RATE				1143			//double
#define MCGV_WEIGHTED_IPDCF						1144			//int
#define MCGV_KIN_ACCELERATOR					1145			//int
#define MCGV_NUMBER_OF_SFG_INTEGRAL_NODES       1146            //int
#define MCGV_PREDICTED_PREC_PARAMETERS			1147			//int
#define MCGV_USE_OSCILLATION_DAMPING			1148			//int
#define MCGV_IGNORE_DATABASE_T_LIMITS   		1149			//int

#define MCGV_DEBUG_KINETIC_STEPSIZE_2DERIVS		1150			//double
#define MCGV_DEBUG_KINETIC_STEPSIZE_MAT_CHANGE	1151			//double
#define MCGV_DEBUG_KINETIC_ARRAY_INDEX			1152			//int
#define MCGV_DEBUG_KINETIC_MATRIX_FLAGS			1153			//int

#define MCGV_TTP_UPPER_TEMPERATURE				1200			//double
#define MCGV_TTP_LOWER_TEMPERATURE				1201			//double
#define MCGV_TTP_MAX_TIME						1202			//double
#define MCGV_TTP_T_CONTROL_TYPE					1205			//int
#define MCGV_TTP_DELTA_TEMPERATURE				1210			//double
#define MCGV_TTP_MIN_TEMPERATURE_GRAD			1220			//double
#define MCGV_TTP_MAX_TEMPERATURE_GRAD			1221			//double
#define MCGV_TTP_DELTA_TEMPERATURE_GRAD_FACT	1222			//double
#define MCGV_TTP_STOP_IF_N_DECR_FACT			1225			//double

#define MCGV_ELEMENTS_ADJUST_TYPE               1250            //int

// ********** Composition Types ******************

#define CT_MOLE_FRACTION			1
#define CT_WEIGHT_FRACTION			2
#define CT_WEIGHT_PERCENT			3
#define CT_U_FRACTION				4
#define CT_MOLE_PERCENT             5
#define CT_U_PERCENT                6

// ********** Composition Set Properties ******************

#define CS_NAME                     1
#define CS_COMP_TYPE                2
#define CS_COMP                     3
#define CS_REF_EL_INDEX             4

// *********** randomize types
#define RMT_INTERFACE_ENERGY		1
#define RMT_DRIVING_FORCE			2
#define RMT_DIFFUSION				3

#define RMT_CONSTANT_UNITY          1
#define RMT_FULL_RANDOM_MULT        10
#define RMT_FULL_RANDOM_ADD         11
#define RMT_LINEAR_MIN_MAX_MULT     20
#define RMT_LINEAR_MIN_MAX_ADD      21

// ************ diffusion geometry (make sure index is continuous. is used in dialog box!)
#define DG_NONE                     0
#define DG_SPHERICAL				1
#define DG_GRAIN_BOUNDARY			2
#define DG_GB_AUTOMATIC 			3


//*****************************************************
// Flags fuer Step
//*****************************************************

#define CES_TYPEEXTERNAL				0
#define CES_TYPETEMPERATURE				1
#define CES_TYPEELEMENTCONTENT			2
//#define CES_TYPEPRESSURE				3
#define CES_TYPEDEFECTDENSITY			3
#define CES_TYPESOLUBILITYTEMP			4
#define CES_TYPETEMPWITHOUTEQUILIB		5
#define CES_TYPET0TEMPERATURE			6
#define CES_TYPESCHEILCALC				100
#define CES_TYPEKINETICCALC				200

#define SPB_TYPE_TEMPERATURE			10
#define SPB_TYPE_ELEMENT				11
#define SPB_TYPE_T0_TEMPERATURE			20

#define KINETIC_ORTHO_EQUILIB_STATE		0
#define KINETIC_PARA_EQUILIB_STATE		1
#define CES_STATE_BEFORE_STEP_COMMAND	0
#define CES_FIRST_VALID_STEP_EQILIB		1
#define CES_STATE_AFTER_LAST_STEP		2
#define CES_STATE_BEFORE_LAST_STEP		3
#define CES_STATE_AFTER_LAST_SUBSTEP	4
#define CES_STATE_BEFORE_LAST_SUBSTEP	5

#define CES_APPENDTOEXISTINGDATA		0x00000001		//Werte werden an existierenden Buffer angehaengt
#define CES_ELEMENTSINWEIGHTPERCENT		0x00000002		//Die Elementgehalte fuer Step in Gewichtsprozent
#define CES_TEMPERATUREINC				0x00000004		//Temperature als C interpretieren
#define CES_APPENDWITHOUTLOAD			0x00000008		//Werte werden an existierenden Buffer angehaengt, aber letzter Zustand nicht geladen
#define CES_VARYELEMENTINPARAEQUILIB	0x00000020		//Elementgehalt in paraequilib constraints aendern
#define CES_FORCETOCOMPFROMPARENT		0x00000040		//Force composition fuer T0 step
#define CES_APPENDINVERSEORDER			0x00000100		//Werte in umgekehrter Reihenfolge speichern
#define CES_INCLUDEHEADERINFILE			0x00000200		//Soll header in file inkludiert werden?
#define CES_SCHEILCALCSOLTEMP			0x00000400		//fuer Scheil calc
#define CES_RANGE_TYPE_LOG				0x00000800		//logarithmic increments?
#define CES_SCHEILCPAUTO				0x00001000		//create phases for scheil automatically


//*****************************************************
// Flags fuer Equilib calc output
//*****************************************************

//QuietStatusFlags bis 0x0000000F
#define EO_QUIET						0x00010000		//keine Meldungen
#define EO_STATUS_ONLY					0x00020000		//nur Status
#define EO_NORMAL						0x00040000		//normal lang
#define EO_SHORT						0x00080000		//kurze ausgabe
#define EO_NO_EVENTS					0x00100000		//keine events

#define EO_NO_AUTO_RESET_ON_MAXITER		0x00000100		//...
#define EO_SAVE_CHECK_COMPOSITION		0x00001000		//bei SetCompositionFromCompArray
#define EO_ECHO_RESET_COMPOSITION		0x00002000		//bei SetSFFromChemPot

//*****************************************************
// Flags fuer PhasesGeneral output
//*****************************************************

#define PG_ELEMENTS_IN_WEIGHT_SCALE		0x00000001
#define PG_ELEMENTS_IN_PERCENT			0x00000002
#define PG_ELEMENTS_IN_U_FRACTION		0x00000004
#define PG_ELEMENTS_IN_UC_FRACTION		0x00000008

#define PG_PAINT_SHOW_CELL_BOUNDARY		0x00000010

#define PG_PHASES_IN_WEIGHT_SCALE		0x00000100
#define PG_PHASES_IN_VOLUME_SCALE		0x00000200
#define PG_PHASES_IN_PERCENT			0x00000400

#define PG_PHASES_IN_U_MOLES			0x00001000
#define PG_COMPOSITION_ONLY				0x00002000
#define PG_MASS_BALANCE_DIFF			0x00004000

#define PG_DISPLAY_RELATIVE_CHANGE		0x00100000
#define PG_DISPLAY_FIRST_DERIVATIVE		0x01000000
#define PG_DISPLAY_DERIVE_REL_CHANGE	0x02000000
#define PG_DISPLAY_SECOND_DERIVATIVE	0x04000000

#define PG_SHOW_NUMERICAL_DERIVS		0x08000000


//show displacement types
#define PV_SHOW_INITIAL_COORDS				0
#define PV_SHOW_CURRENT_COORDS				10
#define PV_SHOW_BOTH_COORDS					20


//*****************************************************
// Flags
//*****************************************************

//DataMgr (unteren 2 Byte)
#define DBMGR_PROJECTDEFINED	0x00000001
#define DBMGR_EQUIDATABASEOPEN	0x00000002
#define DBMGR_EQUIDATABASEREAD	0x00000004
#define DBMGR_DIFFDATABASEOPEN	0x00000008
#define DBMGR_DIFFDATABASEREAD	0x00000010
#define DBMGR_PHYSDATABASEOPEN	0x00000020
#define DBMGR_PHYSDATABASEREAD	0x00000040
#define DBMGR_PHYSDATADEFINED	0x00000080
#define DBMGR_ELEMENTSDEFINED	0x00000100
#define DBMGR_PHASESDEFINED		0x00000200

//Gibbs (oberen 2 Byte)
#define GIBBS_SYSTEMLINKED				0x00010000
#define GIBBS_SYSTEMUPDATED				0x00020000   //wenn gerade gelinkt und noch nicht gerechnet
#define GIBBS_KINETIKCONTINUE			0x00040000   //gesetzt, wenn Continue gedrueckt werden darf
#define GIBBS_SORTRESARRAY				0x00100000   //Soll Progress sortiert werden?
#define GIBBS_STORERESULTLINE           0x00200000   //Soll AddResultLine wirklich durchgefuehrt werden?
#define GIBBS_SYSCALCDEBUG				0x00400000	 //Unterbrechung beim Rechnen

//Phases
#define FLAG_PHASEISSUSPENDED			0x00000001
#define FLAG_PHASEISSTOCHIOMETRIC		0x00000002
#define FLAG_PHASEISSELECTED			0x00000004
#define FLAG_USERSUSPENDED				0x00000008
#define FLAG_PHASEWITHMAGNETIC			0x00000010   //Phase mit magnetischem Anteil
#define FLAG_PHASEISFIXED 				0x00000020   //Phasenstatus FIXED
#define FLAG_PHASEISDORMANT				0x00000040	//Phasenstatus DORMANT
#define FLAG_PHASEISMATRIXPHASE			0x00000080	//Phase ist MatrixPhase
#define FLAG_KINMATRIX					0x00000100	//Ausscheidungstyp: Matrix
#define FLAG_KINPRECIPITATE				0x00000200	//Ausscheidungstyp: Praezipitat
#define FLAG_PHASEISNEWCOMPOSITIONSET	0x00000400   //Phase ist als neues CompositionSet erzeugt worden
#define FLAG_PHASEISACTIVE				0x00000800   //Phase ist aktiv (ENTERED)
#define FLAG_PHASEFORSOLCALC			0x00001000   //fuer diese Phase wird Loeslichkeitstemp bestimmt
#define FLAG_PHASEISDISPERSED			0x00002000   //diese Phase is disperse Phase in Diffusion calc
#define FLAG_PHASEISFIXED_U				0x00004000   //Phasenstatus FIXED u-Phase Fraction
#define FLAG_PHASEISTEMPSUSPENDED		0x00008000   //Phasenstatus temporarily suspended
#define FLAG_PHASEFIXEDKEEPDFMZERO		0x00010000   //Bei fixed status wird dgm auf nullptr gehalten
#define FLAG_PHASEISKINETIC				0x00020000   //Ausscheidungsphase
#define FLAG_PHASEISORDERED				0x00040000   //Geordnete Phase, d.h. besitzt disordered part (obsolete)
#define FLAG_GBBWITHOUTMAGNETIC			0x00080000   //GBB variables ohne magnetischem Anteil rechnen
#define FLAG_ENFORCE_MAJOR_CONST		0x00100000   //make sure major constituents are maintained in simulations
#define FLAG_PHASEISGHOST               0x00200000   //phase is a ghost used for regions simulation and won't gain volume


//PhasenStatus inaktiv
#define PHSTAT_OK							0
#define PHSTAT_CPDIFFER	  					1
#define PHSTAT_POSITIVEDFM					2

//IsFixedSolidPhase
#define FSP_SOLID			1
#define FSP_TRANS			2
#define FSP_TRANS_SOLID		3
#define FSP_KINETIC			10
#define FSP_INDEPENDENT		100		// for use in independent Gibbs node
//#define FSP_DEFECT			20

//for list phases (types)
#define SF_ALL					0x00000001
#define SF_MATRIX_PHASES		0x00000002
#define SF_PRECIPITATES			0x00000004

//chemical potentials type...
//quality for calculation in ascending order !!!
//everything better than CPT_DEFAULT (=0) delivers perfect chemPot
#define CPT_UNKNOWN				-1		//can certainly not calc chemicalc potential
#define CPT_DEFAULT				0		//use default (old) method. Delivers compound potential, but usually not correct
#define CPT_EQU_SYSTEM			2		//ok, but must be evaluated from solution of equation system
#define CPT_CAN_COMPLETE		5		//ok, but must be evaluated based on other potential (must calc first!), must be one more than CPT_DEFAULT
#define CPT_SINGLE_SL			7		//ok
#define CPT_VA_ON_SL			10		//ok
#define CPT_MULTI_VA_ON_SL		11		//ok
#define CPT_EL_ON_ALL_SL		50		//ok
#define CPT_IS_VACANCY			1000	//ok, element itself is vacancy. Per definitionen: mu(Va) = 0.0



//**************** Constituent *****************

#define CON_NOMAJORCONSTMARKED			0x0001
#define CON_JUSTMAJORCONSTITUENTS		0x0002
#define CON_WITHCOLONS						0x0004

#define CONSET_ONLYWITHPERCENTSIGN		0x0001

#define FET_NO_CONDITION				0
#define FET_FIXED_MF					1	//fixed mole fraction
#define FET_FIXED_WP					2	//fixed weight percent
#define FET_FIXED_UF					3	//fixed u-fraction

//**************** Richtung bei UpdateComposition *****************

#define UCDIR_FROMWTP			1
#define UCDIR_FROMAF			2

//***** Anpassung der Elementgehalte bei SetAmountOfElement **********

#define SAE_ADJUST_NOTHING			0	//nichts anpassen
#define SAE_ADJUST_REF_ELEMENT		1	//nur Referenz-Element anpassen
#define SAE_ADJUST_ALL_ELEMENTS		2	//danach alle Elemente relativ anpassen
#define SAE_ADJUST_ALL_BUT_ME		3	//alle andere Elemente relativ anpassen
#define SAE_ADJUST_INTERST_SUBST	5	//interstitial/substitutional eigen behandeln (Diffusionsrechnung)


//**************** Flags beim T0 Calc *****************

#define T0CALC_FIRSTCALCCOMP			0x00000001

//**************** Solid Transformation Types *****************

#define STT_CONSTR_EQUIL			1
#define STT_FULL_EQUIL				2
#define STT_MANUAL_RATIO			3
#define STT_MANUAL_AVRAMI			4
#define STT_MANUAL_KOIST_MAR		5

//*****************************************************
// FunctionManager Codes
//*****************************************************

#define DB_SYMBOLS							1
#define DB_DIFFUSION_SYMBOLS				2
#define DB_PHYSICAL_SYMBOLS					3
#define DB_PHASE_PARAMETERS					10
#define DB_PHASE_DIFFUSION_PARAMETERS		11
#define DB_PHASE_PHYSICAL_PARAMETERS		12

#define TFKT_CONSTITUTION			0x00000001	//Haette gerne die Funktion: G(BCC_A2, FE,MN:VA)
#define TFKT_POLYNOMDATA			0x00000002	//Die Polynomdaten
#define TFKT_ENABLED				0x00000004	//Enabled-flag der Funktion
#define TFKT_POLYNOMVALUES			0x00000008	//aktuelle Werte der Polynomdaten

//*****************************************************
// Database Manager Types
//*****************************************************

#define DBOC_EQUILIBRIUM		1
#define DBOC_DIFFUSION			2
#define DBOC_PHYSICAL			3

//Parser Codes

#define DBLT_DBDESCRIPTION		1
#define DBLT_DBELEMENT			2
#define DBLT_DBPHASE			3

//Parser Typen

#define TYPE_G					0
#define TYPE_L					1
#define TYPE_IS					3	//type internal stress
#define TYPE_OFFSET				5
#define TYPE_TC                 10
#define TYPE_BMAGN				20	// 
#define TYPE_HMVA				30	// enhalpy of vacancy formation
#define TYPE_SMVA				31	// entropy of vacancy formation
//#define TYPE_DEFECT_ENERGY		30
#define TYPE_MQ					50
#define TYPE_MF					51
#define TYPE_DQ					52
#define TYPE_DF					53
#define TYPE_VS					54
#define TYPE_CR					60	//vacancy wind factor

#define TYPE_SYMBOL				100
#define TYPE_DIFFSYMBOL			101
#define TYPE_PHYSSYMBOL			102

#define TYPE_FIRSTPHYSPARAM		10000		//erster physikalischer Parameter Index

//TermParser
#define TDTP_ERROR				0
#define TDTP_SYMBOL				1
#define TDTP_T0					2
#define TDTP_T1					3
#define TDTP_TLNT				4
#define TDTP_TN					5
#define TDTP_RTLN				6
#define TDTP_LN					7
#define TDTP_LOG				8
#define TDTP_EXP				9
#define TDTP_P1					10
#define TDTP_PT					11
#define TDTP_PT2				12
#define TDTP_RT					13
#define TDTP_LAST				100

// ThermFkt defines
#define DT_REGULAR_Y			0		// no disorder / order model
#define DT_DISORDER_ORD_DIS		10		// regular order-disorder contribution (prototypes: BCC_C2, FCC_L12, etc)
#define DT_DISORDER_NO_S		20		// disorder contribution without entropy in X (prototype: SIGMA)
#define DT_PARENT_PHASE			30		// used, for instance, for Cottrell atmospheres


//Fehlercodes
#define DBP_ERRGETLINE			100
#define DBP_ERRKEYWORD			101
#define DBP_ERRTOOLONGWORD		102
#define DBP_ERRPARSINGLINE		103
#define DBP_ERRTYPEDEFSEQ    	104
#define DBP_ERRTYPEDEFGES    	105
#define DBP_ERRPHASENOTFOUND	106


//MultiParameterStep
#define MPS_TEMPERATURE_C		100
#define MPS_TEMPERATURE_K		101

#define MPS_SYSTEM_COMP_X_FIRST	1000
#define MPS_PARA_COMP_X_FIRST	2000


#define MPST_LINEAR				1
#define MPST_LOGARITHMIC		2
#define MPST_TABLE_INDEX_FIRST	100

//Neural Network ...
#define NNP_ACC			1
#define NNP_GM			5
#define NNP_DC			10
#define NNP_DS			11

//transfer functions
#define AFT_TANSIG		1
#define AFT_LOGSIG		2
#define AFT_PURELIN		10


//****************************************
// Verification types
//****************************************

#define MVT_BUFFER_RESULTS      1
#define MVT_CALC_STATE          2

#define MVT_RESULT_ALL_OK          11
#define MVT_RESULT_ONE_WARNING     12
#define MVT_RESULT_ONE_CRITICAL    13

#define MVT_SINGLE_RESULT_OK        1
#define MVT_SINGLE_RESULT_WARNING	2
#define MVT_SINGLE_RESULT_CRITICAL	3

#define MVT_ABS_DEVIATION_WARN_CRIT       1     //absolute deviation from target values with warning and criticality level
#define MVT_REL_DEVIATION_WARN_CRIT       2
#define MVT_ABS_DEVIATION_MIN_MAX        10     //absolute deviation from target to lower and higher value with only criticality level
#define MVT_REL_DEVIATION_MIN_MAX        11

#define MVT_ECHO_QUIET              0
#define MVT_ECHO_SUMMARY            10
#define MVT_ECHO_DETAILS            11
#define MVT_ECHO_ALL                100

//****************************************
//KINETICS ...
//****************************************

// Ashby-Orowan Selection
#define AOT_MINIMUM				1
#define AOT_WEAK				5
#define AOT_STRONG				6


//Model (gibt's nicht mehr)
#define KM_CLASSICAL_NUCL_GROWTH		0
#define KM_SVOBODA_MODEL				10

//thermodynamic data from ...
#define TM_CALPHAD						1
#define TM_CALPHAD_MCA					2
#define TM_NEURAL_NETWORK				10

//Time Integration Modell
#define TIM_EULER_FORWARD				1
#define TIM_EULER_FORWARD_2				2
#define TIM_EULER_FORWARD_EEPFD			3	//estimated equilibrium phase fraction damping
#define TIM_HEUN_1_2					5
#define TIM_HEUN_PC						6
#define TIM_IMPLICIT					100
#define TIM_BULIRSCH_STOER				101

//Status
#define KS_NOT_INITIALIZED				0	//nicht initialisiert
#define KS_INITIALIZED					1	//initialisiert
#define KS_READY						2	//fertig zum Rechnen


//Surface tension, nucleation constant etc.
#define STT_NONE						0
#define STT_FROM_PHYSICAL_PARAMETER		1	//
#define STT_FROM_GLOBAL_TABLE			2	//
#define STT_FIXED_VALUE					10	//

//Temperature control
#define STT_ISOTHERMAL					0
#define STT_CONTINUOUS					2
#define STT_FROM_HEAT_TREATMENT			10

//Timestep control
#define TSC_MANUAL						0
#define TSC_AUTOMATIC					1

//heat treatment definition types (2 out of 3: T_end, T_dot and delta_t)
#define HTDT_INVALID_CODE				0
#define HTDT_TEND_TDOT					1
#define HTDT_TDOT_DELTAT				2
#define HTDT_TEND_DELTAT				3
#define HTDT_TDOT_ACCEPS				4

//dynamic strengthening types
#define DSVT_SIGMA_0                    1
#define DSVT_SIGMA_SAT                  2
#define DSVT_THETA_0                    3

//deformation control types
#define HTDC_INVALID_CODE				0
#define HTDC_AXISYM_COMPRESSION			1
#define HTDC_ROLLING    				2
#define HTDC_AXISYM_TENSILE				3
#define HTDC_USER_DEFINED               5

#define EPS_DOT_AXIS_Z                  0
#define EPS_DOT_AXIS_Y                  1
#define EPS_DOT_AXIS_X                  2

// load control modes
#define HTDL_INVALID_CODE               0
#define HTDL_PLASTIC_STRAIN_RATE        1
#define HTDL_DISPLACEMENT_RATE          2
#define HTDL_CONST_STRESS_LOAD          3
#define HTDL_STRESS_RELAXATION          4

//heat treatment property codes
//#define HTP_SEGMENT_NAME				1
#define HTP_TEMP_START					1
#define HTP_INHERIT_TEMP_START			2
#define HTP_SEGMENT_TEMP_RAMP_CONTROL	6
#define HTP_TEMP_END					10
#define HTP_DELTA_TIME					11
#define HTP_TEMP_DOT					12
#define HTP_SEGMENT_START_TIME			15
#define HTP_STORE_IS_LOG				17
#define HTP_NUM_STORE_INTERVALS			18
#define HTP_LOG_SCALE_FACTOR			19
#define HTP_RESET_STORE_COUNTER         20
#define HTP_PRECIPITATION_DOMAIN		30
#define HTP_EPS_DOT						40
#define HTP_ACC_EPS						41
#define HTP_STRESS_LOAD                 42
#define HTP_SEGMENT_LOAD_CONTROL_TYPE	44
#define HTP_SEGMENT_DEF_CONTROL_TYPE	45
#define HTP_SEGMENT_DEF_AXIS        	46
#define HTP_PRE_SEGMENT_SCRIPT			50
#define HTP_POST_SEGMENT_SCRIPT			51
#define HTP_SEGMENT_COMMENT				60
#define HTP_BETA_SCALE_FOR_STORE		70
#define HTP_STRAIN_RATE_TENSOR  		75
#define HTP_RESET_VA_FRACTION           100
#define HTP_RESET_REXX_FRACTION         110
#define HTP_BREAK_UP_PARTICLES          111
#define HTP_RELEASE_GB_PRECIPITATES     112
#define HTP_RESET_TRAP_PARTITIONING		113
#define HTP_INIT_MS_DATA		114

// dislocation shearing types
#define DST_SINGLE						0
#define DST_PAIR						1

//Kinetic properties ...
#define KP_KINETIC_STATUS				1
#define KP_KINETIC_ALIAS_NAME			2
#define KP_SOLID_PHASE_INDEX			3
#define KP_SPECIAL_OPTIONS				10
#define KP_NUCLEATION_SITE				15
#define KP_NUCL_USE_SITE_SATURATION		16
#define KP_SATURATION_INACTIVE_RADIUS	17
#define KP_SATURATION_GB_DEPLETION_DIST 18
#define KP_NUCL_SITE_EFFICIENCY			19
#define KP_EPITAXIAL_KAPPA_I			20
#define KP_EPITAXIAL_KAPPA_S			21
#define KP_FIXED_NUM_NUCLEATION_SITES   22
#define KP_IGNORE_RESET_PREC_ON_INIT    23
#define KP_USE_STRAIN_IN_GROWTH         24
#define KP_USE_SHAPE_FACTOR				25
#define KP_SHAPE_FACTOR					26
#define KP_NUMBER_PREC_SIZE_CLASSES		30
#define KP_NUMBER_VALID_PREC_CLASSES	31
#define KP_PREC_PARENT_PHASE_HANDLE		35
#define KP_MOLE_FRAC_FROM_PART_ARRAY	50
#define KP_INTERFACE_ENERGY				60
#define KP_AUTO_INTERFACE_ENERGY		64
#define KP_AUTO_IE_SIZE_CORR			65
#define KP_AUTO_VE_SIZE_CORR			66
#define KP_AUTO_IE_DIFFUSE_INTF_CORR	67
#define KP_T_CRIT_REG_SOL				70
#define KP_MINIMUM_NUCLEATION_RADIUS	77
#define KP_NUCL_CONST					80
#define KP_INCUBATION_TIME_MODEL		83
#define KP_INCUBTIME_CONST				85
#define KP_NUCLEUS_COMP_MODEL			90
#define KP_NUCLEUS_DFM_FROM 			95
#define KP_NUCLEATION_MODEL				100
#define KP_DRIVING_FORCE_MODEL			105
#define KP_GROWTH_MODEL                 106
#define KP_SURFACE_THICKNESS            107
#define KP_ATOMIC_ATTACHMENT_CONTROL	110
#define KP_INTF_MOBILITY				111
#define KP_MOB_ONLY_ON_SHRINK           112
#define KP_RADIUS_FORM_FACTOR           115
#define KP_OTHER_PART_NUCL_TYPE			120
#define KP_DISL_LINE_ENERGY_FACTOR      121
#define KP_EQUIV_INTERF_ENERGY			125
#define KP_NUCL_TRANSFORM_RADIUS_MIN	130
#define KP_NUCL_TRANSFORM_RADIUS_MAX	131
#define KP_OTHER_PART_NUCL_INHER_COMP	140
//#define KP_NUCLEATION_TRANSFER_FUNCTION 145
#define KP_NUCL_DFM_WEIGHTING_FUNCTION  146
#define KP_VOLUMETRIC_MISFIT			150
#define KP_AUTO_VOLUMETRIC_MISFIT		151
#define KP_RESTRICT_NUCL_TO_PD			160
#define KP_RESTRICT_NUCL_VALID_MAJ_CON	170
#define KP_RESTRICT_NUCL_PREC_DOMAIN	180
#define KP_ALLOW_AUTO_EXPAND_PREC_SIZE_CLASS    185
#define KP_ALLOW_AUTO_MERGE_PREC_SIZE_CLASS     186
#define KP_ALLOW_AUTO_EXPAND_GRAIN_SIZE_CLASS   187
#define KP_ALLOW_AUTO_MERGE_GRAIN_SIZE_CLASS    188

#define KP_NUCLEATE_ONLY_IN_VALID_CNT   199
#define KP_USE_NUCL_MISFIT_STRESS		200
#define KP_USE_NUCL_GB_ENERGY   		201
#define KP_IGNORE_MISFIT_DURING_DEF		205
#define KP_USE_NUCL_EXCESS_VA			210
#define KP_USE_NUCL_SHAPE_FACTOR		220
//#define KP_AUTO_COHERENCY_RADIUS		230
#define KP_COHERENCY_RADIUS				231
//#define KP_AUTO_SHEARABLE_RADIUS		235
//#define KP_SHEARABLE_RADIUS				236
#define KP_AUTO_BURGERS_VECTOR			240
#define KP_BURGERS_VECTOR				241
#define KP_BREAKABLE_ABOVE_RADIUS       242
#define KP_LINE_TENSION_FROM_SIMPLE		250
//#define KP_LINE_TENSION_THETA_SHEAR		255
#define KP_LINE_TENSION_OUTER_CUT_OFF	256
#define KP_LINE_TENSION_INNER_CUT_OFF	257
//#define KP_STRENGTH_NON_SHEAR_ANGLE		260
#define KP_STRENGTH_COUPLING_TOTAL		270
#define KP_STRENGTH_COUPLING_SHEAR		271
#define KP_STRENGTH_COUPLING_NON_SHEAR	272
#define KP_STRENGTH_COUPLING_SSS    	273
#define KP_STRENGTH_COUPLING_SSS_SUB  	274
#define KP_STRENGTH_COUPLING_SSS_INT   	275
#define KP_STRENGTH_COUPLING_CCD    	276
#define KP_CCD_COEFF_ALPHA          	277
#define KP_CCD_COEFF_NU                 278
#define KP_CCD_COEFF_OMEGA          	279
#define KP_STRENGTH_COUPLING_A_THERMAL 	280
#define KP_STRENGTHENING_MODEL			281
#define KP_SIZE_CLASS_COUPLING_COEFF    282
#define KP_KINETIC_MATRIX_PHASE_HANDLE	285		//for backwards compatibility only
#define KP_PRECIPITATION_DOMAIN_HANDLE	286
#define KP_PD_AUTO_BURGERS_VECTOR		290
#define KP_PD_BURGERS_VECTOR			291
#define KP_PD_STACKING_FAULT_ENERGY		295
#define KP_PD_AUTO_STACK_FAULT_ENERGY	296

#define KP_CO_CLUSTER_BINDING_ENTHALPY	300
#define KP_CLUSTER_BREAKING_ANGLE   	301

#define KP_DIFFUSION_COEFF_FROM_MATRIX	310
#define KP_DIFFUSION_COEFF_FACTOR		311
#define KP_INTERST_DIFF_FROM_MATRIX		320
#define KP_INTERST_DIFF_COEFF_FACTOR	321
#define KP_PREC_DIFF_RADIUS_EXPONENT	325
#define KP_GBDIFF_COEFF_FROM_MATRIX		330
#define KP_GBDIFF_COEFF_FACTOR			331
#define KP_INTERST_GBDIFF_FROM_MATRIX	340
#define KP_INTERST_GBDIFF_COEFF_FACTOR	341
#define KP_DISLDIFF_COEFF_FROM_MATRIX	350
#define KP_DISLDIFF_COEFF_FACTOR		351
#define KP_INTERST_DISLDIFF_FROM_MATRIX	360
#define KP_INTERST_DISLDIFF_COEFF_FACTOR	361
#define KP_OTHER_PART_NUCL_SITES		370
#define KP_COUPLES_PAIRS_NUCL_SITES		375
#define KP_MATRIX_DIFF_ENH_FACTOR_SUBS	380
#define KP_MATRIX_DIFF_ENH_FACTOR_INTS	381
#define KP_EXCESS_VA_ANNIHILATION_FACT	390
#define KP_EXCESS_VA_TRAPPING_FACT		400
#define KP_PREC_COALESCENCE_FACT		410
#define KP_PREC_COALESCENCE_TYPE		411
#define KP_PREC_COALESCENCE_ALPHA		412
#define KP_BINDER_STAUFER_EXP			420
#define KP_PREC_USR_DEFINED_CLUSTER_MOB 421
#define KP_PREC_DIFFUSION_GEOMETRY      422
#define KP_GB_SPHERICAL_MDEF            423
#define KP_GB_FAST_NUCLEATION           425
#define KP_PREC_TOPOLOGY_INTERACTION_FACTOR	430

#define KP_TOTAL_DISL_DENSITY			500
#define KP_EQUILIB_DISL_DENSITY			510
#define KP_EQUILIB_DISL_DENSITY_WALL    511
#define KP_CELL_WALL_VOLUME_FRACTION	513
#define KP_EXCESS_DISL_DENSITY_MOB		514
#define KP_EXCESS_DISL_DENSITY_IMMOB	515
#define KP_EXCESS_DISL_DENSITY_PINNED	516
#define KP_EXCESS_DISL_DENSITY_WALL		517
#define KP_GRAIN_SIZE					520
#define KP_GRAIN_SIZE_ELONG_FACT		521
#define KP_SUB_GRAIN_SIZE				530
#define KP_SUB_GRAIN_SIZE_ELONG_FACT	531
#define KP_SUB_GRAIN_MISORIENTATION     532
#define KP_SUB_GRAIN_DTHETA_DEPS        533
#define KP_SUB_GRAIN_DTHETA_DEPS_DOT    534
#define KP_GS_EVOLUTION_MODEL           535
#define KP_SS_EVOLUTION_MODEL           536
#define KP_SOLUTE_DRAG_MODEL            537
#define KP_DISL_GENERATION_COEFF_A		538
#define KP_DISL_GENERATION_COEFF_B		539
#define KP_DISL_GENERATION_COEFF_C		540
#define KP_DISL_GENERATION_COEFF_AI		541
#define KP_DISL_GENERATION_COEFF_BI		542
#define KP_DISL_GENERATION_COEFF_CI		543
#define KP_DISL_GENERATION_COEFF_AW		545
#define KP_DISL_GENERATION_COEFF_BW		546
#define KP_DISL_GENERATION_COEFF_CW		547
#define KP_DISL_GENERATION_COEFF_H1		548
#define KP_DISL_GENERATION_COEFF_H2		549
#define KP_DISL_GENERATION_COEFF_W1		550
#define KP_DISL_GENERATION_COEFF_W2		551
#define KP_DISL_GENERATION_SIGMA_SAT	552
#define KP_DISL_GENERATION_THETA_0  	553
#define KP_DISL_GENERATION_ADV_A_M 		555
#define KP_DISL_GENERATION_ADV_A_W 		556
#define KP_DISL_GENERATION_ADV_A_FK		557
#define KP_DISL_GENERATION_ADV_A_FP		558
#define KP_DISL_GENERATION_ADV_A_SS_I	559
#define KP_DISL_GENERATION_ADV_B 		566
#define KP_DISL_GENERATION_ADV_B_P   	567
#define KP_DISL_GENERATION_ADV_B_DP     568
#define KP_DISL_GENERATION_ADV_C 		569
#define KP_DISL_GENERATION_ADV_C_P   	570
#define KP_DISL_GENERATION_ADV_C_DP     571

#define KP_READ_SHOCKLEY_DISL_DENSITY   575
#define KP_ABC_SIMILITUDE_PARAM         579

#define KP_SGD_EVOLUTION_MOB_M0			600
#define KP_SGD_EVOLUTION_MOB_Q			603
#define KP_SGD_EVOLUTION_MOB_M0_PINNED	605
#define KP_SGD_EVOLUTION_MOB_Q_PINNED	606
#define KP_SGD_INCLUDE_DISL_PINNING     607
#define KP_SGD_DISL_PINNING_CELL_WIDTH  608
#define KP_GD_EVOLUTION_COEFF_KD_SC		609
#define KP_GD_EVOLUTION_COEFF_K_R		610
#define KP_GD_SC_MC_FACT_GG             611
#define KP_GD_SC_MC_FACT_RX             612
#define KP_GD_TOPOLOGY_FACTOR_RADIUS_MC		613
#define KP_GD_TOPOLOGY_FACTOR_DISL_DENS_MC	614
#define KP_GD_EVOLUTION_MOB_M0			625
#define KP_GD_EVOLUTION_MOB_Q			626
#define KP_GD_EVOLUTION_MOB_M0_PINNED	627
#define KP_GD_EVOLUTION_MOB_Q_PINNED	628
#define KP_GD_IS_AUTO_RETARD_PRESSURE   630
#define KP_GD_RETARD_PRESSURE           631
#define KP_GD_ALLOW_RECRYSTALLIZATION	635
#define KP_GD_REXX_COEFF_CGB			640
#define KP_GD_REXX_COEFF_CPSN			641
#define KP_GD_REXX_PSN_FIXED_N			642
#define KP_GD_REXX_PSN_FIXED_R			643

#define KP_GD_REXX_GROWTH_IMPING_EXP	700
#define KP_GD_REXX_COARSE_IMPING_EXP	701


#define KP_HEAT_GENERATION_MODEL        750

#define KP_IE_DISLOCATION				800
#define KP_IE_GRAIN_BOUNDARY			801
#define KP_IE_SUBGRAIN_BOUNDARY			802
#define KP_MP_USE_SAME_AS_MATRIX		810
#define KP_MP_YOUNGS_MODULUS			820
#define KP_MP_POISSON_RATIO				821
#define KP_STACKING_FAULT_ENERGY		825
#define KP_AUTO_STACKING_FAULT_ENERGY	826
#define KP_STRENGTH_LINEAR_MISFIT		836
#define KP_MP_APB_ENERGY				830
#define KP_APB_DISL_REPULSION_STRONG	831
#define KP_APB_DISL_REPULSION_WEAK		832
#define KP_STRENGTH_USE_LINEAR_MISFIT	835
#define KP_STRENGTH_LINEAR_MISFIT		836
#define KP_MODULUS_STRENGTHENING_MODEL  837
//#define KP_COH_STRENGTH_AVG_FORCE		840
#define KP_DISLOCATION_CHARACTER_ANGLE	840
#define KP_NUM_SHEARING_DISLOCATIONS	841
#define KP_STRENGTH_GG_PINNING_FACTOR	845
#define KP_STRENGTH_GG_PINNING_EXPONENT	846
#define KP_STRESS_RELAX_COEFFICIENT     847
#define KP_STRESS_RELAX_EXPONENT		848

#define KP_SV_USE_IN_DIFFUSION			950
#define KP_SV_VA_DIFFUSION_CORRECTION	960
#define KP_SV_AUTO_MEAN_VA_DIFF_DIST	970
#define KP_SV_MEAN_DIFF_DISTANCE		971
#define KP_SV_EXCESS_VA_EFFICIENCY		980
#define KP_SV_GENERATION_COEFFICIENT	990
#define KP_SV_SITE_FRACTION				991
#define KP_SV_RESET_IN_KINETIC_CALC		992
#define KP_SV_DIFF_DIST_ACCELERATION	1000
#define KP_SV_VA_EVOLUTION_MODEL		1010
#define KP_SV_VA_COND_NUCL_CONST		1020
#define KP_SV_VA_COND_NUCL_GAMMA		1021
#define KP_DISLOCATION_LINE_ENERGY		1025
#define KP_FRANK_LOOP_MEAN_RADIUS		1030
#define KP_FRANK_LOOP_DENSITY			1031
#define KP_JOG_DENSITY_DISLOCATION		1040
#define KP_JOG_DENSITY_FRANK_LOOP		1041
#define KP_STRENGTH_TAYLOR_FACTOR		1055
#define KP_STRENGTH_BASIC_STRENGTH		1060
#define KP_STRENGTH_HALL_PETCH			1065
#define KP_STRENGTH_HALL_PETCH_SGB		1066
#define KP_STRENGTH_DISL_ALPHA_TOT		1070
#define KP_STRENGTH_DISL_ALPHA_WALL		1071
#define KP_STRENGTH_DISL_ALPHA_INT		1072
#define KP_STRENGTH_SMALL_EPS_BETA_1    1080
#define KP_STRENGTH_SMALL_EPS_BETA_2    1081
#define KP_STRENGTH_SMALL_EPS_BETA_EXP  1082

#define KP_USE_SOLUTE_TRAPPING			1100
#define KP_SOLUTE_TRAP_PHASE_HANDLE		1105
#define KP_SOL_TRAP_ONLY_INTERSTITIAL	1106
#define KP_TRAP_COMP_RESET_PHASE_CHANGE	1107
#define KP_TRAP_PHASE_FRACTION_TYPE		1120
#define KP_PHASE_FRACTION_FUNCTION		1121
#define KP_TRAP_PHASE_INIT_COMP_TYPE	1130
#define KP_TRAP_PHASE_EVOLVE_COMP_TYPE	1131
#define KP_PREC_DOMAIN_MDEF_SUBST		1140
#define KP_PREC_DOMAIN_MDEF_INTERST		1141

#define KP_SPEED_OF_SOUND               1150
#define KP_STRAIN_RATE_SENS_EPS_DOT_FACT	    1151
#define KP_DYN_STRENGTH_DELTA_F_SIGMA_0_LT      1152
#define KP_DYN_STRENGTH_DELTA_F_SIGMA_0_HT      1153
#define KP_STRAIN_RATE_SENS_EXP_HT		        1154
#define KP_STRAIN_RATE_SENS_COUPLING_EXP		1155
#define KP_HT_TA_STRAIN_RATE_EXPONENT_SIGMA_0	1156
#define KP_HT_TA_STRAIN_RATE_EXPONENT_SIGMA_SAT	1157
#define KP_HT_TA_STRAIN_RATE_EXPONENT_THETA_0	1158
#define KP_DYN_STRENGTH_DELTA_F_SIGMA_SAT_LT    1160
#define KP_DYN_STRENGTH_DELTA_F_SIGMA_SAT_HT    1161
#define KP_DYN_STRENGTH_DELTA_F_THETA_0_LT      1162
#define KP_DYN_STRENGTH_DELTA_F_THETA_0_HT      1163


// SSS and CCD
#define KP_SSS_COEFFICIENT              1250
#define KP_SSS_EXPONENT                 1251
#define KP_CCD_DELTA_HC                 1255
#define KP_CCD_EXPONENT_N               1256
#define KP_CCD_DELTA_W_BAR              1257

#define KP_SEGSTR_COEFFICIENT			1260
#define KP_SEGSTR_EXPONENT				1261

#define KP_TTP_USER_FLAG				1300
#define KP_TTP_USER_VALUE_1				1305
#define KP_TTP_USER_VALUE_2				1306
#define KP_TTP_USER_VALUE_3				1307

#define KP_GD_LAST_TYPE                 1400
#define KP_GD_LAST_MIN_RADIUS           1401
#define KP_GD_LAST_MEAN_RADIUS          1402
#define KP_GD_LAST_MAX_RADIUS           1403
#define KP_GD_LAST_STD_DEVIATION        1404

// solute drag
#define KP_SD_MOBILITY_M0_GB            1500
#define KP_SD_MOBILITY_Q_GB             1501
#define KP_SD_CRITICAL_ENGERGY_GB       1502
#define KP_SD_TRANSFER_RANGE_GB         1503
#define KP_SD_MOBILITY_M0_SGB           1510
#define KP_SD_MOBILITY_Q_SGB            1511
#define KP_SD_CRITICAL_ENGERGY_SGB      1512
#define KP_SD_TRANSFER_RANGE_SGB        1513

#define KP_SD_MOBILITY_SAME_AS_HAGB     1515
#define KP_PF_MOBILITY_SAME_AS_HAGB     1516

#define KP_SD_CLS_INTERACTION_ENERGY    1520
#define KP_SF_CAHN_CROSS_B_DIFF_FACT    1525
#define KP_SF_CAHN_TRANSIENT_FACT_ETA   1526
#define KP_SF_CAHN_TRANSIENT_FACT_THETA 1527
#define KP_SF_CAHN_IS_FAST_BRANCH       1528
#define KP_SF_CAHN_EFF_SOLUTE_X         1529

#define KP_SD_SFG_ENERGY                     1550
#define KP_SD_WIDTH_GB                  1551
#define KP_SD_WIDTH_SGB                 1552

//grain size distribution
#define KP_GD_NUMBER_SIZE_CLASSES       1600
#define KP_GD_NUMBER_VALID_CLASSES      1602
#define KP_GD_SIZE_CLASS_D              1610
#define KP_GD_SIZE_CLASS_D_DOT          1611
#define KP_GD_SIZE_CLASS_N              1615
#define KP_GD_SIZE_CLASS_N_DOT          1616
#define KP_GD_SIZE_CLASS_DELTA          1620
#define KP_GD_SIZE_CLASS_DELTA_DOT      1621
#define KP_GD_SIZE_CLASS_RO_EX_INT      1625
#define KP_GD_SIZE_CLASS_RO_EX_INT_DOT  1626
#define KP_GD_SIZE_CLASS_RO_EX_WALL     1630
#define KP_GD_SIZE_CLASS_RO_EX_WALL_DOT 1631


#define KP_GD_LAST_GSC_TYPE             1800
#define KP_GD_LAST_GSC_FRACTION         1801
#define KP_GD_LAST_GSC_MIN_RADIUS       1802
#define KP_GD_LAST_GSC_MEAN_RADIUS      1803
#define KP_GD_LAST_GSC_MAX_RADIUS       1805
#define KP_GD_LAST_GSC_STD_DEVIATION    1810


// composition dependence of strength parameters
#define DS_DELTA_START 1901
#define DS_DELTA_F_SIGMA_0_LT_COEFFICIENT 1901
#define DS_DELTA_F_SIGMA_0_HT_COEFFICIENT 1902
#define DS_DELTA_F_SIGMA_SAT_LT_COEFFICIENT 1903
#define DS_DELTA_F_SIGMA_SAT_HT_COEFFICIENT 1904
#define DS_DELTA_F_THETA_0_LT_COEFFICIENT 1905
#define DS_DELTA_F_THETA_0_HT_COEFFICIENT 1906
#define DS_DELTA_F_SIGMA_0_LT_EXPONENT 1907
#define DS_DELTA_F_SIGMA_0_HT_EXPONENT 1908
#define DS_DELTA_F_SIGMA_SAT_LT_EXPONENT 1909
#define DS_DELTA_F_SIGMA_SAT_HT_EXPONENT 1910
#define DS_DELTA_F_THETA_0_LT_EXPONENT 1911
#define DS_DELTA_F_THETA_0_HT_EXPONENT 1912
#define DS_DELTA_END 1912

// sv trapping for subst. mdef
#define KP_SMDEF_CONSIDER_SV_TRAPPING   1950

// region diffusion geometries
#define RDG_SFFK                        0
#define RDG_GB                          1


//precipitation domain type
/*
#define PDT_NONE						0
#define PDT_CUBICAL						10
#define PDT_SHELL						20
#define PDT_SPHERE						30
*/
#define VDT_STATIC						0
#define VDT_DYNAMIC						1

#define STPFT_AUTOMATIC					0
#define STPFT_MANUAL					1

#define ISTP_FROM_PARENT_COMP			0
#define ISTP_FROM_EQUILIBRIUM			1

#define ESTP_FROM_EVOLUTION_EQUS		0
#define ESTP_FROM_EQUILIBRIUM			1

#define PCT_NO_COALESCENCE  			0	//precipitate coalescence type
#define PCT_BINDER_STAUFFER_DYNAMICS    1
#define PCT_USER_DEFINED_CLUSTER_MOB	2

//Precipitation Domain Solute Trap property codes
#define PD_ST_ACTIVE					0
#define PD_ST_TRAPPED_ELEMENT			1
#define PD_ST_TRAP_ELEMENT				2		//0-num_elements: trapping at solute atoms, -1: disl, -2: gb
#define PD_ST_ENTHALPY_TRAP				3
#define PD_ST_ENTHALPY_TRAP_FUNCTION	4
#define PD_ST_TRAP_COORDINATION_NUMBER  5
#define PD_ST_PARTITIONING_RATIO		6
#define PD_ST_MANUAL_PARTITIONING_RATIO	7
#define PD_ST_CALC_TYPE					8
#define PD_ST_MOBILITY                  9
#define PD_ST_PREC_NAME					10
#define PD_ST_COUPLES_PAIRS				11
#define PD_ST_STARTING_CONDITION		20
#define PD_ST_EVALUATE_IN_EQUILIB_CALC	100

//Precipitation Domain Couples&Pairs property codes
#define PD_CP_ACTIVE					0
#define PD_CP_ELEMENT_0					1
#define PD_CP_ELEMENT_1					2
#define PD_CP_ENTHALPY					3
#define PD_CP_ENTHALPY_FUNCTION			4
#define PD_CP_COORDINATION_NUMBER		5
#define PD_CP_PARTITIONING_RATIO_0		6
#define PD_CP_PARTITIONING_RATIO_1		7
#define PD_CP_MAN_PARTITIONING_RATIO_0	8
#define PD_CP_MAN_PARTITIONING_RATIO_1	9
#define PD_CP_CALC_TYPE					10
#define PD_CP_STARTING_CONDITION		15
#define PD_CP_STR_ENTHALPY_FUNCTION		20
#define PD_CP_EVALUATE_IN_EQUILIB_CALC	100

//Trapping/Couples&Pairs starting conditions
#define TCP_SC_IDEAL_SOLUTION			0
#define TCP_SC_EQUILIBRIUM				1

//Nucleation Sites
#define KNS_NONE						0
#define KNS_BULK						0x00000001
#define KNS_DISLOCATION					0x00000002
#define KNS_SUBGRAIN_BOUNDARY			0x00000010
#define KNS_GRAIN_BOUNDARY				0x00000020
#define KNS_GRAIN_BOUNDARY_EDGE			0x00000040
#define KNS_GRAIN_BOUNDARY_CORNER		0x00000080
//#define KNS_OTHER_PARTICLES				0x00000100
#define KNS_SGB_EDGE					0x00001000
#define KNS_SGB_CORNER					0x00002000
#define KNS_FIXED_NUM_NUCL_SITES		0x00080000

//special kinetic options
//#define KPO_FREEZE_RADIUS				0x00000001
//#define KPO_INTERSTITIALS_AS_EQUIL		0x00000002
//#define KPO_FORCE_EQUILIB_COMPOSITION	0x00000004

// texture
#define PM_ISOTROPIC        0
#define PM_CRSS             1

//flags
#define KF_APPEND_TO_BUFFER				0x00000001
#define KF_AUTO_SAVE					0x00000002
#define KF_RESET_MICROSTRUCTURE			0x00000004
#define KF_LOAD_FROM_STATE				0x00000008
#define KF_TTP_STOP_IF_N_DECR			0x00000010
#define KF_TTP_APPEND_TO_BUFFER			0x00000020
#define KF_TTP_NO_FINAL_UPDATE			0x00000040
#define KF_FROM_SIM_MODULE				0x00000100
#define KF_FROM_REGION_MODULE           0x00000200

//Init calc
#define IP_FOR_EQILIBRIUM_STEP		1
#define IP_FOR_KINETIK_STEP			2
#define IP_SVOBODA_CONFIG			3

//Composition of Nucleus
#define NC_ORTHO_EQUILIBRIUM		0	//Zusammensetzung von GGW
#define NC_PARA_EQUILIBRIUM			1	//Zusammensetzung von Para-GGW
#define NC_CONSTRAINED_EQUIL    	2	//Zusammensetzung beachtet compositional constraints
#define NC_MAX_NUCL_RATE			10	//Zusammensetzung mit max nucleation rate
#define NC_MIN_G_STAR				15	//Zusammensetzung mit minimalem G*
#define NC_DIFFUSIONLESS            20  //Zusammensetzung wie Matrix
#define NC_FIXED_COMP				100	//fixe Zusammensetzung, muss gesetzt werden

//Nucleation model
#define NM_BD_TIME_DEP				2
#define NM_DIRECT_TRANSFORMATION	10
#define NM_CLUSTER_DYNAMICS			15

//nucleation driving force from ...
#define NM_DFM_FROM_MATRIX          1
#define NM_DFM_FROM_PARENT          10

//atomic attachment rate during nucleation
#define AAR_MAXIMUM_TIME			0
#define AAR_LOWER_BOUND				1
#define AAR_UPPER_BOUND				2
#define AAR_SFFK					10

//drinving force model
#define DFM_GROWTH_SIZE_CLASS_BASED		1
#define DFM_GROWTH_MEAN_DFM				5
#define DFM_GROWTH_NUCLEATION_DFM		6
#define DFM_GROWTH_INCR_SFFK            10

//growth model
#define GM_SFFK                     1
#define GM_ISFFK                    2

//incubation time
#define ITM_MIXED_CONTROL			0
#define ITM_INTERFACE_CONTROL		1
#define ITM_RELAXATION_CONTROL		2

//other particle nucleation
#define OPNT_NUCLEATE_AT_SURFACE		0
#define OPNT_TRANSFORMATION_RADIUS		1
#define OPNT_EQUIVALENT_INTF_ENERGY		2
#define OPNT_TRANSFER_FUNCTION          3
#define OPNT_CLUSTER_STRUCTURE          4
#define OPNT_NUCLEATE_AT_PRECIPITATE    10

//va evolution
#define VCM_NO_EVOLUTION				0
#define VCM_MEAN_DIFFUSION_DISTANCE		1
#define VCM_FISCHER_SVO_KOZE			2

//modulus strengthening models
#define MSM_NEMBACH_MODEL				1
#define MSM_SIEMS_MODEL					2

//substructure evolution
#define SSE_NO_EVOLUTION				0
#define SSE_SHERSTNEV_KOZE_ABC				10
#define SSE_KREYCA_SIGMA_THETA				    11
#define SSE_SHERSTNEV_KOZE_2_ABC   		13
#define SSE_ADVANCED_ABC         		20

//grain structure evolution
#define GSE_NO_EVOLUTION				0
#define GSE_SINGLE_CLASS				10
#define GSE_MULTI_CLASS					20

//solute drag models
#define SDM_NO_EVOLUTION                0
#define SDM_BUKEN_KOZE_P_CRIT           1
#define SDM_CAHN_IMPURITY_DRAG          2
#define SDM_SVOB_FISCHER_GAMSJ			3

//heat generation models
#define HGM_NO_HEAT_GENERATION          0
#define HGM_NO_IDEA_WHICH_MODEL         1

//Nes model cases ...
#define NES_MODEL_SC		            1
#define NES_MODEL_SG			        2
#define NES_MODEL_SC_SG			        3
#define NES_MODEL_PM					4
#define NES_MODEL_SC_PM					5
#define NES_MODEL_SG_PM					6
#define NES_MODEL_SG_SC_PM				7

//strengthening model
#define SM_IGNORE                       1
#define SM_PREC_STRENGTHENING_MEAN      2       //one class, number weighted radius
#define SM_PREC_STRENGTHENING_VMEAN     3       //one class, volume weighted radius
#define SM_PREC_STRENGTHENING_CLASSES   4       //superposition of individual size classes
#define SM_CO_CLUSTER_STRENGTHENING     5       //Starink model for co-clusters
#define SM_CLUSTER_STRENGTHENING        6       //general cluster strengthening

//flags fuer TTP-diagram
/*
#define TTP_SHOW_NUCL			0x00000001
#define TTP_SHOW_01				0x00000002
#define TTP_SHOW_1				0x00000004
#define TTP_SHOW_5				0x00000008
#define TTP_SHOW_10				0x00000010
#define TTP_SHOW_25				0x00000020
#define TTP_SHOW_50				0x00000040
#define TTP_SHOW_75				0x00000080
#define TTP_SHOW_90				0x00000100
#define TTP_SHOW_95				0x00000200
#define TTP_SHOW_99				0x00000400
#define TTP_SHOW_USER_1			0x00001000
#define TTP_SHOW_USER_2			0x00002000
#define TTP_SHOW_USER_3			0x00004000
#define TTP_SHOW_M95			0x00100000
#define TTP_SHOW_M90			0x00200000
#define TTP_SHOW_M75			0x00400000
#define TTP_SHOW_M50			0x00800000
#define TTP_SHOW_M25			0x01000000
#define TTP_SHOW_M10			0x02000000
#define TTP_SHOW_M5				0x04000000
*/
#define TTPT_ABSOLUTE_FRACTION			0
#define TTPT_RELATIVE_FRACTION			1
#define TTPT_RELATIVE_MAX_F				2
#define TTPT_RELATIVE_FRACTION_END		3
#define TTPT_RELATIVE_MAX_F_END			4

//histogram options
#define HT_NUMBER			0	
#define HT_VOLUME			1

//*****************************************************
//some defines for Monte module functionality
//*****************************************************

#define CDE_GIBBS_ENERGY			10
#define CDE_ENTHALPY				11
#define CDE_ENTROPY					12
#define CDE_CONFIG_ENTROPY			15
#define CDE_GE_MINUS_CONF_ENTROPY	100

#define CDE_OK							0
#define CDE_ERROR_INVALID_TYPE			10
#define CDE_ERROR_TRANSFER_COMP_ARRAY	20


//*****************************************************
// View-Types
//*****************************************************

	//globale Fenstertypen
#define VT_OUTPUT					10      //obsolete. definition needed for downward compatibility
#define VT_PHASEDETAILS				11
#define VT_REGIONDETAILS			15
#define VT_TABULATERESULTS			20
#define VT_LIST_GIBBS_VARIABLES		21
#define VT_LIST_FE_VARIABLES		22
#define VT_TABULATERESULTS_F1		25		//mit Formula one
#define VT_TABULATEGRAINDIST        35      //grain size distribution

#define VT_PHASESUMMARY				100
#define VT_PHASEDATABASEINFO		110
#define VT_PHASENNDATABASEINFO		111
#define VT_PHASEDATASYMBOLINFO		120
#define VT_CHEMPOTSGENERAL			133
#define VT_PHASEDIFFUSIONDATA		134
#define VT_FSKTRAPPARTTABLE			135		//FSK-trap partitioning table
#define VT_PHASENUCLEATIONINFO		140
#define VT_PLOT_PHASENUCLEATIONINFO	141
#define VT_PHASEKIN_ACCEL_INFO      150
#define VT_KIN_ACCEL_SUMMARY        151

    //Specimen
#define VT_PLOTSPECGEOMETRY			250		//Geometrie zeichnen
#define VT_FE_CURSORPOS_INFO		251		//Infos zur Cursorposition
#define VT_FE_INTERFACE_INFO		252		//Infos zu den Interfaces

	//spezielle Typen
#define VT_DEBUG_DX_ARRAYS			300
#define VT_DEBUG_JACO_MATRIX		301
#define VT_DEBUG_GE_DERIVATIVES		302
#define VT_DEBUG_DFM_DERIVATIVES	303
#define VT_DEBUG_MU_DERIVATIVES		304
#define VT_DEBUG_FE_DERIVATIVES		305

#define VT_DEBUG_KINETIC_MATRICES	310

#define VT_DEBUG_DX_ARRAYS_F1		330		//mit Formula one
#define VT_DEBUG_JACO_MATRIX_F1		331		//mit Formula one

#define VT_DEBUG_FE_MATRIX_F1		350		//mit Formula one
#define VT_DEBUG_FE_DX_ARRAYS_F1	351		//mit Formula one
#define VT_DEBUG_FE_JACO_MATRIX_F1	352		//mit Formula one

#define VT_DEBUG_SD_SFG             360
#define VT_DEBUG_PLOT_SD_SFG_XXA    361
#define VT_DEBUG_PLOT_SD_SFG_RFA    362

//CAFE
#define VT_CAFE_SPECIMEN			400		//CAFE-Specimen
#define VT_CAFE_CURSORPOS_INFO		410		//Infos zur Cursorposition
#define VT_CAFE_INTERFACES			420		//interfaces information

#define VT_DEBUG_CAFE_THERM_MATRIX	500		//CAFE-calculation matrices
#define VT_DEBUG_CAFE_DIFF_MATRIX	501		//CAFE-calculation matrices

// profiler
#define VT_DEBUG_PROFILER           502     // profiler

//monte
#define VT_MONTE_NEAREST_NEIGHBOR		600		//monte carlo nearest neighbor list
#define VT_MONTE_NEIGHBOR_SHELL			601		//monte carlo nearest neighbor shell list
#define VT_MONTE_PAIR_DISTRIBUTION		602		//monte carlo pair distribution
#define VT_MONTE_MEAN_COMP_ENERGIES		603		//monte carlo mean composition energy general
#define VT_MONTE_MEAN_COMP_ENERGY_CURVE	604		//monte carlo mean composition energy specific
#define VT_MONTE_CLUSTER_ANALYSIS		605		//monte carlo cluster analysis window
#define VT_MONTE_CA_PROXIGRAM			606		//proxigram for cluster analysis
#define VT_MONTE_LCE_ENERGY				607		//local chemical environment
#define VT_MONTE_CONCENTRATION_PROFILE	608		//concentration profile in box
#define VT_MONTE_EFFECTIVE_INTERACTIONS 609     //effective interactions

#define FIRSTPLOTWINDOW				1000
#define VT_PLOTPROGRESS				FIRSTPLOTWINDOW + 10
#define VT_PLOT_GRID_PROFILE		FIRSTPLOTWINDOW + 15	//one-dimensional profile
#define VT_PLOT_GRID_SURFACE		FIRSTPLOTWINDOW + 16	//2-dim surface plot
#define VT_PLOT_CELL_HISTORY		FIRSTPLOTWINDOW + 17	//cell property evolution
#define VT_PLOT_CONTOURPLOT			FIRSTPLOTWINDOW + 18

#define VT_TTPDIAGRAM				FIRSTPLOTWINDOW + 20
#define VT_DIAGRAMPARTICLEDIST		FIRSTPLOTWINDOW + 21
#define VT_TABULATEPARTICLEDIST		FIRSTPLOTWINDOW + 22
#define VT_IM_VELOCITY_FUNCTION		FIRSTPLOTWINDOW + 23
#define VT_DIAGRAMPARTICLE_SCATTER	FIRSTPLOTWINDOW + 25
#define VT_PAINT_CELLS_2D			FIRSTPLOTWINDOW + 30	//self drawn
#define VT_PAINT_MC_3D				FIRSTPLOTWINDOW + 40	//3-dim monte carlo

#define VT_PLOT_ARRAYPLOT	        FIRSTPLOTWINDOW + 50

#define VT_SPECIMENNODALRESULT		FIRSTPLOTWINDOW + 100
#define VT_SIMULATIONRESULT			FIRSTPLOTWINDOW + 101
#define VT_SPECIMENELEMENTRESULT	FIRSTPLOTWINDOW + 102

#define VT_CAFE_SPEC_PROPERTIES		FIRSTPLOTWINDOW + 200

#define VT_GRAIN_SIZE_DIST          FIRSTPLOTWINDOW + 210
#define VT_GRAIN_SIZE_SCATTER       FIRSTPLOTWINDOW + 211

#define VT_PLOTPROGRESS_REGION		FIRSTPLOTWINDOW + 300
#define VT_TTTDIAGRAM               FIRSTPLOTWINDOW + 301

#define VT_POLE_FIGURE_SCATTER		FIRSTPLOTWINDOW + 400


#define LASTPLOTWINDOW				FIRSTPLOTWINDOW + 1000


#define PT_PARTICLE_DIST			100
#define PT_PARTICLE_SCATTER			110
#define PT_PARTICLE_TTP				120
#define PT_XY						200
#define PT_GRAIN_SIZE_DIST          520
#define PT_GRAIN_SIZE_SCATTER       521
#define PT_POLE_FIGURE_SCATTER      600
#define PT_1D_PROFILE				1000
#define PT_CELL_HISTORY				1010
#define PT_2D_SURFACE				1100
#define PT_CONTOURPLOT				1200
#define PT_ARRAYPLOT                1300
#define PT_PHASE_NUCLEATION_INFO	1400
#define PT_SD_SFG_XXA               1500
#define PT_SD_SFG_RFA               1501



//*****************************************************
// Hint-Typen fuer UpdateAllViews()
//*****************************************************

#define VT_HINT_NORMAL				0
#define VT_HINT_FE_CURSOR			10		//gilt nur fuer FE-cursorpos infos


//*****************************************************
// View-Flags
//*****************************************************

#define VF_WINDOWISFROZEN			0x00000001		//fixiert?

#define VF_WINDOWISFIXEDTONODE		0x00010000		//an einen bestimmten Knoten gebunden?

//*****************************************************
// Flags fuer paint output FE
//*****************************************************

#define PO_SHOW_GRIDPOINTS				0x00000001
#define PO_SHOW_ELEMENTS				0x00000002
#define PO_SHOW_CONTOURS				0x00000004
#define PO_SHOW_CONTOUR_FILL			0x00000008
#define PO_AUTO_CONTOURS				0x00000010
#define PO_SHOW_HEAT_SOURCES			0x00000020
#define PO_SHOW_INTERFACE_GPS			0x00000040
#define PO_SHOW_INTERFACES				0x00000080
#define PO_SHOW_ELEMENT_SUBSTRUCT		0x00000100
#define PO_SHOW_2D_PROPLINES			0x00000200
#define PO_SHOW_GP_INDEX				0x00000400
#define PO_SHOW_GP_ID					0x00000800
#define PO_SHOW_SPECIMEN				0x00001000

//*****************************************************
// Flags fuer paint output CAFD
//*****************************************************

#define PO_SHOW_CAFD_ELEMENT_BOUNDS			0x00000001		//Zellen Grenzen
#define PO_SHOW_CAFD_BLOCK_BOUNDS			0x00000002		//Block-Grenzen
#define PO_SHOW_CAFD_GRIDPOINTS				0x00000010		//CA-Zentrum als Kreis
#define PO_SHOW_CAFD_INTERFACE_CELLS		0x00000020		//Interface elements als X

//#define PO_DISPL_NOTHING					0x00000000		//nichts darstellen
#define PO_DISPL_SELECTED_CAS				0x00010000		//ausgewaehlte ca's
#define PO_DISPL_PHASE_FIELD				0x00020000		//alle phasen
#define PO_DISPL_CA_VARIABLE				0x00040000		//eine ausgewaehlte variable

#define PO_DISPL_RANGE_AUTOMATIC			0x01000000		//Auto-range bei variable


//*****************************************************
// Variablentypen
//*****************************************************

#define SVT_GIBBS					1	//Variablen aus MC_core
#define SVT_SPECIMENNODAL			2	//Variablen aus MC_real nodal solution
#define SVT_SPECIMENELEMENT			3	//Variablen aus MC_real elemental solution
#define SVT_PARTICLE_DISTRIBUTION	4	//Variablen aus MC_core Phase: Particle distribution
#define SVT_IM_VELOCITY_FUNCTION	5	//Variablen aus MC_real Phase: IM_Velocity_Functions
#define SVT_CAFE_SPEC_PROPS			6	//Variablen aus MC_fdtr: 1D-diagram

//Table types
#define TT_GIBBS					1
#define TT_SPECIMEN					2

//element types in phases
#define PET_NOT_IN_PHASE			0
#define PET_SUBSTITUTIONAL			1
#define PET_INTERSTITIAL			2
#define PET_SUBSTITUTIONAL_REF		10	//nur in CAFE verwendet

//fsk trapping, trap sorts
#define FSK_TRAP_DISL				1000
#define FSK_TRAP_GB					1001
#define FSK_TRAP_SGB				1005
#define FSK_TRAP_FIRST_COUPLES_PAIRS	1050
#define FSK_TRAP_FIRST_PREC			1100

//fsk trapping evaluation in equilibrium calculation
//#define FSK_EQUI_CALC_FULL_COUPLING	0	//old routine
//#define FSK_EQUI_CALC_FIXED			0
#define FSK_EQUI_CALC_DEFAULT		10
#define FSK_EQUI_CALC_MANUAL		0
//#define FSK_EQUI_CALC_DECOUPLED		1
#define FSK_EQUI_CALC_EQUILIBRIUM	1
//#define FSK_EQUI_CALC_DORMANT		2
#define FSK_EQUI_CALC_DYNAMIC		2
#define FSK_EQUI_CALC_IGNORE		3

//*****************************************************
// Variables ...
//*****************************************************

//physical units
#define VU_DIMENSION_LESS				0
#define VU_STEPVALUE					1
#define VU_TEMPERATURE					10
#define VU_LENGTH						20
#define VU_AREA							21
#define VU_VOLUME						22
#define VU_TIME							25
#define VU_PRESSURE						30
#define VU_NUMBER						40
#define VU_MOLE							41
#define VU_MASS							50

#define VU_PER_LENGTH					100
#define VU_PER_AREA						101
#define VU_PER_VOLUME					102
#define VU_PER_TEMPERATURE				103
#define VU_PER_TIME						105
#define VU_PER_AREA_AND_TIME			106

#define VU_MASS_PER_VOLUME				150
#define VU_MOLES_PER_VOLUME				151
#define VU_MOLES_PER_TIME				152
#define VU_AREA_PER_TIME				160
#define VU_NUMBER_PER_TIME				161
#define VU_NUMBER_PER_VOLUME			170
#define VU_NUMBER_PER_VOLUME_TIME		171
#define VU_NUMBER_PER_AREA_TIME			172
#define VU_NUMBER_PER_LENGTH_TIME       173
#define VU_TEMPERATURE_PER_TIME			180

#define VU_ENERGY						200
#define VU_ENERGY_PER_MOLE				210
#define VU_ENERGY_PER_ATOM				211
#define VU_ENERGY_PER_MOLE_MOLE			215
#define VU_ENERGY_PER_MOLE_TEMP			216

#define VU_ENERGY_PER_NUMBER			220
#define VU_ENERGY_PER_NUMBER_NUMBER		221
#define VU_ENERGY_PER_NUMBER_TEMP		225

#define VU_ENERGY_PER_LENGTH			230
#define VU_ENERGY_PER_AREA				231
#define VU_ENERGY_PER_VOLUME			232

#define VU_LENGTH_PER_TIME				300
#define VU_LENGHT_4_PER_ENERGY_TIME		310

//preferred units
#define PVU

//variable categories
#define MV_CAT_ALL						0
#define MV_CAT_GENERAL					1
#define MV_CAT_PHASE_FRACTION			10
#define MV_CAT_COMPOSITION				11
#define MV_CAT_SITE_FRACTION			12
#define MV_CAT_MASS_VOLUME_DENSITY		20
#define MV_CAT_PHYSICAL_PARAMS			30
#define MV_CAT_STATE_VARIABLES			35
#define MV_CAT_STATE_VARS_GBB			36
#define MV_CAT_CHEMICAL_POTENTIALS		40
#define MV_CAT_ACTIVITIES				41
#define MV_CAT_DIFFUSION				50
#define MV_CAT_DRIVING_FORCES			55
#define MV_CAT_KINETIC_GENERAL			60
#define MV_CAT_PREC_DOMAIN_STRUCT_SC	64
#define MV_CAT_PREC_DOMAIN_STRUCT_MC	65
#define MV_CAT_GRAIN_SIZE_DISTRIBUTION  66
#define MV_CAT_PREC_DOMAIN_STRENGTH		67
#define MV_CAT_PREC_DOMAIN_SPECIAL		68
#define MV_CAT_PREC_DOMAIN_MSE   		69
#define MV_CAT_PRECIPITATES				75
#define MV_CAT_NUCLEATION				80
#define MV_CAT_PREC_STRENGTH			85
//#define MV_CAT_NEURAL_NETWORKING		99
#define MV_CAT_CONSTANTS				100
#define MV_CAT_VARIABLES				105
#define MV_CAT_STRING_VARIABLES			106
#define MV_CAT_FUNCTIONS				107
#define MV_CAT_PRECIPITATE_DIST			150
#define MV_CAT_OBSOLETE					200
#define MV_CAT_SCRIPTING				300
#define MV_CAT_DEBUG					900
#define MV_CAT_SIMPLE_MSE				910

#define MV_CAT_LAST_CORE_CAT			999	//no more category above this number. then come mc_cafe category

//variable seperators
#define VS_NONE				0
#define VS_NUMBER			1
#define VS_PHASE			10		//all phases
#define VS_KIN_PHASE		11		//all precipitates
#define VS_KIN_MATRIX		12		//all kinetic matrix phases
#define VS_PREC_DOMAIN		15		//all precipitation domains
#define VS_REGION   		17		//all regions
#define VS_ELEMENT			20		//all elements
#define VS_ELEMENT_WO_VA	21		//all elements without vacancies
#define VS_ELEMENT_W_SV 	22		//all elements without interstitial vacancies and with substitutional vacancies
#define VS_ELEMENT_W_DEF	24		//all elements with defects (dislocations, grain boundaries, ...)
#define VS_ELEMENT_TRAP 	25		//all elements on trapped sublattices (dislocations, grain boundaries, ...)
#define VS_SUBLATTICE		30		//all sublattices
#define VS_ELEMENT_KIN_X	31		//all site fractions for precipitate calc
#define VS_TRAP				40		//all traps (sorted after index)
#define VS_COUPLES_PAIRS	41		//all couples/pairs (sorted after index)
#define VS_BUFFER_INDEX		100		//all buffers
#define VS_TABLE_INDEX		101		//all tables
#define VS_ARRAY_INDEX		102		//all arrays
#define VS_SMSE_INDEX		103		//all arrays


//********************************************************************
//
// Variablencodes
//
//********************************************************************

#define VC_Z							0	//Z-Variable
#define VC_X							1	//X-Variable (Achse)
#define VC_Y							2	//Y-Variable (Achse)

#define VC_UNDEFINED					0
#define VC_PHASEFRACTION				1
#define VC_ELEMENTAMOUNT				2
#define VC_ELEMENTCONCENTRATION			3
#define VC_SITAMOUNT					4
#define VC_U_FRACTION					10
#define VC_UC_FRACTION					11
#define VC_ELEMENTAMOUNT_SYSTEM			15
#define VC_ELEMENTAMOUNT_PREC_DOMAIN	16
#define VC_ELEMENTAMOUNT_PRECIPITATE	17
#define VC_ELEMENTAMOUNT_CCD_PD     	18
#define VC_ELEMENTAMOUNT_PRECIPITATE_SURF   19
#define VC_U_FRACTION_SYSTEM			20
#define VC_U_FRACTION_PREC_DOMAIN		21
#define VC_U_FRACTION_NOMINAL			22
#define VC_NOMINAL_COMPOSITION			25
#define VC_SITAMOUNT_IN_MF				27
#define VC_MOLAR_GIBBSENERGY			50
#define VC_MOLAR_ENTHALPY				51
#define VC_MOLAR_ENTROPY				52
#define VC_MOLAR_CONF_ENTROPY			53
#define VC_MOLAR_HEAT_CAPACITY			55
#define VC_MOLAR_MAGNETIC_ENTHALPY		56
#define VC_CURIE_TEMPERATURE			60
#define VC_INTERNAL_STRESS_OFFSET		61
#define VC_MOLAR_GIBBSENERGY_PREC_SYS   65
#define VC_MOLAR_PHASE_GIBBSENERGY		70
#define VC_MOLAR_PHASE_ENTHALPY			71
#define VC_MOLAR_PHASE_ENTROPY			72
#define VC_MOLAR_PHASE_CONF_ENTROPY		73
#define VC_MOLAR_PHASE_HEAT_CAP			74
#define VC_MOLAR_PHASE_GM_VA_FORMATION	80
#define VC_MOLAR_PHASE_HM_VA_FORMATION	81
#define VC_MOLAR_PHASE_SM_VA_FORMATION	82
#define VC_DRIVING_FORCE				90
#define VC_CHEM_DRIVING_FORCE			91
#define VC_MECH_DRIVING_FORCE			92
#define VC_CHEMICAL_POT_GLOBAL			95
#define VC_CHEMICAL_POTENTIAL			96
#define VC_CHEMICAL_POTENTIAL_0			97		//anderer Ref-State
#define VC_ACTIVITY_GLOBAL_298			100
#define VC_ACTIVITY						101
#define VC_ACTIVITY_298					102		//anderer Ref-State
#define VC_ACTIVITY_COEFFICIENT			105
#define VC_ACTIVITY_COEFFICIENT_298		106		//anderer Ref-State
#define VC_THERMODYNAMIC_FACTOR_RED		107
#define VC_THERMODYNAMIC_FACTOR_MEAN	108
#define VC_TOTAL_MEAN_DISL_DENSITY      109
#define VC_TOTAL_DEF_DISL_DENSITY       110
#define VC_TOTAL_DISL_DENSITY_DOT		111
#define VC_TOTAL_DISL_GENERATION_RATE	112
#define VC_CELL_WALL_VOLUME_FRACTION	113
#define VC_EQUIL_DISLOCATION_DENSITY	114
#define VC_EQUIL_DISLOCATION_DENSITY_W	115
#define VC_READ_SHOCKLEY_DISL_DENSITY	116
#define VC_EXCESS_DISL_DENSITY_TOTAL	117
#define VC_EXCESS_DISL_DENSITY_INTERNAL	118
#define VC_EXCESS_DISL_DENSITY_MOBILE	119
#define VC_EXCESS_DISL_DENSITY_IMMOBILE	120
#define VC_EXCESS_DISL_DENSITY_PINNED	121
#define VC_EXCESS_DISL_DENSITY_WALL 	122
#define VC_COND_DISLOCATION_DENSITY		123
#define VC_GRAIN_DIAMETER				124
#define VC_GRAIN_DIAMETER_NUM_WEIGHT	125
#define VC_DISL_GENERATION_RATE_INT     126
#define VC_DISL_GENERATION_RATE_WALL    127
#define VC_DISL_GENERATION_RATE_MOB     128
#define VC_DISL_GENERATION_RATE_IMOB    129
#define VC_DISL_GENERATION_RATE_PIN     130
#define VC_DISL_GENERATION_RATE_RS      131
#define VC_TOTAL_GRAIN_AREA         	135
#define VC_GRAIN_DIAMETER_MC			136
#define VC_GRAIN_DIAMETER_MC_0			137
#define VC_GRAIN_DIAMETER_MC_1			138
#define VC_GRAIN_DIAMETER_MCV			139
#define VC_GRAIN_DIAMETER_STD			140
#define VC_GRAIN_DIAMETER_V_STD			141
#define VC_SUBGRAIN_DIAMETER_MC			142
#define VC_SUBGRAIN_DIAMETER_MC_0		143
#define VC_SUBGRAIN_DIAMETER_MC_1		144
#define VC_SUBGRAIN_DIAMETER_MCV		145
#define VC_NUMBER_GRAINS_MC				146
#define VC_NUMBER_GRAINS_MC_0			147
#define VC_NUMBER_GRAINS_MC_1			148
#define VC_GRAIN_GROWTH_WF_MC_0			149
#define VC_GRAIN_GROWTH_WF_MC_1 		150
#define VC_MEAN_EX_DISL_DENS_INT_MCA    151
#define VC_MEAN_EX_DISL_DENS_INT_MCV    152
#define VC_MEAN_EX_DISL_DENS_WALL_MCA   153
#define VC_MEAN_EX_DISL_DENS_WALL_MCV   154
#define VC_MIN_DISLOCATION_DENSITY_MC 	155
#define VC_MAX_DISLOCATION_DENSITY_MC 	156
#define VC_SUB_GRAIN_DIAMETER			157
#define VC_RECRYSTALLIZED_FRACTION_MC   158
#define VC_PD_YOUNGS_MODULUS			159
#define VC_PD_SHEAR_MODULUS				160
#define VC_PD_POISSONS_RATIO			161
#define VC_PD_BURGERS_VECTOR			162
#define VC_VOLUME_GRAINS_MC             163
#define VC_VOLUME_GRAINS_MC_0           164
#define VC_VOLUME_GRAINS_MC_1           165
#define VC_MECHANICAL_WORK_K_DOT_MC     166
#define VC_TOT_DISL_DENS_MCV_0          167
#define VC_TOT_DISL_DENS_MCV_1          168
#define VC_AREA_GRAINS_MC               169

//#define VC_CALC_TOTAL_DISL_DENSITY		170
#define VC_DISL_DENSITY_SAT_INTERNAL	171
#define VC_DISL_DENSITY_SAT_WALL    	172
#define VC_ASPECT_RATIO_X_DEF           174
#define VC_ASPECT_RATIO_Y_DEF           175
#define VC_ASPECT_RATIO_Z_DEF           176
#define VC_DISL_CELL_DIRECTION_X        177
#define VC_DISL_CELL_DIRECTION_Y        178
#define VC_DISL_CELL_DIRECTION_Z        179
#define VC_CALC_GRAIN_DIAMETER			180
#define VC_CALC_GRAIN_DIAMETER_DEF      181
#define VC_CALC_GRAIN_DIAMETER_RX       182
#define VC_GD_RECRYSTALLIZED_FRACTION   183
#define VC_GD_X_NREXX_AVAILABLE         184
#define VC_N_GRAINS_DEFORMED            185
#define VC_N_GRAINS_RECRYSTALLIZED      186
#define VC_N_GRAIN_REXX_GB              187
#define VC_N_GRAIN_REXX_PSN             188
#define VC_N_GRAINS_TOTAL               189
#define VC_SGD_CRIT_DRX                 192
#define VC_RHO_CRIT_DRX                  193
//#define VC_GRAIN_DIAMETER_STEADY_STATE	185
//#define VC_CALC_SUB_GRAIN_DIAMETER		190
#define VC_SG_DIAMETER_STEADY_STATE		195
#define VC_SUB_GRAIN_MISORIENTATION     196
#define VC_KINETIC_TIME					200
#define VC_KINETIC_DELTA_TIME			205
#define VC_KINETIC_TMT_SEG_TIME			210
#define VC_KINETIC_TMT_SEG_START_TIME	211
#define VC_KINETIC_TMT_SEG_END_TIME		212
#define VC_KINETIC_TMT_TOTAL_END_TIME   215
#define VC_KINETIC_ACC_CALC_TIME		230
#define VC_KINETIC_DISPLACEMENT		    231
#define VC_KINETIC_ACC_EPS			    232
#define VC_KINETIC_EPS_ELASTIC		    233
#define VC_KINETIC_ACC_EPS_EQU			235
#define VC_KINETIC_EPS_DOT				240
#define VC_KINETIC_EPS_DOT_PLASTIC		241
#define VC_KINETIC_EPS_DOT_ELASTIC		242
#define VC_KINETIC_DYN_STRESS_LOAD		245
#define VC_EST_MATRIX_EQUIL_COMP		250
#define VC_DIFFUSION_MQ					300
#define VC_DIFFUSION_MF					301
#define VC_TRACER_DIFFUSIVITY			310
#define VC_INTRINSIC_DIFFUSIVITY		311
#define VC_CHEMICAL_DIFFUSIVITY			312
#define VC_TRACER_DIFFUSIVITY_DISL		315
#define VC_TRACER_DIFFUSIVITY_GB		316
#define VC_TRACER_DIFFUSIVITY_EFF		317
#define VC_MANNING_CORRECTION_R			319
#define VC_CHEM_DIFF_DT_PHI				320
#define VC_MOBILITY						330
#define VC_STEPVALUE					350
#define VC_DERIVE_CHEM_POT				360
#define VC_DIS_SITAMOUNT				361
#define VC_SYSTEMMASS					400
#define VC_PHASEMASS					401
#define VC_MOLAR_SYSTEMMASS				405
#define VC_MOLAR_PHASEMASS				406
#define VC_ELEMPHASEMASS				410
#define VC_DEFECT_SITE_FRACTION			415
#define VC_SYSTEMVOLUME					420
#define VC_PHASEVOLUME					421
#define VC_MOLAR_SYSTEMVOLUME			430
#define VC_MOLAR_PHASEVOLUME			431
#define VC_SUBS_ATOMIC_VOLUME			440
#define VC_ATOMIC_VOLUME				441
#define VC_SYSTEMDENSITY				450
#define VC_PHASEDENSITY					451
#define VC_SYSTEMTRUETHERMEXP			460
#define VC_PHASETRUETHERMEXP			461
#define VC_SYSTEMLINTHERMEXP			465
#define VC_PHASELINTHERMEXP				466
#define VC_SYSTEMSTRAIN					470
#define VC_PHASESTRAIN					471
#define VC_SYSTEMLENGHT					480
#define VC_PHASELENGHT					481
#define VC_LATTICE_CONSTANT				490
#define VC_SUBST_LATTICE_CONSTANT		491

//#define VC_INTERFACE_ENERGY				500
#define VC_CRITICAL_ENERGY				510
#define VC_CRITICAL_ENERGY_MIN_NUCL_RAD	511
#define VC_HET_ENERGY                   512
#define VC_CRITICAL_RADIUS				515
#define VC_CRITICAL_RADIUS_GB			516
#define VC_CRITICAL_RADIUS_DT			517
#define VC_CRITICAL_NUMBER_ATOMS		520
#define VC_CALC_INTERFACE_ENERGY		530
#define VC_EFFECTIVE_INTERFACE_ENERGY	540
#define VC_CALC_INTERFACE_DI_CORR		545
#define VC_KR_LSW_PRECIPITATE			550
#define VC_NUCLEATION_RATE				560
#define VC_NUCLEATION_RATE_SS			561
//#define VC_NUCLEATION_RATE_BD			562
#define VC_DELTA_V_NUCLEATION			565
//#define VC_NUCLEATION_RATE_BDTD			463
#define VC_DELTA_V_GROWTH				580
#define VC_DELTA_V_GROWTH_IN_PHASE		581
#define VC_X_NUCLEUS					590
#define VC_YX_NUCLEUS					591
#define VC_EST_INTERSTITIAL_YVA			595
#define VC_MEAN_INTERFACIAL_PRESSURE	596
#define VC_TOT_NUMBER_OF_PARTICLES		600
#define VC_NUMBER_OF_PARTICLES			601
#define VC_TOT_FRACTION_OF_PARTICLES	605
#define VC_MEAN_RADIUS					610
#define VC_MEAN_RADIUS_VOL				611
#define VC_MEAN_EQU_RADIUS_H			620
#define VC_MEAN_EQU_RADIUS_D			621
#define VC_MIN_RADIUS					630
#define VC_MAX_RADIUS					631
#define VC_MEAN_VA_DIFFUSION_DIST		640

#define VC_CLUSTER_HARDENING			645		//cluster strengthening
#define VC_CO_CLUSTER_HARDENING			646		//co-cluster strengthening
#define VC_CRIT_ASHBY_OROWAN_STRESS		650		//Critical Orowan Stress
#define VC_CHEMICAL_HARDENING			651		//
#define VC_CHEMICAL_HARDENING_W			652		//
#define VC_CHEMICAL_HARDENING_S			653		//
#define VC_COHERENCY_STRAIN_HARDENING	656		//
#define VC_COHERENCY_STRAIN_HARDENING_W	657		//
#define VC_COHERENCY_STRAIN_HARDENING_S	658		//
#define VC_MODULUS_MISMATCH_HARDENING	660		//
#define VC_MODULUS_MISMATCH_HARDENING_W	661		//
#define VC_MODULUS_MISMATCH_HARDENING_S	662		//
#define VC_APB_ENERGY_HARDENING			665		//
#define VC_APB_ENERGY_HARDENING_W		666		//
#define VC_APB_ENERGY_HARDENING_S		667		//
#define VC_SF_ENERGY_HARDENING			668		//
#define VC_SF_ENERGY_HARDENING_W		669		//
#define VC_SF_ENERGY_HARDENING_S		670		//
#define VC_TOT_SHEAR_STRESS_SHEARING	671		//
#define VC_TOT_SHEAR_STRESS_SHEARING_W	672		//
#define VC_TOT_SHEAR_STRESS_SHEARING_S	673		//
#define VC_TOT_SHEAR_STRESS_PREC		674		//
#define VC_MEAN_DISTANCE_2D				675
#define VC_MEAN_DISTANCE_3D				676
#define VC_MEAN_PDISTANCE_3D			677
#define VC_BURGERS_VECTOR_PREC			678
#define VC_TOT_MEAN_DIST_2D				680
#define VC_TOT_MEAN_DIST_3D				681
#define VC_TOT_MEAN_PDIST_3D			682
#define VC_TOT_MEAN_RADIUS				683
#define VC_DIST_DIFF_CORR				690
#define VC_RHO_DIFF_CORR					691
#define VC_TRAP_DIFF_CORR				692
#define VC_XVA_DIFF_CORR				695
#define VC_EX_SV_AT_TRAP_DIFF_CORR		696


#define VC_DISL_LINE_TENSION_SIMPLE		700
#define VC_DISL_LINE_TENSION_WEAK		701
#define VC_DISL_LINE_TENSION_STRONG		702

#define VC_TOT_YIELD_STRENGTH_0_TH        706
#define VC_TOT_YIELD_STRENGTH_0_AT        707
#define VC_PD_SHEAR_STRESS_SHEARING		  708		//
#define VC_PD_SHEAR_STRESS_NON_SHEAR	  710		//
#define VC_PD_SHEAR_STRESS_PREC			  711		//
#define VC_PD_YIELD_STRESS_PREC			  712		//
#define VC_BASIC_YIELD_STRENGTH			  713
#define VC_TOT_SEGREGATION_STRENGTH		  714
#define VC_TOT_SOLID_SOLUTION_STRENGTH	  715
#define VC_SOLID_SOLUTION_STRENGTH_EL	  716
#define VC_TOT_CC_DIFFUSION_STRENGTH	  717
#define VC_CC_DIFFUSION_STRENGTH_EL 	  718
#define VC_TOT_CLUSTER_STRENGTH           720
#define VC_TOT_CO_CLUSTER_STRENGTH        721
#define VC_TOT_DISLOCATION_STRENGTH		  722
#define VC_TOT_FINE_GRAIN_STRENGTH		  723
#define VC_TOT_SUB_GRAIN_STRENGTH		  724
#define VC_TOT_YIELD_STRENGTH_0			  725
#define VC_TOT_YIELD_STRENGTH_0_TH_I	  726
#define VC_TOT_YIELD_STRENGTH_0_AT_I      727
#define VC_TOT_YIELD_STRENGTH_0_TH_W	  728
#define VC_TOT_YIELD_STRENGTH_0_AT_W      729
#define VC_TOT_YIELD_STR_DYN              730
#define VC_TOT_YIELD_STR_DYN_LT           731
#define VC_TOT_YIELD_STR_DYN_HT           732
#define VC_TOT_YIELD_STR_DYN_NHC          733
#define VC_TOT_YIELD_STR_DYN_COC          734
#define VC_TOT_YIELD_STR_DYN_POW_HT       735
#define VC_TOT_YIELD_STR_DYN_POW_HT_BD    736
#define VC_TOT_YIELD_STR_DYN_POW_LT       737
#define VC_TOT_YIELD_STR_DYN_POW_LT_BD    738
#define VC_TOT_YIELD_STR_DYN_HDC          739
#define VC_TOT_YIELD_STR_DYN_GBS_DISL     740
#define VC_TOT_YIELD_STR_DYN_GBS_GB       741
#define VC_TOT_YIELD_STRENGTH_NB          742
#define VC_TOT_YIELD_STRENGTH             743
#define VC_TOT_YIELD_STRENGTH_BETA        744
#define VC_TOT_YS_THETA                   745
#define VC_TOT_YS_DSIGMA_DEPS_DOT         746

#define VC_PD_DISL_EVOLUTION_COEFF_A        747
#define VC_PD_DISL_EVOLUTION_COEFF_B        748
#define VC_PD_DISL_EVOLUTION_COEFF_C        749
#define VC_PD_DISL_EVOLUTION_COEFF_AP       750
#define VC_PD_DISL_EVOLUTION_COEFF_SIG_SAT       751
#define VC_PD_DISL_EVOLUTION_COEFF_SIG_SAT_LT    752
#define VC_PD_DISL_EVOLUTION_COEFF_SIG_SAT_HT    753
#define VC_PD_DISL_EVOLUTION_COEFF_THETA_0       754
#define VC_PD_DISL_EVOLUTION_COEFF_THETA_0_LT    755
#define VC_PD_DISL_EVOLUTION_COEFF_THETA_0_HT    756
#define VC_PD_ADVANCED_ABC_L_EFF        758

#define VC_TOT_NUM_ATOMS_BULK			760
#define VC_TOT_NUM_ATOMS_DISL			764
#define VC_TOT_NUM_ATOMS_GB				765
#define VC_TOT_NUM_ATOMS_GBE			766
#define VC_TOT_NUM_ATOMS_GBC			767
#define VC_TOT_NUM_ATOMS_SGB			768
#define VC_TOT_NUM_ATOMS_SGBE			769
#define VC_TOT_NUM_ATOMS_SGBC			770

#define VC_MEAN_RADIUS_ST_DEV			775
#define VC_MEAN_DIAMETER				779
#define VC_MEAN_DIAMETER_VOL			780
#define VC_MEAN_POROD					781
#define VC_MEAN_GUINIER					782
#define VC_NUCLEATION_DRIVING_FORCE		786
#define VC_NUCLEATION_DFM_WEIGHT_FUNC   787
#define VC_NUCLEATION_CHEM_DFM			790
#define VC_NUCLEATION_MECH_DFM			791
#define VC_NUCLEATION_VA_DFM1			792
#define VC_NUCLEATION_VA_DFM2			795
#define VC_D_EFFECTIVE_SVO				800
#define VC_NUCL_INCUBATION_TIME			805
#define VC_NUCLEATION_INTERF_ENERGY		810
#define VC_NUCLEATION_INTERF_S_CORR		815
#define VC_NUCLEATION_VOLUME_S_CORR		816
#define VC_ATOMIC_ATTACHMENT_RATE		820
#define VC_ATOMIC_ATTACHMENT_RATE_MT	821
#define VC_ATOMIC_ATTACHMENT_RATE_LB	822
#define VC_ATOMIC_ATTACHMENT_RATE_UB	823
#define VC_ATOMIC_ATTACHMENT_RATE_MD	824
#define VC_ZELDOVICH_FACTOR				890
#define VC_EQUIL_CLUSTER_DISTR_FRAC		900
#define VC_NUCL_INC_TIME_INTF_CONTR		910
#define VC_NUCL_INC_TIME_RELAX_CONTR	911
#define VC_R_DISTR_MOMENT				920
#define VC_R_DISTR_MOMENT_SCALED		921

#define VC_DOM_X_VA_EQUIL				950
#define VC_DOM_X_VA_CURR				951
#define VC_DOM_X_VA_LAT     			952
#define VC_DOM_X_VA_TRAP				953
#define VC_DOM_X_VA_STEADY_STATE		955
#define VC_DOM_X_VA_J_DISL              956
#define VC_DOM_X_VA_J_GB                957
#define VC_DOM_X_VA_J_FL                958
#define VC_DOM_X_CPX_J_GB               959
#define VC_DOM_X_CPX_J_DISL             960
#define VC_DOM_DX_VA_GB					965
#define VC_DOM_DX_VA_DISL				966
#define VC_DOM_DX_VA_FL					967
#define VC_DOM_DX_VA_DEF				968
#define VC_DOM_FSK_YT_L					970
#define VC_DOM_FSK_YT_TK				971
#define VC_DOM_FSK_YXT_L				972
#define VC_DOM_FSK_YXT_TK				973
#define VC_DOM_FSK_YXT_TK_SUM			974
#define VC_DOM_FSK_XTRAP				975
#define VC_DOM_FSK_XTRAP_EFF			976
#define VC_DOM_FSK_PART_RATIO			977
#define VC_DOM_FSK_BETA_L				978
#define VC_DOM_FSK_BETA_TK				979
#define VC_DOM_FSK_V_L					980
#define VC_DOM_FSK_V_TK					981
//#define VC_DOM_Y_L                      985
//#define VC_DOM_Y_TK                     986
//#define VC_DOM_YX_L                     987
//#define VC_DOM_YX_TK                    988
//#define VC_DOM_BETA_TRAP                990
//#define VC_DOM_BETA_PRIME_TRAP			991
//#define VC_DOM_ALPHA_DISL               995
#define VC_DOM_CP_YT_L					990
#define VC_DOM_CP_YT_TK					991
#define VC_DOM_CP_YXT_L					992
#define VC_DOM_CP_YXT_TK				993
#define VC_DOM_CP_YXT_TK_SUM			994
#define VC_DOM_CP_XTRAP					995
#define VC_DOM_CP_XTRAP_EFF				996
#define VC_DOM_CP_PART_RATIO			997
#define VC_DOM_CP_BETA_L				998
#define VC_DOM_CP_BETA_TK				999

#define VC_PHASE_FRACTION_OF_PART		1000
#define VC_PHASE_FRACTION_OF_INCOHERENT	1001
#define VC_PD_RADIUS					1010
#define VC_PD_NUMBER_PARTICLES			1015
#define VC_EQU_RADIUS_H					1020
#define VC_EQU_RADIUS_D					1021
#define VC_PD_COMPOSITION				1030
#define VC_PD_VOLUME					1035
#define VC_PD_VOLUME_TOT				1036
#define VC_PD_ATOMS_PER_PREC			1040
#define VC_PD_TOTAL_DFM					1050
#define VC_PD_PARTIAL_DFM				1051
#define VC_PD_INTERFACIAL_DFM   		1052
#define VC_PD_MECHANICAL_DFM			1055
#define VC_PD_HETEROGENEOUS_DFM			1056
#define VC_PD_SMOOTH_MECHANICAL_DFM		1057
#define VC_PD_D_RADIUS_DT				1060
#define VC_PD_D_NUMBER_PARTICLES_DT		1065
#define VC_PD_D_COMPOSITION_DT			1070
#define VC_PD_D_RADIUS_DT_0				1075
#define VC_PD_D_COMPOSITION_DT_0		1080
#define VC_PD_D_COMPOSITION_DT_1		1081
#define VC_PD_EST_INTERSTITIAL_YVA		1090
#define VC_PD_CUR_INTERSTITIAL_YVA		1091
#define VC_PD_OSC_DAMPING_FACTOR		1100
#define VC_PD_OSC_DAMPING_FACTOR_VA		1101
#define VC_PD_OSC_DAMPING_FACTOR_WK		1102
#define VC_PD_IE_SIZE_CORRECTION		1105
#define VC_PD_VE_SIZE_CORRECTION		1106
#define VC_PD_VE_DFM_FACTOR             1110
#define VC_PD_NUCL_SIZE_FACTOR          1112

#define VC_DHM_DF_PRECIPITATE			1200
#define VC_DGM_DF_PRECIPITATE			1201
#define VC_DHM_DF_DX_PRECIPITATE		1205
#define VC_L_IJ_ATOMIC_INTERACTION		1210
#define VC_REGULAR_DESOL				1220
#define VC_REGULAR_TSOL					1221
#define VC_TOTAL_NUM_NUCL_SITES			1230
#define VC_OCCUP_NUM_NUCL_SITES			1235
#define VC_EFFECT_NUM_NUCL_SITES		1240

#define VC_PD_MECH_F					1300
#define VC_PD_MECH_F_DOT				1301
#define VC_PD_VOL_CREEP					1302
#define VC_PD_MECH_DIFF_RETARD_FACT		1310
#define VC_PD_SHEAR_STRESS_PREC_CLASS   1312

#define VC_LINEAR_MISFIT_STRAIN			1320
#define VC_VOLUME_MISFIT_STRAIN			1321
#define VC_PRECIPITATE_VOLUME			1325
#define VC_PRECIPITATE_AREA				1326
#define VC_TOT_IE_CONTRIBUTION          1330

//#define VC_CALC_COHERENCY_RADIUS		1350
//#define VC_CALC_SHEARABLE_RADIUS		1351
//#define VC_NUM_PREC_SMALLER_THAN_CRIT	1360
//#define VC_MEAN_RAD_SMALLER_THAN_CRIT	1361
//#define VC_F_SMALLER_THAN_CRIT			1365
//#define VC_NUM_PREC_LARGER_THAN_CRIT	1370
//#define VC_MEAN_RAD_LARGER_THAN_CRIT	1371
//#define VC_F_LARGER_THAN_CRIT			1375

#define VC_NN_CHEMICAL_POTENTIAL		1390	//obsolete
#define VC_NN_ACTIVITY					1391	//obsolete
#define VC_NN_ACTIVITY_COEFFICIENT		1392	//obsolete
#define VC_NN_MOD_ACTIVITY_COEFF		1393	//obsolete
#define VC_NN_MOD_GIBBS_ENERGY			1394	//obsolete
#define VC_NN_GIBBS_ENERGY				1395	//obsolete

#define VC_VACANCY_CONC_EQUIL			1400		//equilibrium vacancy concentration
#define VC_VACANCY_CONC_CURRENT			1401		//current vacancy concentration
#define VC_SUBST_DIFF_CORR_FACTOR		1410		//diffusion correction factor according to excess vacancies
#define VC_FRANK_LOOP_MEAN_RADIUS		1415		//Frank loops
#define VC_FRANK_LOOP_DENSITY			1416		//Frank loops
#define VC_DISLOCATION_LINE_ENERGY		1420		//dislocation line energy (e.g. for Frank loops)

#define VC_DISLOCATION_JOG_FRACTION		1430		//equilibrium dislocation jog fraction

#define VC_PREC_RETARDING_PRESSURE_GB   1440        //precipitate retarding pressure at grain boundries
#define VC_PREC_RETARDING_PRESSURE_SGB  1441        //precipitate retarding pressure at subgrain boundries
#define VC_DRIVING_FORCE_GRAIN_GROWTH   1442        //driving force for subgrain growth of deformed grains
#define VC_DRIVING_FORCE_SG_GROWTH      1443        //driving force for grain growth of unrecrystallized grains
#define VC_DRIVING_FORCE_REXX_TOTAL     1445        //driving force for recrystallization
#define VC_DRIVING_FORCE_REXX_DISL      1446        //driving force for recrystallization: only dislocation part
#define VC_DRIVING_FORCE_REXX_SGB       1447        //driving force for recrystallization: only subgrain boundary part
#define VC_DRIVING_FORCE_REXX_GG        1448        //driving force for recrystallization: only grain growth part

#define VC_NUCLEATION_RATE_REXX         1450        //nucleatoin rate for recrystallization
#define VC_NUCLEATION_SDF_REXX          1455        //size distribution factor for rexx nucleation

#define VC_HAGB_MOBILITY				1460        //grain boundary mobility for high angle grain boundaries (Turnbull)
#define VC_LAGB_MOBILITY				1461        //grain boundary mobility for low angle grain boundaries (aus Humphreys)
#define VC_GB_EFFECTIVE_MOBILITY_GG     1470        //effective grain boundary mobility for grain growth
#define VC_GB_EFFECTIVE_MOBILITY_REXX   1471        //effective grain boundary mobility for rexx
#define VC_GB_INTRINSIC_MOBILITY        1475        //intrinsic grain boundary mobility
#define VC_GB_PINNED_MOBILITY           1476        //pinned grain boundary mobility
#define VC_GB_SOLUTE_DRAG_MOBILITY      1477        //pinned grain boundary mobility
#define VC_SGB_EFFECTIVE_MOBILITY       1480        //effective subgrain boundary mobility
#define VC_SGB_INTRINSIC_MOBILITY       1485        //intrinsic subgrain boundary mobility
#define VC_SGB_PINNED_MOBILITY          1486        //pinned subgrain boundary mobility
#define VC_SGB_SOLUTE_DRAG_MOBILITY     1487        //pinned subgrain boundary mobility

#define VC_GB_SOLUTE_DRAG_ALPHA_CAHN    1490        //solute drag ALPHA Cahn
#define VC_GB_SOLUTE_DRAG_A_CAHN_EL     1491        //solute drag ALPHA Cahn per element
#define VC_GB_SOLUTE_DRAG_MOB_CAHN      1496        //solute drag mobility from Cahn model
#define VC_GB_SOLUTE_DRAG_MOB_CAHN_EL   1497        //solute drag mobility from Cahn model per element

#define VC_GB_SOLUTE_DRAG_EF_SFG        1500        //solute drag grain boundary enrichment factor from SFG model
#define VC_SGB_SOLUTE_DRAG_EF_SFG       1501        //solute drag subgrain boundary enrichment factor from SFG model
#define VC_DMU_SOLUTE_DRAG_SFG          1502        //solute drag delta chemical potential from SFG model
#define VC_VT_SOLUTE_DRAG_SFG           1503        //solute drag normalized velocity from SFG model

#define VC_TEST_VARIABLE				1800

#define VC_VARIABLE						2000

#define VC_TEMPERATURE					2100
#define VC_PRESSURE						2101
#define VC_BASE_TEMPERATURE             2105
#define VC_EXCESS_TEMPERATURE			2106
#define VC_GAS_CONSTANT					2010
#define VC_PI							2011
#define VC_AVOGADRO_CONSTANT			2012
#define VC_BOLTZMANN_CONSTANT			2013
#define VC_ELEMENTARY_CHARGE   			2014

#define VC_GSD_DIAMETER                 2200    // grain size class diameter (equivalent sphere)
#define VC_GSD_DIAMETER_DOT             2201    // gradient for size class
#define VC_GSD_N                        2202    // grain size class number of grains
#define VC_GSD_VOL                      2203    // volume
#define VC_GSD_AREA                     2204    // area
#define VC_GSD_ACC_EPS                  2205    // accumulated eps for deformed grains
#define VC_GSD_ACC_EPS_EQU              2206    // accumulated equivalent eps for deformed grains
#define VC_GSD_X_NR_AVAILABLE           2207    // available grain boundary area for rexx nucleation
#define VC_GSD_DD_EX_MOBILE             2208    // mobile excess dislocation density
#define VC_GSD_DD_EX_IMMOBILE           2209    // immobile excess dislocation density
#define VC_GSD_DD_EX_WALL               2210    // wall excess dislocation density
#define VC_GSD_DD_EX_INTERNAL_DOT       2211    // internal excess dislocation density gradient
#define VC_GSD_DD_EX_WALL_DOT           2212    // wall excess dislocation density gradient
#define VC_GSD_DD_INT_GENERATION_RATE   2213    // internal dislocation generation rate
#define VC_GSD_DD_WALL_GENERATION_RATE  2214    // wall dislocation generation rate
#define VC_GSD_DD_EX_INTERNAL_SAT       2215    // internal saturation excess dislocation density
#define VC_GSD_DD_EX_WALL_SAT           2216    // wall saturation excess dislocation density
#define VC_GSD_SGD                      2220    // subgrain diameter
#define VC_GSD_SGD_DOT                  2221    // gradient for subgrain diameter
#define VC_GSD_SGD_SAT                  2222    // saturation subgrain diameter
#define VC_GSD_SGD_CRIT_D_REXX          2223    // critical subgrain diameter
#define VC_GSD_DFM_GG                   2224    // driving force for grain growth
#define VC_GSD_DFM_REXX                 2225    // driving force for rexx
#define VC_GSD_GB_MOB                   2226    // grain boundary mobility used in calculation
#define VC_GSD_SGB_MISORIENTATION       2228    // subgrain boundary angle
#define VC_GSD_EULER_ANGLE_PSI      2229    // grain orientation with respect to external load
#define VC_GSD_EULER_ANGLE_THETA      2230    // grain orientation with respect to external load
#define VC_GSD_EULER_ANGLE_PHI      2231    // grain orientation with respect to external load
#define VC_GSD_ASPECT_RATIO_X           2232    // shape factor for grains
#define VC_GSD_ASPECT_RATIO_Y           2233    // shape factor for grains
#define VC_GSD_ASPECT_RATIO_Z           2234    // shape factor for grains
#define VC_GSD_H_I_FACT                 2235    // random number in Riedel-Svoboda Modell
#define VC_GSD_FIT_FUNCTION             2236    // fit function for grain size class sorting
#define VC_GSD_TOPOLOGY_D_MEAN          2237    // r_mean for topology consideration for grain growth
#define VC_GSD_GRAIN_GROWTH_WF          2238    // grain growth weighting factor
#define VC_GSD_REXX_CLASS_INDEX         2239    // rexx class index for topology
#define VC_GSD_N_DOT_REXX_GB            2240    // Recrystallization rate at gb
#define VC_GSD_N_DOT_REXX_PSN           2241    // Recrystallization rate at particles
#define VC_GSD_D_REXX                   2245    // Recrystallized grain size during nucleation
#define VC_GSD_SIGMA_CLASS              2250    // strength of size class
#define VC_GSD_EPS_DOT_CLASS            2251    // individual eps_dot for size class

#define VC_SMSE_TIME					2300
#define VC_SMSE_DISL_DENSITY			2301
#define VC_SMSE_TEMPERATURE             2302
#define VC_SMSE_PARAM_A					2303
#define VC_SMSE_PARAM_B					2304
#define VC_SMSE_PARAM_C					2305
#define VC_SMSE_EP_RATE					2306
#define VC_SMSE_S0_LT					2307
#define VC_SMSE_TH0_LT					2308
#define VC_SMSE_SINF_LT					2309
#define VC_SMSE_S0_HT					2310
#define VC_SMSE_TH0_HT					2311
#define VC_SMSE_SINF_HT					2312
#define VC_SMSE_S0						2313
#define VC_SMSE_TH0						2314
#define VC_SMSE_DCRIT					2316
#define VC_SMSE_GRAIN_SIZE				2317
#define VC_SMSE_SUB_GRAIN_SIZE			2318
#define VC_SMSE_SIGMA_MT				2319
#define VC_SMSE_X_VA_EQU				2320
#define VC_SMSE_X_VA_CURR   			2321
#define VC_SMSE_X_VA_DOT       			2322
#define VC_SMSE_DISL_DENSITY_SAT		2350
#define VC_SMSE_S0_NHC					2360
#define VC_SMSE_S0_COC					2361
#define VC_SMSE_SYIELD					2500
#define VC_SMSE_SYIELD_0				2501
#define VC_SMSE_SYIELD_BETA				2505
#define VC_SMSE_SYIELD_BASIC			2510
#define VC_SMSE_DSYIELD_DEPS            2515
#define VC_SMSE_DSYIELD_DEPS_DOT        2516
#define VC_SMSE_SYIELD_SSS_EL_1			2520
#define VC_SMSE_SYIELD_SSS_EL_2			2521
#define VC_SMSE_SYIELD_SSS_TOT			2522
#define VC_SMSE_SYIELD_CCD_EL_1			2525
#define VC_SMSE_SYIELD_CCD_EL_2			2526
#define VC_SMSE_CCD_X_EFF_EL_1			2530
#define VC_SMSE_CCD_X_EFF_EL_2			2531
#define VC_SMSE_SYIELD_CCD_TOT			2527
#define VC_SMSE_SYIELD_GD   			2540
#define VC_SMSE_SYIELD_DISL   			2545
#define VC_SMSE_SYIELD_PREC1_LS			2550    // mean 2D distance
#define VC_SMSE_SYIELD_PREC1_WEAK		2551
#define VC_SMSE_SYIELD_PREC1_OROWAN		2552
#define VC_SMSE_SYIELD_PREC1_CLUSTER	2553
#define VC_SMSE_SYIELD_PREC1  			2554
#define VC_SMSE_SYIELD_PREC2_LS			2555    // mean 2D distance
#define VC_SMSE_SYIELD_PREC2_WEAK		2556
#define VC_SMSE_SYIELD_PREC2_OROWAN		2557
#define VC_SMSE_SYIELD_PREC2_CLUSTER	2558
#define VC_SMSE_SYIELD_PREC2  			2559
#define VC_SMSE_SYIELD_PREC  			2562
#define VC_SMSE_ACC_EPS                 2600
#define VC_SMSE_X_1                     2610
#define VC_SMSE_X_2                     2611
#define VC_SMSE_PREC1_X_1_EQU           2626
#define VC_SMSE_PREC1_X_2_EQU           2628
#define VC_SMSE_PREC1_F                 2630
#define VC_SMSE_PREC1_F_EQU             2631
#define VC_SMSE_PREC1_N                 2632
#define VC_SMSE_PREC1_R                 2633
#define VC_SMSE_PREC1_DR                2634
#define VC_SMSE_PREC1_DR_COARSE         2635
#define VC_SMSE_PREC1_DFM               2636
#define VC_SMSE_PREC1_G_STAR            2637
#define VC_SMSE_PREC1_R_CRIT            2638
#define VC_SMSE_PREC1_BETA_STAR         2639
#define VC_SMSE_PREC1_J                 2640
#define VC_SMSE_PREC1_Z                 2641
#define VC_SMSE_PREC1_D_EFF             2642
#define VC_SMSE_PREC2_X_1_EQU           2666
#define VC_SMSE_PREC2_X_2_EQU           2668
#define VC_SMSE_PREC2_F                 2670
#define VC_SMSE_PREC2_F_EQU             2671
#define VC_SMSE_PREC2_N                 2672
#define VC_SMSE_PREC2_R                 2673
#define VC_SMSE_PREC2_DR                2674
#define VC_SMSE_PREC2_DR_COARSE         2675
#define VC_SMSE_PREC2_DFM               2676
#define VC_SMSE_PREC2_G_STAR            2677
#define VC_SMSE_PREC2_R_CRIT            2678
#define VC_SMSE_PREC2_BETA_STAR         2679
#define VC_SMSE_PREC2_J                 2680
#define VC_SMSE_PREC2_Z                 2681
#define VC_SMSE_PREC2_D_EFF             2682


#define VC_BUFFER_SIZE					3000	// size of a buffer (number of states)
#define VC_TABLE_SIZE					3005	// size of a table
#define VC_ARRAY_ROWS					3010	// number of rows in array
#define VC_ARRAY_COLS					3011	// number of columns in array

#define VC_GRID_SIM_VARIABLE			7000

#define VC_MONTE_SIM_VARIABLE			8000

#define VC_REGION_VARIABLE              9000

#define VC_USER_FUNCTION				TYPE_FIRSTPHYSPARAM - 1

#define VC_FIRSTPHYSICAL				TYPE_FIRSTPHYSPARAM // = 10000
//#define VC_PHYS_PARAM_DP				TYPE_FIRSTPHYSPARAM + 5
//#define VC_PHYS_PARAM_HC				TYPE_FIRSTPHYSPARAM + 10


//Values for Spectrogram plot
#define SP_MEANINTERPOL					1
#define SP_SHEPARDINTERPOL				2


// SimpleMSE

// results
#define SMSE_OK                        0
#define SMSE_INVALID_PARAMETER         1
#define SMSE_INVALID_TMT_INDEX         2

#define SMSE_CAT_GENERAL_PROPS_MC               (1 << 0)
#define SMSE_CAT_GENERAL_PROPS_NOT_MC           (1 << 1)
#define SMSE_CAT_VACANCY_EVOLUTION_MC           (1 << 2)
#define SMSE_CAT_VACANCY_EVOLUTION_NOT_MC       (1 << 3)
#define SMSE_CAT_STRAIN_HARDENING_MC            (1 << 4)
#define SMSE_CAT_STRAIN_HARDENING_NOT_MC        (1 << 5)
#define SMSE_CAT_SOLID_SOLUTION_STRENGTH_MC     (1 << 6)
#define SMSE_CAT_SOLID_SOLUTION_STRENGTH_NOT_MC (1 << 7)
#define SMSE_CAT_CROSS_CORE_DIFFUSION_MC        (1 << 8)
#define SMSE_CAT_CROSS_CORE_DIFFUSION_NOT_MC    (1 << 9)
#define SMSE_CAT_PRECIPITATION_MC               (1 << 10)
#define SMSE_CAT_PRECIPITATION_NOT_MC           (1 << 11)
#define SMSE_CAT_ALL                           \
	(                                          \
	 SMSE_CAT_GENERAL_PROPS_MC               | \
	 SMSE_CAT_GENERAL_PROPS_NOT_MC           | \
     SMSE_CAT_VACANCY_EVOLUTION_MC           | \
     SMSE_CAT_VACANCY_EVOLUTION_NOT_MC       | \
     SMSE_CAT_STRAIN_HARDENING_MC            | \
	 SMSE_CAT_STRAIN_HARDENING_NOT_MC        | \
	 SMSE_CAT_SOLID_SOLUTION_STRENGTH_MC     | \
	 SMSE_CAT_SOLID_SOLUTION_STRENGTH_NOT_MC | \
	 SMSE_CAT_CROSS_CORE_DIFFUSION_MC        | \
	 SMSE_CAT_CROSS_CORE_DIFFUSION_NOT_MC    | \
	 SMSE_CAT_PRECIPITATION_MC               | \
	 SMSE_CAT_PRECIPITATION_NOT_MC             \
	)

// variables
#define SMSE_VAR_INVALID                     -1
#define SMSE_VAR_USE_SIGMA_THETA_MODEL       0 // true: sigma-theta model; false: ABC-model
#define SMSE_VAR_SIGMA_BASIC                 1 // basic yield strength at 0K [Pa]
#define SMSE_VAR_SIGMA_0_EX                  2 // excess stress for sigma_0 (prec, sss, ...)
#define SMSE_VAR_NU                          3 // Poissons ratio [-]
#define SMSE_VAR_E                           4 // Young's modulus
#define SMSE_VAR_SINF                        5 // High temperature saturation stress [Pa]
#define SMSE_VAR_THETA_0                     6 // strain hardening rate [Pa]
#define SMSE_VAR_B                           7 // Burgers vector [m]
#define SMSE_VAR_M                           8 // Taylor factor [-]
#define SMSE_VAR_ALPHA                       9 // Strengthening coefficient [-]
#define SMSE_VAR_C                           10 // Speed of sound [m/s]
#define SMSE_VAR_QVAC                        11 // Activation energy for vacancy formation [J]
//#define SMSE_VAR_QV                          11 // Activation energy for lattice diffusion [J]
#define SMSE_VAR_QM                          12 // Activation energy for effective matrix diffusion [J]
#define SMSE_VAR_DM0                         13 // Pre-exponential factor for effective matrix diffusion [m2/s]
#define SMSE_VAR_QD                          14 // Activation energy for pipe diffusion [J]
#define SMSE_VAR_DD0                         15 // Pre-exponential factor for pipe diffusion [m2/s]
#define SMSE_VAR_QG                          16 // Activation energy for grain boundary diffusion [J]
#define SMSE_VAR_DG0                         17 // Pre-exponential factor for grain boundary diffusion [m2/s]

#define SMSE_VAR_DFS0_LT                     18 // Low temp. act. energy for dynamic strength [J/mol]
#define SMSE_VAR_DFS0_HT                     19 // High temp. act. energy for dynamic strength [J/mol]
#define SMSE_VAR_DFTH0_LT                    20 // Low temp. act. energy for strain hardening rate [J/mol]
#define SMSE_VAR_DFTH0_HT                    21 // High temp. act. energy for strain hardening rate [J/mol]
#define SMSE_VAR_DYN_HT_EXP                  22  // High temp. factor for act. energy for strain hardening rate [J/mol]
#define SMSE_VAR_DFSINF_LT                   23  // Low temp. act. energy for saturation stress [J/mol]
#define SMSE_VAR_DFSINF_HT                   24  // High temp. act. energy for saturation stress [J/mol]
#define SMSE_VAR_DS_HT_SR_EXP_S0             28  // High temperature stran rate exponent
#define SMSE_VAR_DS_HT_SR_EXP_SSAT           29  // High temperature stran rate exponent
#define SMSE_VAR_DS_HT_SR_EXP_T0             30  // High temperature stran rate exponent
#define SMSE_VAR_DS_COUPLING_EXP             31  // dynamic strength contribution coupling exponent
#define SMSE_VAR_DFS0_LT_CC_1                40  // composition dependence coefficient for sigma_0_lt, element 1
#define SMSE_VAR_DFS0_LT_CE_1                41  // composition dependence exponent for sigma_0_lt, element 1
#define SMSE_VAR_DFS0_LT_CC_2                42  // composition dependence coefficient for sigma_0_lt, element 2
#define SMSE_VAR_DFS0_LT_CE_2                43  // composition dependence exponent for sigma_0_lt, element 2
#define SMSE_VAR_DFS0_HT_CC_1                44  // composition dependence coefficient for sigma_0_ht, element 1
#define SMSE_VAR_DFS0_HT_CE_1                45  // composition dependence exponent for sigma_0_ht, element 1
#define SMSE_VAR_DFS0_HT_CC_2                46  // composition dependence coefficient for sigma_0_ht, element 2
#define SMSE_VAR_DFS0_HT_CE_2                47  // composition dependence exponent for sigma_0_ht, element 2
#define SMSE_VAR_DFTH0_LT_CC_1               50  // composition dependence coefficient for theta_0_lt, element 1
#define SMSE_VAR_DFTH0_LT_CE_1               51  // composition dependence exponent for theta_0_lt, element 1
#define SMSE_VAR_DFTH0_LT_CC_2               52  // composition dependence coefficient for theta_0_lt, element 2
#define SMSE_VAR_DFTH0_LT_CE_2               53  // composition dependence exponent for theta_0_lt, element 2
#define SMSE_VAR_DFTH0_HT_CC_1               54  // composition dependence coefficient for theta_0_ht, element 1
#define SMSE_VAR_DFTH0_HT_CE_1               55  // composition dependence exponent for theta_0_ht, element 1
#define SMSE_VAR_DFTH0_HT_CC_2               56  // composition dependence coefficient for theta_0_ht, element 2
#define SMSE_VAR_DFTH0_HT_CE_2               57  // composition dependence exponent for theta_0_ht, element 2
#define SMSE_VAR_DFSINF_LT_CC_1              60  // composition dependence coefficient for sigma_sat_lt, element 1
#define SMSE_VAR_DFSINF_LT_CE_1              61  // composition dependence exponent for sigma_sat_lt, element 1
#define SMSE_VAR_DFSINF_LT_CC_2              62  // composition dependence coefficient for sigma_sat_lt, element 2
#define SMSE_VAR_DFSINF_LT_CE_2              63  // composition dependence exponent for sigma_sat_lt, element 2
#define SMSE_VAR_DFSINF_HT_CC_1              64  // composition dependence coefficient for sigma_sat_ht, element 1
#define SMSE_VAR_DFSINF_HT_CE_1              65  // composition dependence exponent for sigma_sat_ht, element 1
#define SMSE_VAR_DFSINF_HT_CC_2              66  // composition dependence coefficient for sigma_sat_ht, element 2
#define SMSE_VAR_DFSINF_HT_CE_2              67  // composition dependence exponent for sigma_sat_ht, element 2

#define SMSE_VAR_USE_FSAK_MODEL              70   // use FSAK model for excess vacancy evolution

#define SMSE_VAR_ABC_PARAMETER_A             130  // ABC-Model Parameter A
#define SMSE_VAR_ABC_PARAMETER_B             131  // ABC-Model Parameter B
#define SMSE_VAR_ABC_PARAMETER_C             132  // ABC-Model Parameter C

#define SMSE_VAR_SSS_COEFF_1                 140  // Solid solution strengthening coefficient for element 1
#define SMSE_VAR_SSS_COEFF_2                 141  // Solid solution strengthening coefficient for element 2
#define SMSE_VAR_SSS_EXP_1                   145  // Solid solution strengthening exponent for element 1
#define SMSE_VAR_SSS_EXP_2                   146  // Solid solution strengthening exponent for element 1
#define SMSE_VAR_SSS_COUPLING_EXP            150  // Solid solution strengthening coupling exponent

#define SMSE_VAR_CCD_DELTA_HC_1              160  // Cross core diffusion energy for element 1
#define SMSE_VAR_CCD_DELTA_W_BAR_1           161  // Cross core diffusion delta-omega-bar for element 1
#define SMSE_VAR_CCD_EXP_1                   162  // Cross core diffusion exponent for element 1
#define SMSE_VAR_CCD_DELTA_HC_2              165  // Cross core diffusion energy for element 2
#define SMSE_VAR_CCD_DELTA_W_BAR_2           166  // Cross core diffusion delta-omega-bar for element 2
#define SMSE_VAR_CCD_EXP_2                   167  // Cross core diffusion exponent for element 2
#define SMSE_VAR_CCD_ALPHA                   170  // Cross core diffusion: coefficient alpha
#define SMSE_VAR_CCD_OMEGA                   171  // Cross core diffusion: coefficient omega
#define SMSE_VAR_CCD_ATTEMPT_FREQUENCY       172  // Cross core diffusion: attempt frequency
#define SMSE_VAR_CCD_COUPLING_EXP            175  // Cross core diffusion: strength coupling coefficient

#define SMSE_VAR_RHO_EQ                      217 // equilibrium dislocation density
#define SMSE_VAR_VM_1                        218 // molar volume
#define SMSE_VAR_WP1                         219 // chemical composition element 1 [wt%] (Mg)
#define SMSE_VAR_WP2                         220 // chemical composition element 2 [wt%] (Si)
#define SMSE_VAR_M0                          221 // molar mass of matrix element [g/mol] (Al)
#define SMSE_VAR_M1                          222 // molar mass of element 1 [g/mol] (Mg)
#define SMSE_VAR_M2                          223 // molar mass of element 2 [g/mol] (Si)

#define SMSE_VAR_X1_P1                       234 // stoichiometry: Mg5Si6 (beta_double_prime)
#define SMSE_VAR_X2_P1                       235 // stoichiometry: Mg5Si6 (beta_double_prime)
#define SMSE_VAR_GAMMA_1                     236 // interfacial energy prec 1 [J/m2]
#define SMSE_VAR_GAMMA_PRT_1                 237 // effective interfacial energy for particle-related transformation of prec 1 [J/m2]
#define SMSE_VAR_LSW_FACT_1                  238 // coarsening factor for LSW model of prec 1 [J/m2]
#define SMSE_VAR_MIN_NUCL_R_1                239 // minimum nucleation radius for particle-related transformation of prec 1 [m]
#define SMSE_VAR_D_EFF_1_D0                  247 // effective diffusion coefficient for prec 1 (Mg): pre-exponential factor
#define SMSE_VAR_D_EFF_1_Q                   248 // effective diffusion coefficient for prec 1 (Mg): activation energy
#define SMSE_VAR_PREC1_PRE_FACT              249 // prefactor for delta_G calculation
#define SMSE_VAR_PREC1_A                     260 // Delta_G param A
#define SMSE_VAR_PREC1_B                     261 // Delta_G param B
#define SMSE_VAR_PREC1_N0                    262 // number of nucleation sites
#define SMSE_VAR_PREC1_N0_DISL_FACT          263 // number of nucleation sites factor for dislocations
#define SMSE_VAR_PREC1_N0_GB_FACT            264 // number of nucleation sites factor for grain boundaries
#define SMSE_VAR_PREC1_PC_WEAK               270 // precipitation strengthening coefficient for weak contribution
#define SMSE_VAR_PREC1_PC_OROWAN             271 // precipitation strengthening coefficient for Orowan mechanism
#define SMSE_VAR_PREC1_VOL_MISFIT            275 // precipitation strengthening: volumetric misfit
#define SMSE_VAR_PREC1_CLUSTER_DELTA_H       280 // precipitation strengthening: co-cluster strengthening after Wang and Starink

#define SMSE_VAR_X1_P2                       300 // stoichiometry: Mg5Si6 (beta_double_prime)
#define SMSE_VAR_X2_P2                       301 // stoichiometry: Mg5Si6 (beta_double_prime)
#define SMSE_VAR_GAMMA_2                     302 // interfacial energy prec 1 [J/m2]
#define SMSE_VAR_GAMMA_PRT_2                 303 // effective interfacial energy for particle-related transformation of prec 2 [J/m2]
#define SMSE_VAR_LSW_FACT_2                  304 // coarsening factor for LSW model of prec 2 [J/m2]
#define SMSE_VAR_MIN_NUCL_R_2                305 // minimum nucleation radius for particle-related transformation of prec 2 [m]
#define SMSE_VAR_D_EFF_2_D0                  310 // effective diffusion coefficient for prec 2 (Mg): pre-exponential factor
#define SMSE_VAR_D_EFF_2_Q                   311 // effective diffusion coefficient for prec 2 (Mg): activation energy
#define SMSE_VAR_PREC2_PRE_FACT              312 // prefactor for delta_G calculation
#define SMSE_VAR_PREC2_A                     320 // Delta_G param A
#define SMSE_VAR_PREC2_B                     321 // Delta_G param B
#define SMSE_VAR_PREC2_N0                    322 // number of nucleation sites
#define SMSE_VAR_PREC2_N0_DISL_FACT          323 // number of nucleation sites factor for dislocations
#define SMSE_VAR_PREC2_N0_GB_FACT            324 // number of nucleation sites factor for grain boundaries
#define SMSE_VAR_PREC2_PC_WEAK               330 // precipitation strengthening coefficient for weak contribution
#define SMSE_VAR_PREC2_PC_OROWAN             331 // precipitation strengthening coefficient for Orowan mechanism
#define SMSE_VAR_PREC2_VOL_MISFIT            335 // precipitation strengthening: volumetric misfit
#define SMSE_VAR_PREC2_CLUSTER_DELTA_H       340 // precipitation strengthening: co-cluster strengthening after Wang and Starink

#define SMSE_VAR_BETA_1                      400 // flow curve correction for low strains: beta_1
#define SMSE_VAR_BETA_2                      401 // flow curve correction for low strains: beta_2
#define SMSE_VAR_BETA_EXP                    402 // flow curve correction for low strains: exponent for summation
#define SMSE_VAR_INITIAL_GRAIN_DIAMETER      410 // initial grain diameter
#define SMSE_VAR_HALL_PETCH_COEFF            411 // Hall-Petch coefficient


// c++ exceptions to API error code
#define ERROR_CPP_EXCEPTION          0xf0000000
#define ERROR_LOGIC_ERROR            (ERROR_CPP_EXCEPTION |  100)
#define ERROR_INVALID_ARGUMENT       (ERROR_CPP_EXCEPTION |  200)
#define ERROR_DOMAIN_ERROR           (ERROR_CPP_EXCEPTION |  300)
#define ERROR_LENGTH_ERROR           (ERROR_CPP_EXCEPTION |  400)
#define ERROR_OUT_OF_RANGE           (ERROR_CPP_EXCEPTION |  500)
#define ERROR_FUTURE_ERROR           (ERROR_CPP_EXCEPTION |  600)
#define ERROR_BAD_OPTIONAL_ACCESS    (ERROR_CPP_EXCEPTION |  700)
#define ERROR_RUNTIME_ERROR          (ERROR_CPP_EXCEPTION |  800)
#define ERROR_RANGE_ERROR            (ERROR_CPP_EXCEPTION |  900)
#define ERROR_OVERFLOW_ERROR         (ERROR_CPP_EXCEPTION | 1000)
#define ERROR_UNDERFLOW_ERROR        (ERROR_CPP_EXCEPTION | 1100)
#define ERROR_REGEX_ERROR            (ERROR_CPP_EXCEPTION | 1200)
#define ERROR_NONEXISTENT_LOCAL_TIME (ERROR_CPP_EXCEPTION | 1300)
#define ERROR_AMBIGUOUS_LOCAL_TIME   (ERROR_CPP_EXCEPTION | 1400)
#define ERROR_TX_EXCEPTION           (ERROR_CPP_EXCEPTION | 1500)
#define ERROR_SYSTEM_ERROR           (ERROR_CPP_EXCEPTION | 1600)
#define ERROR_IOS_BASE_FAILURE       (ERROR_CPP_EXCEPTION | 1700)
#define ERROR_FILESYSTEM_ERROR       (ERROR_CPP_EXCEPTION | 1800)
#define ERROR_BAD_TYPEID             (ERROR_CPP_EXCEPTION | 1900)
#define ERROR_BAD_CAST               (ERROR_CPP_EXCEPTION | 2000)
#define ERROR_BAD_ANY_CAST           (ERROR_CPP_EXCEPTION | 2100)
#define ERROR_BAD_WEAK_PTR           (ERROR_CPP_EXCEPTION | 2200)
#define ERROR_BAD_FUNCTION_CALL      (ERROR_CPP_EXCEPTION | 2300)
#define ERROR_BAD_ALLOC              (ERROR_CPP_EXCEPTION | 2400)
#define ERROR_BAD_ARRAY_NEW_LENGTH   (ERROR_CPP_EXCEPTION | 2500)
#define ERROR_BAD_EXCEPTION          (ERROR_CPP_EXCEPTION | 2600)
#define ERROR_BAD_VARIANT_ACCESS     (ERROR_CPP_EXCEPTION | 2700)


//**********************************************

// dialog types used for certain gui windows

#define DLG_TYPE_PRECIPITATION      1
#define DLG_TYPE_CELL_SIMULATON     5
#define DLG_TYPE_REGION_CALC        10


#ifdef MY_QT
#include <stdexcept>
extern int MCC_ExceptionToErrorCode(std::exception const& );
#endif

#endif
