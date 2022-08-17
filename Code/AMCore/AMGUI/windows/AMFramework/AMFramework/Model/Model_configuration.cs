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
    public class Model_configuration : ModelAbstract
    {
        private string _API_path = "Not defined";
        [Order]
        public string API_path
        {
            get { return _API_path; }
            set
            {
                _API_path = value;

                if (_is_loaded) 
                {
                    Controller.Controller_Config.ApiHandle.update_path(_API_path);
                }
                
                OnPropertyChanged("API_path");
            }
        }

        private string _external_API_path = "Not defined external API";
        [Order]
        public string External_API_path
        {
            get { return _external_API_path; }
            set
            {
                _external_API_path = value;
                OnPropertyChanged("External_API_path");
            }
        }

        private string _working_directory = "Not defined working directory";
        [Order]
        public string Working_Directory
        {
            get { return _working_directory; }
            set
            {
                _working_directory = value;
                OnPropertyChanged("Working_Directory");
            }
        }

        private string _thermodynamic_database_path = "Not defined thermodynamic database";
        [Order]
        public string Thermodynamic_database_path
        {
            get { return _thermodynamic_database_path; }
            set
            {
                _thermodynamic_database_path = value;
                OnPropertyChanged("Thermodynamic_database_path");
            }
        }

        private string _physical_database_path = "Not defined physical database";
        [Order]
        public string Physical_database_path
        {
            get { return _physical_database_path; }
            set
            {
                _physical_database_path = value;
                OnPropertyChanged("Physical_database_path");
            }
        }

        private string _mobility_database_path = "Not defined mobility";
        [Order]
        public string Mobility_database_path
        {
            get { return _mobility_database_path; }
            set
            {
                _mobility_database_path = value;
                OnPropertyChanged("Mobility_database_path");
            }
        }

        private bool _is_loaded = false;
        [Order]
        public bool IsLoaded
        {
            get { return _is_loaded; }
            set 
            {
                _is_loaded = value;
                OnPropertyChanged("IsLoaded");
            }
        }

        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_configuration>();
        }

        // for configuretion we do not store in a datatable
        public override string Get_Table_Name()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
