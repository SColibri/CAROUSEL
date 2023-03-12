using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_Project : Controller_Abstract_Models<Model_Projects>
    {
        // Constructors
        public ControllerM_Project(IAMCore_Comm comm) : base(comm)
        {
            
        }
        public ControllerM_Project(IAMCore_Comm comm, ModelController<Model_Projects> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// Loads selected elements in project level
        /// </summary>
        public void Load_SelectedElements()
        {
            MCObject.ModelObject.SelectedElements = ControllerM_SelectedElements.Get_SelectedElements_FromIDProject(_comm, MCObject.ModelObject.ID);
        }

        /// <summary>
        /// Loads the CALPHAD database paths
        /// </summary>
        public void Load_Databases() 
        {
            // Databases
            MCObject.ModelObject.Databases = ControllerM_CALPHADDatabase.GetDatabaseFromProjectID(_comm, MCObject.ModelObject.ID);
        }

        /// <summary>
        /// Loads all active phases obtained from simulation
        /// </summary>
        public void Load_ActivePhases()
        {
            // Phases
            MCObject.ModelObject.ActivePhases = ControllerM_ActivePhases.Get_ActivePhaseList(_comm, MCObject.ModelObject.ID);
            // Configuration data
            MCObject.ModelObject.ActivePhasesConfiguration = ControllerM_ActivePhasesConfiguration.Get_ActivePhaseConfiguration_FromIDProject(_comm, MCObject.ModelObject.ID);
            // Element composition
            MCObject.ModelObject.ActivePhasesElementComposition = ControllerM_ActivePhasesElementComposition.Get_ActivePhaseElementComposition_FromIDProject(_comm, MCObject.ModelObject.ID);
        }

        /// <summary>
        /// Load all cases related to this project
        /// </summary>
        public void Load_cases()
        {
            MCObject.ModelObject.Cases = ControllerM_Case.Get_CasesByIDProject(_comm, MCObject.ModelObject.ID);
        }

        /// <summary>
        /// Clear all dependent data
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDProject"></param>
        public static void Clear_SimulationData(IAMCore_Comm comm, int IDProject)
        {
            var outy = comm.run_lua_command("project_remove_dependentData", IDProject.ToString());
        }

        #endregion


    }
}
