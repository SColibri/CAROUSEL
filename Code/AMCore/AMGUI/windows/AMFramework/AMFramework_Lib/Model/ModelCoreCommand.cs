namespace AMFramework_Lib.Model
{
    /// <summary>
    /// Abstract class that implements the CoreCommands interface. Used for
    /// calling core commands related to data models like load, save and
    /// others -- se enum for possible options.
    /// Note: not all options apply for every data model.
    /// </summary>
    public class ModelCoreCommand : Interfaces.CoreCommand_Interface
    {
        #region Constructor
        /// <summary>
        /// Core communication
        /// </summary>
        private Core.IAMCore_Comm _coreComm;

        /// <summary>
        /// Constructor, this class needs core communication for running commands
        /// </summary>
        public ModelCoreCommand()
        {
        }

        #endregion

        #region Parameters
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
