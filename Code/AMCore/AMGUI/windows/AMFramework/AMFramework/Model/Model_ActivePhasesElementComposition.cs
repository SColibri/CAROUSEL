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
    public class Model_ActivePhasesElementComposition : ModelAbstract
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

        private int _idProject = -1;
        [Order]
        public int IDProject
        {
            get { return _idProject; }
            set
            {
                _idProject = value;
                OnPropertyChanged("IDProject");
            }
        }

        private int _idElement = -1;
        [Order]
        public int IDElement
        {
            get { return _idElement; }
            set
            {
                _idElement = value;
                OnPropertyChanged("IDElement");
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
        private string _elementName = "";
        public string ElementName 
        { 
            get { return _elementName; }
            set 
            { 
                _elementName = value;
                OnPropertyChanged("ElementName");
            }
        }
        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_ActivePhasesElementComposition>();
        }

        public override string Get_Table_Name()
        {
            return "ActivePhases_ElementComposition";
        }
        #endregion
    }
}
