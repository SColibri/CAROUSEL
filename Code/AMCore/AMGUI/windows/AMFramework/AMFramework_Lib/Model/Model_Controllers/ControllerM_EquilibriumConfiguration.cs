using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_EquilibriumConfiguration : Controller_Abstract_Models<Model_EquilibriumConfiguration>
    {
        // Constructors
        public ControllerM_EquilibriumConfiguration(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_EquilibriumConfiguration(IAMCore_Comm comm, ModelController<Model_EquilibriumConfiguration> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods

        #endregion
    }
}
