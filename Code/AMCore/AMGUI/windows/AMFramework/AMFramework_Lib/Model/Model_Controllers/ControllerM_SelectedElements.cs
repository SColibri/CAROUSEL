using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_SelectedElements : Controller_Abstract_Models<Model_SelectedElements>
    {
        // Constructors
        public ControllerM_SelectedElements(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_SelectedElements(IAMCore_Comm comm, ModelController<Model_SelectedElements> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// Returns all selected elements and composition for specified Case by ID
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDCase"></param>
        /// <returns></returns>
        public static List<ModelController<Model_SelectedElements>> Get_SelectedElements_FromIDProject(IAMCore_Comm comm, int IDProject)
        {
            // load Selected elements
            List<ModelController<Model_SelectedElements>> result = ModelController<Model_SelectedElements>.LoadIDProject(ref comm, IDProject);

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
