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
    public class Model_ScheilConfiguration : ModelAbstract
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

        private string _temperatureType = "";
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

        private int _dependentPhase = -1;
        [Order]
        public int DependentPhase
        {
            get { return _dependentPhase; }
            set
            {
                _dependentPhase = value;
                OnPropertyChanged(nameof(DependentPhase));
            }
        }

        private double _minLiquidFraction = -1;
        [Order]
        public double MinLiquidFraction
        {
            get { return _minLiquidFraction; }
            set
            {
                _minLiquidFraction = value;
                OnPropertyChanged(nameof(MinLiquidFraction));
            }
        }


        #region Other
       
        private string _dependentPhaseName = "";
        public string DependentPhaseName
        {
            get { return _dependentPhaseName; }
            set
            {
                _dependentPhaseName = value;
                OnPropertyChanged(nameof(DependentPhaseName));
            }
        }
        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_ScheilConfiguration>();
        }
        public override string Get_Table_Name()
        {
            return "ScheilConfiguration";
        }
        public override string Get_Scripting_ClassName()
        {
            return "ScheilConfig";
        }
        #endregion
    }
}
