using AMFramework_Lib.Model;

namespace AMFramework_Lib.Scripting.LUA
{
    public class Scripting_LUA_Project : Scripting_LUA_Abstract
    {
        private Model_Projects MObject;
        public Scripting_LUA_Project(Model_Projects modelP)
        {
            MObject = modelP;
            VariableName = MObject.Get_Scripting_ClassName() + "_" + _count++;
        }

        public override string Create_Object()
        {
            Scripting_LUA_Template<Model_Projects> projectObject_Script = new(MObject);

            // Create project object first
            string ScriptingText = projectObject_Script.Create_Object();

            return ScriptingText;
        }

        public override string Load_Object()
        {
            string loadScript = "local " + VariableName + " = " + MObject.Get_Scripting_ClassName() + ":new{ ID = " + MObject.ID + "} \n\n";

            return loadScript;
        }

        public override string Save_Object()
        {
            throw new NotImplementedException();
        }
    }
}
