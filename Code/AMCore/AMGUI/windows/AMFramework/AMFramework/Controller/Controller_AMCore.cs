using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_AMCore : INotifyPropertyChanged
    {
        public Controller_AMCore() 
        { 
        
        }
        
        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Model
        private String _coreOutput = "Welcome!";
        public String CoreOutput
        {
            get { return _coreOutput; }
            set
            {
                _coreOutput = value;
                OnPropertyChanged("CoreOutput");
            }
        }
        #endregion
    }
}
