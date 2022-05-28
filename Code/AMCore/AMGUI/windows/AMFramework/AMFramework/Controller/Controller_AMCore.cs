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

        private Core.AMCore_Socket _AMCore_Socket;
        public Controller_AMCore(Core.AMCore_Socket socket) 
        {
            _AMCore_Socket = socket;
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

        


        private List<string> _DB_tables = new();
        public List<string> DB_tables
        {
            get
            {
                string outy = _AMCore_Socket.send_receive("database_tableList");
                _DB_tables = outy.Split(",").ToList();
                return _DB_tables;
            }
        }
        #endregion
    }
}
