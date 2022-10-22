using AMFramework.Components.Windows;
using AMFramework.Core;
using AMFramework.Model;
using AMFramework.Model.Model_Controllers;
using AMFramework.Views.ActivePhases;
using AMFramework.Views.Elements;
using AMFramework.Views.Projects.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Controller_Project(IAMCore_Comm comm) : base(comm) 
        {
            ElementList = ModelController<Model_Element>.LoadAll(ref _comm);
        }

        #region Parameters
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

                if (_selectedProject == null) return; 
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

        #region Methods
        /// <summary>
        /// Create a new project model controller and sets it up as the selected object
        /// </summary>
        public void Create_new_project()
        {
            SelectedProject = new(_comm);
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

        #region Commands

        #region open_case_creator

        private ICommand _openCaseCreator;
        public ICommand OpenCaseCreator
        {
            get
            {
                if (_openCaseCreator == null)
                {
                    _openCaseCreator = new RelayCommand(
                        param => this.Open_CaseCreator(),
                        param => this.Can_OpenCaseCreator()
                    );
                }
                return _openCaseCreator;
            }
        }

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

        #region Save

        private ICommand _saveProject;
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
            SelectedProject.MCObject.SaveAction.DoAction();

            // Save Active phases. We do not add the activephases since this is a calculated value
            SelectedProject.MCObject.ModelObject.ActivePhasesConfiguration.SaveAction.DoAction();
            foreach (var item in SelectedProject.MCObject.ModelObject.ActivePhasesElementComposition)
            {
                item.SaveAction.DoAction();
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
            // TODO: notify the user, he will be confused if nothing happens
            if (SelectedProject == null) return;

            var refAP = SelectedProject.MCObject.ModelObject.ActivePhasesConfiguration;
            Controller_ActivePhasesConfiguration conAP = new(ref _comm, refAP);

            if (conAP.Find_Active_Phases.CanExecute(null)) conAP.Find_Active_Phases.Execute(null);
        }

        private bool FindActivePhases_Check()
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
            // TODO: notify the user, he will be confused if nothing happens
            if (SelectedProject == null) return;

            ElementView_List eList = new() { DataContext = this };
            AM_popupWindow popWin = new();
            popWin.ContentPage.Children.Add(eList);

            Controller_Global.MainControl?.Show_Popup(popWin);
        }

        private bool ShowElementList_Check()
        {
            if (MessageBox.Show("This action will remove all simulation data, do you want to proceed?", "Project reset", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                return true;
            else return false;
        }
        #endregion
        #endregion
    }
}
