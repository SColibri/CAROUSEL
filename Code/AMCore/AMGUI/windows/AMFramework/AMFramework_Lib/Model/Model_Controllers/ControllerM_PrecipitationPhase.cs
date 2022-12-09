using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_PrecipitationPhase : Controller_Abstract_Models<Model_PrecipitationPhase>
    {
        // Constructors
        public ControllerM_PrecipitationPhase(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_PrecipitationPhase(IAMCore_Comm comm, ModelController<Model_PrecipitationPhase> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        public static List<ModelController<Model_PrecipitationPhase>> Get_PrecipitationPhases_FromIDCase(IAMCore_Comm comm, int IDCase)
        {
            // load precipitation phases
            var result = ModelController<Model_PrecipitationPhase>.LoadIDCase(ref comm, IDCase);

            return result;
        }
        #endregion
    }
}
