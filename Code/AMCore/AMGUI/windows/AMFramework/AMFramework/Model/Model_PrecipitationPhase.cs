using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_PrecipitationPhase : Interfaces.Model_Interface
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

        private int _IDCase = -1;
        public int IDCase
        {
            get { return _IDCase; }
            set
            {
                _IDCase = value;
                OnPropertyChanged("IDCase");
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

        private int _NumberSizeClasses = -1;
        public int NumberSizeClasses
        {
            get { return _NumberSizeClasses; }
            set
            {
                _NumberSizeClasses = value;
                OnPropertyChanged("NumberSizeClasses");
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

        private string _nucleationSites = "";
        public string NucleationSites
        {
            get { return _nucleationSites; }
            set
            {
                _nucleationSites = value;
                OnPropertyChanged("NucleationSites");
            }
        }

        private int _IDPrecipitationDomain = -1;
        public int IDPrecipitationDomain
        {
            get { return _IDPrecipitationDomain; }
            set
            {
                _IDPrecipitationDomain = value;
                OnPropertyChanged("IDPrecipitationDomain");
            }
        }

        private string _calcType = "";
        public string CalcType
        {
            get { return _calcType; }
            set
            {
                _calcType = value;
                OnPropertyChanged("CalcType");
            }
        }

        private double _minRadius = 0.000001;
        public double MinRadius
        {
            get { return _minRadius; }
            set
            {
                _minRadius = value;
                OnPropertyChanged("MinRadius");
            }
        }

        private double _meanRadius = 0.000002;
        public double MeanRadius
        {
            get { return _meanRadius; }
            set
            {
                _meanRadius = value;
                OnPropertyChanged("MeanRadius");
            }
        }

        private double _maxRadius = 0.000001;
        public double MaxRadius
        {
            get { return _maxRadius; }
            set
            {
                _maxRadius = value;
                OnPropertyChanged("MaxRadius");
            }
        }

        private double _stdDev = 0.05;
        public double StdDev
        {
            get { return _stdDev; }
            set
            {
                _stdDev = value;
                OnPropertyChanged("StdDev");
            }
        }

        private string _precipitateDistribution = "";
        public string PrecipitateDistribution
        {
            get { return _precipitateDistribution; }
            set
            {
                _precipitateDistribution = value;
                OnPropertyChanged("PrecipitateDistribution");
            }
        }

        public string Get_csv()
        {
            string outy = ID + "," +
                IDCase + "," +
                IDPhase + "," +
                NumberSizeClasses + "," +
                Name + "," +
                NucleationSites + "," +
                IDPrecipitationDomain + "," +
                CalcType + "," +
                MinRadius + "," +
                MeanRadius + "," +
                MaxRadius + "," +
                StdDev + "," +
                PrecipitateDistribution;
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

        private string _phaseName = "";
        public string PhaseName 
        { 
            get { return _phaseName; } 
            set 
            { 
                _phaseName = value;
                OnPropertyChanged("PhaseName");
            }
        }

        private string _domainName = "";
        public string DomainName
        {
            get { return _domainName; }
            set
            {
                _domainName = value;
                OnPropertyChanged("DomainName");
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
