using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMFramework_Lib.Interfaces
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
        public object? Tag { get; set; }
        /// <summary>
        /// Command as Executor type
        /// </summary>
        public Type Executor_Type { get; set; }
        /// <summary>
        /// String command for core
        /// </summary>
        public string Command_instruction { get; set; }

        /// <summary>
        /// Object type to which it belongs
        /// </summary>
        public Type ObjectType { get; set; }


    }
}
