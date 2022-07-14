using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_PrecipitationDomain : Interfaces.Model_Interface
    {
        private int _ID = -1;
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        private string _name = "";
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
        public double ExcessVacancyEfficiency
        {
            get { return _excessVacancyEfficiency; }
            set
            {
                _excessVacancyEfficiency = value;
                OnPropertyChanged("ExcessVacancyEfficiency");
            }
        }

        public string Get_csv()
        {
            string outy = ID + "," +
                        Name + "," +
                        IDPhase + "," +
                        InitialGrainDiameter + "," +
                        EquilibriumDiDe + "," +
                        VacancyEvolutionModel + "," +
                        ConsiderExVa + "," +
                        ExcessVacancyEfficiency;
            return outy;
        }

        #region Other_properties
        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
