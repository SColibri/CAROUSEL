using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System.Collections.Generic;

namespace AMFramework.Controller
{
    public class Controller_EquilibriumPhaseFraction : ControllerAbstract
    {

        #region Constructor
        public Controller_EquilibriumPhaseFraction(ref IAMCore_Comm comm, Controller_Cases caseController) : base(comm)
        {
            if (caseController.SelectedCase != null)
                EquilibriumPhaseFractions = ControllerM_EquilibriumPhaseFraction.Get_EquilibriumPhaseFractions_FromIDCase(_comm, caseController.SelectedCase.ModelObject.ID);
        }
        #endregion

        #region Data
        List<ModelController<Model_EquilibriumPhaseFraction>> _equilibriumPhaseFractions = new();
        public List<ModelController<Model_EquilibriumPhaseFraction>> EquilibriumPhaseFractions
        {
            get => _equilibriumPhaseFractions;
            set
            {
                _equilibriumPhaseFractions = value;
                OnPropertyChanged(nameof(_equilibriumPhaseFractions));
            }
        }
        #endregion

    }
}
