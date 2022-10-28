using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections;
using System.Windows.Input;
using System.Net.Sockets;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Controller;


namespace AMFramework.Controller
{
    public class Controller_Cases : ControllerAbstract
    {

        #region Cons_Des
        private Controller.Controller_DBS_Projects _ControllerProjects;
        private int _idProject = -1;
        private int _selectedIDCase = -1;

        public Controller_Cases(ref IAMCore_Comm socket,
                               Controller.Controller_DBS_Projects _project) : base(socket)
        {
            _comm = socket;
            _ControllerProjects = _project;
            _selectedPhases = new Controller_Selected_Phases(ref socket, this);
            _elementComposition = new(ref socket, this);
            _equilibriumPhaseFractions = new(ref socket, this);
            _equilibriumConfigurations = new(ref socket, this);
            _scheilConfigurations = new(ref socket, this);
            _scheilPhaseFractions = new(ref socket, this);
            _PrecipitationPhase = new(ref socket, this);
            _PrecipitationDomain = new(ref socket, this);
            _Controller_HeatTreatment = new(ref socket, this);
            _Controller_PrecipitateSimulationData = new(ref socket, _Controller_HeatTreatment);
            

        }

        public Controller_Cases(ref IAMCore_Comm comm, Controller_Project projectController) : base(comm)
        {
            _comm = comm;
            //_ControllerProjects = _project;
            _selectedPhases = new Controller_Selected_Phases(ref comm, this);
            _elementComposition = new(ref comm, this);
            _equilibriumPhaseFractions = new(ref comm, this);
            _equilibriumConfigurations = new(ref comm, this);
            _scheilConfigurations = new(ref comm, this);
            _scheilPhaseFractions = new(ref comm, this);
            _PrecipitationPhase = new(ref comm, this);
            _PrecipitationDomain = new(ref comm, this);
            _Controller_HeatTreatment = new(ref comm, this);
            _Controller_PrecipitateSimulationData = new(ref comm, _Controller_HeatTreatment);
        }
        #endregion

        #region getters
        public Controller.Controller_DBS_Projects get_project_controller()
        {
            return _ControllerProjects;
        }
        #endregion

        #region Data

        List<Model_Case> _cases = new();

        public List<Model_Case> Cases
        {
            get
            {
                return _cases;
            }
        }

        public void save(Model_Case model)
        {
            string outComm = _comm.run_lua_command("singlepixel_case_save", model.Get_csv());
            if (outComm.CompareTo("OK") != 0)
            {
                MainWindow.notify.ShowBalloonTip(5000, "Error: Case was not saved", outComm, System.Windows.Forms.ToolTipIcon.Error);
            }
        }

        public void save_Handle(object sender, EventArgs e) 
        {
            save(SelectedCaseOLD);
            refresh();
        }

        List<string> _precipitationPhasesNames = new();

        private List<Model_Phase> _availablePhases = new();
        public List<Model_Phase> AvailablePhases { get { return _availablePhases; } }

        public void load_database_available_phases()
        {
            _availablePhases = Controller_Phase.Get_available_phases_in_database(ref _comm);
            OnPropertyChanged("AvailablePhase");
        }

        public void get_phase_selection_from_current_case()
        {
            foreach (Model_Phase item in AvailablePhases)
            {
                item.IsSelected = false;
            }

            foreach (var item in SelectedPhases)
            {
                Model_Phase? tempFindPhase = AvailablePhases.Find(e => e.ID == item.IDPhase);
                if (tempFindPhase is null) continue;
                tempFindPhase.IsSelected = true;
            }

        }
        #endregion

        #region Flags
        private Model_Case _selected_caseOLD;
        public Model_Case SelectedCaseOLD 
        { 
            get { return _selected_caseOLD; } 
            set 
            {
                _selected_caseOLD = value;
                _selectedIDCase = _selected_caseOLD.ID;
                _ControllerProjects.get_phase_selection_from_current_case();
                _ControllerProjects.set_phase_selection_ifActive();
                _selectedPhases.refresh();
                _scheilConfigurations.Model = _scheilConfigurations.get_scheil_configuration_case(_selected_caseOLD.ID);
                OnPropertyChanged("SelectedCase");
                OnPropertyChanged(nameof(SelectedPhases));
                OnPropertyChanged(nameof(ElementComposition));
            }
        }

