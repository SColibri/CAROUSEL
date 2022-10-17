using AMFramework.Core;
using AMFramework.Model;
using AMFramework.Model.Model_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Controller
{
    /// <summary>
    /// Project view controller.
    /// 
    /// This controller is used for showing all project relevant information.
    /// </summary>
    public class Controller_Project:ControllerAbstract
    {
        public Controller_Project(IAMCore_Comm comm): base(comm) { }

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
                OnPropertyChanged("SelectedProject");
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
                OnPropertyChanged("ProjectList");
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
            System.Threading.Thread TH01 = new System.Threading.Thread(Load_project_async);
            TH01.Priority = System.Threading.ThreadPriority.Highest;
            TH01.Start(ID);
        }
        private void Load_project_async(object? ID) 
        {
            // check if object is null, because of threading, this is specified as object and not int
            if (ID == null) return;

            // Find project from list, previously loaded using Load_projectList.
            SelectedProject = ProjectList.Find(e => ((Model_Projects)e.Model_Object).ID == (int)ID);

            // Set all projects as unselected. Selection is visible in the project selection menu
            foreach (var item in ProjectList)
            {
                ((Model_Projects)item.Model_Object).IsSelected = false;
            }

            // Check if project is contained. If contained set as selected and load data.
            if (SelectedProject == null) return;
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
            System.Threading.Thread TH01 = new System.Threading.Thread(Load_projectList_async);
            TH01.Priority = System.Threading.ThreadPriority.Highest;
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
        }


        #endregion

        #endregion
    }
}
