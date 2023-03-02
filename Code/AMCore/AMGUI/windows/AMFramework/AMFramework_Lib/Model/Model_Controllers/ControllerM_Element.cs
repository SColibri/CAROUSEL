using AMFramework_Lib.Core;
using AMFramework_Lib.Model.Controllers;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_Element : Controller_Abstract_Models<Model_Element>
    {
        // Constructors
        public ControllerM_Element(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_Element(IAMCore_Comm comm, ModelController<Model_Element> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        /// <summary>
        /// Loads all elements used in a project
        /// </summary>
        /// <param name="comm">IAMCore_Comm object</param>
        /// <param name="IDProject">ID Project</param>
        /// <returns></returns>
        public static List<ModelController<Model_Element>> Get_elementsFromProjectID(IAMCore_Comm comm, int IDProject)
        {
            return ModelController<Model_Element>.LoadIDProject(ref comm, IDProject);
        }

        /// <summary>
        /// Returns all elements contained in the database
        /// </summary>
        /// <param name="comm">IAMCore_Comm object</param>
        /// <returns></returns>
        public static List<ModelController<Model_Element>> Get_allElements(IAMCore_Comm comm)
        {
            return ModelController<Model_Element>.LoadAll(ref comm);
        }

        /// <summary>
        /// Returns elements contained in the CALPHAD database
        /// </summary>
        /// <param name="comm">IAMCore_Comm object</param>
        /// <returns></returns>
        public static List<ModelController<Model_Element>> LoadFromDatabase(IAMCore_Comm comm)
        {
            List<ModelController<Model_Element>> result = new();

            // get and load into local database available phase names from CALPHAD database
            string Query = "get_elementNames";
            string outCommand = comm.run_lua_command(Query, "");
            List<string> elementList = outCommand.Split("\n").ToList();

            // get related ID's from database by given name, missing ID's are ignored
            Query = "database_table_custom_query SELECT * FROM Element WHERE ";
            for (int i = 2; i < elementList.Count - 1; i++)
            {
                // Load by name
                string tempQuery = Query + " Name = '" + elementList[i].Replace("\r", "") + "' ";
                outCommand = comm.run_lua_command(tempQuery, "");

                // check if output is csv
                List<string> columnItems = outCommand.Split(",").ToList();
                if (columnItems.Count < 2) continue;

                // Add to list
                int IDObject = Convert.ToInt32(columnItems[0]);
                ModelController<Model_Element> newElementObject = Get_ElementByID(comm, IDObject);
                result.Add(newElementObject);
            }

            return result;
        }

        /// <summary>
        /// Creates a Modelcontroller of model type by loading the object ID
        /// </summary>
        /// <param name="comm">IAMCore_Comm object</param>
        /// <param name="ID">Type ID</param>
        /// <returns></returns>
        public static ModelController<Model_Element> Get_ElementByID(IAMCore_Comm comm, int ID)
        {
            ModelController<Model_Element> refTemp = new(ref comm);
            refTemp.ModelObject.ID = ID;
            refTemp.LoadByIDAction?.DoAction();

            return refTemp;
        }
        #endregion
    }
}
