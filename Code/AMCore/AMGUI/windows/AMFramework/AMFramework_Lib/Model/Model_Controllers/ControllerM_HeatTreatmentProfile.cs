using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_HeatTreatmentProfile : Controller_Abstract_Models<Model_HeatTreatmentProfile>
    {
        // Constructors
        public ControllerM_HeatTreatmentProfile(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_HeatTreatmentProfile(IAMCore_Comm comm, ModelController<Model_HeatTreatmentProfile> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods

        #endregion
    }
}
