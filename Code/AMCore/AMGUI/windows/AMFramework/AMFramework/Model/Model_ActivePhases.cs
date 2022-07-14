using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    internal class Model_ActivePhases : Interfaces.Model_Interface
    {
        private int _id = -1;
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
        public int IDProject
        {
            get { return _idProject; }
            set
            {
                _idProject = value;
                OnPropertyChanged("IDProject");
            }
        }

        private int _idPhase = -1;
        public int IDPhase
        {
            get { return _idPhase; }
            set
            {
                _idPhase = value;
                OnPropertyChanged("IDPhase");
            }
        }

        public string Get_csv()
        {
            string outy = ID + "," + IDProject + "," + IDPhase;
            return outy;
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
