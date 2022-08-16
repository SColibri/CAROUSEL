using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMFramework.Interfaces
{
    /// <summary>
    /// Executes commands 
    /// </summary>
    public interface CoreCommandExecutor_Interface
    {
        /// <summary>
        /// Object Identifier 
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// String command in csv format for parameters
        /// </summary>
        public string Command_parameters { get; set; }
        /// <summary>
        /// booleand that states if the command can be executed or not
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// delegate for core communication
        /// </summary>
        public Core.IAMCore_Comm CoreCommunication { get; }

        /// <summary>
        /// Output of the executed command
        /// </summary>
        public string CoreOutput { get; set; }
        /// <summary>
        /// Icommand implementation for wpf that executes the command
        /// </summary>
        public ICommand CoreCommand { get; }
        /// <summary>
        /// Runs the command in core
        /// </summary>
        public void DoAction();
        /// <summary>
        /// Checks if commmand can be run
        /// </summary>
        /// <returns></returns>
        public bool CheckBeforeAction();
        /// <summary>
        /// Object type to which it belongs
        /// </summary>
    }
}
