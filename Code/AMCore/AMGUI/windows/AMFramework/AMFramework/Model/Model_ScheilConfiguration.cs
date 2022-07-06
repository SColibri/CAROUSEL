using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_ScheilConfiguration : INotifyPropertyChanged
    {
        private int _id = -1;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
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

        private double _startTemperature = -1;
        public double StartTemperature
        {
            get { return _startTemperature; }
            set
            {
                _startTemperature = value;
                OnPropertyChanged("StartTemperature");
            }
        }

        private double _endTemperature = -1;
        public double EndTemperature
        {
            get { return _endTemperature; }
            set
            {
                _endTemperature = value;
                OnPropertyChanged("EndTemperature");
            }
        }

        private string _temperatureType = "";
        public string TemperatureType
        {
            get { return _temperatureType; }
            set
            {
                _temperatureType = value;
                OnPropertyChanged("TemperatureType");
            }
        }

        private double _stepSize = -1;
        public double StepSize
        {
            get { return _stepSize; }
            set
            {
                _stepSize = value;
                OnPropertyChanged("StepSize");
            }
        }

        private int _dependentPhase = -1;
        public int DependentPhase
        {
            get { return _dependentPhase; }
            set
            {
                _dependentPhase = value;
                OnPropertyChanged("DependentPhase");
            }
        }

        private double _minLiquidFraction = -1;
        public double MinLiquidFraction
        {
            get { return _minLiquidFraction; }
            set
            {
                _minLiquidFraction = value;
                OnPropertyChanged("MinLiquidFraction");
            }
        }

        public string get_csv()
        {
            string outy = ID + "," + IDCase + "," + StartTemperature + "," + EndTemperature + "," + StepSize + "," + DependentPhase + "," + MinLiquidFraction;
            return outy;
        }

        #region Other
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
