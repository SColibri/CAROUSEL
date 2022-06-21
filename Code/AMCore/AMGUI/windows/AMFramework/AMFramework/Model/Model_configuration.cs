using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_configuration : INotifyPropertyChanged
    {
        private string _API_path = "Not defined";
        public string API_path
        {
            get { return _API_path; }
            set
            {
                _API_path = value;
                OnPropertyChanged("API_path");
            }
        }

        private string _external_API_path = "Not defined external API";
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
        public string Mobility_database_path
        {
            get { return _mobility_database_path; }
            set
            {
                _mobility_database_path = value;
                OnPropertyChanged("Mobility_database_path");
            }
        }

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
