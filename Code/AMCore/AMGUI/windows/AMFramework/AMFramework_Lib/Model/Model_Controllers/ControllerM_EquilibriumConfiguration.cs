using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_EquilibriumConfiguration : Controller_Abstract_Models<Model_EquilibriumConfiguration>
    {
        // Constructors
        public ControllerM_EquilibriumConfiguration(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_EquilibriumConfiguration(IAMCore_Comm comm, ModelController<Model_EquilibriumConfiguration> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// Returns Equilibrium configuration for specified IDCase
        /// </summary>
        /// <param name="comm">Core communication object</param>
        /// <param name="IDCase">Case ID</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ModelController<Model_EquilibriumConfiguration> Get_EquilibriumConfiguration_FromIDCase(IAMCore_Comm comm, int IDCase)
        {
            // load Selected elements
            List<ModelController<Model_EquilibriumConfiguration>> result = ModelController<Model_EquilibriumConfiguration>.LoadIDCase(ref comm, IDCase);

            if (result.Count > 0) return result[0];
            else return new(ref comm);
        }
        #endregion

        #region Core_Methods
        /// <summary>
        /// Runs Equilibrium solidification simulation in parallel using the case id range
        /// </summary>
        /// <param name="comm">Core communication object</param>
        /// <param name="IDProject">Project ID reference</param>
        /// <param name="fromIDCase">Start ID case -> smaller or equal to toIDCase </param>
        /// <param name="toIDCase">End ID Case -> Bigger or equal to toIDCase </param>
        /// <returns></returns>
        public static bool Run_EquilibriumSimulation(IAMCore_Comm comm, int IDProject, int fromIDCase, int toIDCase)
        {
            // Create query and execute
            string Query = IDProject + "||" + fromIDCase + "-" + toIDCase;
            string outMessage = comm.run_lua_command("pixelcase_step_equilibrium_parallel ", Query);

            // Check if simulation was done
            bool result = (outMessage.CompareTo("OK") == 0);
            return result;
        }


        #endregion
    }
}
