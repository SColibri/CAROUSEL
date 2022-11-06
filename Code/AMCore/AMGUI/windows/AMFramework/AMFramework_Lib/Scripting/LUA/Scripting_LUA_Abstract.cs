using AMFramework_Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Scripting.LUA
{
    /// <summary>
    /// Lua implementation for creating scripts from objects. Note: We should consider calling the core implementation
    /// for calling the return values for these classes
    /// </summary>
    public abstract class Scripting_LUA_Abstract : IScripting
    {
        /// <summary>
        /// Static number used for naming the variables by adding a unique number
        /// </summary>
        protected static int _count = 0;

        /// <summary>
        /// Name used for the scripted variable, this is used for refering to it after
        /// creating the object.
        /// </summary>
        public string VariableName { get; set; } = "";
        public abstract string Create_Object();
        public abstract string Load_Object();
        public abstract string Save_Object();

    }
}
