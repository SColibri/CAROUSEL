using AMFramework_Lib.AMSystem.Attributes;

namespace AMFramework_Lib.Model
{
    public class Model_PrecipitationDomain : ModelAbstract
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

        private double _initialGrainDiameter = 0.00005;
        [Order]
        public double InitialGrainDiameter
        {
            get { return _initialGrainDiameter; }
            set
            {
                _initialGrainDiameter = value;
                OnPropertyChanged(nameof(InitialGrainDiameter));
            }
        }

        private double _equilibriumDiDe = 100000000000;
        [Order]
        public double EquilibriumDiDe
        {
            get { return _equilibriumDiDe; }
            set
            {
                _equilibriumDiDe = value;
                OnPropertyChanged(nameof(EquilibriumDiDe));
            }
        }

        private string _vacancyEvolutionModel = "";
        [Order]
        public string VacancyEvolutionModel
        {
            get { return _vacancyEvolutionModel; }
            set
            {
                _vacancyEvolutionModel = value;
                OnPropertyChanged(nameof(VacancyEvolutionModel));
            }
        }

        private int _considerExVa = 0;
        [Order]
        public int ConsiderExVa
        {
            get { return _considerExVa; }
            set
            {
                _considerExVa = value;
                OnPropertyChanged(nameof(ConsiderExVa));
            }
        }

        private double _excessVacancyEfficiency = 0.0;
        [Order]
        public double ExcessVacancyEfficiency
        {
            get { return _excessVacancyEfficiency; }
            set
            {
                _excessVacancyEfficiency = value;
                OnPropertyChanged(nameof(ExcessVacancyEfficiency));
            }
        }



        #region Other_properties

        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_PrecipitationDomain>();
        }

        public override string Get_Table_Name()
        {
            return "PrecipitationDomain";
        }

        public override string Get_Scripting_ClassName()
        {
            return "PrecipitationDomain";
        }
        #endregion
    }
}
