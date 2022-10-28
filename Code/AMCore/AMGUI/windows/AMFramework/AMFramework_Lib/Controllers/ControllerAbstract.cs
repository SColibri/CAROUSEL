using AMFramework_Lib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Controller
{
    public class ControllerAbstract : Interfaces.Controller_Interface
    {
        protected IAMCore_Comm _comm;
        public ControllerAbstract() { }
        public ControllerAbstract(IAMCore_Comm comm) 
        {
            _comm = comm;
        }

        #region Flags
        private bool _loadingData = false;
        public bool LoadingData
        {
            get { return _loadingData; }
            set
            {
                _loadingData = value;
                OnPropertyChanged(nameof(LoadingData));
            }
        }
        #endregion

        #region INotifyPropertyChanged_Interface
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Controller_Interface
        public virtual void Refresh()
        {
            throw new NotImplementedException();
        }
        #endregion 
    }
}
