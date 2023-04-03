using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    /// <summary>
    /// ControllerM_CALPHADDatabase creates and handles all load, delete and other methods with respect to this class
    /// </summary>
    public class ControllerM_CALPHADDatabase : Controller_Abstract_Models<Model_CALPHADDatabase>
    {
        // Constructors
        public ControllerM_CALPHADDatabase(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_CALPHADDatabase(IAMCore_Comm comm, ModelController<Model_CALPHADDatabase> modelMC) : base(comm, modelMC)
        { }

        #region Model methods
        /// <summary>
        /// Gets the CALPHADDatabase object for the project id
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDProject"></param>
        /// <returns></returns>
        public static ModelController<Model_CALPHADDatabase> GetDatabaseFromProjectID(IAMCore_Comm comm, int IDProject) 
        {
            // Load from project ID
            List<ModelController<Model_CALPHADDatabase>> result = ModelController<Model_CALPHADDatabase>.LoadIDProject(ref comm, IDProject);

            return result.FirstOrDefault() ?? new ModelController<Model_CALPHADDatabase>(ref comm, new());
        }
        #endregion
    }
}
