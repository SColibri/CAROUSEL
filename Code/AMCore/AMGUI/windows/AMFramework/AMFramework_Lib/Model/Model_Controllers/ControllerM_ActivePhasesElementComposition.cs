using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_ActivePhasesElementComposition : Controller_Abstract_Models<Model_ActivePhasesElementComposition>
    {
        // Constructors
        public ControllerM_ActivePhasesElementComposition(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_ActivePhasesElementComposition(IAMCore_Comm comm, ModelController<Model_ActivePhasesElementComposition> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// Returns all elements used in the active phases based on the ID of the project
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDProject"></param>
        /// <returns></returns>
        public static List<ModelController<Model_ActivePhasesElementComposition>> Get_ActivePhaseElementComposition_FromIDProject(IAMCore_Comm comm, int IDProject)
        {
            // Load all elements
            List<ModelController<Model_ActivePhasesElementComposition>> result = ModelController<Model_ActivePhasesElementComposition>.LoadIDProject(ref comm, IDProject);

            // Get the element name
            foreach (var item in result)
            {
                item.ModelObject.ElementName = ControllerM_Element.Get_ElementByID(comm, item.ModelObject.IDElement).ModelObject.Name;
            }

            return result;
        }
        #endregion
    }
}
