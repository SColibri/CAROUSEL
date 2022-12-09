using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.AMSystem.Attributes;

namespace AMFramework_Lib.Model
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
                OnPropertyChanged(nameof(ID)); 
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
                OnPropertyChanged(nameof(Name));
            }
        }


        private string _apiName = "Not_set";
        [Order]
        public string APIName
        {
            get { return _apiName; }
            set 
            {
                _apiName = value;
                OnPropertyChanged(nameof(APIName));
            }
        }

        private string _externalAPI_Name = "Not_set";
        [Order]
        public string ExternalAPI_Name
        {
            get { return _externalAPI_Name; }
            set
            {
                _externalAPI_Name = value;
                OnPropertyChanged(nameof(ExternalAPI_Name));
            }
        }

        #region Relational_properties

        // --------------------------------------------------
        //                   Case
        // --------------------------------------------------

        // Selected elements
        private List<ModelController<Model_Case>> _cases = new();
        public List<ModelController<Model_Case>> Cases
        {
            get { return _cases; }
            set
            {
                _cases = value;
                OnPropertyChanged(nameof(Cases));
            }
        }

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
                OnPropertyChanged(nameof(SelectedElements));
            }
        }

        // --------------------------------------------------
        //                   ACTIVE PHASES
        // --------------------------------------------------

        // Configuration
        private ModelController<Model_ActivePhasesConfiguration>? _activePhasesConfiguration = null;
        public ModelController<Model_ActivePhasesConfiguration>? ActivePhasesConfiguration
        {
            get { return _activePhasesConfiguration; }
            set 
            {
                _activePhasesConfiguration = value;
                OnPropertyChanged(nameof(ActivePhasesConfiguration));
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
                OnPropertyChanged(nameof(ActivePhases));
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

                OnPropertyChanged(nameof(ActivePhasesElementComposition));

                var refElement = SelectedElements.Find(e => e.ModelObject.ISReferenceElement == 1);
                if (refElement == null) return;

                foreach (var item in _activePhasesElementComposition)
                {
                    if (item.ModelObject.IDElement != refElement.ModelObject.IDElement)
                    {
                        item.ModelObject.PropertyChanged += ActivePhasesElementComposition_CheckValues;
                    }
                    else 
                    {
                        item.ModelObject.IsReferenceElement = true;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the reference element based ont the composition of all other elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActivePhasesElementComposition_CheckValues(object? sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName?.CompareTo("Value") != 0) return;

            var refElement = SelectedElements.Find(e => e.ModelObject.ISReferenceElement == 1);
            if (refElement == null) return;

            double totalSum = 0;
            foreach (var item in ActivePhasesElementComposition)
            {
                if(item.ModelObject.IDElement != refElement.ModelObject.IDElement) 
                {
                    totalSum += item.ModelObject.Value;
                }
            }

            var refElementComposition = ActivePhasesElementComposition.Find(e => e.ModelObject.IDElement == refElement.ModelObject.IDElement);
            
            if(refElementComposition != null)
                refElementComposition.ModelObject.Value = 1 - totalSum;
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

        public override string Get_Scripting_ClassName()
        {
            return "Project";
        }
        #endregion
    }
}
