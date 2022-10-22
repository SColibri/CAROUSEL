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
    public class Model_SelectedElements : ModelAbstract
    {
        private int _ID = -1;
        [Order]
        public int ID 
        { 
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged(nameof(ID));
            } 
        }

        private int _IDProject = -1;
        [Order]
        public int IDProject
        {
            get { return _IDProject; }
            set
            {
                _IDProject = value;
                OnPropertyChanged(nameof(IDProject));
            }
        }

        private int _IDElement = -1;
        [Order]
        public int IDElement
        {
            get { return _IDElement; }
            set
            {
                _IDElement = value;
                OnPropertyChanged(nameof(IDElement));
            }
        }

        private int _isReferenceElement = -1;
        [Order]
        public int ISReferenceElement
        {
            get { return _isReferenceElement; }
            set
            {
                _isReferenceElement = value;

                if (value == 1) ISReferenceElementBool = true;
                else ISReferenceElementBool = false;

                OnPropertyChanged(nameof(ISReferenceElement));
            }
        }

        private string _ElementName = "";
        public string ElementName
        {
            get { return _ElementName; }
            set
            {
                _ElementName = value;
                OnPropertyChanged(nameof(ElementName));
            }
        }



        

        #region Other_properties

        private bool _isReferenceElement_bool = false;
        public bool ISReferenceElementBool
        {
            get { return _isReferenceElement_bool; }
            set
            {
                _isReferenceElement_bool = value;
                OnPropertyChanged(nameof(ISReferenceElementBool));
            }
        }
        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_SelectedElements>();
        }
        public override string Get_Table_Name()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
