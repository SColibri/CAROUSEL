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

            ModelController<Model_Element> tRef = new(ref _comm);
            foreach (var item in MCObject.ModelObject.SelectedElements)
            {
                tRef.ModelObject.ID = item.ModelObject.IDElement;
                tRef.LoadByIDAction.DoAction();
                item.ModelObject.ElementName = tRef.ModelObject.Name;
            }
        }

        public void Load_ActivePhases()
        {
            MCObject.ModelObject.ActivePhases = ModelController<Model_ActivePhases>.LoadIDProject(ref _comm, MCObject.ModelObject.ID);

            foreach (var item in MCObject.ModelObject.ActivePhases)
            {
                ModelController<Model_Phase> pName = new(ref _comm);
                pName.ModelObject.ID = item.ModelObject.IDPhase;
                pName.LoadByIDAction.DoAction();

                item.ModelObject.PhaseName = pName.ModelObject.Name;
            }

            List<ModelController<Model_ActivePhasesConfiguration>> refConfig = ModelController<Model_ActivePhasesConfiguration>.LoadIDProject(ref _comm, MCObject.ModelObject.ID);
            if (refConfig.Count > 0)
            {
                MCObject.ModelObject.ActivePhasesConfiguration = ModelController<Model_ActivePhasesConfiguration>.LoadIDProject(ref _comm, MCObject.ModelObject.ID)[0];
            }
            else 
            {
                Model_ActivePhasesConfiguration refModelConfig = new() { IDProject = MCObject.ModelObject.ID };
                MCObject.ModelObject.ActivePhasesConfiguration = new ModelController<Model_ActivePhasesConfiguration>(ref _comm, refModelConfig);
                MCObject.ModelObject.ActivePhasesConfiguration.SaveAction.DoAction();
            }
            
            MCObject.ModelObject.ActivePhasesElementComposition = ModelController<Model_ActivePhasesElementComposition>.LoadIDProject(ref _comm, MCObject.ModelObject.ID);

            ModelController<Model_Element> tRef = new(ref _comm);
            foreach (var item in MCObject.ModelObject.ActivePhasesElementComposition)
            {
                tRef.ModelObject.ID = item.ModelObject.IDElement;
                tRef.LoadByIDAction.DoAction();
                item.ModelObject.ElementName = tRef.ModelObject.Name;
            }
        }
        #endregion


    }
}
