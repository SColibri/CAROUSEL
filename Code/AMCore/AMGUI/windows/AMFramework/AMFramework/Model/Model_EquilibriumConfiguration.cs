using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_EquilibriumConfiguration : INotifyPropertyChanged
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

        private double _Temperature = -1;
        public double Temperature
        {
            get { return _Temperature; }
            set
            {
                _Temperature = value;
                OnPropertyChanged("Temperature");
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

        private double _pressure = -1;
        public double Pressure
        {
            get { return _stepSize; }
            set
            {
                _stepSize = value;
                OnPropertyChanged("Pressure");
            }
        }

        public string get_csv()
        {
            string outy = ID + "," + IDCase + "," + Temperature + "," + StartTemperature + "," + EndTemperature + "," + TemperatureType + "," + StepSize + "," + Pressure;
            return outy;
        }

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
