using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

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
        public static List<ModelController<Model_PrecipitationDomain>> Get_PrecipitationDomains_FromIDCase(IAMCore_Comm comm, int IDCase)
        {
            return ModelController<Model_PrecipitationDomain>.LoadIDCase(ref comm, IDCase);
        }

        /// <summary>
        /// Return precipitation domain using ID
        /// </summary>
        /// <param name="comm">IAMCore_Comm</param>
        /// <param name="ID">ID from precipitation domain</param>
        /// <returns></returns>
        public static ModelController<Model_PrecipitationDomain> Get_PrecipitationDomain_FromID(IAMCore_Comm comm, int ID)
        {
            ModelController<Model_PrecipitationDomain> result = new(ref comm);
            result.ModelObject.ID = ID;
            result.LoadByIDAction?.DoAction();

            return result;
        }
        #endregion
    }
}
