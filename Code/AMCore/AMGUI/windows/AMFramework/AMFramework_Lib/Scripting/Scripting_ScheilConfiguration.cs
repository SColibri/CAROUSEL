using AMFramework_Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Scripting
{
    internal class Scripting_ScheilConfiguration : Scripting_Abstract
    {
        private Model_ScheilConfiguration _model;
        public Scripting_ScheilConfiguration(Model_ScheilConfiguration mSC) 
        { 
            _model = mSC;
            VariableName = _model.Get_Scripting_ClassName() + "_" + _count++;
        }

        public override string Create_Object()
        {
            return "local " + VariableName + " = " + _model.Get_Scripting_ClassName() + ":new{}";
        }

        public override string Load_Object()
        {
            return "local " + VariableName + " = " + _model.Get_Scripting_ClassName() + ":new{ ID = " + _model.ID + " }";
        }

        public override string ScriptText()
        {
            throw new NotImplementedException();
        }
    }
}
