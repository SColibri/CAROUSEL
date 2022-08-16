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
    public class Model_EquilibriumPhaseFraction : ModelAbstract
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

        private int _IDPhase = -1;
        [Order]
        public int IDPhase
        {
            get { return _IDPhase; }
            set
            {
                _IDPhase = value;
                OnPropertyChanged("IDPhase");
            }
        }

        private string _typeComposition = "";
        [Order]
        public string TypeComposition
        {
            get { return _typeComposition; }
            set
            {
                _typeComposition = value;
                OnPropertyChanged("TypeComposition");
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

        private double _value = -1;
        [Order]
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        #region Other
        private string _phaseName = "";
        public string PhaseName
        {
            get { return _phaseName; }
            set
            {
                _phaseName = value;
                OnPropertyChanged("PhaseName");
            }
        }
        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_EquilibriumPhaseFraction>();
        }

        public override int Load_csv(List<string> DataRaw)
        {
            throw new NotImplementedException();
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
