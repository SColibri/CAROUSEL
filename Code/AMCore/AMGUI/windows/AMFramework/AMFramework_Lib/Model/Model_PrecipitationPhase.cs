using AMFramework_Lib.AMSystem.Attributes;

namespace AMFramework_Lib.Model
{
    public class Model_PrecipitationPhase : ModelAbstract
    {
        private int _ID = -1;
        [Order]
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private int _IDCase = -1;
        [Order]
        public int IDCase
        {
            get { return _IDCase; }
            set
            {
                _IDCase = value;
                OnPropertyChanged(nameof(IDCase));
            }
        }

        private int _IDPhase = -1;
        [Order]
        public int IDPhase
        {
            get { return _IDPhase; }
            set
            {
                _IDPhase = value;
                OnPropertyChanged(nameof(IDPhase));
            }
        }

        private int _NumberSizeClasses = -1;
        [Order]
        public int NumberSizeClasses
        {
            get { return _NumberSizeClasses; }
            set
            {
                _NumberSizeClasses = value;
                OnPropertyChanged(nameof(NumberSizeClasses));
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

        private string _nucleationSites = "";
        [Order]
        public string NucleationSites
        {
            get { return _nucleationSites; }
            set
            {
                _nucleationSites = value;
                OnPropertyChanged(nameof(NucleationSites));
            }
        }

        private int _IDPrecipitationDomain = -1;
        [Order]
        public int IDPrecipitationDomain
        {
            get { return _IDPrecipitationDomain; }
            set
            {
                _IDPrecipitationDomain = value;
                OnPropertyChanged(nameof(IDPrecipitationDomain));
            }
        }

        private string _calcType = "";
        [Order]
        public string CalcType
        {
            get { return _calcType; }
            set
            {
                _calcType = value;
                OnPropertyChanged(nameof(CalcType));
            }
        }

        private double _minRadius = 0.000001;
        [Order]
        public double MinRadius
        {
            get { return _minRadius; }
            set
            {
                _minRadius = value;
                OnPropertyChanged(nameof(MinRadius));
            }
        }

        private double _meanRadius = 0.000002;
        [Order]
        public double MeanRadius
        {
            get { return _meanRadius; }
            set
            {
                _meanRadius = value;
                OnPropertyChanged(nameof(MeanRadius));
            }
        }

        private double _maxRadius = 0.000001;
        [Order]
        public double MaxRadius
        {
            get { return _maxRadius; }
            set
            {
                _maxRadius = value;
                OnPropertyChanged(nameof(MaxRadius));
            }
        }

        private double _stdDev = 0.05;
        [Order]
        public double StdDev
        {
            get { return _stdDev; }
            set
            {
                _stdDev = value;
                OnPropertyChanged(nameof(StdDev));
            }
        }

        private string _precipitateDistribution = "";
        [Order]
        public string PrecipitateDistribution
        {
            get { return _precipitateDistribution; }
            set
            {
                _precipitateDistribution = value;
                OnPropertyChanged(nameof(PrecipitateDistribution));
            }
        }

        #region Other_properties

        private string _phaseName = "";
        public string PhaseName
        {
            get { return _phaseName; }
            set
            {
                _phaseName = value;
                OnPropertyChanged(nameof(PhaseName));
            }
        }

        private string _domainName = "";
        public string DomainName
        {
            get { return _domainName; }
            set
            {
                _domainName = value;
                OnPropertyChanged(nameof(DomainName));
            }
        }
        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_PrecipitationPhase>();
        }
        public override string Get_Table_Name()
        {
            return "PrecipitationPhase";
        }

        public override string Get_Scripting_ClassName()
        {
            return "PrecipitationPhase";
        }
        #endregion
    }
}
