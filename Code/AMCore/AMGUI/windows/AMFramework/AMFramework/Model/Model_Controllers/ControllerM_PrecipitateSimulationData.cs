using AMFramework.Core;
using AMFramework.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.Model_Controllers
{
    public class ControllerM_PrecipitateSimulationData : Controller_Abstract_Models<Model_PrecipitateSimulationData>
    {
        // Constructors
        public ControllerM_PrecipitateSimulationData(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_PrecipitateSimulationData(IAMCore_Comm comm, ModelController<Model_PrecipitateSimulationData> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods

        #endregion
    }
}