        private bool _show_popup = false;
        public bool ShowPopup 
        {
            get { return _show_popup;}
            set 
            {
                _show_popup = value;
                OnPropertyChanged(nameof(ShowPopup));
            }
        }
        #endregion

        #region Views
        private Components.Windows.AM_popupWindow _popupView = new();

        public Components.Windows.AM_popupWindow PopupView 
        { 
            get => _popupView;
            set 
            { 
                _popupView = value;
                OnPropertyChanged(nameof(PopupView));
            }
        }
        #endregion

        #region Methods
        public void refresh() 
        {
            load_data();
        }

        private string load_data() 
        {
            string Query = "database_table_custom_query SELECT * FROM \'Case\' WHERE IDProject = " + _ControllerProjects.SelectedProject.ID.ToString();
            string outy = _comm.run_lua_command(Query,"");
            List<string> rowItems = outy.Split("\n").ToList();
            _cases = new List<Model_Case>();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();


                if (columnItems.Count > 8)
                {
                    Model_Case model = new()
                    {
                        ID = Convert.ToInt32(columnItems[0]),
                        IDProject = Convert.ToInt32(columnItems[1]),
                        IDGroup = Convert.ToInt32(columnItems[2]),
                        Name = columnItems[3],
                        Script = columnItems[4],
                        Date = columnItems[5],
                        PosX = Convert.ToDouble(columnItems[6]),
                        PosY = Convert.ToDouble(columnItems[7]),
                        PosZ = Convert.ToDouble(columnItems[8]),
                        ElementCompositionOLD = new()
                    };
                    _cases.Add(model);
                    Controller_HeatTreatment.fill_case_model(ref _comm, model);
                }
            }

            _selectedPhases.fill_models_with_selectedPhases();
            _elementComposition.fill_models_with_composition();
            _equilibriumConfigurations.fill_models_with_equilibroiumConfiguration();
            _PrecipitationPhase.fill_models_with_precipitation_phases();
            _PrecipitationDomain.fill_models_with_precipitation_domains();
            _precipitationPhasesNames = Controller_PrecipitationPhase.Get_phases_names(_comm, _ControllerProjects.SelectedProject.ID);
            OnPropertyChanged(nameof(Cases));
            return outy;
        }

        public void update_phaseFractions(Model_Case model) 
        {
            if (model is null) return;
            _equilibriumPhaseFractions.fill_model_phase_fraction(model);
            _scheilPhaseFractions.fill_model_phase_fraction(model);
        }

        public void Clear_phase_fractions(Model_Case model)
        {
            _equilibriumPhaseFractions.clear_models_phaseFractions();
        }
        #endregion

        #region Controllers
        private Controller.Controller_Selected_Phases _selectedPhases;
        private Controller.Controller_ElementComposition _elementComposition;
        private Controller.Controller_EquilibriumConfiguration _equilibriumConfigurations;
        private Controller.Controller_EquilibriumPhaseFraction _equilibriumPhaseFractions;
        private Controller.Controller_ScheilConfiguration _scheilConfigurations;
        private Controller.Controller_ScheilPhaseFraction _scheilPhaseFractions;
        private Controller.Controller_PrecipitationPhase _PrecipitationPhase;
        private Controller.Controller_PrecipitationDomain _PrecipitationDomain;
        private Controller.Controller_PrecipitateSimulationData _Controller_PrecipitateSimulationData;
        private Controller.Controller_HeatTreatment _Controller_HeatTreatment;

        public List<Model_SelectedPhases> SelectedPhases { get { return _selectedPhases.Phases; } }
        public List<Model_ElementComposition> ElementComposition { get { return _elementComposition.Composition; } }
        public List<Model_EquilibriumPhaseFraction> EquilibriumPhaseFraction { get { return _equilibriumPhaseFractions.Equilibrium; } }
        public List<Model_ScheilPhaseFraction> ScheilPhaseFraction { get { return _scheilPhaseFractions.Equilibrium; } }

