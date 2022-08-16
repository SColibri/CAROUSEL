using AMFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMFramework.Model
{
    /// <summary>
    /// Abstract class that implements the CoreCommands interface. Used for
    /// calling core commands related to data models like load, save and
    /// others -- se enum for possible options.
    /// Note: not all options apply for every data model.
    /// </summary>
    public class ModelCoreCommand:Interfaces.CoreCommand_Interface
    {
        #region Constructor
        /// <summary>
        /// Core communication
        /// </summary>
        private Core.IAMCore_Comm _coreComm;

        /// <summary>
        /// Constructor, this class needs core communication for running commands
        /// </summary>
        public ModelCoreCommand(ref Core.IAMCore_Comm comm)
        {
            Set_CoreCommunication(comm);
        }

        public void Set_CoreCommunication(Core.IAMCore_Comm comm) { _coreComm = comm; }

        #endregion

        #region Parameters
        /// <summary>
        /// Enum that holds all available options on datamodels
        /// </summary>
        public enum Commands
        {
            NONE,
            TABLENAME,
            SAVE_ID,
            LOAD_ID,
            LOAD_BYNAME,
            LOAD_IDPROJECT,
            LOAD_ALL,
            DELTE_ID
        }

        /// <summary>
        /// LUA command to be executed, see Core project for more information
        /// </summary>
        public string Command_instruction { get; set; } = "";
        /// <summary>
        /// Parameter to be passed , only as a line of string
        /// in csv format
        /// </summary>
        public string Command_parameters { get; set; } = "";


        public object? Tag { get; set; }

        private string _coreOutput = "";
        public string CoreOutput
        {
            get { return _coreOutput; }
        }

        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get { return _isEnabled; }
        }

        public Type ObjectType { get; set; }
        public int Command_Type { get; set; }

        #endregion

        #region CommandActions
        private ICommand? _coreCommand;
        /// <summary>
        /// Icommand that runs the currtent stored command
        /// </summary>
        public ICommand CoreCommand
        {
            get 
            {
                if (_coreCommand == null) 
                {
                    _coreCommand = new Controller.RelayCommand(
                        param => this.DoAction(),
                        param => this.CheckBeforeAction()
                    );
                }

                return _coreCommand;
            }
        }

        /// <summary>
        /// calls core command and stores the output in coreOutput.
        /// </summary>
        public void DoAction() 
        {
            if (!CheckBeforeAction()) return;
            _coreOutput = _coreComm.run_lua_command(Command_lua, Command_paramters);
        }

        /// <summary>
        /// Checks for additional requirements 
        /// </summary>
        /// <returns></returns>
        public bool CheckBeforeAction() 
        {
            bool Result = true;
            
            if (Command_instruction.Length == 0) 
            {
                _coreOutput = "Error: No command specified!";
                Result = false;
            }

            if (!_isEnabled) 
            {
                _coreOutput = "Error: Not enabled!";
                Result = false;
            }
            return Result;
        }

        #endregion

    }
}
