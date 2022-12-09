using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_HeatTreatment : Controller_Abstract_Models<Model_HeatTreatment>
    {
        // Constructors
        public ControllerM_HeatTreatment(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_HeatTreatment(IAMCore_Comm comm, ModelController<Model_HeatTreatment> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// Loads all heat treatment objects
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDCase"></param>
        /// <returns></returns>
        public static List<ModelController<Model_HeatTreatment>> Get_heatTreatmentsFromCaseID(IAMCore_Comm comm, int IDCase)
        {
            List<ModelController<Model_HeatTreatment>> result = ModelController<Model_HeatTreatment>.LoadIDCase(ref comm, IDCase);

            foreach (var item in result)
            {
                item.ModelObject.TemplatedStartTemperature = item.ModelObject.StartTemperature.ToString();
                item.ModelObject.HeatTreatmentSegment = ControllerM_HeatTreatmentSegment.Get_heatTreatmentsSegmentsFromHeatTreatmentID(comm, item.ModelObject.ID);
            }

            return result;
        }
        #endregion
    }
}
