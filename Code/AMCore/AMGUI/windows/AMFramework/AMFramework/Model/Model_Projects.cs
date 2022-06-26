using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_Projects : INotifyPropertyChanged
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

        private string _name = "New Name";
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

        public string get_csv() 
        {
            string outy =  ID + "," + Name.Replace(" ","#") + "," + APIName ;
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
