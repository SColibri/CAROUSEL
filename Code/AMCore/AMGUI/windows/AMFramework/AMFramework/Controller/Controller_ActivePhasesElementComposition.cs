using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System.Collections.Generic;

namespace AMFramework.Controller
{
    /// <summary>
    /// Active phases, element composition configuration controller
    /// </summary>
    public class Controller_ActivePhasesElementComposition : ControllerAbstract
    {
        #region Field
        /// <summary>
        /// Composition list
        /// </summary>
        private List<ModelController<Model_ActivePhasesElementComposition>> _composition = new();

        #endregion
        
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="projectController"></param>
        public Controller_ActivePhasesElementComposition(ref IAMCore_Comm comm, Controller_Project projectController) : base(comm)
        {
            if (projectController.SelectedProject != null)
                Composition = ControllerM_ActivePhasesElementComposition.Get_ActivePhaseElementComposition_FromIDProject(_comm, projectController.SelectedProject.MCObject.ModelObject.ID);
        }
        #endregion

        #region Composition
        /// <summary>
        /// Composition list
        /// </summary>
        public List<ModelController<Model_ActivePhasesElementComposition>> Composition
        {
            get => _composition;
            set
            {
                _composition = value;
                OnPropertyChanged();
            }
        }

        #endregion

    }
}
