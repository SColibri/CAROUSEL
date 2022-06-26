using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Views.Projects
{
    public class Project_ViewModel : Interfaces.ViewModel_Interface
    {
#region interface
        public bool close()
        {
            return true;
        }

        public bool save()
        {
            return true;
        }
#endregion  

        public Views.Projects.Project_contents get_project_content(Controller.Controller_DBS_Projects projectController) 
        {
            return new Project_contents(ref projectController);
        }

    }
}
