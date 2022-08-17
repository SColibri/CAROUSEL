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
    public class Model_ElementComposition : ModelAbstract
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

        private int _id_case = -1;
        [Order]
        public int IDCase
        {
            get { return _id_case; }
            set
            {
                _id_case = value;
                OnPropertyChanged("IDCase");
            }
        }

        private int _id_element = -1;
        [Order]
        public int IDElement
        {
            get { return _id_element; }
            set
            {
                _id_element = value;
                OnPropertyChanged("IDElement");
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
        private string _ElementName = "";
        [Order]
        public string ElementName
        {
            get { return _ElementName; }
            set
            {
                _ElementName = value;
                OnPropertyChanged("ElementName");
            }
        }
        #endregion


        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_ElementComposition>();
        }

        public override string Get_Table_Name()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
