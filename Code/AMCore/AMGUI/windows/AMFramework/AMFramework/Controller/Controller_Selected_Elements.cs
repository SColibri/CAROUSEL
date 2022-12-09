﻿using System;
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
    public class Controller_Selected_Elements : ControllerAbstract
    {

        #region Socket
        public Controller_Selected_Elements(ref IAMCore_Comm comm, Controller_Project projectController) : base(comm)
        {
            if(projectController.SelectedProject != null)
                SelectedElements = ControllerM_SelectedElements.Get_SelectedElements_FromIDProject(comm, projectController.SelectedProject.MCObject.ModelObject.ID);
        }

        #endregion

        #region Properties
        private List<ModelController<Model_SelectedElements>> _selectedElements = new();
        /// <summary>
        /// get/set selected elements
        /// </summary>
        public List<ModelController<Model_SelectedElements>> SelectedElements
        {
            get => _selectedElements;
            set 
            {
                _selectedElements = value;
                OnPropertyChanged(nameof(_selectedElements));
            }
        }
        #endregion
    }
}
