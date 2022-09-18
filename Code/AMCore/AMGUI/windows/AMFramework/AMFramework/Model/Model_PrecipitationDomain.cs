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
                OnPropertyChanged("ID");
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
                OnPropertyChanged("IDCase");
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

        private int _IDPhase = -1;
        [Order]
        public int IDPhase
        {
            get { return _IDPhase; }
            set
            {
                _IDPhase = value;
                OnPropertyChanged("IDPhase");
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
                OnPropertyChanged("InitialGrainDiameter");
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
                OnPropertyChanged("EquilibriumDiDe");
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
                OnPropertyChanged("VacancyEvolutionModel");
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
                OnPropertyChanged("ConsiderExVa");
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
                OnPropertyChanged("ExcessVacancyEfficiency");
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
            throw new NotImplementedException();
        }
        #endregion
    }
}
