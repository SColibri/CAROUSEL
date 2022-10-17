using AMFramework.Core;
using AMFramework.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.Model_Controllers
{
    internal class ControllerM_Phase : Controller_Abstract_Models<Model_Phase>
    {
        // Constructors
        public ControllerM_Phase(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_Phase(IAMCore_Comm comm, ModelController<Model_Phase> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods

        #endregion
    }
}
