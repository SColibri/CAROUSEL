using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMFramework.Controller
{
    /// <summary>
    /// Active phases controller
    /// </summary>
    public class Controller_ActivePhases : AMFramework_Lib.Controller.ControllerAbstract
    {
        #region Field
        /// <summary>
        /// Project controller
        /// </summary>
        private Controller_Project _projectController;

        /// <summary>
        /// List of Active phases
        /// </summary>
        private List<ModelController<Model_ActivePhases>> _ActivePhases;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="projectController"></param>
        public Controller_ActivePhases(Controller_Project projectController)
        {
            _projectController = projectController;
        }
        #endregion


        #region Properties

        /// <summary>
        /// List of all active phases
        /// </summary>
        public List<ModelController<Model_ActivePhases>> ActivePhases
        {
            get => _ActivePhases;
            set
            {
                _ActivePhases = value;
                OnPropertyChanged(nameof(ActivePhases));
            }
        }
        #endregion


        #region Methods
        /// <summary>
        /// Update values
        /// </summary>
        public override void Refresh()
        {
            RefreshActivePhases();
        }

        /// <summary>
        /// Update active phases for current project
        /// </summary>
        private void RefreshActivePhases()
        {
            if (_projectController.SelectedProject?.MCObject?.ModelObject == null) return;
            ActivePhases = ControllerM_ActivePhases.Get_ActivePhaseList(Controller_Global.ApiHandle,
                                                    _projectController.SelectedProject.MCObject.ModelObject.ID);
        }
        #endregion
    }
}
