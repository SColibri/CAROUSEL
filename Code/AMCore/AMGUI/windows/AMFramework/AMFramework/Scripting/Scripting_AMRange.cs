using AMFramework.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace AMFramework.Scripting
{
    internal class Scripting_AMRange : Scripting_Abstract
    {
        private string _AMRange;
        public Scripting_AMRange(string value) 
        { 
            _AMRange = value;
            VariableName = "AMRange_" + _count++;
        }

        public override string Create_Object()
        {
            string result = "local " + VariableName + " = AMRange:new{}\n";
            result += "add_range(" + _AMRange + ")\n\n";
            return result;
        }

        public override string Load_Object()
        {
            throw new NotImplementedException();
        }

        public override string ScriptText()
        {
            throw new NotImplementedException();
        }
    }
}
