using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework.Interfaces;
using AMFramework.AMSystem.Attributes;

namespace AMFramework.Model
{
    public class Model_Case : ModelAbstract
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

        private int _id_project = -1;
        [Order]
        public int IDProject
        {
            get { return _id_project; }
            set
            {
                _id_project = value;
                OnPropertyChanged("IDProject");
            }
        }

        private int _id_group = -1;
        [Order]
        public int IDGroup
        {
            get { return _id_group; }
            set
            {
                _id_group = value;
                OnPropertyChanged("IDGroup");
            }
        }

        private string _name = "";
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

        private string _script = "";
        [Order]
        public string Script
        {
            get { return _script; }
            set
            {
                _script = value;
                OnPropertyChanged("Script");
            }
        }

        private string _date = "";
        [Order]
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        private double _pos_x = 0.0;
        [Order]
        public double PosX
        {
            get { return _pos_x; }
            set
            {
                _pos_x = value;
                OnPropertyChanged("PosX");
            }
        }

        private double _pos_y = 0.0;
        [Order]
        public double PosY
        {
            get { return _pos_y; }
            set
            {
                _pos_y = value;
                OnPropertyChanged("PosY");
            }
        }

        private double _pos_z = 0.0;
        [Order]
        public double PosZ
        {
            get { return _pos_z; }
            set
            {
                _pos_z = value;
                OnPropertyChanged("PosZ");
            }
        }
  

        #region Template
        public struct CaseTemplateStructure
        {
            public Model.Model_Element ElementName;
            public double RangeStart;
            public double RangeEnd;
            public int Steps;

            public CaseTemplateStructure()
            {
                ElementName = new();
                RangeStart = 0;
                RangeEnd = 0;
                Steps = 0;
            }

            public CaseTemplateStructure(Model.Model_Element ElementN, double startRange, double endRange, int stepsTemplate )
            {
                ElementName = ElementN;
                RangeStart = startRange;
                RangeEnd = endRange;
                Steps = stepsTemplate;
            }
        }

        private List<CaseTemplateStructure> _caseTemplates = new List<CaseTemplateStructure>();
        public List<CaseTemplateStructure> CaseTemplates 
        { 
            get { return _caseTemplates; }
            set 
            { 
                _caseTemplates = value;
                OnPropertyChanged("CaseTemplate");
            }
        }

        private CaseTemplateStructure _selectedCaseTemplate = new();
        public CaseTemplateStructure SelectedCaseTemplate
        {
            get { return _selectedCaseTemplate; }
            set
            {
                _selectedCaseTemplate = value;
                OnPropertyChanged("SelectedCaseTemplate");
            }
        }

        public void Add_template(Model.Model_Element ElementN, double startRange, double endRange, int stepsTemplate) 
        {
            CaseTemplateStructure newTemplate = new(ElementN, startRange, endRange, stepsTemplate);
            CaseTemplates.Add(newTemplate);
        }
        #endregion

        #region Other

        private List<Model.Model_SelectedPhases> _SelectedPhases = new();
        public List<Model.Model_SelectedPhases> SelectedPhases
        {
            get { return _SelectedPhases; }
            set
            {
                _SelectedPhases = value;
                OnPropertyChanged("SelectedPhases");
            }
        }

        public void Add_selectedPhases(Model.Model_SelectedPhases model) 
        {
            _SelectedPhases.Add(model);
            OnPropertyChanged("SelectedPhases");
        }

        public void Clear_selectedPhases() 
        {
            _SelectedPhases.Clear();
            OnPropertyChanged("SelectedPhases");
        }

        List<Model.Model_ElementComposition> _elementComposition = new();
        public List<Model.Model_ElementComposition> ElementComposition
        {
            get { return _elementComposition; }
            set
            {
                _elementComposition = value;
                OnPropertyChanged("ElementComposition");
            }
        }

        List<Model.Model_EquilibriumPhaseFraction> _equilibriumPhaseFractions = new();
        public List<Model.Model_EquilibriumPhaseFraction> EquilibriumPhaseFractions
        {
            get { return _equilibriumPhaseFractions; }
            set
            {
                _equilibriumPhaseFractions = value;
                OnPropertyChanged("EquilibriumPhaseFractions");
            }
        }

        Model.Model_EquilibriumConfiguration _equilibriumConfiguration = new();
        public Model.Model_EquilibriumConfiguration EquilibriumConfiguration
        {
            get { return _equilibriumConfiguration; }
            set
            {
                _equilibriumConfiguration = value;
                OnPropertyChanged("EquilibriumConfiguration");
            }
        }

        List<Model.Model_ScheilPhaseFraction> _scheilPhaseFractions = new();
        public List<Model.Model_ScheilPhaseFraction> ScheilPhaseFractions
        {
            get { return _scheilPhaseFractions; }
            set
            {
                _scheilPhaseFractions = value;
                OnPropertyChanged("ScheilPhaseFractions");
            }
        }

        Model.Model_ScheilConfiguration _scheilConfiguration = new();
        public Model.Model_ScheilConfiguration ScheilConfiguration
        {
            get { return _scheilConfiguration; }
            set
            {
                _scheilConfiguration = value;
                OnPropertyChanged("ScheilConfiguration");
            }
        }

        List<Model.Model_PrecipitationPhase> _precipitationPhases = new();
        public List<Model.Model_PrecipitationPhase> PrecipitationPhases
        {
            get { return _precipitationPhases; }
            set 
            { 
                _precipitationPhases = value;
                OnPropertyChanged("PrecipitationPhases");
            }
        }

        List<Model.Model_PrecipitationDomain> _precipitationDomains = new();
        public List<Model.Model_PrecipitationDomain> PrecipitationDomains
        {
            get { return _precipitationDomains; }
            set
            {
                _precipitationDomains = value;
                OnPropertyChanged("PrecipitationDomains");
            }
        }

        List<Model.Model_HeatTreatment> _HeatTreatments = new();
        public List<Model.Model_HeatTreatment> HeatTreatments
        {
            get { return _HeatTreatments; }
            set
            {
                _HeatTreatments = value;
                OnPropertyChanged("HeatTreatments");
            }
        }

        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_Case>();
        }

        public override string Get_Table_Name()
        {
            return "Case";
        }
        #endregion

    }
}
