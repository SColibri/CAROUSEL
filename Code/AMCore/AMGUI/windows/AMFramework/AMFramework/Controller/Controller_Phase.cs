﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using AMFramework_Lib.Model.Model_Controllers;
using AMFramework_Lib.Model;
using AMFramework_Lib.Core;
using AMFramework_Lib.Controller;

namespace AMFramework.Controller
{
    internal class Controller_Phase : ControllerAbstract
    {
        #region Socket
        private Controller_Cases _CaseController;

        [Obsolete("Controller is independent from other controllers, bad design for the phase object")]
        public Controller_Phase(ref IAMCore_Comm socket, Controller_Cases caseController):base(socket)
        {
            _CaseController = caseController;
        }

        public Controller_Phase(IAMCore_Comm comm) : base(comm)
        { 
            
        }

        #endregion

        #region Properties

        #region SearchText
        private string _searchText = "";
        /// <summary>
        /// Used for search bars it filters the visible objects
        /// </summary>
        public string SearchString
        {
            get { return _searchText; }
            set 
            {
                _searchText = value;
                SearchText_Action(); 
                OnPropertyChanged(nameof(SearchString));
            }
        }

        private void SearchText_Action()
        {
            if (SearchString.Length == 0) 
            {
                foreach (var item in PhaseList) 
                {
                    item.MCObject.ModelObject.IsVisible = true;
                }
                return;
            }

            foreach (var item in PhaseList)
            {
                if (item.MCObject.ModelObject.Name.ToUpper().Contains(SearchString.ToUpper()) == true)
                {
                    item.MCObject.ModelObject.IsVisible = true;
                }
                else 
                {
                    item.MCObject.ModelObject.IsVisible = false;
                }
            }
        }
        #endregion

        #region PhaseList
        private List<ControllerM_Phase> _phaseList = new();
        /// <summary>
        /// Represents a list of all phases available in current selected database
        /// </summary>
        public List<ControllerM_Phase> PhaseList
        {
            get { return _phaseList; }
            set 
            {
                _phaseList = value;
                OnPropertyChanged(nameof(PhaseList));
            }
        }

        /// <summary>
        /// Returns all selected phases
        /// </summary>
        /// <returns></returns>
        public List<ControllerM_Phase> Get_Selected() 
        {
            return PhaseList.FindAll(e => e.MCObject.ModelObject.IsSelected);
        }

        #region LoadData
        /// <summary>
        /// Loads phases from CALPHAD database
        /// </summary>
        public void LoadFromDatabase() 
        {
            PhaseList = ControllerM_Phase.LoadFromDatabase(_comm);
        }

        /// <summary>
        /// Loads ALL phases from local database
        /// </summary>
        public void LoadAll() 
        {
            PhaseList = ControllerM_Phase.LoadPhases(_comm);
        }

        /// <summary>
        /// Loads Phases used in project
        /// </summary>
        /// <param name="IDProject"></param>
        public void LoadByIDProject(int IDProject) 
        {
            PhaseList = ControllerM_Phase.UniquePhasesByIDProject(_comm, IDProject);
        }

        /// <summary>
        /// Loads phases used in case
        /// </summary>
        /// <param name="IDCase"></param>
        public void LoadByIDCase(int IDCase) 
        {
            PhaseList = ControllerM_Phase.UniquePhasesByIDCase(_comm, IDCase);
        }



        #endregion


        #endregion

        #endregion

        #region commands

        #endregion

        #region Obsolete
        //--------------------------------------------------------------------------------
        //       OBSOLETE FUNCTIONS TODO: Remove all dependencies
        //-------------------------------------------------------------------------------


        /// <summary>
        /// Loads all phases from the database
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [Obsolete("We should now return the M controllers: ControllerM_Phase")]
        public static List<Model_Phase> Get_available_phases_in_database(ref IAMCore_Comm socket)
        {
            // get available element names from database
            List<Model_Phase> composition = new();
            string Query = "matcalc_database_phaseNames";
            string outCommand = socket.run_lua_command(Query, "");
            List<string> pahseList = outCommand.Split("\n").ToList();

            // get related ID's from database by given name, missing ID's are ignored
            if (pahseList.Count == 0) return composition;
            Query = "database_table_custom_query SELECT * FROM Phase WHERE ";
            for (int i = 2; i < pahseList.Count - 1; i++)
            {
                string tempQuery = Query + " Name = '" + pahseList[i].Replace("\r", "") + "' ";
                outCommand = socket.run_lua_command(tempQuery, "");

                List<string> columnItems = outCommand.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        [Obsolete("Models are now loaded using the ModelAbstract object and model core executors")]
        private static Model_Phase fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 2) throw new Exception("Error: Element RawData is wrong");

            Model_Phase modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                Name = DataRaw[1]
            };

            return modely;
        }

        [Obsolete("Use ControllerM to call this function")]
        public static List<Model_Phase> get_unique_phases_from_caseList(ref IAMCore_Comm socket, int IDProject)
        {
            List<Model_Phase> composition = new();
            string Query = "database_table_custom_query SELECT DISTINCT Phase.*, SelectedPhases.IDPhase FROM Phase INNER JOIN SelectedPhases ON Phase.ID = SelectedPhases.IDPhase INNER JOIN \'Case\' ON \'Case\'.IDProject = " + IDProject;

            string outCommand = socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }
        [Obsolete("Use controllerM to call this function")]
        public List<Model_Phase> get_phases_from_case(int IDCase)
        {
            List<Model_Phase> composition = new();
            string Query = "database_table_custom_query SELECT Phase.ID as IDP, Phase.Name, SelectedPhases.* FROM SelectedPhases INNER JOIN Phase ON Phase.ID=SelectedPhases.IDPhase WHERE IDCase = " + IDCase;
            string outCommand = _comm.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }
        [Obsolete("Use controllerM to call this function")]
        public List<Model_Phase> get_phaselist()
        {
            List<Model_Phase> composition = new();
            string Query = "database_table_custom_query SELECT Phase.* FROM Phase";
            string outCommand = _comm.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        
        #endregion
    }
}
