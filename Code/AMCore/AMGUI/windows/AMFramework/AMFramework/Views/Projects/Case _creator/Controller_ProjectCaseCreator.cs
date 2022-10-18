using AMFramework.Model;
using AMFramework.Model.Model_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Views.Projects.Other
{
    public class Controller_ProjectCaseCreator : Controller.ControllerAbstract
    {
        public Controller_ProjectCaseCreator(ref Core.IAMCore_Comm comm, ControllerM_Project projectController):base(comm) 
        {
            _projectController = projectController;
            CaseTemplate = new(_comm);
            UpdateCaseTemplate_Elements();

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

            var refElement = _projectController.MCObject.ModelObject.SelectedElements.Find(e => e.ModelObject.ISReferenceElementBool);
            var eObj = ModelController<Model_Element>.LoadAll(ref _comm);
            List<ModelController<Model_ElementComposition >> compTable = new();
            foreach (var item in _projectController.MCObject.ModelObject.SelectedElements)
            {
                var refEObj = eObj.Find(e => e.ModelObject.ID == item.ModelObject.IDElement);
                if (refEObj == null) continue;

                Model_ElementComposition refComp = new()
                {
                    IDElement = refEObj.ModelObject.ID,
                    ElementName = refEObj.ModelObject.Name
                };

                compTable.Add(new(ref _comm, refComp));

                if (refElement == null) continue;
                if (refEObj.ModelObject.ID == refElement.ModelObject.IDElement) refComp.IsReferenceElement = true;
            }
            CaseTemplate.MCObject.ModelObject.ElementComposition = compTable;
        }


        #endregion


    }
}
