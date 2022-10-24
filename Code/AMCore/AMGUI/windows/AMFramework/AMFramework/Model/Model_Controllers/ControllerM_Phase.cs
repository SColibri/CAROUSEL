using AMFramework.Core;
using AMFramework.Model.Controllers;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.Model_Controllers
{
    public class ControllerM_Phase : Controller_Abstract_Models<Model_Phase>
    {
        // Constructors
        public ControllerM_Phase(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_Phase(IAMCore_Comm comm, ModelController<Model_Phase> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods

        #region Selection

        /// <summary>
        /// Sets as selected all phases used in a case ID
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="phaseList"></param>
        /// <param name="IDCase"></param>
        public static void SetSelectedFromIDCase(IAMCore_Comm comm, List<ControllerM_Phase> phaseList, int IDCase) 
        {
            List<ControllerM_Phase> selectedObjects = UniquePhasesByIDCase(comm, IDCase);

            foreach (var item in phaseList)
            {
                ControllerM_Phase? refObject = selectedObjects.Find(e => e.MCObject.ModelObject.Name.CompareTo(item.MCObject.ModelObject.Name) == 0);
                
                if (refObject == null) continue;
                item.MCObject.ModelObject.IsSelected = true;
            }
        }


        #endregion

        #region Load_List
        /// <summary>
        /// Loads all models from selected database
        /// </summary>
        /// <returns></returns>
        public static List<ControllerM_Phase> LoadFromDatabase(IAMCore_Comm comm)
        {
            List<ControllerM_Phase> result = new();

            // get and load into local database available phase names from CALPHAD database
            string Query = "matcalc_database_phaseNames";
            string outCommand = comm.run_lua_command(Query, "");
            List<string> pahseList = outCommand.Split("\n").ToList();

            // Check for results, if non return empty list
            if (pahseList.Count == 0) return result;

            // get related ID's from database by given name, missing ID's are ignored
            Query = "database_table_custom_query SELECT * FROM Phase WHERE ";
            for (int i = 2; i < pahseList.Count - 1; i++)
            {
                // Load by name
                string tempQuery = Query + " Name = '" + pahseList[i].Replace("\r", "") + "' ";
                outCommand = comm.run_lua_command(tempQuery, "");

                // check if output is csv
                List<string> columnItems = outCommand.Split(",").ToList();
                if (columnItems.Count < 2) continue;

                // Add to list
                int IDObject = Convert.ToInt32(columnItems[0]);
                ControllerM_Phase newPhaseObject = CreateModelFromID(comm, IDObject);
                result.Add(newPhaseObject);
            }

            return result;
        }

        /// <summary>
        /// Loads ALL phases from local database, does not distinguish between databases
        /// </summary>
        /// <param name="comm"></param>
        /// <returns></returns>
        public static List<ControllerM_Phase> LoadPhases(IAMCore_Comm comm)
        {
            // get and load into local database available phase names from CALPHAD database
            string Query = "database_table_custom_query SELECT Phase.* FROM Phase";
            string outCommand = comm.run_lua_command(Query, "");
            List<string> rowItems = outCommand.Split("\n").ToList();

            return CreateControllersFromData(comm, rowItems);
        }

        /// <summary>
        /// Loads all phases used in a project
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDProject"></param>
        /// <returns></returns>
        public static List<ControllerM_Phase> UniquePhasesByIDProject(IAMCore_Comm comm, int IDProject)
        {
            // load from database using the unique query
            string Query = "database_table_custom_query SELECT DISTINCT Phase.*, SelectedPhases.IDPhase FROM Phase INNER JOIN SelectedPhases ON Phase.ID = SelectedPhases.IDPhase INNER JOIN \'Case\' ON \'Case\'.IDProject = " + IDProject;
            string outCommand = comm.run_lua_command(Query, "");
            List<string> rowItems = outCommand.Split("\n").ToList();

            return CreateControllersFromData(comm, rowItems);
        }

        /// <summary>
        /// Load phases used in a case object
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDCase"></param>
        /// <returns></returns>
        public static List<ControllerM_Phase> UniquePhasesByIDCase(IAMCore_Comm comm, int IDCase)
        {
            // load from database using the unique query
            string Query = "database_table_custom_query SELECT Phase.ID as IDP, Phase.Name, SelectedPhases.* FROM SelectedPhases INNER JOIN Phase ON Phase.ID=SelectedPhases.IDPhase WHERE IDCase = " + IDCase;
            string outCommand = comm.run_lua_command(Query, "");
            List<string> rowItems = outCommand.Split("\n").ToList();

            return CreateControllersFromData(comm, rowItems);
        }

        /// <summary>
        /// Function that loads by ID an phase controllerM object
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        private static ControllerM_Phase CreateModelFromID(IAMCore_Comm comm, int ID) 
        {          
            ControllerM_Phase refTemp = new(comm);
            refTemp.MCObject.ModelObject.ID = ID;
            refTemp.MCObject.LoadByIDAction.DoAction();

            return refTemp;
        }

        /// <summary>
        /// Helper function for creating ControllerM_Phase objects from list of csv rows.
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="rowItems"></param>
        /// <returns></returns>
        private static List<ControllerM_Phase> CreateControllersFromData(IAMCore_Comm comm, List<string> rowItems) 
        {
            List<ControllerM_Phase> result = new();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;

                // Add to list
                int IDObject = Convert.ToInt32(columnItems[0]);
                ControllerM_Phase newPhaseObject = CreateModelFromID(comm, IDObject);
                result.Add(newPhaseObject);
            }

            return result;
        }
        #endregion

        #endregion
    }
}
