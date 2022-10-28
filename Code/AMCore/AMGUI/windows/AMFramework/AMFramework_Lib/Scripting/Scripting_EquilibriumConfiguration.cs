using AMFramework_Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Scripting
{
    internal class Scripting_EquilibriumConfiguration : Scripting_Abstract
    {
        private Model_EquilibriumConfiguration _model;
        public Scripting_EquilibriumConfiguration(Model_EquilibriumConfiguration mEC) 
        {
            _model = mEC;
            VariableName = _model.Get_Scripting_ClassName() + "_" + _count++;
        }
        
        public override string Create_Object()
        {
            throw new NotImplementedException();
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
