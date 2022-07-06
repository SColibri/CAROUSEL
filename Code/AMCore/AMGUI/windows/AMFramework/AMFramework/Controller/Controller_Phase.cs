using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    internal class Controller_Phase : INotifyPropertyChanged
    {
        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_Cases _CaseController;
        public Controller_Phase(ref Core.IAMCore_Comm socket, Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Data
        public static List<Model.Model_Phase> get_unique_phases_from_caseList(ref Core.IAMCore_Comm socket, int IDProject)
        {
            List<Model.Model_Phase> composition = new();
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
        public List<Model.Model_Phase> get_phases_from_case(int IDCase)
        {
            List<Model.Model_Phase> composition = new();
            string Query = "database_table_custom_query SELECT Phase.ID as IDP, Phase.Name, SelectedPhases.* FROM SelectedPhases INNER JOIN Phase ON Phase.ID=SelectedPhases.IDPhase WHERE IDCase = " + IDCase;
            string outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }
        public List<Model.Model_Phase> get_phaselist()
        {
            List<Model.Model_Phase> composition = new();
            string Query = "database_table_custom_query SELECT Phase.* FROM Phase";
            string outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        public static List<Model.Model_Phase> get_available_phases_in_database(ref Core.IAMCore_Comm socket)
        {
            // get available element names from database
            List<Model.Model_Phase> composition = new();
            string Query = "matcalc_database_phaseNames";
            string outCommand = socket.run_lua_command(Query,"");
            List<string> pahseList = outCommand.Split("\n").ToList();

            // get related ID's from database by given name, missing ID's are ignored
            if (pahseList.Count == 0) return composition;
            Query = "database_table_custom_query SELECT * FROM Phase WHERE ";
            for (int i = 2; i < pahseList.Count -1; i++)
            {
                string tempQuery = Query + " Name = '" + pahseList[i].Replace("\r", "") + "' ";
                outCommand = socket.run_lua_command(tempQuery,"");

                List<string> columnItems = outCommand.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }
        private static Model.Model_Phase fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 2) throw new Exception("Error: Element RawData is wrong");

            Model.Model_Phase modely = new();
            modely.ID = Convert.ToInt32(DataRaw[0]);
            modely.Name = DataRaw[1];

            return modely;
        }
        #endregion
    }
}
