using AMFramework.Core;
using AMFramework.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.Model_Controllers
{
    public class ControllerM_HeatTreatment : Controller_Abstract_Models<Model_HeatTreatment>
    {
        // Constructors
        public ControllerM_HeatTreatment(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_HeatTreatment(IAMCore_Comm comm, ModelController<Model_HeatTreatment> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods

        #endregion
    }
}
