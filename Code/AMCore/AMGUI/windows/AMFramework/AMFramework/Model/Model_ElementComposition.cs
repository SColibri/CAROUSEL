using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_ElementComposition : INotifyPropertyChanged
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

        private int _id_case = -1;
        public int IDCase
        {
            get { return _id_case; }
            set
            {
                _id_case = value;
                OnPropertyChanged("IDCase");
            }
        }

        private int _id_element = -1;
        public int IDElement
        {
            get { return _id_element; }
            set
            {
                _id_element = value;
                OnPropertyChanged("IDElement");
            }
        }

        private string _typeComposition = "";
        public string TypeComposition
        {
            get { return _typeComposition; }
            set
            {
                _typeComposition = value;
                OnPropertyChanged("TypeComposition");
            }
        }

        private double _value = -1;
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
        #endregion

        public string get_csv()
        {
            string outy = ID + "," + IDCase + "," + IDElement + "," + TypeComposition.Replace(" ", "#") + "," + Value;
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
