using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System.Collections.Generic;

namespace AMFramework.Controller
{
    internal class Controller_HeatTreatment : ControllerAbstract
    {
        #region Socket
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="caseController"></param>
        public Controller_HeatTreatment(ref IAMCore_Comm comm, Controller_Cases caseController) : base(comm)
        {
            if (caseController.SelectedCase != null)
                _heatTreatments = ControllerM_HeatTreatment.Get_heatTreatmentsFromCaseID(_comm, caseController.SelectedCase.ModelObject.ID);
            else
                _heatTreatments = new();
        }
        #endregion

        #region Data
        private List<ModelController<Model_HeatTreatment>> _heatTreatments;
        /// <summary>
        /// gets list of heat treatments
        /// </summary>
        public List<ModelController<Model_HeatTreatment>> HeatTreatments { get { return _heatTreatments; } }

        #endregion
    }
}
