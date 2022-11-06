using AMFramework_Lib.Core;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_Case : Controller_Abstract_Models<Model_Case>
    {
        public ControllerM_Case(IAMCore_Comm comm) : base(comm)
        {
            Add_SolidificationConfigurations();
        }
        public ControllerM_Case(IAMCore_Comm comm, ModelController<Model_Case> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        public void Load_ElementComposition() 
        {
            
            // TODO Refactor Case object
            //CaseMC.ModelObject.ElementComposition = ModelController<Model_ElementComposition>.LoadIDCase(ref _comm,CaseMC.ModelObject.ID);
        }

        #region ObjectPrivateMethods
        private void Add_SolidificationConfigurations()
        {
            MCObject.ModelObject.EquilibriumConfiguration = new(ref _comm);
            MCObject.ModelObject.ScheilConfiguration = new(ref _comm);
        }
        #endregion

        #region Save_templated_object
        public static void Save_Templated_Object(IAMCore_Comm comm, Model_Case modelCase) 
        {
            if (modelCase.IDProject == -1) throw new Exception("Case has no reference to any project, this is a sad case :´(");


           
        }

        public static List<ModelController<Model_Case>> Get_CasesByIDProject(IAMCore_Comm comm, int IDProject) 
        {
            return ModelController<Model_Case>.LoadIDProject(ref comm, IDProject);
        }
        #endregion

        #endregion



    }
}
