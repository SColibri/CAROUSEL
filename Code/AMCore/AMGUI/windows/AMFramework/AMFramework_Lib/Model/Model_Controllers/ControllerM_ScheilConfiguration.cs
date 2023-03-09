using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_ScheilConfiguration : Controller_Abstract_Models<Model_ScheilConfiguration>
    {
        // Constructors
        public ControllerM_ScheilConfiguration(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_ScheilConfiguration(IAMCore_Comm comm, ModelController<Model_ScheilConfiguration> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// Returns simulation data
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDCase"></param>
        /// <returns></returns>
        public static List<ModelController<Model_ScheilPhaseFraction>> Get_ScheilSolidificationSimulation_FromIDCase(IAMCore_Comm comm, int IDCase)
        {
            return ModelController<Model_ScheilPhaseFraction>.LoadIDCase(ref comm, IDCase);
        }

        /// <summary>
        /// Run scheil solidification simulation
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDProject"></param>
        /// <param name="fromIDCase"></param>
        /// <param name="toIDCase"></param>
        /// <returns></returns>
        public static bool Run_ScheilSimulation(IAMCore_Comm comm, int IDProject, int fromIDCase, int toIDCase)
        {
            // Create query and execute
            string Query = IDProject + "||" + fromIDCase + "-" + toIDCase;
            string outMessage = comm.run_lua_command("pixelcase_step_scheil_parallel ", Query);

            // Check if simulation was done
            bool result = (outMessage.CompareTo("OK") == 0);
            return result;
        }
        #endregion
    }
}
