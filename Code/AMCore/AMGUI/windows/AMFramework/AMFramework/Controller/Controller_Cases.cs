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
using AMFramework_Lib.Model.Model_Controllers;
using AMFramework.Views.Phase;
using System.Windows.Controls;
using AMFramework_Lib.Structures;
using AMFramework.Views.HeatTreatments;
using AMFramework.Views.Solidification;

namespace AMFramework.Controller
{
    public class Controller_Cases : ControllerAbstract
    {
        #region Fields
        private Controller_Project _projectController;

        #endregion
        #region Cons_Des
        public Controller_Cases(ref IAMCore_Comm comm, Controller_Project projectController) : base(comm)
        {
            _comm = comm;

            _projectController = projectController;

            if (_projectController.SelectedProject != null)
                Cases = _projectController.SelectedProject.MCObject.ModelObject.Cases;

            if (projectController.SelectedProject != null)
                Cases = ControllerM_Case.Get_CasesByIDProject(comm, projectController.SelectedProject.MCObject.ModelObject.ID);

            //_selectedPhases = new Controller_Selected_Phases(ref comm, this);
            //_elementComposition = new(ref comm, this);
            //_equilibriumPhaseFractions = new(ref comm, this);
            //_equilibriumConfigurations = new(ref comm, this);
            //_scheilConfigurations = new(ref comm, this);
            //_scheilPhaseFractions = new(ref comm, this);
            //_PrecipitationPhase = new(ref comm, this);
            //_PrecipitationDomain = new(ref comm, this);
            //_Controller_HeatTreatment = new(ref comm, this);
            //_Controller_PrecipitateSimulationData = new(ref comm, _Controller_HeatTreatment);
        }
        #endregion

        #region views

        #region Popup
        private bool _show_popup = false;
        public bool ShowPopup
        {
            get { return _show_popup; }
            set
            {
                _show_popup = value;
                OnPropertyChanged(nameof(ShowPopup));
            }
        }

        private Components.Windows.AM_popupWindow _popupView = new();
        /// <summary>
        /// popup window of type AM_popupWindow, maybe we should box it to an object class to make it more generic
        /// </summary>
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

        #region PhaseList
        private object? _phaseList = null;
        /// <summary>
        /// phaseList view, consider not using object?
        /// </summary>
        public object? PhaseList 
        {
            get { return _phaseList; }
            set 
            {
                _phaseList = value;
                OnPropertyChanged(nameof(PhaseList));
            }
        }

        #endregion

        #region Solidification simulation
        private object? _solidificationConfiguration = null;
        /// <summary>
        /// Solidification configuration view container
        /// </summary>
        public object? SolidificationConfiguration
        {
            get => _solidificationConfiguration;
            set
            {
                _solidificationConfiguration = value;
                OnPropertyChanged(nameof(SolidificationConfiguration));
            }
        }
        #endregion

        #region HeatTreatment
        private object? _heatTreatmentView = null;
        /// <summary>
        /// Heat treatment view container
        /// </summary>
        public object? HeatTreatmentView 
        {
            get => _heatTreatmentView;
            set 
            {
                _heatTreatmentView = value;
                OnPropertyChanged(nameof(HeatTreatmentView));
            }
        }
        #endregion

        #endregion

        #region Properties
        #region CaseList
        private List<ModelController<Model_Case>> _cases = new();
        /// <summary>
        /// get/set list of cases available
        /// </summary>
        public List<ModelController<Model_Case>> Cases
        {
            get
            {
                return _cases;
            }
            set 
            {
                _cases = value;
                OnPropertyChanged(nameof(Cases));
            }
        }
        #endregion
        #region SelectedCase
        private ModelController<Model_Case> _selectedCase;
        /// <summary>
        /// get/set Selected case
        /// </summary>
        public ModelController<Model_Case> SelectedCase
        {
            get { return _selectedCase; }
            set
            {
                _selectedCase = value;

                // creates phaselist_view for selected case
                CreateViews();
                
                OnPropertyChanged(nameof(SelectedCase));
            }
        }

