using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_SelectedPhases : Controller_Abstract_Models<Model_SelectedPhases>
    {
        // Constructors
        public ControllerM_SelectedPhases(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_SelectedPhases(IAMCore_Comm comm, ModelController<Model_SelectedPhases> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// Loads all selected phases in IDCase
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDCase"></param>
        /// <returns></returns>
        public static List<ModelController<Model_SelectedPhases>> Get_SelectedPhases_FromIDCase(IAMCore_Comm comm, int IDCase)
        {
            // load Selected phases
            List<ModelController<Model_SelectedPhases>> result = ModelController<Model_SelectedPhases>.LoadIDCase(ref comm, IDCase);

            // Get phase name
            foreach (var item in result)
            {
                item.ModelObject.PhaseName = ControllerM_Phase.Get_PhaseByID(comm, item.ModelObject.IDPhase).ModelObject.Name;
            }

            return result;
        }
        #endregion
    }
}
