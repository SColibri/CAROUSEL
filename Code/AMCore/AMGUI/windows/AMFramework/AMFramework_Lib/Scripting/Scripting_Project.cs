using AMFramework_Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Scripting
{
    internal class Scripting_Project : Scripting_Abstract
    {
        private Model_Projects MObject;
        public Scripting_Project(Model_Projects modelP)  
        {
            MObject = modelP;
            VariableName = MObject.Get_Scripting_ClassName() + "_" + _count++;
        }

        public override string Create_Object()
        {
            throw new NotImplementedException();
        }

        public override string Load_Object() 
        {
            string loadScript = "local " + VariableName + " = " + MObject.Get_Scripting_ClassName() + ":new{ ID = " + MObject.ID + "} \n\n";

            return loadScript;
        }


        public override string ScriptText()
        {
            Scripting_Template<Model_Projects> projectObject_Script = new(MObject);
            
            // Create project object first
            string ScriptingText = projectObject_Script.ScriptText();


            throw new NotImplementedException();
        }
    }
}
