using AMFramework.Model;
using AMFramework.Views.Projects.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Scripting
{
    public class Scripting_ProjectCaseCreator : Scripting_Abstract
    {
        private Model_Projects _mProject;
        private Model_Case _mCase;
        public Scripting_ProjectCaseCreator(Model_Projects mProject, Model_Case mCase) 
        {
            _mProject = mProject;
            _mCase = mCase;
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
            Scripting.Scripting_Project sProject = new(_mProject);

            string result = "require\"AMFramework\" \n\n";
            result = sProject.Load_Object();

            // Project should already exist when using the project case creator. We make sure that the templated
            // case had the correct project id by just assigning it.
            _mCase.IDProject = _mProject.ID;

            // We now have to create all cases

            return result;
        }
    }
}
