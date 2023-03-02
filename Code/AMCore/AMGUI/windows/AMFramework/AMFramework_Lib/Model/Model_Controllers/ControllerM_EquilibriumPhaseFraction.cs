using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_EquilibriumPhaseFraction : Controller_Abstract_Models<Model_EquilibriumPhaseFraction>
    {
        // Constructors
        public ControllerM_EquilibriumPhaseFraction(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_EquilibriumPhaseFraction(IAMCore_Comm comm, ModelController<Model_EquilibriumPhaseFraction> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// returns a list of all phase fraction data points obtained from the solidifcation simulation
        /// </summary>
        /// <param name="comm">Core communication object</param>
        /// <param name="IDCase">case id</param>
        /// <returns></returns>
        public static List<ModelController<Model_EquilibriumPhaseFraction>> Get_EquilibriumPhaseFractions_FromIDCase(IAMCore_Comm comm, int IDCase)
        {
            // load Selected elements
            List<ModelController<Model_EquilibriumPhaseFraction>> result = ModelController<Model_EquilibriumPhaseFraction>.LoadIDCase(ref comm, IDCase);

            return result;
        }
        #endregion
    }
}
