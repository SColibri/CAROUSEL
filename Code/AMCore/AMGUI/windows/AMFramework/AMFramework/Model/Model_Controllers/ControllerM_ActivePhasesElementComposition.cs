using AMFramework.Core;
using AMFramework.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AMFramework.Model.Model_Controllers
{
    public class ControllerM_ActivePhasesElementComposition : Controller_Abstract_Models<Model_ActivePhasesElementComposition>
    {
        // Constructors
        public ControllerM_ActivePhasesElementComposition(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_ActivePhasesElementComposition(IAMCore_Comm comm, ModelController<Model_ActivePhasesElementComposition> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        
        #endregion
    }
}
