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
                OnPropertyChanged("ID");
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
                OnPropertyChanged("IDProject");
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
                OnPropertyChanged("IDElement");
            }
        }

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

                OnPropertyChanged("ISReferenceElement");
            }
        }

        #region Other_properties
        private bool _isSelected = false;
        [Order]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private bool _isReferenceElement_bool = false;
        [Order]
        public bool ISReferenceElementBool
        {
            get { return _isReferenceElement_bool; }
            set
            {
                _isReferenceElement_bool = value;
                OnPropertyChanged("ISReferenceElementBool");
            }
        }
        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_SelectedElements>();
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
