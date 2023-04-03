using AMFramework_Lib.AMSystem.Attributes;
using System.ComponentModel;

namespace AMFramework_Lib.Model
{
    /// <summary>
    /// Model_Projects class that defines the structure of a project model object
    /// </summary>
    public class Model_Projects : ModelAbstract
    {
        #region Fields
        /// <summary>
        /// Databases used in this project
        /// </summary>
        private ModelController<Model_CALPHADDatabase>? _databases;

        /// <summary>
        /// Cases contained in this project
        /// </summary>
        private List<ModelController<Model_Case>> _cases = new();

        /// <summary>
        /// List of selected elements
        /// </summary>
        private List<ModelController<Model_SelectedElements>> _selectedElements = new();

        /// <summary>
        /// Active phase search configuration
        /// </summary>
        private ModelController<Model_ActivePhasesConfiguration>? _activePhasesConfiguration = null;

        /// <summary>
        /// Element composition to be used for active phase search
        /// </summary>
        private List<ModelController<Model_ActivePhasesElementComposition>> _activePhasesElementComposition = new();

        /// <summary>
        /// List of found active phases
        /// </summary>
        private List<ModelController<Model_ActivePhases>> _activePhases = new();

        #endregion

        #region Model Fields
        /// <summary>
        /// Get/set Identifier
        /// </summary>
        private int _id = -1;

        /// <summary>
        /// Project name
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// API used for communication
        /// </summary>
        private string _apiName = "Not_set";

        /// <summary>
        /// External Software used
        /// </summary>
        private string _externalAPI_Name = "Not_set";
        #endregion

        #region Properties

        /// <summary>
        /// Get/set Identifier
        /// </summary>
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

        /// <summary>
        /// Get/set name
        /// </summary>
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

        /// <summary>
        /// API used for communication
        /// </summary>
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

        /// <summary>
        /// External Software used
        /// </summary>
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
        #endregion

        #region Relational_properties

        /// <summary>
        /// Get/set Databases used in this project
        /// </summary>
        public ModelController<Model_CALPHADDatabase> Databases 
        {
            get => _databases;
            set 
            { 
                _databases = value;
                OnPropertyChanged(nameof(Databases));
            }
        }

        /// <summary>
        /// Get/set Cases contained in this project
        /// </summary>
        public List<ModelController<Model_Case>> Cases
        {
            get { return _cases; }
            set
            {
                _cases = value;
                OnPropertyChanged(nameof(Cases));
            }
        }

        /// <summary>
        /// Get/set List of selected elements
        /// </summary>
        public List<ModelController<Model_SelectedElements>> SelectedElements
        {
            get { return _selectedElements; }
            set
            {
                _selectedElements = value;
                OnPropertyChanged(nameof(SelectedElements));
            }
        }

        /// <summary>
        /// Get/set Active phase search configuration
        /// </summary>
        public ModelController<Model_ActivePhasesConfiguration>? ActivePhasesConfiguration
        {
            get { return _activePhasesConfiguration; }
            set
            {
                _activePhasesConfiguration = value;
                OnPropertyChanged(nameof(ActivePhasesConfiguration));
            }
        }

        /// <summary>
        /// Get/set List of found active phases
        /// </summary>
        public List<ModelController<Model_ActivePhases>> ActivePhases
        {
            get { return _activePhases; }
            set
            {
                _activePhases = value;
                OnPropertyChanged(nameof(ActivePhases));
            }
        }

        /// <summary>
        /// Get/set Element composition to be used for active phase search
        /// </summary>
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
                if (item.ModelObject.IDElement != refElement.ModelObject.IDElement)
                {
                    totalSum += item.ModelObject.Value;
                }
            }

            var refElementComposition = ActivePhasesElementComposition.Find(e => e.ModelObject.IDElement == refElement.ModelObject.IDElement);

            if (refElementComposition != null)
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
