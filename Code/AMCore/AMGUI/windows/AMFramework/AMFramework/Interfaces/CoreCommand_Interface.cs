using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMFramework.Interfaces
{
    /// <summary>
    /// This interface enables listing all available commands and execute them using an implementation of the 
    /// CoreCommandExecutor_interface.
    /// </summary>
    public interface CoreCommand_Interface
    {
        /// <summary>
        /// Object Identifier 
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Command type flag
        /// </summary>
        public int Command_Type { get; set; }
        /// <summary>
        /// String command for core
        /// </summary>
        public string Command_instruction { get; set; }


        // Move out of here - Deprecated!
        // -------------------------------------------------------------
        /// <summary>
        /// String command in csv format for parameters
        /// </summary>
        public string Command_parameters { get; set; }
        /// <summary>
        /// booleand that states if the command can be executed or not
        /// </summary>
        public bool IsEnabled { get; }
        /// <summary>
        /// Output of the executed command
        /// </summary>
        public string CoreOutput { get; }
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
        // -----------------------------------------------------------




        /// <summary>
        /// Object type to which it belongs
        /// </summary>
        public Type ObjectType { get; set; }


    }
}
