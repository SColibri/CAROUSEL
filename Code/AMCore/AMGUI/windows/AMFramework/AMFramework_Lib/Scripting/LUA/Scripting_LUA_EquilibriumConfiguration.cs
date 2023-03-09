using AMFramework_Lib.Model;

namespace AMFramework_Lib.Scripting.LUA
{
    internal class Scripting_LUA_EquilibriumConfiguration : Scripting_LUA_Abstract
    {
        private Model_EquilibriumConfiguration _model;
        public Scripting_LUA_EquilibriumConfiguration(Model_EquilibriumConfiguration mEC)
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

        public override string Save_Object()
        {
            throw new NotImplementedException();
        }
    }
}