        public Controller.Controller_PrecipitationPhase PrecipitationPhaseController { get { return _PrecipitationPhase; } }
        public Controller.Controller_ScheilConfiguration ScheilConfigurationsController { get { return _scheilConfigurations; } }
        public Controller.Controller_ScheilPhaseFraction ScheilPhaseFractionsController { get { return _scheilPhaseFractions; } }
        #endregion

        #region Templates

        public void Create_templates(Model_Case OriginalCase) 
        { 
            if(OriginalCase.CaseTemplates.Count == 0) 
            {
                MainWindow.notify.ShowBalloonTip(5000, "Missing templates", "Please define one or more templates",System.Windows.Forms.ToolTipIcon.Info);
                return;
            }

            // create new case template
            _comm.run_lua_command("template_pixelcase_new", "");

            // send element composition
            string compositionString = OriginalCase.ElementCompositionOLD[0].ElementName + "||" + OriginalCase.ElementCompositionOLD[0].Value.ToString();
            foreach (Model_ElementComposition comp in OriginalCase.ElementCompositionOLD.Skip(1))
            {
                compositionString += "||" + comp.ElementName + "||" + comp.Value.ToString();
            }
            _comm.run_lua_command("template_pixelcase_setComposition ", compositionString);

            string phaseString = OriginalCase.SelectedPhasesOLD[0].PhaseName;
            foreach (Model_SelectedPhases comp in OriginalCase.SelectedPhasesOLD.Skip(1))
            {
                phaseString += "||" + comp.PhaseName ;
            }
            _comm.run_lua_command("template_pixelcase_selectPhases ", phaseString);
            _comm.run_lua_command("template_pixelcase_setEquilibriumTemperatureRange ", 
                                            OriginalCase.EquilibriumConfigurationOLD.StartTemperature.ToString() + "||" +
                                            OriginalCase.EquilibriumConfigurationOLD.EndTemperature.ToString() + "||" +
                                            OriginalCase.EquilibriumConfigurationOLD.StepSize.ToString());

            _comm.run_lua_command("template_pixelcase_setScheilTemperatureRange ",
                                            OriginalCase.ScheilConfigurationOLD.StartTemperature.ToString() + "||" +
                                            OriginalCase.ScheilConfigurationOLD.EndTemperature.ToString() + "||" +
                                            OriginalCase.ScheilConfigurationOLD.StepSize.ToString());

            _comm.run_lua_command("template_pixelcase_setScheilLiquidFraction ",
                                            OriginalCase.ScheilConfigurationOLD.MinLiquidFraction.ToString());

           

        }

        #endregion

        #region Commands
        #region run_equilibrium

        private ICommand _run_equilibrium;
        public ICommand Run_equilibrium
        {
            get
            {
                if (_run_equilibrium == null)
                {
                    _run_equilibrium = new RelayCommand(
                        param => this.Run_equilibrium_controll(),
                        param => this.Can_Run_equilibrium()
                    );
                }
                return _run_equilibrium;
            }
        }

        private void Run_equilibrium_controll()
        {
            string Query = SelectedCaseOLD.IDProject + "||" + SelectedCaseOLD.ID + "-" + SelectedCaseOLD.ID;
            string outMessage = _comm.run_lua_command("pixelcase_step_equilibrium_parallel ", Query);
            if (outMessage.CompareTo("OK") == 0) 
            { 
            
            }

            refresh();
        }

        private bool Can_Run_equilibrium()
        {
            return true;
        }
        #endregion

        #region run_scheil

        private ICommand _run_scheil;
        public ICommand Run_scheil
        {
            get
            {
                if (_run_scheil == null)
                {
                    _run_scheil = new RelayCommand(
                        param => this.Run_scheil_controll(),
                        param => this.Can_Run_scheil()
                    );
                }
                return _run_scheil;
            }
        }

        private void Run_scheil_controll()
        {

            refresh();
        }

        private bool Can_Run_scheil()
        {
            return true;
        }
        #endregion

        #region Save

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_run_equilibrium == null)
                {
                    _run_equilibrium = new RelayCommand(
                        param => this.Run_Save_Command(),
                        param => this.Can_Save_Command()
                    );
                }
                return _run_equilibrium;
            }
        }

        private void Run_Save_Command()
        {
            save(SelectedCaseOLD);
            refresh();
        }

        private bool Can_Save_Command()
        {
            return true;
        }
        #endregion

        #endregion

    }
}
