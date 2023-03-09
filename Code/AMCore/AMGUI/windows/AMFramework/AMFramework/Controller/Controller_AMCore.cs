using AMFramework_Lib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AMFramework.Controller
{
    public class Controller_AMCore : INotifyPropertyChanged
    {

        private IAMCore_Comm _AMCore_comm;
        public Controller_AMCore(IAMCore_Comm socket)
        {
            _AMCore_comm = socket;
        }


        #region Interfaces
        public event PropertyChangedEventHandler? PropertyChanged;

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
                OnPropertyChanged(nameof(CoreOutput));
            }
        }




        private List<string> _DB_tables = new();
        public List<string> DB_tables
        {
            get
            {
                string outy = _AMCore_comm.run_lua_command("database_tableList", "");
                _DB_tables = outy.Split(",").ToList();
                return _DB_tables;
            }
        }
        #endregion

        #region Commands
        public string Run_command(string commy)
        {
            List<string> comms = commy.Split(" ").ToList();
            string parameters = "";

            for (int n1 = 1; n1 < comms.Count; n1++)
            {
                if (n1 > 1) { parameters += " "; }
                parameters += comms[n1];
            }

            CoreOutput = _AMCore_comm.run_lua_command(comms[0], parameters);
            return CoreOutput;
        }
        #endregion
    }
}
