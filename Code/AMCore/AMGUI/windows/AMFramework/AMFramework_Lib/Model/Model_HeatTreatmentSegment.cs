using AMFramework_Lib.AMSystem.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model
{
    public class Model_HeatTreatmentSegment : ModelAbstract
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

        private int _stepIndex = -1;
        [Order]
        public int StepIndex
        {
            get { return _stepIndex; }
            set
            {
                _stepIndex = value;
                OnPropertyChanged(nameof(StepIndex));
            }
        }

        private int _idHeatTreatment = -1;
        [Order]
        public int IDHeatTreatment
        {
            get { return _idHeatTreatment; }
            set
            {
                _idHeatTreatment = value;
                OnPropertyChanged(nameof(IDHeatTreatment));
            }
        }

        private int _idPrecipitationDomain = -1;
        [Order]
        public int IDPrecipitationDomain
        {
            get { return _idPrecipitationDomain; }
            set
            {
                _idPrecipitationDomain = value;
                OnPropertyChanged(nameof(IDPrecipitationDomain));
            }
        }

        private double _endTemperature = 0;
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

        private double _temperatureGradient = 0;
        [Order]
        public double TemperatureGradient
        {
            get { return _temperatureGradient; }
            set
            {
                _temperatureGradient = value;
                if (value >= 0) SelectedModeType = ModeType.TEMPERATURE_GRADIENT;
                OnPropertyChanged(nameof(TemperatureGradient));
            }
        }

        private double _duration = 0;
        [Order]
        public double Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                if(value >= 0) SelectedModeType = ModeType.TIME_INTERVAL;
                OnPropertyChanged(nameof(Duration));
            }
        }

        #region Other
        public enum ModeType
        {
            [Description("No selection (default)")]
            NONE,
            [Description("Time interval (delta t)")]
            TIME_INTERVAL,
            [Description("Cooling rate (C°/s)")]
            TEMPERATURE_GRADIENT
        }

        private ModeType _selectedModeType = ModeType.NONE;
        public ModeType SelectedModeType
        {
            get { return _selectedModeType; }
            set 
            {
                _selectedModeType = value;
                OnPropertyChanged(nameof(SelectedModeType));
            }
        }

        private double _inputModeValue;
        public double InputModeValue
        {
            get { return _inputModeValue; }
            set
            {
                _inputModeValue = value;

                TemperatureGradient = 0;
                Duration = 0;

                switch (SelectedModeType)
                {
                    case ModeType.TIME_INTERVAL:
                        Duration = _inputModeValue;
                        break;
                    case ModeType.TEMPERATURE_GRADIENT:
                        TemperatureGradient = _inputModeValue;
                        break;
                    default:
                        break;
                }

                OnPropertyChanged(nameof(InputModeValue));
            }
        }


        public IEnumerable<ModeType> ModeTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(ModeType))
                    .Cast<ModeType>();
            }
        }

        private string _templateValue = "0";
        /// <summary>
        /// Template value is used for indicating ranges and templated objects for the
        /// Duration or cooling/heating rate.
        /// </summary>
        public string TemplateValue
        {
            get { return _templateValue; }
            set
            {
                _templateValue = value;
                OnPropertyChanged(nameof(TemplateValue));
            }
        }


        private string _templatedEndTemperature = "0";
        /// <summary>
        /// Templated value is used for indicating ranges for the end temperature value.
        /// </summary>
        public string TemplatedEndTemperature
        {
            get { return _templatedEndTemperature; }
            set
            {
                _templatedEndTemperature = value;
                OnPropertyChanged(nameof(TemplatedEndTemperature));
            }
        }
        #endregion

        #region ImplementsModelAbstract
        public override string Get_Table_Name()
        {
            return "HeatTreatmentSegment";
        }

        public override string Get_Scripting_ClassName()
        {
            return "HeatTreatmentSegment";
        }
        #endregion
    }
}
