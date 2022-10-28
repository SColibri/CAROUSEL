using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.AMSystem.Attributes;

namespace AMFramework_Lib.Model
{
    public class Model_PrecipitateSimulationData : ModelAbstract
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

        private int _idPrecipitationPhase = -1;
        [Order]
        public int IDPrecipitationPhase
        {
            get { return _idPrecipitationPhase; }
            set
            {
                _idPrecipitationPhase = value;
                OnPropertyChanged(nameof(IDPrecipitationPhase));
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

        private double _time = -1;
        [Order]
        public double Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private double _phaseFraction = -1;
        [Order]
        public double PhaseFraction
        {
            get { return _phaseFraction; }
            set
            {
                _phaseFraction = value;
                OnPropertyChanged(nameof(PhaseFraction));
            }
        }

        private double _numberDensity = -1;
        [Order]
        public double NumberDensity
        {
            get { return _numberDensity; }
            set
            {
                _numberDensity = value;
                OnPropertyChanged(nameof(NumberDensity));
            }
        }

        private double _meanRadius = -1;
        [Order]
        public double MeanRadius
        {
            get { return _meanRadius; }
            set
            {
                _meanRadius = value;
                OnPropertyChanged(nameof(MeanRadius));
            }
        }

        #region Other

        private string _precipitationName = "";
        public string PrecipitationName
        {
            get { return _precipitationName; }
            set
            {
                _precipitationName = value;
                OnPropertyChanged(nameof(PrecipitationName));
            }
        }

        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_PrecipitateSimulationData>();
        }

        public override string Get_Table_Name()
        {
            return "PrecipitateSimulationData";
        }

        public override string Get_Scripting_ClassName()
        {
            return "PrecipitateSimulationData";
        }
        #endregion
    }
}
