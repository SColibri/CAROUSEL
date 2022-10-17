using AMFramework.Model;
using AMFramework.Model.Model_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Views.Projects.Other
{
    class Controller_ProjectCaseCreator : Controller.ControllerAbstract
    {
        public Controller_ProjectCaseCreator(ref Core.IAMCore_Comm comm, ControllerM_Project projectController):base(comm) 
        {
            _projectController = projectController;
            CaseTemplate = new(_comm);
        }


        #region Models
        private ControllerM_Project _projectController;
        public ControllerM_Project ProjectController
        { 
            get { return _projectController; } 
            set 
            {
                _projectController = value;
                OnPropertyChanged("ProjectController");
            }
        }

        
        private ControllerM_Case _caseTemplate;
        public ControllerM_Case CaseTemplate 
        { 
            get { return _caseTemplate; }
            set 
            { 
                _caseTemplate = value;
                OnPropertyChanged("CaseTemplate");
            }
        }
        #endregion

        #region Methods

        private void UpdateCaseTemplate_Elements() 
        {
            
            // Get reference to model object
            Model_Projects refProject = (Model_Projects)_projectController.Model_Object;
            Model_Case refCase = (Model_Case)_caseTemplate.Model_Object;

            // Use selected elements and fill composition elements on templated case
            foreach (var element in refProject.SelectedElements)
            {
                
            }
            
        }

        #endregion


    }
}
