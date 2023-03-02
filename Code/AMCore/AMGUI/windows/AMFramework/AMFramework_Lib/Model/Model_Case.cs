using AMFramework_Lib.AMSystem.Attributes;

namespace AMFramework_Lib.Model
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
                OnPropertyChanged(nameof(ID));
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
                OnPropertyChanged(nameof(IDProject));
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
                OnPropertyChanged(nameof(IDGroup));
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
                OnPropertyChanged(nameof(Name));
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
                OnPropertyChanged(nameof(Script));
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
                OnPropertyChanged(nameof(Date));
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
                OnPropertyChanged(nameof(PosX));
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
                OnPropertyChanged(nameof(PosY));
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
                OnPropertyChanged(nameof(PosZ));
            }
        }


        #region Template
        // This is deprecated -- REMOVE AL REFERENCES
        [Obsolete("Templates are now used on the templated parameters in combination with ControllerM -> Save_Templated_Object")]
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

            public CaseTemplateStructure(Model.Model_Element ElementN, double startRange, double endRange, int stepsTemplate)
            {
                ElementName = ElementN;
                RangeStart = startRange;
                RangeEnd = endRange;
                Steps = stepsTemplate;
            }
        }

        private List<CaseTemplateStructure> _caseTemplates = new();
        [Obsolete("Templates are now used on the templated parameters in combination with ControllerM -> Save_Templated_Object")]
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
        [Obsolete("Templates are now used on the templated parameters in combination with ControllerM -> Save_Templated_Object")]
        public CaseTemplateStructure SelectedCaseTemplate
        {
            get { return _selectedCaseTemplate; }
            set
            {
                _selectedCaseTemplate = value;
                OnPropertyChanged(nameof(SelectedCaseTemplate));
            }
        }
        [Obsolete("Templates are now used on the templated parameters in combination with ControllerM -> Save_Templated_Object")]
        public void Add_template(Model.Model_Element ElementN, double startRange, double endRange, int stepsTemplate)
        {
            CaseTemplateStructure newTemplate = new(ElementN, startRange, endRange, stepsTemplate);
            CaseTemplates.Add(newTemplate);
        }
        #endregion

        #region Extended_Parameters

        private List<ModelController<Model_SelectedPhases>> _selectedPhases = new();
        public List<ModelController<Model_SelectedPhases>> SelectedPhases
        {
            get { return _selectedPhases; }
            set
            {
                _selectedPhases = value;
                OnPropertyChanged(nameof(SelectedPhases));
            }
        }

        private List<ModelController<Model_ElementComposition>> _elementComposition = new();
        public List<ModelController<Model_ElementComposition>> ElementComposition
        {
            get { return _elementComposition; }
            set
            {
                _elementComposition = value;
                OnPropertyChanged(nameof(ElementComposition));
            }
        }

        private List<ModelController<Model_EquilibriumPhaseFraction>> _equilibriumPhaseFractions = new();
        public List<ModelController<Model_EquilibriumPhaseFraction>> EquilibriumPhaseFractions
        {
            get { return _equilibriumPhaseFractions; }
            set
            {
                _equilibriumPhaseFractions = value;
                OnPropertyChanged(nameof(EquilibriumPhaseFractions));
            }
        }

        private ModelController<Model_EquilibriumConfiguration>? _equilibriumConfiguration;
        public ModelController<Model_EquilibriumConfiguration>? EquilibriumConfiguration
        {
            get { return _equilibriumConfiguration; }
            set
            {
                _equilibriumConfiguration = value;
                OnPropertyChanged(nameof(EquilibriumConfiguration));
            }
        }

        private List<ModelController<Model_ScheilPhaseFraction>> _scheilPhaseFractions = new();
        public List<ModelController<Model_ScheilPhaseFraction>> ScheilPhaseFractions
        {
            get { return _scheilPhaseFractions; }
            set
            {
                _scheilPhaseFractions = value;
                OnPropertyChanged(nameof(ScheilPhaseFractions));
            }
        }

        private ModelController<Model_ScheilConfiguration>? _scheilConfiguration;
        public ModelController<Model_ScheilConfiguration>? ScheilConfiguration
        {
            get { return _scheilConfiguration; }
            set
            {
                _scheilConfiguration = value;
                OnPropertyChanged(nameof(ScheilConfiguration));
            }
        }

        private List<ModelController<Model_PrecipitationPhase>> _precipitationPhases = new();
        public List<ModelController<Model_PrecipitationPhase>> PrecipitationPhases
        {
            get { return _precipitationPhases; }
            set
            {
                _precipitationPhases = value;
                OnPropertyChanged(nameof(PrecipitationPhases));
            }
        }

        private List<ModelController<Model_PrecipitationDomain>> _precipitationDomains = new();
        public List<ModelController<Model_PrecipitationDomain>> PrecipitationDomains
        {
            get { return _precipitationDomains; }
            set
            {
                _precipitationDomains = value;
                OnPropertyChanged(nameof(PrecipitationDomains));
            }
        }

        private List<ModelController<Model_HeatTreatment>> _HeatTreatments = new();
        public List<ModelController<Model_HeatTreatment>> HeatTreatments
        {
            get { return _HeatTreatments; }
            set
            {
                _HeatTreatments = value;
                OnPropertyChanged(nameof(HeatTreatments));
            }
        }


        #region Deprecated

        private List<Model.Model_SelectedPhases> _SelectedPhasesOLD = new();
        public List<Model.Model_SelectedPhases> SelectedPhasesOLD
        {
            get { return _SelectedPhasesOLD; }
            set
            {
                _SelectedPhasesOLD = value;
                OnPropertyChanged(nameof(SelectedPhasesOLD));
            }
        }

        public void Add_selectedPhasesOLD(Model.Model_SelectedPhases model)
        {
            _SelectedPhasesOLD.Add(model);
            OnPropertyChanged(nameof(SelectedPhasesOLD));
        }

        public void Clear_selectedPhasesOLD()
        {
            _SelectedPhasesOLD.Clear();
            OnPropertyChanged(nameof(SelectedPhasesOLD));
        }

        List<Model.Model_ElementComposition> _elementCompositionOLD = new();
        public List<Model.Model_ElementComposition> ElementCompositionOLD
        {
            get { return _elementCompositionOLD; }
            set
            {
                _elementCompositionOLD = value;
                OnPropertyChanged(nameof(ElementCompositionOLD));
            }
        }

        List<Model.Model_EquilibriumPhaseFraction> _equilibriumPhaseFractionsOLD = new();
        public List<Model.Model_EquilibriumPhaseFraction> EquilibriumPhaseFractionsOLD
        {
            get { return _equilibriumPhaseFractionsOLD; }
            set
            {
                _equilibriumPhaseFractionsOLD = value;
                OnPropertyChanged(nameof(EquilibriumPhaseFractionsOLD));
            }
        }

        Model.Model_EquilibriumConfiguration _equilibriumConfigurationOLD = new();
        public Model.Model_EquilibriumConfiguration EquilibriumConfigurationOLD
        {
            get { return _equilibriumConfigurationOLD; }
            set
            {
                _equilibriumConfigurationOLD = value;
                OnPropertyChanged(nameof(EquilibriumConfigurationOLD));
            }
        }

        List<Model.Model_ScheilPhaseFraction> _scheilPhaseFractionsOLD = new();
        public List<Model.Model_ScheilPhaseFraction> ScheilPhaseFractionsOLD
        {
            get { return _scheilPhaseFractionsOLD; }
            set
            {
                _scheilPhaseFractionsOLD = value;
                OnPropertyChanged(nameof(ScheilPhaseFractions));
            }
        }

        Model.Model_ScheilConfiguration _scheilConfigurationOLD = new();
        public Model.Model_ScheilConfiguration ScheilConfigurationOLD
        {
            get { return _scheilConfigurationOLD; }
            set
            {
                _scheilConfigurationOLD = value;
                OnPropertyChanged(nameof(ScheilConfiguration));
            }
        }

        List<Model.Model_PrecipitationPhase> _precipitationPhasesOLD = new();
        public List<Model.Model_PrecipitationPhase> PrecipitationPhasesOLD
        {
            get { return _precipitationPhasesOLD; }
            set
            {
                _precipitationPhasesOLD = value;
                OnPropertyChanged(nameof(PrecipitationPhases));
            }
        }

        List<Model.Model_PrecipitationDomain> _precipitationDomainsOLD = new();
        public List<Model.Model_PrecipitationDomain> PrecipitationDomainsOLD
        {
            get { return _precipitationDomainsOLD; }
            set
            {
                _precipitationDomainsOLD = value;
                OnPropertyChanged(nameof(PrecipitationDomains));
            }
        }

        List<Model.Model_HeatTreatment> _HeatTreatmentsOLD = new();
        public List<Model.Model_HeatTreatment> HeatTreatmentsOLD
        {
            get { return _HeatTreatmentsOLD; }
            set
            {
                _HeatTreatmentsOLD = value;
                OnPropertyChanged(nameof(HeatTreatments));
            }
        }

        #endregion
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

        public override string Get_Scripting_ClassName()
        {
            return "Case";
        }
        #endregion

    }
}
