using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Model;
using AMFramework_Lib.Core;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Model.Model_Controllers;

namespace AMFramework.Controller
{
    internal class Controller_Element : ControllerAbstract
    {
        #region Constructor
        /// <summary>
        /// Load data elements related to a project
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="projectController"></param>
        public Controller_Element(ref IAMCore_Comm socket, Controller_Project projectController) : base(socket)
        {
            if ( projectController.SelectedProject != null)
                Elements = ControllerM_Element.Get_elementsFromProjectID(_comm, projectController.SelectedProject.MCObject.ModelObject.ID);
        }

        /// <summary>
        /// Load data from database
        /// </summary>
        /// <param name="socket"></param>
        public Controller_Element(ref IAMCore_Comm socket) : base(socket)
        {
            Elements = ControllerM_Element.LoadFromDatabase(_comm);
        }
        #endregion

        #region Properties
        private List<ModelController<Model_Element>> _elements = new();
        /// <summary>
        /// Get/set List of allavailable elements
        /// </summary>
        public List<ModelController<Model_Element>> Elements 
        { 
            get => _elements;
            set 
            {
                _elements = value;
                OnPropertyChanged(nameof(_elements));
            }
        }
        #endregion
    }
}
