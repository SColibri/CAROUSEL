using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_SelectedElements : INotifyPropertyChanged
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

        private int _IDProject = -1;
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
        public int ISReferenceElement
        {
            get { return _isReferenceElement; }
            set
            {
                _isReferenceElement = value;
                OnPropertyChanged("ISReferenceElement");
            }
        }

        public string get_csv()
        {
            string outy = ID + "," + IDProject + "," + IDElement + "," + ISReferenceElement;
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
