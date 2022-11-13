using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_PrecipitationDomain : Controller_Abstract_Models<Model_PrecipitationDomain>
    {
        // Constructors
        public ControllerM_PrecipitationDomain(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_PrecipitationDomain(IAMCore_Comm comm, ModelController<Model_PrecipitationDomain> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        public static List<ModelController<Model_PrecipitationPhase>> Get_PrecipitationDomains(IAMCore_Comm comm, int IDCase) 
        {
            return ModelController<Model_PrecipitationPhase>.LoadIDCase(ref comm, IDCase);
        }
        #endregion
    }
}
