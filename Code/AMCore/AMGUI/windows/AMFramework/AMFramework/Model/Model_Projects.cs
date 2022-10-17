﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework.Interfaces;
using AMFramework.AMSystem.Attributes;

namespace AMFramework.Model
{
    public class Model_Projects : ModelAbstract
    {

        private int _id = -1;
        [Order]
        public int ID 
        { 
            get { return _id; } 
            set 
            {
                _id = value;
                OnPropertyChanged("ID"); 
            } 
        }

        private string _name = "New Name";
        [Order]
        public string Name
        {
            get { return _name; }
            set 
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }


        private string _apiName = "";
        [Order]
        public string APIName
        {
            get { return _apiName; }
            set 
            {
                _apiName = value;
                OnPropertyChanged("APIName");
            }
        }

        #region Relational_properties

        // --------------------------------------------------
        //                   ELEMENTS
        // --------------------------------------------------

        // Selected elements
        private List<ModelController<Model_SelectedElements>> _selectedElements = new();
        public List<ModelController<Model_SelectedElements>> SelectedElements 
        {
            get { return _selectedElements; }
            set 
            {
                _selectedElements = value;
                OnPropertyChanged("SelectedElements");
            }
        }

        // --------------------------------------------------
        //                   ACTIVE PHASES
        // --------------------------------------------------

        // Configuration
        private ModelController<Model_ActivePhasesConfiguration> _activePhasesConfiguration = null;
        public ModelController<Model_ActivePhasesConfiguration> ActivePhasesConfiguration
        {
            get { return _activePhasesConfiguration; }
            set 
            {
                _activePhasesConfiguration = value;
                OnPropertyChanged("ActivePhasesConfiguration");
            }
        }

        // Active Phases
        private List<ModelController<Model_ActivePhases>> _activePhases= new();
        public List<ModelController<Model_ActivePhases>> ActivePhases
        {
            get { return _activePhases; }
            set
            {
                _activePhases = value;
                OnPropertyChanged("ActivePhases");
            }
        }

        // Composition
        private List<ModelController<Model_ActivePhasesElementComposition>> _activePhasesElementComposition = new();
        public List<ModelController<Model_ActivePhasesElementComposition>> ActivePhasesElementComposition
        {
            get { return _activePhasesElementComposition; }
            set
            {
                _activePhasesElementComposition = value;
                OnPropertyChanged("ActivePhasesElementComposition");
            }
        }
        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_Projects>();
        }

        public override string Get_Table_Name()
        {
            return "Projects";
        }
        #endregion
    }
}
