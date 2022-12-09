#ifndef SHARED_TYPES_H
#define SHARED_TYPES_H

// Grain size

#include <string>
#include <cstring>

#ifdef __cplusplus
#include <tuple>
#endif

struct struct_randomize_prec_params
{
    double rand_ie_fact;
    double rand_D_fact;
    double rand_dfm_fact;
};

class TDMGRElementData
{
public:
    char name[32];
    char def_ref_state[32];
    double mass;
    double enthalpie;
    double entropie;
    double x0;                    //Gesamtgehalt
    double x0p;                   //Gesamtgehalt in wt%
    int IsSelected;
    int OldSelected;  //fuer Ueberpruefung->Aenderung der Auswahl
public:
    TDMGRElementData()
    {
        name[0] = 0;
        def_ref_state[0] = 0;
        mass = 0.0;
        enthalpie = 0.0;
        entropie = 0.0;
        x0 = 0.0;
        x0p = 0.0;
        IsSelected = false;
        OldSelected = false;
    }
};

class TDMGRPhaseData
{
public:
    int NameIndex;			//Index auf PhasenName
    int ConstIndex;			//Index auf Constituents in Constituentsarray
    char name[32];			//Phasenname
    char constituents[400];	//Constituents
    char datatype[32];  //Dataype aus TDB-File
    int NumSL;										//Anzahl Sublattices
    double Sites[8];			 		 			//Anzahl Plaetze auf Sublattices
    double p;         //Magnetic Parameter
    int IsSelected;
    int OldSelected;
    int IsPossible;
    int disordered_index;	//gibt's disordered part?
    int disorder_type;	//welches order-disorder model?
    char phase_description[400];	//short description of phase
    int recommended_phase_relevance;	//0 .. seldom used, 1 .. sometimes used, 2 .. often used

public:
    TDMGRPhaseData()
    {
        NameIndex = -1;
        ConstIndex = -1;
        name[0] = 0;
        constituents[0] = 0;
        datatype[0] = 0;
        NumSL = 0;
        p=0.0;
        IsSelected = false;
        OldSelected = false;
        IsPossible = false;
        disordered_index = -1;
        disorder_type = 0;	//DT_REGULAR_Y
        phase_description[0] = 0;
        recommended_phase_relevance = 0;	//lowest level
    }
};

class TDMGRCompSet
{
public:
    char PhaseName[32];
    char CompSetString[200];
    int CompSetIndex;
public:
    TDMGRCompSet()
    {
        PhaseName[0] = 0;
        CompSetString[0] = 0;
        CompSetIndex = -1;
    }
};

class CMPS_ParamData
{
public:
    int type_code;
    int step_type;
    double start;
    double stop;
    int intervals;
    int table_handle;
public:
    CMPS_ParamData()
    {
        type_code = -1;
        step_type = -1;
        start = 0.0;
        stop = 0.0;
        intervals = 0;
        table_handle = -1;
    }
};

struct CMCSlipSystem_Data
{
	using Self = CMCSlipSystem_Data;

    bool IsActive = false;
    const char *name = 0;

    enum class SlipType {
        Invalid = -1,
        Slip,
        CrossSlip,
        Twinning
    } slip_type = SlipType::Slip;

    enum class LoadType {
        Invalid = -1,
        Tension,
        Compression,
        Symmetric
    } load_type = LoadType::Tension;

    // Miller indices of slipping direction [U V T W]
    int sd_U = 0;
    int sd_V = 0;
    int sd_T = 0; //only needed in hcp. Redundant, because h+k+i=0
    int sd_W = 0;

    // Miller indices of slipping plane normal (h k i l)
    int sn_h = 0; // slip / twinning plane normal
    int sn_k = 0;
    int sn_i = 0; //redundant in hcp: U+V+T=0
    int sn_l = 0;

    // activation threshold
    const char *CRSS = 0; //critical resolved shear stress

    // ************************
    // temporary variables ...
    // transformation from four-index to three-index coordinates crystal coordinates
    // vector with length 1
    double U_prime = 0.0;
    double V_prime = 0.0;
    double W_prime = 0.0;

    double h_prime = 0.0;
    double k_prime = 0.0;
    double l_prime = 0.0;

    double sigma_i = 0.0;

#ifdef MY_QT
    CMCSlipSystem_Data() = default;
    CMCSlipSystem_Data(CMCSlipSystem_Data const& rhs);
    CMCSlipSystem_Data& operator = (CMCSlipSystem_Data const& rhs);

	static constexpr auto Variables()
	{
		return std::make_tuple(
		            std::make_pair("active", &Self::IsActive),
		            std::make_pair("sd_U", &Self::sd_U),
		            std::make_pair("sd_V", &Self::sd_V),
		            std::make_pair("sd_T", &Self::sd_T),
		            std::make_pair("sd_W", &Self::sd_W),
		            std::make_pair("sn_h", &Self::sn_h),
		            std::make_pair("sn_k", &Self::sn_k),
		            std::make_pair("sn_i", &Self::sn_i),
		            std::make_pair("sn_l", &Self::sn_l),
		            std::make_pair("slip_type", &Self::slip_type),
		            std::make_pair("load_type", &Self::load_type),
		            std::make_pair("CRSS", &Self::CRSS),
		            std::make_pair("sigma_i", &Self::sigma_i)
		            );
	}
#endif
};

// SimpleMSE
struct CMCSimpleMSE_Data_v1
{
	// THE x-variable ...
	double time;

