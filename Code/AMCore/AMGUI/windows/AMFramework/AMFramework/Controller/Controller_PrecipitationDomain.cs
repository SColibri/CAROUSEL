using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System.Collections.Generic;

namespace AMFramework.Controller
{
    public class Controller_PrecipitationDomain : ControllerAbstract
    {

        #region Socket
        private Controller.Controller_Cases _CaseController;
        public Controller_PrecipitationDomain(ref IAMCore_Comm comm, Controller.Controller_Cases caseController) : base(comm)
        {
            _CaseController = caseController;

            if (caseController.SelectedCase != null)
                PrecipitationDomains = ControllerM_PrecipitationDomain.Get_PrecipitationDomains_FromIDCase(_comm, caseController.SelectedCase.ModelObject.ID);
        }

        #endregion

        #region Parameters

        private List<ModelController<Model_PrecipitationDomain>> _precipitationDomains = new();
        public List<ModelController<Model_PrecipitationDomain>> PrecipitationDomains
        {
            get { return _precipitationDomains; }
            set
            {
                _precipitationDomains = value;
                OnPropertyChanged(nameof(PrecipitationDomains));
            }
        }

        #endregion

    }
}
