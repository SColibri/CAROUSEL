using AMFramework.Core;
using AMFramework.Interfaces;
using AMFramework.Model.Controllers;
using AMFramework.Views.ActivePhases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.Model_Controllers
{
    public class ControllerM_Project : Controller_Abstract_Models<Model_Projects>
    {
        // Constructors
        public ControllerM_Project(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_Project(IAMCore_Comm comm, ModelController<Model_Projects> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        public void Load_SelectedElements() 
        {
            MCObject.ModelObject.SelectedElements = ModelController<Model_SelectedElements>.LoadIDProject(ref _comm, MCObject.ModelObject.ID);
        }

        public void Load_ActivePhases()
        {
            MCObject.ModelObject.ActivePhases = ModelController<Model_ActivePhases>.LoadIDProject(ref _comm, MCObject.ModelObject.ID);
            MCObject.ModelObject.ActivePhasesConfiguration = ModelController<Model_ActivePhasesConfiguration>.LoadIDProject(ref _comm, MCObject.ModelObject.ID)[0];
            MCObject.ModelObject.ActivePhasesElementComposition = ModelController<Model_ActivePhasesElementComposition>.LoadIDProject(ref _comm, MCObject.ModelObject.ID);
        }
        #endregion


    }
}
