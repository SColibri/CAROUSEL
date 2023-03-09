using AMFramework_Lib.Core;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Model.Controllers;
using AMFramework_Lib.Model.ModelCoreExecutors;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_ActivePhases : Controller_Abstract_Models<Model_ActivePhases>
    {
        // Constructors
        public ControllerM_ActivePhases(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_ActivePhases(IAMCore_Comm comm, ModelController<Model_ActivePhases> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        public static List<ModelController<Model_ActivePhases>> Get_ActivePhaseList(IAMCore_Comm comm, int IDProject)
        {
            Model_Interface refModel = new Model_ActivePhases();
            List<ModelController<Model_ActivePhases>> result = new();

            string Query = "SELECT ActivePhases.*, Phase.Name As PhaseName FROM ActivePhases INNER JOIN Phase ON Phase.ID=ActivePhases.IDPhase WHERE IDProject=" + IDProject;
            MCE_LoadByQuery mce = new(ref comm, ref refModel, Query);
            mce.DoAction();

            foreach (Model_ActivePhases item in mce.ModelObjects)
            {
                ModelController<Model_ActivePhases> tempMc = new(ref comm, item);

                // gets the phase name
                Set_PhaseName(comm, ref tempMc);

                result.Add(tempMc);
            }

            return result;
        }

        private static void Set_PhaseName(IAMCore_Comm comm, ref ModelController<Model_ActivePhases> modController)
        {
            modController.ModelObject.PhaseName = ControllerM_Phase.Get_PhaseByID(comm, modController.ModelObject.IDPhase).ModelObject.Name;
        }
        #endregion
    }
}
