using AMFramework_Lib.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
