using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_ActivePhases : INotifyPropertyChanged
    {
        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_DBS_Projects _ProjectController;
        public Controller_ActivePhases(ref Core.IAMCore_Comm socket, Controller_DBS_Projects projectController)
        {
            _AMCore_Socket = socket;
            _ProjectController = projectController;
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
