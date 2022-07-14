using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_ActivePhasesElementComposition : Interfaces.Model_Interface
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

        private int _idElement = -1;
        public int IDElement
        {
            get { return _idElement; }
            set
            {
                _idElement = value;
                OnPropertyChanged("IDElement");
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

        public string Get_csv()
        {
            string outy = ID + "," + IDProject + "," + IDElement + "," + Value;
            return outy;
        }

        #region Other
        private string _elementName = "";
        public string ElementName 
        { 
            get { return _elementName; }
            set 
            { 
                _elementName = value;
                OnPropertyChanged("ElementName");
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
