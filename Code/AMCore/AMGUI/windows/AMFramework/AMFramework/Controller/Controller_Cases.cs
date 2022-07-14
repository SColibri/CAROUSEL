using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections;
using System.Windows.Input;

namespace AMFramework.Controller
{
    public class Controller_Cases : INotifyPropertyChanged
    {

        #region Cons_Des
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller.Controller_DBS_Projects _ControllerProjects;
        private int _idProject = -1;
        private int _selectedIDCase = -1;

        public Controller_Cases(ref Core.IAMCore_Comm socket,
                               Controller.Controller_DBS_Projects _project)
        {
            _AMCore_Socket = socket;
            _ControllerProjects = _project;
            _selectedPhases = new Controller_Selected_Phases(ref socket, this);
            _elementComposition = new(ref socket, this);
            _equilibriumPhaseFractions = new(ref socket, this);
            _equilibriumConfigurations = new(ref socket, this);
            _scheilConfigurations = new(ref socket, this);
            _scheilPhaseFractions = new(ref socket, this);
        }

        public Controller_Cases()
        {

        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region getters
        public Controller.Controller_DBS_Projects get_project_controller()
        {
            return _ControllerProjects;
        }
        #endregion

        #region Data

        List<Model.Model_Case> _cases = new List<Model.Model_Case>();

        public List<Model.Model_Case> Cases
        {
            get
            {
                return _cases;
            }
        }

        public void save(Model.Model_Case model)
        {
            string outComm = _AMCore_Socket.run_lua_command("singlepixel_case_save", model.get_csv());
            if (outComm.CompareTo("OK") != 0)
            {
                MainWindow.notify.ShowBalloonTip(5000, "Error: Case was not saved", outComm, System.Windows.Forms.ToolTipIcon.Error);
            }
        }

        public void save_Handle(object sender, EventArgs e) 
        {
            save(SelectedCase);
            refresh();
        }

        #endregion

        #region Flags
        private Model.Model_Case _selected_case;
        public Model.Model_Case SelectedCase 
        { 
            get { return _selected_case; } 
            set 
            {
                _selected_case = value;
                OnPropertyChanged("SelectedCase");
                OnPropertyChanged("SelectedPhases");
                OnPropertyChanged("ElementComposition");
            }
        }

        private bool _show_popup = false;
        public bool ShowPopup 
        {
            get { return _show_popup;}
            set 
            {
                _show_popup = value;
                OnPropertyChanged("ShowPopup");
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
                OnPropertyChanged("PopupView");
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
            string outy = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outy.Split("\n").ToList();
            _cases = new List<Model.Model_Case>();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();


                if (columnItems.Count > 8)
                {
                    Model.Model_Case model = new Model.Model_Case();
                    model.ID = Convert.ToInt32(columnItems[0]);
                    model.IDProject = Convert.ToInt32(columnItems[1]);
                    model.IDGroup = Convert.ToInt32(columnItems[2]);
                    model.Name = columnItems[3];
                    model.Script = columnItems[4];
                    model.Date = columnItems[5];
                    model.PosX = Convert.ToDouble(columnItems[6]);
                    model.PosY = Convert.ToDouble(columnItems[7]);
                    model.PosZ = Convert.ToDouble(columnItems[8]);
                    model.ElementComposition = new();
                    _cases.Add(model);
                }
            }

            _selectedPhases.fill_models_with_selectedPhases();
            _elementComposition.fill_models_with_composition();
            _equilibriumConfigurations.fill_models_with_equilibroiumConfiguration();
            OnPropertyChanged("Cases");
            return outy;
        }

        public void update_phaseFractions(Model.Model_Case model) 
        {
            if (model is null) return;
            _equilibriumPhaseFractions.fill_model_phase_fraction(model);
            _scheilPhaseFractions.fill_model_phase_fraction(model);
        }

        public void Clear_phase_fractions(Model.Model_Case model)
        {
            _equilibriumPhaseFractions.clear_models_phaseFractions();
        }
        #endregion

        #region Controllers
        private Controller.Controller_Selected_Phases _selectedPhases;
        public List<Model.Model_SelectedPhases> SelectedPhases { get { return _selectedPhases.Phases; } }

        private Controller.Controller_ElementComposition _elementComposition;
        public List<Model.Model_ElementComposition> ElementComposition { get { return _elementComposition.Composition; } }

        private Controller.Controller_EquilibriumConfiguration _equilibriumConfigurations;
        private Controller.Controller_EquilibriumPhaseFraction _equilibriumPhaseFractions;
        public List<Model.Model_EquilibriumPhaseFraction> EquilibriumPhaseFraction { get { return _equilibriumPhaseFractions.Equilibrium; } }

        private Controller.Controller_ScheilConfiguration _scheilConfigurations;
        private Controller.Controller_ScheilPhaseFraction _scheilPhaseFractions;
        public List<Model.Model_ScheilPhaseFraction> ScheilPhaseFraction { get { return _scheilPhaseFractions.Equilibrium; } }


        #endregion

        #region Templates

        public void Create_templates(Model.Model_Case OriginalCase) 
        { 
            if(OriginalCase.CaseTemplates.Count == 0) 
            {
                MainWindow.notify.ShowBalloonTip(5000, "Missing templates", "Please define one or more templates",System.Windows.Forms.ToolTipIcon.Info);
                return;
            }

            // create new case template
            _AMCore_Socket.run_lua_command("template_pixelcase_new", "");

            // send element composition
            string compositionString = OriginalCase.ElementComposition[0].ElementName + "||" + OriginalCase.ElementComposition[0].Value.ToString();
            foreach (Model.Model_ElementComposition comp in OriginalCase.ElementComposition.Skip(1))
            {
                compositionString += "||" + comp.ElementName + "||" + comp.Value.ToString();
            }
            _AMCore_Socket.run_lua_command("template_pixelcase_setComposition ", compositionString);

            string phaseString = OriginalCase.SelectedPhases[0].PhaseName;
            foreach (Model.Model_SelectedPhases comp in OriginalCase.SelectedPhases.Skip(1))
            {
                phaseString += "||" + comp.PhaseName ;
            }
            _AMCore_Socket.run_lua_command("template_pixelcase_selectPhases ", phaseString);
            _AMCore_Socket.run_lua_command("template_pixelcase_setEquilibriumTemperatureRange ", 
                                            OriginalCase.EquilibriumConfiguration.StartTemperature.ToString() + "||" +
                                            OriginalCase.EquilibriumConfiguration.EndTemperature.ToString() + "||" +
                                            OriginalCase.EquilibriumConfiguration.StepSize.ToString());

            _AMCore_Socket.run_lua_command("template_pixelcase_setScheilTemperatureRange ",
                                            OriginalCase.ScheilConfiguration.StartTemperature.ToString() + "||" +
                                            OriginalCase.ScheilConfiguration.EndTemperature.ToString() + "||" +
                                            OriginalCase.ScheilConfiguration.StepSize.ToString());

            _AMCore_Socket.run_lua_command("template_pixelcase_setScheilLiquidFraction ",
                                            OriginalCase.ScheilConfiguration.MinLiquidFraction.ToString());

           

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
            string Query = SelectedCase.IDProject + "||" + SelectedCase.ID + "-" + SelectedCase.ID;
            string outMessage = _AMCore_Socket.run_lua_command("pixelcase_step_equilibrium_parallel ", Query);
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
            save(SelectedCase);
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
