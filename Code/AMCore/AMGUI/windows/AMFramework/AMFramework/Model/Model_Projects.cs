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
    public class Model_Projects : ModelAbstract
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

        private string _name = "New Name";
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


        private string _apiName = "";
        [Order]
        public string APIName
        {
            get { return _apiName; }
            set 
            {
                _apiName = value;
                OnPropertyChanged("APIName");
            }
        }

        #region Other_properties

        #endregion

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_Projects>();
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
