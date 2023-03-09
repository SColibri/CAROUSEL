using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System.Collections.Generic;

namespace AMFramework.Controller
{
    public class Controller_PrecipitationPhase : ControllerAbstract
    {

        #region Socket
        public Controller_PrecipitationPhase(ref IAMCore_Comm comm, Controller.Controller_Cases caseController) : base(comm)
        {
            if (caseController.SelectedCase != null)
                PrecipitationPhases = ControllerM_PrecipitationPhase.Get_PrecipitationPhases_FromIDCase(_comm, caseController.SelectedCase.ModelObject.ID);
        }

        #endregion

        #region Properties
        private List<ModelController<Model_PrecipitationPhase>> _precipitationPhases = new();
        /// <summary>
        /// get/set Precipitation phases
        /// </summary>
        public List<ModelController<Model_PrecipitationPhase>> PrecipitationPhases
        {
            get { return _precipitationPhases; }
            set
            {
                _precipitationPhases = value;
                OnPropertyChanged(nameof(PrecipitationPhases));
            }
        }
        #endregion

    }
}
