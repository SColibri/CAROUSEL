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
    public class Model_Element : ModelAbstract
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

        private string _name = "";
        [Order]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        #region Other_parameters
        private bool _isReferenceElement = false;
        public bool IsReferenceElement
        {
            get { return _isReferenceElement; }
            set
            {
                _isReferenceElement = value;
                OnPropertyChanged(nameof(IsReferenceElement));
            }
        }
        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list() 
        {
            return ModelAbstract.Get_parameters<Model_Element>();
        }

        public override string Get_Table_Name()
        {
            return "Element";
        }

        public override string Get_Scripting_ClassName()
        {
            return "Element";
        }
        #endregion

    }
}
