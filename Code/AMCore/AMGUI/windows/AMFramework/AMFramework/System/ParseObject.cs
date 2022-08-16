using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.AMSystem
{
    public class ParseObject
    {
        public enum PTYPE 
        { 
            NONE,
            CLASS,
            FUNCTION,
            GLOBAL_VARIABLE
        }

        public string ModuleName = "";
        public PTYPE ObjectType = PTYPE.NONE;
        public string Name = "";
        public string Description = "";
        public string ParametersType = "";
        public List<string> Parameters = new();
        public List<ParseObject> functions = new();
        public Interfaces.Model_Interface? ObjectModel = null;

    }
}
