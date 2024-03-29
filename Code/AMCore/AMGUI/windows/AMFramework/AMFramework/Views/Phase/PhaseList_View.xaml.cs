﻿using AMFramework.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;

namespace AMFramework.Views.Phase
{
    /// <summary>
    /// Interaction logic for PhaseList_View.xaml
    /// </summary>
    public partial class PhaseList_View : UserControl
    {
        private ModelController<Model_Case>? _caseModelController;

        /// <summary>
        /// Default constructor, loads phases from current database 
        /// </summary>
        /// <param name="comm"></param>
        public PhaseList_View(IAMCore_Comm comm)
        {
            InitializeComponent();

            Create_DataContext(comm);
        }

        /// <summary>
        /// Shows phases from CALPHAD database and sets as selected all the phases selected in the case level, deprecate?
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDCase"></param>
        public PhaseList_View(IAMCore_Comm comm, ModelController<Model_Case> caseModel)
        {
            InitializeComponent();

            _caseModelController = caseModel;
            Create_DataContext(comm);
        }

        /// <summary>
        /// Creates a controller phase object and uses it as the datacontext. To retrieve selection use the Get_selection
        /// Method included in this view or call the controller's function
        /// </summary>
        /// <param name="comm"></param>
        private void Create_DataContext(IAMCore_Comm comm)
        {
            var dC = new Controller_Phase(comm);
            DataContext = dC;

            dC.IsLoading = true;
            Thread TH01 = new(LoadPhases_Async);
            TH01.Start((Controller_Phase)DataContext);
        }

        /// <summary>
        /// Loads phase list and selection.
        /// </summary>
        private void LoadPhases_Async(object? param)
        {
            if (param == null) return;

            Controller_Phase tRef = (Controller_Phase)param;
            tRef.LoadFromDatabase();

            if (_caseModelController != null)
                tRef.SetSelected(_caseModelController);

            tRef.IsLoading = false;
        }

        /// <summary>
        /// Returns all selected phases
        /// </summary>
        /// <returns></returns>
        public List<ModelController<Model_Phase>> Get_Selection()
        {
            var selectedList = ((Controller_Phase)DataContext).Get_Selected();
            List<ModelController<Model_Phase>> result = new();

            // extract the modelcontroller
            foreach (var item in selectedList)
            {
                result.Add(item.MCObject);
            }

            return result;
        }




    }
}
