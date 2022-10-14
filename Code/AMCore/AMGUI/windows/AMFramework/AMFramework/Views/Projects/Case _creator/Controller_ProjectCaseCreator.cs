using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Views.Projects.Other
{
    class Controller_ProjectCaseCreator : Controller.ControllerAbstract
    {

        public Controller_ProjectCaseCreator(Model.Model_Projects project) 
        {
            _projects = project;    
        }


        #region Models
        private Model.Model_Projects _projects;
        public Model.Model_Projects Projects 
        { 
            get { return _projects; } 
            set 
            { 
                _projects = value;
                OnPropertyChanged("Projects");
            }
        }

        
        private Model.Model_Case _caseTemplate = new();
        public Model.Model_Case CaseTemplate 
        { 
            get { return _caseTemplate; }
            set 
            { 
                _caseTemplate = value;
                OnPropertyChanged("CaseTemplate");
            }
        }
        #endregion


    }
}
