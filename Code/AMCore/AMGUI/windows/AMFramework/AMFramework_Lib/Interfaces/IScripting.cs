using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Interfaces
{
    internal interface IScripting
    {
        /// <summary>
        /// Returns the script for creating a specific object
        /// </summary>
        /// <returns></returns>
        public string ScriptText();

    }
}
