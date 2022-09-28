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
    public class Model_HeatTreatment : ModelAbstract
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

        private int _idCase = -1;
        [Order]
        public int IDCase
        {
            get { return _idCase; }
            set
            {
                _idCase = value;
                OnPropertyChanged("IDCase");
            }
        }

        private string _name = "";
        [Order]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private int _maxTemperatureStep = 10;
        [Order]
        public int MaxTemperatureStep
        {
            get { return _maxTemperatureStep; }
            set
            {
                _maxTemperatureStep = value;
                OnPropertyChanged("MaxTemperatureStep");
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


        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_HeatTreatment>();
        }

        public override string Get_Table_Name()
        {
            throw new NotImplementedException();
        }
        #endregion

        List<Model.Model_HeatTreatmentProfile> _HeatTreatmentProfile = new();
        public List<Model.Model_HeatTreatmentProfile> HeatTreatmentProfile
        {
            get { return _HeatTreatmentProfile; }
            set
            {
                _HeatTreatmentProfile = value;
                OnPropertyChanged("HeatTreatmentProfile");
            }
        }

        List<Model.Model_PrecipitateSimulationData> _precipitationData = new();
        public List<Model.Model_PrecipitateSimulationData> PrecipitationData
        {
            get { return _precipitationData; }
            set
            {
                _precipitationData = value;
                OnPropertyChanged("PrecipitationData");
            }
        }

    }
}