        /// <summary>
        /// Changing the phase list view happens when we select a new case object, this is
        /// we update the SelectedCase property
        /// </summary>
        private void CreateViews()
        {
            // Set all views
            _phaseList = new PhaseList_View(_comm, SelectedCase);
            _heatTreatmentView = new HeatTreatment_View(new(_comm, SelectedCase.ModelObject));
            _solidificationConfiguration = new SolidificationConfigurations(SelectedCase.ModelObject);

            _show_popup = false;
        }

        #endregion
        #endregion

        #region Methods
        public void Set_SelectedCase(Model_Case modelObject) 
        { 
            var caseS = Cases.Find(x => x.ModelObject == modelObject);
            if (caseS is null) return;
            SelectedCase = caseS;
        }
        #endregion

        #region Commands
        #region run_equilibrium

        private ICommand _run_equilibrium;
        /// <summary>
        /// Run Equilibrium solidification simulation
        /// </summary>
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
            int IDProject = SelectedCase.ModelObject.IDProject;
            int IDCase = SelectedCase.ModelObject.ID;
            
            if (ControllerM_EquilibriumConfiguration.Run_EquilibriumSimulation(_comm, IDProject, IDCase, IDCase))
            {
                
            }
            else 
            { 
            
            }

        }

        private bool Can_Run_equilibrium()
        {
            return true;
        }
        #endregion

        #region run_scheil

        private ICommand _run_scheil;

        /// <summary>
        /// Run scheil solidification simulation
        /// </summary>
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
            int IDProject = SelectedCase.ModelObject.IDProject;
            int IDCase = SelectedCase.ModelObject.ID;

            if (ControllerM_ScheilConfiguration.Run_ScheilSimulation(_comm, IDProject, IDCase, IDCase))
            {

            }
            else
            {

            }
        }

        private bool Can_Run_scheil()
        {
            return true;
        }
        #endregion

        #region Save

        private ICommand _saveCommand;
        /// <summary>
        /// Saves case object
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => this.Run_Save_Command(),
                        param => this.Can_Save_Command()
                    );
                }
                return _saveCommand;
            }
        }

        private void Run_Save_Command()
        {
            SelectedCase.SaveAction?.DoAction();
        }

        private bool Can_Save_Command()
        {
            return true;
        }
        #endregion

        #region ShowPhaseList

        private ICommand _showPhaseList;
        /// <summary>
        /// Show phase list in popup window
        /// </summary>
        public ICommand ShowPhaseList
        {
            get
            {
                if (_showPhaseList == null)
                {
                    _showPhaseList = new RelayCommand(
                        param => this.ShowPhaseList_Action(),
                        param => this.ShowPhaseList_Check()
                    );
                }
                return _showPhaseList;
            }
        }

        private void ShowPhaseList_Action()
        {
            PopupView.ContentPage.Children.Clear();
            PopupView.clear_buttons();

            if (PhaseList != null)
                PopupView.ContentPage.Children.Add((UserControl)PhaseList);
            else Controller_Global.MainControl?.Show_Notification("","",(int)FontAwesome.WPF.FontAwesomeIcon.Warning, null, null ,null);
        }

        private bool ShowPhaseList_Check()
        {
            return true;
        }
        #endregion

        #region ShowPhaseList

        private ICommand _changePhaseSelection;
        public ICommand ChangePhaseSelection
        {
            get
            {
                if (_changePhaseSelection == null)
                {
                    _changePhaseSelection = new RelayCommand(
                        param => this.ChangePhaseSelection_Action(),
                        param => this.ChangePhaseSelection_Check()
                    );
                }
                return _changePhaseSelection;
            }
        }

        private void ChangePhaseSelection_Action()
        {
            if (PhaseList == null) return;

            var selection = ((PhaseList_View)PhaseList).Get_Selection();

            

        }

        private bool ChangePhaseSelection_Check()
        {
            return true;
        }
        #endregion
        #endregion
    }
}
