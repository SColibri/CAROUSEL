using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_SelectedPhases : INotifyPropertyChanged
    {
        private int _ID = -1;
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
        public int IDPhase
        {
            get { return _IDPhase; }
            set
            {
                _IDPhase = value;
                OnPropertyChanged("IDPhase");
            }
        }

        private string _PhaseName = "";
        public string PhaseName
        {
            get { return _PhaseName; }
            set
            {
                _PhaseName = value;
                OnPropertyChanged("PhaseName");
            }
        }


        public string get_csv()
        {
            string outy = ID + "," + IDCase + "," + IDPhase;
            return outy;
        }

        #region Other_properties
        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private bool _isDependentPhase = false;
        public bool IsDependentPhase
        {
            get { return _isDependentPhase; }
            set
            {
                _isDependentPhase = value;
                OnPropertyChanged("IsDependentPhase");
            }
        }
        #endregion
        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
}
