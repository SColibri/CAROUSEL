using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

namespace AMFramework_Lib.Model.Model_Controllers
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
