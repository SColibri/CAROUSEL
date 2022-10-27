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
    public class Model_EquilibriumConfiguration : ModelAbstract
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

        private double _Temperature = -1;
        [Order]
        public double Temperature
        {
            get { return _Temperature; }
            set
            {
                _Temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private double _startTemperature = -1;
        [Order]
        public double StartTemperature
        {
            get { return _startTemperature; }
            set
            {
                _startTemperature = value;
                OnPropertyChanged(nameof(StartTemperature));
            }
        }

        private double _endTemperature = -1;
        [Order]
        public double EndTemperature
        {
            get { return _endTemperature; }
            set
            {
                _endTemperature = value;
                OnPropertyChanged(nameof(EndTemperature));
            }
        }

        private string _temperatureType = "°C";
        [Order]
        public string TemperatureType
        {
            get { return _temperatureType; }
            set
            {
                _temperatureType = value;
                OnPropertyChanged(nameof(TemperatureType));
            }
        }

        private double _stepSize = -1;
        [Order]
        public double StepSize
        {
            get { return _stepSize; }
            set
            {
                _stepSize = value;
                OnPropertyChanged(nameof(StepSize));
            }
        }

        private double _pressure = -1;
        [Order]
        public double Pressure
        {
            get { return _pressure; }
            set
            {
                _pressure = value;
                OnPropertyChanged(nameof(Pressure));
            }
        }

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_EquilibriumConfiguration>();
        }
        public override string Get_Table_Name()
        {
            return "EquilibriumConfiguration";
        }

        public override string Get_Scripting_ClassName()
        {
            return "EquilibriumConfig";
        }
        #endregion
    }
}
