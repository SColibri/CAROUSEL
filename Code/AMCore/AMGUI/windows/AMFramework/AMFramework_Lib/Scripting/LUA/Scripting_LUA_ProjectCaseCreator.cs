using AMFramework_Lib.Model;

namespace AMFramework_Lib.Scripting.LUA
{
    public class Scripting_LUA_ProjectCaseCreator : Scripting_LUA_Abstract
    {
        private Model_Projects _mProject;
        private Model_Case _mCase;
        public Scripting_LUA_ProjectCaseCreator(Model_Projects mProject, Model_Case mCase)
        {
            _mProject = mProject;
            _mCase = mCase;
        }

        public override string Create_Object()
        {
            Scripting_LUA_Project sProject = new(_mProject);

            string result = "require\"AMFramework_Lib\" \n\n";
            result += sProject.Load_Object();

            // Project should already exist when using the project case creator. We make sure that the templated
            // case had the correct project id by just assigning it.
            _mCase.IDProject = _mProject.ID;

            // We now use the case as a template and let it find all
            // range variables
            Scripting_LUA_Case sCase = new(_mCase);
            result += sCase.Create_Ranged_Object(sProject);

            // Create all cases based on the template model
            result += sProject.VariableName + ":create_cases(" + sCase.VariableName + ")";

            // Add run command (what do we want to run?)

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
