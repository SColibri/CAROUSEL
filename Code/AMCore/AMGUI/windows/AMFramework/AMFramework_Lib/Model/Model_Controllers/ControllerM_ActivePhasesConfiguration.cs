using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_ActivePhasesConfiguration : Controller_Abstract_Models<Model_ActivePhasesElementComposition>
    {
        // Constructors
        public ControllerM_ActivePhasesConfiguration(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_ActivePhasesConfiguration(IAMCore_Comm comm, ModelController<Model_ActivePhasesElementComposition> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        public static ModelController<Model_ActivePhasesConfiguration> Get_ActivePhaseConfiguration_FromIDProject(IAMCore_Comm comm, int IDProject) 
        {
            ModelController<Model_ActivePhasesConfiguration> result;

            // load configuration
            List<ModelController<Model_ActivePhasesConfiguration>> refConfig = ModelController<Model_ActivePhasesConfiguration>.LoadIDProject(ref comm, IDProject);
            
            // If no configuration exists, add one else return [0] position, this is done because the loadby project returns a list.
            // no other reason.
            if (refConfig.Count > 0)
            {
                result = refConfig[0];
            }
            else
            {
                Model_ActivePhasesConfiguration refModelConfig = new() { IDProject = IDProject };
                result = new ModelController<Model_ActivePhasesConfiguration>(ref comm, refModelConfig);
                result.SaveAction?.DoAction();
            }

            return result;
        }


        #endregion
    }
}
