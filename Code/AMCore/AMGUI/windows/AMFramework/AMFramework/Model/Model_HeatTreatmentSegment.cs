using AMFramework.AMSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model
{
    class Model_HeatTreatmentSegment : ModelAbstract
    {
        private int _id = -1;
        [Order]
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
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
                OnPropertyChanged("StepIndex");
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
                OnPropertyChanged("IDHeatTreatment");
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
                OnPropertyChanged("IDPrecipitationDomain");
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
                OnPropertyChanged("EndTemperature");
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
