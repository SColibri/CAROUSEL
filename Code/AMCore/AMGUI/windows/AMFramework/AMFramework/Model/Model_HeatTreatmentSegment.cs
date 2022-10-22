using AMFramework.AMSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model
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


        #region ImplementsModelAbstract
        public override string Get_Table_Name()
            {
                throw new NotImplementedException();
            }
        #endregion
    }
}
