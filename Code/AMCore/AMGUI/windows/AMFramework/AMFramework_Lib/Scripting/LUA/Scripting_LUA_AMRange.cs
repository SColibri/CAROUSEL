namespace AMFramework_Lib.Scripting.LUA
{
    internal class Scripting_LUA_AMRange : Scripting_LUA_Abstract
    {
        private string _AMRange;
        public Scripting_LUA_AMRange(string value)
        {
            _AMRange = value;
            VariableName = "AMRange_" + _count++;
        }

        public override string Create_Object()
        {
            string result = "local " + VariableName + " = AMRange:new{}\n";
            result += VariableName + ":add_range(" + _AMRange + ")\n";
            return result;
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
