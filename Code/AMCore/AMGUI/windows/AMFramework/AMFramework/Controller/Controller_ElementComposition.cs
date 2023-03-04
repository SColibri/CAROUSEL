using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System.Collections.Generic;

namespace AMFramework.Controller
{
    public class Controller_ElementComposition : ControllerAbstract
    {
        #region Socket
        /// <summary>
        /// Controller related that hold data on the composition configuration for a specific IDcase
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="caseController"></param>
        public Controller_ElementComposition(ref IAMCore_Comm comm, Controller_Cases caseController) : base(comm)
        {
            int IDCase = caseController.SelectedCase != null ? caseController.SelectedCase.ModelObject.ID : -1;
            _composition = ControllerM_ElementComposition.Get_ElementComposition_FromIDCase(comm, IDCase);
        }
        #endregion

        #region Data
        List<ModelController<Model_ElementComposition>> _composition;
        /// <summary>
        /// Returns composition list for current IDCase
        /// </summary>
        public List<ModelController<Model_ElementComposition>> Composition
        {
            get { return _composition; }
        }
        #endregion

    }
}
