using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System.Collections.Generic;

namespace AMFramework.Controller
{
    public class Controller_ActivePhasesElementComposition : ControllerAbstract
    {
        #region Socket
        public Controller_ActivePhasesElementComposition(ref IAMCore_Comm comm, Controller_Project projectController) : base(comm)
        {
            if (projectController.SelectedProject != null)
                Composition = ControllerM_ActivePhasesElementComposition.Get_ActivePhaseElementComposition_FromIDProject(_comm, projectController.SelectedProject.MCObject.ModelObject.ID);
        }
        #endregion

        #region Composition
        private List<ModelController<Model_ActivePhasesElementComposition>> _composition = new();
        public List<ModelController<Model_ActivePhasesElementComposition>> Composition
        {
            get { return _composition; }
            set
            {
                _composition = value;
                OnPropertyChanged(nameof(Composition));
            }
        }

        #endregion

    }
}