    // ... and all input parameters ...
    double paramA;
	double paramB;
	double paramC;
    double eps_dot;
    double sigma_0_lt;
    double theta_0_lt;
    double sigma_inf_lt;
    double sigma_0_ht;
    double theta_0_ht;
    double sigma_inf_ht;
    double sigma_0_dyn;
    double theta_0;
    double sigma_yield_beta;        // beta function
    double sigma_yield_0;           // sigma without beta function
    double sigma_yield;             // untimate yield strength
    double dsigma_deps;             // gradient
    double dsigma_deps_dot;         // gradient
    double sigma_0_nhc;             // Nabarro-Herring creep
    double sigma_0_coc;             // Coble creep

	double dcrit;

    // ... and all y-variables ...
    double temperature;             // temperature
    double acc_eps;                 // accumulated strain
    double disl_dens;               // dislocation density
    double disl_dens_sat;           // saturation dislocation density
    double grain_diameter;              // grain size
    double subgrain_diameter;          // subgrain size
    double d_grain_diameter;            // grain size gradient
    double d_subgrain_diameter;        // subgrain size gradient

    //compositions
    double X_1;                     // matrix content element 1
    double X_2;                     // matrix content element 2
    double X_1_eff_ccd;             // effective concentration from ccd, element 1
    double X_2_eff_ccd;             // effective concentration from ccd, element 2

    //vacancy evolution
    double X_Va_equ;                // excess vacancy rate
    double X_Va_curr;               // current excess vacancy rate
    double X_Va_dot;                // equilibrium excess vacancy rate

    //precipitates
    double prec1_f;                 // precipitate 1 phase
    double prec1_f_equ;             // precipitate 1 phase, equilibrium
    double prec1_X_1_equ;           // precipitate 1 element 1, equilibrium
    double prec1_X_2_equ;           // precipitate 1 element 2, equilibrium
    double prec1_N;                 // precipitate 1 number density
    double prec1_r;                 // precipitate 1 mean radius
    double prec1_dr;                // precipitate 1 mean growth rate
    double prec1_dr_coarse;         // precipitate 1 mean coarsening rate
    double prec1_dfm;               // precipitate 1 driving force
    double prec1_G_star;            // precipitate 1 critical nucleation energy
    double prec1_r_crit;            // precipitate 1 critical nucleation radius
    double prec1_beta_star;         // precipitate 1 beta star
    double prec1_J;                 // precipitate 1 nucleationn rate
    double prec1_Z;                 // precipitate 1 nucleationn rate
    double prec1_Deff;              // precipitate 1 effective diffusion coefficient
    double prec2_f;                 // precipitate 2 phase
    double prec2_f_equ;             // precipitate 2 phase, equilibrium
    double prec2_X_1_equ;           // precipitate 2 element 1, equilibrium
    double prec2_X_2_equ;           // precipitate 2 element 2, equilibrium
    double prec2_N;                 // precipitate 2 number density
    double prec2_r;                 // precipitate 2 mean radius
    double prec2_dr;                // precipitate 2 mean growth rate
    double prec2_dr_coarse;         // precipitate 2 mean coarsening rate
    double prec2_dfm;               // precipitate 2 driving force
    double prec2_G_star;            // precipitate 2 critical nucleation energy
    double prec2_r_crit;            // precipitate 2 critical nucleation radius
    double prec2_beta_star;         // precipitate 2 beta star
    double prec2_J;                 // precipitate 2 nucleationn rate
    double prec2_Z;                 // precipitate 2 nucleationn rate
    double prec2_Deff;              // precipitate 2 effective diffusion coefficient


    // total flow stress and contributions ...
    double sigma_basic;             // basic yield stress
    double sigma_sss_1;             // solid solution strengthening yield stress element 1
    double sigma_sss_2;             // solid solution strengthening yield stress element 2
    double sigma_sss;               // total solid solution strengthening yield stress
    double sigma_ccd_1;             // cross core diffusion yield stress element 1
    double sigma_ccd_2;             // cross core diffusion yield stress element 2
    double sigma_ccd;               // total cross core diffusion yield stress
    double sigma_gd;                // Hall-Petch (grain size) strengthening
    double sigma_disl;              // dislocation forest hardening
    double sigma_prec1_LS;           // precipitation strengthening: mean 2D distance on glide plane
    double sigma_prec1_weak;        // precipitation strengthening: Weak contribution
    double sigma_prec1_Orowan;      // precipitation strengthening: Orowan contribution
    double sigma_prec1_cluster;     // co-cluster strengthening
    double sigma_prec1;             // precipitation strengthening
    double sigma_prec2_LS;          // precipitation strengthening: mean 2D distance on glide plane
    double sigma_prec2_weak;        // precipitation strengthening: Weak contribution
    double sigma_prec2_Orowan;      // precipitation strengthening: Orowan contribution
    double sigma_prec2_cluster;     // co-cluster strengthening
    double sigma_prec2;             // precipitation strengthening
    double sigma_prec;              // precipitation strengthening
    double sigma_0_th;              // total thermal yield stress (thermally activated)
    double sigma_0_ath;             // total athermal yield stress
    double sigma_mt;                // total thermal yield stress (no thermal activation; = mechanical threshold)
};

using CMCSimpleMSE_Data = CMCSimpleMSE_Data_v1;

union CMCSimpleMSE_VariableData
{
	bool flag;
	double value_as_double;
};


#endif // SHARED_TYPES_H
