using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;

namespace AMFramework.Controller
{
    public class Controller_EquilibriumConfiguration : ControllerAbstract
    {

        #region Socket
        public Controller_EquilibriumConfiguration(ref IAMCore_Comm comm, Controller_Cases caseController) : base(comm)
        {
            if (caseController.SelectedCase != null)
                _configuration = ControllerM_EquilibriumConfiguration.Get_EquilibriumConfiguration_FromIDCase(comm, caseController.SelectedCase.ModelObject.ID);
            else
                _configuration = ControllerM_EquilibriumConfiguration.Get_EquilibriumConfiguration_FromIDCase(comm, -1);
        }
        #endregion

        #region Data
        ModelController<Model_EquilibriumConfiguration> _configuration;
        /// <summary>
        /// Equilibrium solidifcation setup
        /// </summary>
        public ModelController<Model_EquilibriumConfiguration> Configuration
        {
            get { return _configuration; }
        }
        #endregion

    }
}
