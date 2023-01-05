using AMFramework.Components.Windows;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using AMFramework_Lib.Controller;
using AMFramework.Views.ActivePhases;
using AMFramework.Views.Elements;
using AMFramework.Views.Projects.Other;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AMFramework.Components.Button;
using System;

namespace AMFramework.Controller
{
    /// <summary>
    /// Project view controller.
    /// 
    /// This controller is used for showing all project relevant information.
    /// </summary>
    public class Controller_Project : ControllerAbstract
    {
        public Controller_Project(IAMCore_Comm comm) : base(comm) 
        {
            ElementList = ModelController<Model_Element>.LoadAll(ref _comm);
            CaseController = new(ref comm, this);
        }

        #region Properties
        private ControllerM_Project? _selectedProject = null;
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
                
                // Update Element list (TODO: we should move this logic somewhere else)
                foreach (var item in ElementList)
                {
                    var itemRef = _selectedProject.MCObject.ModelObject.SelectedElements.Find(e => e.ModelObject.IDElement == item.ModelObject.ID);
                    if (itemRef == null)
                    {
                        item.ModelObject.IsSelected = false;
                        item.ModelObject.IsReferenceElement = false;
                    }
                    else
                    {
                        item.ModelObject.IsSelected = true;
                        item.ModelObject.IsReferenceElement = itemRef.ModelObject.ISReferenceElementBool;
                    }
                }
            }
        }

        private List<ControllerM_Project> _projectList = new();
        /// <summary>
        /// List of all available projects
        /// </summary>
        public List<ControllerM_Project> ProjectList
        {
            get { return _projectList; }
            set
            {
                _projectList = value;
                OnPropertyChanged(nameof(ProjectList));
            }
        }

        private List<ModelController<Model_Phase>> _usedPhases = new();
        /// <summary>
        /// get/set list of phases used in this project
        /// </summary>
        public List<ModelController<Model_Phase>> UsedPhases
        {
            get => _usedPhases;
            set 
            {
                _usedPhases = value;
                OnPropertyChanged(nameof(UsedPhases));
            }
        }


        private List<string> _solidificationCalculationMethods = new() { "Equilibrium", "Scheil" };
        /// <summary>
        /// Solidification calculation method
        /// </summary>
        public List<string> SolidificationCalculationMethods
        {
            get { return _solidificationCalculationMethods; }
            set
            {
                _solidificationCalculationMethods = value;
                OnPropertyChanged(nameof(SolidificationCalculationMethods));
            }
        }

        private string _selectedSolidificationCalculationMethod = "";
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

        private object? _activePhasesConfigurationPage;
        public object? ActivePhasesConfigurationPage
        {
            get { return _activePhasesConfigurationPage; }
            set
            {
                _activePhasesConfigurationPage = value;
                OnPropertyChanged(nameof(ActivePhasesConfigurationPage));
            }
        }

        private List<ModelController<Model_Element>> _elementList = new();
        public List<ModelController<Model_Element>> ElementList
        {
            get { return _elementList; }
            set
            {
                _elementList = value;
                OnPropertyChanged(nameof(ElementList));
            }
        }

        #endregion

        #region Controllers
        private Controller_Cases _caseController;
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

            List<ModelController<Model_Phase>> result = new();
            foreach (var item in SelectedProject.MCObject.ModelObject.Cases)
            {
                List<ModelController<Model_SelectedPhases>> casePhases = item.ModelObject.SelectedPhases;

                foreach (var casePhaseItem in casePhases)
                {
                    ModelController<Model_Phase>? iFound = result.Find(e => e.ModelObject.Name == casePhaseItem.ModelObject.PhaseName);

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
        private void Load_project_async(object? ID)
        {
            // check if object is null, because of threading, this is specified as object and not int
            if (ID == null) { LoadingData = false; return; }

            // Find project from list, previously loaded using Load_projectList.
            SelectedProject = ProjectList.Find(e => ((Model_Projects)e.Model_Object).ID == (int)ID);

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

            // Add cases to case controller
            CaseController.Cases = SelectedProject.MCObject.ModelObject.Cases;

            Application.Current.Dispatcher.BeginInvoke(() => 
            { 
                _controller_xDataTreeView.Refresh_DTV(this.SelectedProject);

                Controller_ActivePhasesConfiguration refController = new(ref _comm, SelectedProject.MCObject.ModelObject.ActivePhasesConfiguration);
                ActivePhasesView_Configuration winRef = new() { DataContext = refController };
                ActivePhasesConfigurationPage = winRef;
            });

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

        private void Load_projectList_async()
        {
            var rawTable = ModelController<Model_Projects>.LoadAll(ref _comm);

            List<ControllerM_Project> pList = new();
            foreach (var item in rawTable)
            {
                pList.Add(new(_comm, item));
            }

            ProjectList = pList;

            LoadingData = false;
        }


        #endregion

        #endregion

        #region GUI_Objects

        #region Treeview
        private Controller_XDataTreeView _controller_xDataTreeView = new Controller_XDataTreeView();
        public Controller_XDataTreeView Controller_xDataTreeView => _controller_xDataTreeView;
        #endregion

        #endregion

        #region Commands

        //-----------------------------------
        // Show Views
        //-----------------------------------
        #region open_case_creator

        private ICommand _openCaseCreator;
        /// <summary>
        /// Opens the case creator view
        /// </summary>
        public ICommand OpenCaseCreator => _openCaseCreator ??= new RelayCommand(param => Open_CaseCreator(), param => Can_OpenCaseCreator());

        private void Open_CaseCreator()
        {
            // TODO: notify the user, he will be confused if nothing happens
            if (SelectedProject == null) return;

            AM_popupWindow pWindow = new();
            Controller_ProjectCaseCreator cCreator = new(ref _comm, SelectedProject);
            pWindow.ContentPage.Children.Add(new Views.Projects.Other.Project_case_creator() { DataContext = cCreator });

            Controller_Global.MainControl?.Show_Popup(pWindow);
        }

        private bool Can_OpenCaseCreator()
        {
            return true;
        }
        #endregion
        #region open_project_list

        private ICommand _openProjectListCommand;
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
        #region open_project_list

        private ICommand _openNewProjectCommand;
        /// <summary>
        /// Shows the project list selector
        /// </summary>
        public ICommand OpenNewProjectCommand => _openNewProjectCommand ??= new RelayCommand(param => OpenNewProjectCommand_Command(), param => OpenNewProjectCommand_Action());

        private void OpenNewProjectCommand_Command()
        {
            // set new project object as selected
            SelectedProject = new(_comm);

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

        private ICommand _showElementList;
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

        private void ShowElementList_Action()
        {
            if (SelectedProject == null) return;

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

        private bool ShowElementList_Check()
        {
            return true;
        }
        #endregion
        #region Accept_elementSelection

        private ICommand _acceptElementList;
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

        private void AcceptElementList_Action()
        {
            if (SelectedProject == null) return;

            // remove ALL dependent data from project level
            ControllerM_Project.Clear_SimulationData(_comm, SelectedProject.MCObject.ModelObject.ID);
            SelectedProject.MCObject.ModelObject.SelectedElements.Clear();

            // TODO: remove project previous simulation data

            // create new selected items list
            var selectedElements = ElementList.FindAll(e => e.ModelObject.IsSelected == true);
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
        }

        private bool AcceptElementList_Check()
        {
            if (MessageBox.Show("This action will remove all simulation data, do you want to proceed?", "Project reset", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                return true;
            else return false;
        }
        #endregion

        //-----------------------------------
        // Actions
        //-----------------------------------
        #region Save

        private ICommand _saveProject;
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

        private void SaveProject_Action()
        {
            // TODO: notify the user, he will be confused if nothing happens
            if (SelectedProject == null) return;

            // Save project data
            SelectedProject.MCObject.SaveAction?.DoAction();

            // Save Active phases. We do not add the activephases since this is a calculated value
            SelectedProject.MCObject.ModelObject?.ActivePhasesConfiguration?.SaveAction?.DoAction();
            foreach (var item in SelectedProject.MCObject.ModelObject.ActivePhasesElementComposition)
            {
                item.SaveAction?.DoAction();
            }
        }

        private bool SaveProject_Check()
        {
            if (MessageBox.Show("Saving project, proceed?", "Save", MessageBoxButton.YesNo) == MessageBoxResult.Yes) return true;
            return false;
        }
        #endregion
        #region Find_active_phases

        private ICommand _findActivePhases;
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

        private void FindActivePhases_Action()
        {
            if (SelectedProject == null) return;

            // Save all compositions
            SelectedProject.MCObject.ModelObject.ActivePhasesElementComposition.ForEach(e => e.SaveAction?.DoAction());
            SelectedProject.MCObject.ModelObject.ActivePhasesConfiguration?.SaveAction?.DoAction();

            var refAP = SelectedProject.MCObject.ModelObject.ActivePhasesConfiguration;
            Controller_ActivePhasesConfiguration conAP = new(ref _comm, refAP);

            if (conAP.Find_Active_Phases.CanExecute(null)) conAP.Find_Active_Phases.Execute(null);
        }

        private bool FindActivePhases_Check()
        {
            return true;
        }
        #endregion
        #region Select_project

        private ICommand _selectProject;
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

        private bool SelectProject_Check()
        {
            return true;
        }
        #endregion
        #endregion
    }
}
