using AMFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Scripting
{
    public abstract class Scripting_Abstract : IScripting
    {
        protected static int _count = 0;
        public string VariableName { get; set; } = "";
        public abstract string ScriptText();
        public abstract string Create_Object();
        public abstract string Load_Object();

    }
}
