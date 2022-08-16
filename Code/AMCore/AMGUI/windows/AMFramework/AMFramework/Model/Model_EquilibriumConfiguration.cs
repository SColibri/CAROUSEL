﻿using System;
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

        private double _Temperature = -1;
        [Order]
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
        [Order]
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
        [Order]
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
        [Order]
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
        [Order]
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
        [Order]
        public double Pressure
        {
            get { return _stepSize; }
            set
            {
                _stepSize = value;
                OnPropertyChanged("Pressure");
            }
        }

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_EquilibriumConfiguration>();
        }

        public override string Get_save_command()
        {
            throw new NotImplementedException();
        }

        public override string Get_load_command()
        {
            throw new NotImplementedException();
        }

        public override string Get_load_command_table(Model_Interface.SEARCH findType)
        {
            throw new NotImplementedException();
        }

        public override string Get_delete_command()
        {
            throw new NotImplementedException();
        }

        public override string Get_Table_Name()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
