using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_ElementComposition : Controller_Abstract_Models<Model_ElementComposition>
    {
        // Constructors
        public ControllerM_ElementComposition(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_ElementComposition(IAMCore_Comm comm, ModelController<Model_ElementComposition> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        public static List<ModelController<Model_ElementComposition>> Get_ElementComposition_FromIDCase(IAMCore_Comm comm, int IDCase)
        {
            // load Selected elements
            List<ModelController<Model_ElementComposition>> result = ModelController<Model_ElementComposition>.LoadIDCase(ref comm, IDCase);

            // Get element name
            foreach (var item in result)
            {
                item.ModelObject.ElementName = ControllerM_Element.Get_ElementByID(comm, item.ModelObject.IDElement).ModelObject.Name;
            }

            return result;
        }
        #endregion
    }
}
