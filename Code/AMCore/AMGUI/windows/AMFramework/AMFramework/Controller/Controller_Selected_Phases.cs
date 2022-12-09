using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Model.Model_Controllers;

namespace AMFramework.Controller
{
    public class Controller_Selected_Phases : ControllerAbstract
    {

        #region Socket
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="caseController"></param>
        public Controller_Selected_Phases(ref IAMCore_Comm comm, Controller_Cases caseController) : base(comm)
        {
            if(caseController.SelectedCase != null)
                SelectedPhases = ControllerM_SelectedPhases.Get_SelectedPhases_FromIDCase(_comm, caseController.SelectedCase.ModelObject.ID);
        }

        #endregion

        #region Properties
        private List<ModelController<Model_SelectedPhases>> _selectedPhases = new();
        /// <summary>
        /// get/set selected phases
        /// </summary>
        public List<ModelController<Model_SelectedPhases>> SelectedPhases
        {
            get { return _selectedPhases; }
            set
            {
                _selectedPhases = value;
                OnPropertyChanged(nameof(SelectedPhases));
            }
        }
        #endregion

    }
}
