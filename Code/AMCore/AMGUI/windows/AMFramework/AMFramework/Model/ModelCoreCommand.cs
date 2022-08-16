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

        public object? Tag { get; set; }

        public Type ObjectType { get; set; }
        public Type Executor_Type { get; set; }

        #endregion



    }
}
