using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Controller
{
    public class ControllerAbstract : Interfaces.Controller_Interface
    {
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
