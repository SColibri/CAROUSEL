using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    internal class ControllerM_ScheilPhaseFraction : Controller_Abstract_Models<Model_ScheilPhaseFraction>
    {
        // Constructors
        public ControllerM_ScheilPhaseFraction(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_ScheilPhaseFraction(IAMCore_Comm comm, ModelController<Model_ScheilPhaseFraction> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// returns a list of all phase fraction data points obtained from the solidifcation simulation
        /// </summary>
        /// <param name="comm">Core Communication object</param>
        /// <param name="IDCase">Case id</param>
        /// <returns></returns>
        public static List<ModelController<Model_ScheilPhaseFraction>> Get_ScheilPhaseFractions_FromIDCase(IAMCore_Comm comm, int IDCase)
        {
            // load Selected elements
            List<ModelController<Model_ScheilPhaseFraction>> result = ModelController<Model_ScheilPhaseFraction>.LoadIDCase(ref comm, IDCase);

            return result;
        }
        #endregion

    }
}
