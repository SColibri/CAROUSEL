using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_HeatTreatmentSegment : Controller_Abstract_Models<Model_HeatTreatmentSegment>
    {
        // Constructors
        public ControllerM_HeatTreatmentSegment(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_HeatTreatmentSegment(IAMCore_Comm comm, ModelController<Model_HeatTreatmentSegment> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// Returns a list of all heat treatment segments
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDHeatTreatment"></param>
        /// <returns></returns>
        public static List<ModelController<Model_HeatTreatmentSegment>> Get_heatTreatmentsSegmentsFromHeatTreatmentID(IAMCore_Comm comm, int IDHeatTreatment)
        {
            string Query = "SELECT * FROM HeatTreatmentSegments WHERE IDHeatTreatment = " + IDHeatTreatment;
            var result = ModelController<Model_HeatTreatmentSegment>.LoadByQuery(ref comm, Query);

            foreach (var item in result)
            {
                item.ModelObject.TemplatedEndTemperature = item.ModelObject.EndTemperature.ToString();

                if (item.ModelObject.Duration == 0)
                {
                    item.ModelObject.TemplateValue = item.ModelObject.TemperatureGradient.ToString();
                    item.ModelObject.SelectedModeType = Model_HeatTreatmentSegment.ModeType.TEMPERATURE_GRADIENT;
                }
                else 
                {
                    item.ModelObject.TemplateValue = item.ModelObject.Duration.ToString();
                    item.ModelObject.SelectedModeType = Model_HeatTreatmentSegment.ModeType.TIME_INTERVAL;
                }               
            }

            return result;
        }
        #endregion
    }
}

