using AMFramework.Components.Button;
using AMFramework.Components.Windows;
using AMFramework.Views.ActivePhases;
using AMFramework.Views.Elements;
using AMFramework.Views.Projects.Other;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using Catel.Collections;
using Catel.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AMFramework.Controller
{
    /// <summary>
    /// Project view controller.
    /// 
    /// This controller is used for showing all project relevant information.
    /// </summary>
    public class Controller_Project : ControllerAbstract
    {

        #region Fields

        /// <summary>
        /// Solidification calculation methods available
        /// </summary>
        private ObservableCollection<string> _solidificationCalculationMethods = new() { "Equilibrium", "Scheil" };

        /// <summary>
        /// Selected Solidification calculation method
        /// </summary>
        private string _selectedSolidificationCalculationMethod = string.Empty;

        /// <summary>
        /// Active phases view
        /// </summary>
        private object? _activePhasesConfigurationPage;

        // ----------------------------------------------------------------------------------
        //                                    CONTROLLERS
        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Selected project item
        /// </summary>
        private ControllerM_Project? _selectedProject = null;

        /// <summary>
        /// List of available projects
        /// </summary>
        private ObservableCollection<ControllerM_Project> _projectList = new();

        /// <summary>
        /// Case controller, contains all cases related to this project
        /// </summary>
        private Controller_Cases _caseController;

        /// <summary>
        /// Treeview controller
        /// </summary>
        private Controller_XDataTreeView _controller_xDataTreeView = new Controller_XDataTreeView();

        /// <summary>
        /// List of all elements
        /// </summary>
        private ObservableCollection<ModelController<Model_Element>> _elementList = new();

        /// <summary>
        /// List of available phases
        /// </summary>
        private ObservableCollection<ModelController<Model_Phase>> _usedPhases = new();

        // ----------------------------------------------------------------------------------
        //                                     COMMANDS
        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Case creator command
        /// </summary>
        private ICommand _openCaseCreator;

        /// <summary>
        /// Opens a list of available projects
        /// </summary>
        private ICommand _openProjectListCommand;

        /// <summary>
        /// Opens a new project
        /// </summary>
        private ICommand _openNewProjectCommand;

        /// <summary>
        /// Shows available elements from selected database
        /// </summary>
        private ICommand _showElementList;

        /// <summary>
        /// Accepts current element selection
        /// </summary>
        private ICommand _acceptElementList;

        /// <summary>
        /// Saves project
        /// </summary>
        private ICommand _saveProject;

        /// <summary>
        /// Searches for the active phases using current configuration
        /// </summary>
        private ICommand _findActivePhases;

        /// <summary>
        /// Selects a project
        /// </summary>
        private ICommand _selectProject;

        /// <summary>
        /// Set thermodynamic database
        /// </summary>
        private ICommand _selectThermodynamicDatabase;

        /// <summary>
        /// Set thermodynamic database
        /// </summary>
        private ICommand _selectMobilityDatabase;

        /// <summary>
        /// Set thermodynamic database
        /// </summary>
        private ICommand _selectPhysicalDatabase;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comm"></param>
        public Controller_Project(IAMCore_Comm comm) : base(comm)
        {
            ElementList.AddRange(ModelController<Model_Element>.LoadAll(ref _comm));
            CaseController = new(ref comm, this);
        }

        #endregion

        #region Properties


        /// <summary>
        /// Solidification calculation method
        /// </summary>
        public ObservableCollection<string> SolidificationCalculationMethods
        {
            get { return _solidificationCalculationMethods; }
            set
            {
                _solidificationCalculationMethods = value;
                OnPropertyChanged(nameof(SolidificationCalculationMethods));
            }
        }

        /// <summary>
        /// Set/Get method used for solidification calculations
        /// </summary>
        public string SelectedSolidificationCalculationMethod
        {
            get { return _selectedSolidificationCalculationMethod; }
            set
            {
                _selectedSolidificationCalculationMethod = value;
                OnPropertyChanged(nameof(SelectedSolidificationCalculationMethod));

                if (value.CompareTo(_solidificationCalculationMethods[0]) == 0 && SelectedProject != null)
                {
                    Controller_ActivePhasesConfiguration refController = new(ref _comm, SelectedProject.MCObject.ModelObject.ActivePhasesConfiguration);
                    ActivePhasesView_Configuration winRef = new() { DataContext = refController };
                    ActivePhasesConfigurationPage = winRef;
                }
                else ActivePhasesConfigurationPage = null;
            }
        }


        // ----------------------------------------------------------------------------------
        //                                      VIEWS
        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Get/Set Active phases configuration page
        /// </summary>
        public object? ActivePhasesConfigurationPage
        {
            get { return _activePhasesConfigurationPage; }
            set
            {
                _activePhasesConfigurationPage = value;
                OnPropertyChanged(nameof(ActivePhasesConfigurationPage));
            }
        }

        // ----------------------------------------------------------------------------------
        //                                    CONTROLLERS
        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Selected project :)
        /// </summary>
        public ControllerM_Project? SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));

                // Check for null value
                if (_selectedProject == null) return;

                // Get list of all used phases
                Get_UsedPhases();

                // Set model tag as selected and if element is a reference element
                SetSelectedElements();

            }
        }

        /// <summary>
        /// List of all available projects
        /// </summary>
        public ObservableCollection<ControllerM_Project> ProjectList
        {
            get { return _projectList; }
            set
            {
                _projectList = value;
                OnPropertyChanged(nameof(ProjectList));
            }
        }


        /// <summary>
        /// get/set list of phases used in this project
        /// </summary>
        public ObservableCollection<ModelController<Model_Phase>> UsedPhases
        {
            get => _usedPhases;
            set
            {
                _usedPhases = value;
                OnPropertyChanged(nameof(UsedPhases));
            }
        }

        /// <summary>
        /// Get/Set List of elements available in database
        /// </summary>
        public ObservableCollection<ModelController<Model_Element>> ElementList
        {
            get { return _elementList; }
            set
            {
                _elementList = value;
                OnPropertyChanged(nameof(ElementList));
            }
        }

        /// <summary>
        /// Get/set Treeview Controller
        /// </summary>
        public Controller_XDataTreeView Controller_xDataTreeView => _controller_xDataTreeView;

        /// <summary>
        /// Case controller, contains all cases related to this project
        /// </summary>
        public Controller_Cases CaseController
        {
            get { return _caseController; }
            set
            {
                _caseController = value;
                OnPropertyChanged(nameof(_caseController));
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Create a new project model controller and sets it up as the selected object
        /// </summary>
        public void Create_new_project()
        {
            SelectedProject = new(_comm);
        }

        /// <summary>
        /// Fills used phases list with all phases used in all cases
        /// </summary>
        private void Get_UsedPhases()
        {
            if (SelectedProject == null) return;

            ObservableCollection<ModelController<Model_Phase>> result = new();
            foreach (var item in SelectedProject.MCObject.ModelObject.Cases)
            {
                List<ModelController<Model_SelectedPhases>> casePhases = item.ModelObject.SelectedPhases;

                foreach (var casePhaseItem in casePhases)
                {
                    ModelController<Model_Phase>? iFound = result.ToList().Find(e => e.ModelObject.Name == casePhaseItem.ModelObject.PhaseName);

                    if (iFound == null)
                    {
                        result.Add(new(ref _comm));
                        result.Last().ModelObject.ID = casePhaseItem.ModelObject.IDPhase;
                        result.Last().LoadByIDAction?.DoAction();
                    }
                }
            }

            UsedPhases = result;
        }

        /// <summary>
        /// Updates the element list used for calculating the active phases
        /// </summary>
        private void UpdateElementsForActivePhases()
        {
            List<ModelController<Model_ActivePhasesElementComposition>> tempList = new();
            foreach (var element in SelectedProject.MCObject.ModelObject.SelectedElements)
            {
                ModelController<Model_ActivePhasesElementComposition> selModel = new(ref _comm);
                selModel.ModelObject.IDProject = SelectedProject.MCObject.ModelObject.ID;
                selModel.ModelObject.IDElement = element.ModelObject.ID;
                selModel.ModelObject.ElementName = element.ModelObject.ElementName;
                selModel.ModelObject.IsReferenceElement = element.ModelObject.ISReferenceElementBool;
                selModel.ModelObject.Value = 0;
                selModel.SaveAction?.DoAction();

                tempList.Add(selModel);
            }

            SelectedProject.MCObject.ModelObject.ActivePhasesElementComposition = tempList;
        }

        /// <summary>
        /// Sets flags in the element list based on the element list from the selected project
        /// </summary>
        private void SetSelectedElements()
        {
            // Check for null value
            if (_selectedProject == null) return;

            // Update Element list (TODO: we should move this logic somewhere else)
            ElementList.ToList().ForEach(e =>
            {
                if (_selectedProject.MCObject.ModelObject.SelectedElements.Find(s => s.ModelObject.IDElement == e.ModelObject.ID) is ModelController<Model_SelectedElements> itemRef)
                {
                    e.ModelObject.IsSelected = true;
                    e.ModelObject.IsReferenceElement = itemRef.ModelObject.ISReferenceElementBool;
                }
                else
                {
                    e.ModelObject.IsSelected = false;
                    e.ModelObject.IsReferenceElement = false;
                }
            });
        }

        #region Load
        /// <summary>
        /// Sets as selected the project with id of ID, and it loads all necessary data for first load. This invokes a async method
        /// and uses the flag LoadingData to specify when the function has finished.
        /// </summary>
        /// <param name="ID"></param>
        public void Load_project(int ID)
        {
            // If project is already loading, stop
            if (LoadingData) return;

            // set loading flag and use threading for loading data.
            LoadingData = true;
            System.Threading.Thread TH01 = new(Load_project_async)
            {
                Priority = System.Threading.ThreadPriority.Highest
            };
            TH01.Start(ID);

        }

        /// <summary>
        /// Load project async
        /// </summary>
        /// <param name="ID"></param>
        private void Load_project_async(object? ID)
        {
            // check if object is null, because of threading, this is specified as object and not int
            if (ID == null) { LoadingData = false; return; }

            // Find project from list, previously loaded using Load_projectList.
            SelectedProject = ProjectList.ToList().Find(e => ((Model_Projects)e.Model_Object).ID == (int)ID);

            // Set all projects as unselected. Selection is visible in the project selection menu
            foreach (var item in ProjectList)
            {
                ((Model_Projects)item.Model_Object).IsSelected = false;
            }

            // Check if project is contained. If contained set as selected and load data.
            if (SelectedProject == null) { LoadingData = false; return; }
            ((Model_Projects)SelectedProject.Model_Object).IsSelected = true;
            SelectedProject.Load_SelectedElements();
            SelectedProject.Load_ActivePhases();
            SelectedProject.Load_cases();
            SelectedProject.Load_Databases();

            // Add cases to case controller
            CaseController.Cases = SelectedProject.MCObject.ModelObject.Cases;

            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                _controller_xDataTreeView.Refresh_DTV(this.SelectedProject);

                Controller_ActivePhasesConfiguration refController = new(ref _comm, SelectedProject.MCObject.ModelObject.ActivePhasesConfiguration);
                ActivePhasesView_Configuration winRef = new() { DataContext = refController };
                ActivePhasesConfigurationPage = winRef;
            });

            // TODO: change where the core loads the database paths, the old version loaded it form the configuration file, but because of a last
            // minute change this is patched here, so the best approach is to modify this so that each project gets to choose the database it want to
            // use
            Controller_Global.Configuration.Thermodynamic_database_path = SelectedProject.MCObject.ModelObject.Databases.ModelObject.ThermodynamicDatabase;
            Controller_Global.Configuration.Physical_database_path = SelectedProject.MCObject.ModelObject.Databases.ModelObject.PhysicalDatabase;
            Controller_Global.Configuration.Mobility_database_path = SelectedProject.MCObject.ModelObject.Databases.ModelObject.MobilityDatabase;
            Controller_Global.ApiHandle.run_lua_command("configuration_set_thermodynamic_database_path " + Controller_Global.Configuration.Thermodynamic_database_path, "");
            Controller_Global.ApiHandle.run_lua_command("configuration_set_physical_database_path " + Controller_Global.Configuration.Physical_database_path, "");
            Controller_Global.ApiHandle.run_lua_command("configuration_set_mobility_database_path " + Controller_Global.Configuration.Mobility_database_path, "");

            // set loading data to false for visual
            LoadingData = false;
        }

        /// <summary>
        /// Loads all projects in database. Note to self: If the project list becomes too big, we should not load all.
        /// </summary>
        public void Load_projectList()
        {
            if (LoadingData) return;

            LoadingData = true;
            System.Threading.Thread TH01 = new(Load_projectList_async)
            {
                Priority = System.Threading.ThreadPriority.Highest
            };
            TH01.Start();
        }

        /// <summary>
        /// Load list of available projects async
        /// </summary>
        private void Load_projectList_async()
        {
            var rawTable = ModelController<Model_Projects>.LoadAll(ref _comm);

            ObservableCollection<ControllerM_Project> pList = new();
            foreach (var item in rawTable)
            {
                pList.Add(new(_comm, item));
            }

            ProjectList = pList;

            LoadingData = false;
        }


        #endregion

        #endregion

        #region Commands
        #region open_case_creator


        /// <summary>
        /// Opens the case creator view
        /// </summary>
        public ICommand OpenCaseCreator => _openCaseCreator ??= new RelayCommand(param => Open_CaseCreator(), param => Can_OpenCaseCreator());

        /// <summary>
        /// Open case creator handle
        /// </summary>
        private void Open_CaseCreator()
        {
            // Check if project has been selected
            if (SelectedProject == null)
            {
                Controller_Global.MainControl?.Show_Notification("Project", "Please select a project first", (int)FontAwesome.WPF.FontAwesomeIcon.Warning, null, null, null);
                return;
            }

            // Show new popup window
            AM_popupWindow pWindow = new();
            Controller_ProjectCaseCreator cCreator = new(ref _comm, SelectedProject);
            pWindow.ContentPage.Children.Add(new Views.Projects.Other.Project_case_creator() { DataContext = cCreator });

            // Show popup on main window
            Controller_Global.MainControl?.Show_Popup(pWindow);
        }

        private bool Can_OpenCaseCreator()
        {
            return true;
        }
        #endregion
        #region open_project_list
        /// <summary>
        /// Shows the project list selector
        /// </summary>
        public ICommand OpenProjectListCommand => _openProjectListCommand ??= new RelayCommand(param => OpenProjectListCommand_Command(), param => OpenProjectListCommand_Action());

        private void OpenProjectListCommand_Command()
        {
            Views.Projects.Project_list Pg = new(this);
            AM_popupWindow Pw = new() { Title = "Open" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = FontAwesome.WPF.FontAwesomeIcon.Upload.ToString(),
                Margin = new Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.Command = SelectProject;
            nbutt.ClickButton += Pw.AM_Close_Window_Event;

            Pw.add_button(nbutt);
            Controller_Global.MainControl?.Show_Popup(Pw);
        }

        private bool OpenProjectListCommand_Action()
        {
            return true;
        }
        #endregion
        #region New project

        /// <summary>
        /// Shows the project list selector
        /// </summary>
        public ICommand OpenNewProjectCommand => _openNewProjectCommand ??= new RelayCommand(param => OpenNewProjectCommand_Command(), param => OpenNewProjectCommand_Action());

        private void OpenNewProjectCommand_Command()
        {
            // set new project object as selected
            SelectedProject = new(_comm);
            SelectedProject.MCObject.ModelObject.APIName = Path.GetFileName(Controller_Global.Configuration?.API_path);

            // Create new view
            Views.Projects.Project_general Pg = new(this);
            AM_popupWindow Pw = new() { Title = "New project" };
            Pw.ContentPage.Children.Add(Pg);

            // Add window buttons
            Components.Button.AM_button nbutt = new()
            {
                IconName = "Save",
                Margin = new Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };

            // Add button handles
            nbutt.Command = SaveProject;
            nbutt.ClickButton += Pw.AM_Close_Window_Event;

            Pw.add_button(nbutt);
            Controller_Global.MainControl?.Show_Popup(Pw);
        }

        private bool OpenNewProjectCommand_Action()
        {
            return true;
        }
        #endregion
        #region Show_element_selection
        /// <summary>
        /// Shows available elements from selected database
        /// </summary>
        public ICommand ShowElementList
        {
            get
            {
                if (_showElementList == null)
                {
                    _showElementList = new RelayCommand(
                        param => this.ShowElementList_Action(),
                        param => this.ShowElementList_Check()
                    );
                }
                return _showElementList;
            }
        }

        /// <summary>
        /// Show element list handle
        /// </summary>
        private void ShowElementList_Action()
        {
            // Check if project is valid
            if (SelectedProject == null) return;

            // Set current selected elements
            SetSelectedElements();

            ElementView_List eList = new() { DataContext = this };
            AM_popupWindow popWin = new();
            popWin.ContentPage.Children.Add(eList);

            // Save button
            AM_button SaveButton = new();
            SaveButton.IconName = FontAwesome.WPF.FontAwesomeIcon.Save.ToString();
            SaveButton.Command = AcceptElementList;

            popWin.add_button(SaveButton);

            Controller_Global.MainControl?.Show_Popup(popWin);
        }

        /// <summary>
        /// Checks if showing element list is allowed
        /// </summary>
        /// <returns></returns>
        private bool ShowElementList_Check()
        {
            return true;
        }
        #endregion
        #region Accept_elementSelection
        /// <summary>
        /// Accepts current element selection
        /// </summary>
        public ICommand AcceptElementList
        {
            get
            {
                if (_acceptElementList == null)
                {
                    _acceptElementList = new RelayCommand(
                        param => this.AcceptElementList_Action(),
                        param => this.AcceptElementList_Check()
                    );
                }
                return _acceptElementList;
            }
        }

        /// <summary>
        /// Accept selection handle
        /// </summary>
        private void AcceptElementList_Action()
        {
            if (SelectedProject == null) return;

            // remove ALL dependent data from project level
            ControllerM_Project.Clear_SimulationData(_comm, SelectedProject.MCObject.ModelObject.ID);
            SelectedProject.MCObject.ModelObject.SelectedElements.Clear();

            // create new selected items list
            var selectedElements = ElementList.ToList().FindAll(e => e.ModelObject.IsSelected == true);
            List<ModelController<Model_SelectedElements>> tempList = new();
            foreach (var element in selectedElements)
            {
                ModelController<Model_SelectedElements> selModel = new(ref _comm);
                selModel.ModelObject.IDProject = SelectedProject.MCObject.ModelObject.ID;
                selModel.ModelObject.IDElement = element.ModelObject.ID;
                selModel.ModelObject.ElementName = element.ModelObject.Name;
                selModel.ModelObject.ISReferenceElementBool = element.ModelObject.IsReferenceElement;
                selModel.ModelObject.ISReferenceElement = Convert.ToInt16(element.ModelObject.IsReferenceElement);
                selModel.SaveAction?.DoAction();

                tempList.Add(selModel);
            }

            SelectedProject.MCObject.ModelObject.SelectedElements = tempList;
            UpdateElementsForActivePhases();

        }

        /// <summary>
        /// Checks if current selection is allowed
        /// </summary>
        /// <returns></returns>
        private bool AcceptElementList_Check()
        {
            if (MessageBox.Show("This action will remove all simulation data, do you want to proceed?", "Project reset", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                return true;
            else return false;
        }
        #endregion
        #region Save

        /// <summary>
        /// Saves the current selected project
        /// </summary>
        public ICommand SaveProject
        {
            get
            {
                if (_saveProject == null)
                {
                    _saveProject = new RelayCommand(
                        param => this.SaveProject_Action(),
                        param => this.SaveProject_Check()
                    );
                }
                return _saveProject;
            }
        }

        /// <summary>
        /// Save project handle
        /// </summary>
        private void SaveProject_Action()
        {
            // Check if valid project is selected
            if (SelectedProject == null)
            {
                Controller_Global.MainControl?.Show_Notification("Project", "Please select a project first", (int)FontAwesome.WPF.FontAwesomeIcon.Warning, null, null, null);
                return;
            }

            // Save project data
            SelectedProject.MCObject.SaveAction?.DoAction();

			// Save Active phases. We do not add the activephases since this is a calculated value
			SelectedProject.MCObject.ModelObject?.ActivePhasesConfiguration?.SaveAction?.DoAction();
            foreach (var item in SelectedProject.MCObject.ModelObject.ActivePhasesElementComposition)
            {
                item.SaveAction?.DoAction();
            }

            // Save database paths
            if (SelectedProject.MCObject.ModelObject.Databases != null)
            {
                SelectedProject.MCObject.ModelObject.Databases.ModelObject.IDProject = SelectedProject.MCObject.ModelObject.ID;
                SelectedProject.MCObject.ModelObject.Databases.SaveAction?.DoAction();
            }
        }

        /// <summary>
        /// Checks if save is allowed
        /// </summary>
        /// <returns></returns>
        private bool SaveProject_Check()
        {
            if (MessageBox.Show("Saving project, proceed?", "Save", MessageBoxButton.YesNo) == MessageBoxResult.Yes) return true;
            return false;
        }
        #endregion
        #region Find_active_phases

        /// <summary>
        /// Searches for the active phases using current configuration
        /// </summary>
        public ICommand FindActivePhases
        {
            get
            {
                if (_findActivePhases == null)
                {
                    _findActivePhases = new RelayCommand(
                        param => this.FindActivePhases_Action(),
                        param => this.FindActivePhases_Check()
                    );
                }
                return _findActivePhases;
            }
        }

        /// <summary>
        /// Search active phases handle
        /// </summary>
        private void FindActivePhases_Action()
        {
            if (SelectedProject == null) return;

            SaveProject_Action();
			// Save project before running
			SelectedProject.MCObject.SaveAction?.DoAction();

            // Save database
            SelectedProject.MCObject.ModelObject.Databases.SaveAction?.DoAction();

			// Save all compositions
			SelectedProject.MCObject.ModelObject.ActivePhasesElementComposition.ForEach(e => e.SaveAction?.DoAction());
            SelectedProject.MCObject.ModelObject.ActivePhasesConfiguration?.SaveAction?.DoAction();

            var refAP = SelectedProject.MCObject.ModelObject.ActivePhasesConfiguration;
            Controller_ActivePhasesConfiguration conAP = new(ref _comm, refAP);

            if (conAP.Find_Active_Phases.CanExecute(null)) conAP.Find_Active_Phases.Execute(null);
            SelectedProject.Load_ActivePhases();
            SelectedProject.Load_Databases();
        }

        /// <summary>
        /// Checks if searching for active phases is allowed
        /// </summary>
        /// <returns></returns>
        private bool FindActivePhases_Check()
        {
            return true;
        }
        #endregion
        #region Select_project

        /// <summary>
        /// Selects a project
        /// </summary>
        public ICommand SelectProject
        {
            get
            {
                if (_selectProject == null)
                {
                    _selectProject = new RelayCommand(
                        param => this.SelectProject_Action(),
                        param => this.SelectProject_Check()
                    );
                }
                return _selectProject;
            }
        }

        /// <summary>
        /// Select project handle
        /// </summary>
        private void SelectProject_Action()
        {
            for (int n1 = 0; n1 < _projectList.Count(); n1++)
            {
                if (_projectList[n1].MCObject.ModelObject.IsSelected)
                {
                    SelectedProject = _projectList[n1];
                    Load_project(SelectedProject.MCObject.ModelObject.ID);
                    break;
                }
            }
        }

        /// <summary>
        /// Checks if selecting a project is allowed
        /// </summary>
        private bool SelectProject_Check()
        {
            return true;
        }
        #endregion
        #region select thermodynamic database
        /// <summary>
        /// Opens the case creator view
        /// </summary>
        public ICommand SelectThermodynamicDatabase => _selectThermodynamicDatabase ??= new RelayCommand(param => SelectThermodynamicDatabaseAction(), param => Can_SelectThermodynamicDatabase());

        /// <summary>
        /// Open case creator handle
        /// </summary>
        private void SelectThermodynamicDatabaseAction()
        {
            // Check if project has been selected
            if (SelectedProject == null)
            {
                Controller_Global.MainControl?.Show_Notification("Project", "Please select a project first", (int)FontAwesome.WPF.FontAwesomeIcon.Warning, null, null, null);
                return;
            }

            OpenFileDialog OFD = new()
            {
                Multiselect = false,
                Filter = "thermodynamic database|*.tdb",
                Title = "Select a thermodynamic database",
                CheckFileExists = true,
                InitialDirectory = SelectedProject.MCObject.ModelObject.Databases?.ModelObject.ThermodynamicDatabase
            };

            if (OFD.ShowDialog() == true)
            {
                SelectedProject.MCObject.ModelObject.Databases.ModelObject.ThermodynamicDatabase = OFD.FileName;
                SelectedProject.MCObject?.SaveAction?.DoAction();
                ControllerM_Project.Clear_SimulationData(_comm, SelectedProject.MCObject.ModelObject.ID);

                Controller_Global.Configuration.Thermodynamic_database_path = OFD.FileName;
                Controller_Global.ApiHandle.run_lua_command("configuration_set_thermodynamic_database_path " + Controller_Global.Configuration.Thermodynamic_database_path, "");
                // Make sure core is initialized
                string puty = _comm.run_lua_command("initialize_core", "");

                ElementList.Clear();
                ElementList.AddRange(ModelController<Model_Element>.LoadAll(ref _comm));
            }

        }

        private bool Can_SelectThermodynamicDatabase()
        {
            return true;
        }
        #endregion
        #region SelectMobilityDatabase
        /// <summary>
        /// Opens the case creator view
        /// </summary>
        public ICommand SelectMobilityDatabase => _selectMobilityDatabase ??= new RelayCommand(param => SelectMobilityDatabaseAction(), param => Can_SelectMobilityDatabase());

        /// <summary>
        /// Open case creator handle
        /// </summary>
        private void SelectMobilityDatabaseAction()
        {
            // Check if project has been selected
            if (SelectedProject == null)
            {
                Controller_Global.MainControl?.Show_Notification("Project", "Please select a project first", (int)FontAwesome.WPF.FontAwesomeIcon.Warning, null, null, null);
                return;
            }

            OpenFileDialog OFD = new()
            {
                Multiselect = false,
                Filter = "mobility database|*.ddb",
                Title = "Select a mobility database",
                CheckFileExists = true,
                InitialDirectory = SelectedProject.MCObject.ModelObject.Databases.ModelObject.MobilityDatabase
            };

            if (OFD.ShowDialog() == true)
            {
                SelectedProject.MCObject.ModelObject.Databases.ModelObject.MobilityDatabase = OFD.FileName;
                SelectedProject.MCObject?.SaveAction?.DoAction();
                ControllerM_Project.Clear_SimulationData(_comm, SelectedProject.MCObject.ModelObject.ID);
            }

        }

        private bool Can_SelectMobilityDatabase()
        {
            return true;
        }
        #endregion
        #region SelectMobilityDatabase
        /// <summary>
        /// Opens the case creator view
        /// </summary>
        public ICommand SelectPhysicalDatabase => _selectPhysicalDatabase ??= new RelayCommand(param => SelectPhysicalDatabaseAction(), param => Can_SelectPhysicalDatabase());

        /// <summary>
        /// Open case creator handle
        /// </summary>
        private void SelectPhysicalDatabaseAction()
        {
            // Check if project has been selected
            if (SelectedProject == null)
            {
                Controller_Global.MainControl?.Show_Notification("Project", "Please select a project first", (int)FontAwesome.WPF.FontAwesomeIcon.Warning, null, null, null);
                return;
            }

            OpenFileDialog OFD = new()
            {
                Multiselect = false,
                Filter = "physical database|*.pdb",
                Title = "Select a physical database",
                CheckFileExists = true,
                InitialDirectory = SelectedProject.MCObject.ModelObject.Databases.ModelObject.PhysicalDatabase
            };

            if (OFD.ShowDialog() == true)
            {
                SelectedProject.MCObject.ModelObject.Databases.ModelObject.PhysicalDatabase = OFD.FileName;
                SelectedProject.MCObject?.SaveAction?.DoAction();
                ControllerM_Project.Clear_SimulationData(_comm, SelectedProject.MCObject.ModelObject.ID);
            }
        }

        private bool Can_SelectPhysicalDatabase()
        {
            return true;
        }
        #endregion
        #endregion
    }
}
