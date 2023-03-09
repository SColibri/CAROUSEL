using AMFramework_Lib.Model;

namespace AMFramework_Lib.Scripting.LUA
{
    internal class Scripting_LUA_ScheilConfiguration : Scripting_LUA_Abstract
    {
        private Model_ScheilConfiguration _model;
        public Scripting_LUA_ScheilConfiguration(Model_ScheilConfiguration mSC)
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

        public override string Save_Object()
        {
            throw new NotImplementedException();
        }
    }
}
