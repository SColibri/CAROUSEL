using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Model;
using AMFramework_Lib.Core;

namespace AMFramework.Controller
{
    public class Controller_ActivePhases : INotifyPropertyChanged
    {
        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_DBS_Projects _ProjectController;
        public Controller_ActivePhases(ref IAMCore_Comm socket, Controller_DBS_Projects projectController)
        {
            _AMCore_Socket = socket;
            _ProjectController = projectController;
        }
        #endregion

        #region ActivePhases
        private List<Model_ActivePhases> _ActivePhases;
        public List<Model_ActivePhases> ActivePhases 
        { 
            get { return _ActivePhases; } 
            set 
            { 
                _ActivePhases = value;
                OnPropertyChanged(nameof(ActivePhases));
            }
        }

        public void save(Model_ActivePhases model)
        {
            string outComm = _AMCore_Socket.run_lua_command("project_active_phases_save", model.Get_csv());
            if (outComm.CompareTo("-1") == 0)
            {
                MainWindow.notify.ShowBalloonTip(5000, "Error: Case was not saved", outComm, System.Windows.Forms.ToolTipIcon.Error);
            }
        }

        public List<Model_ActivePhases> get_ativePhaselist()
        {
            List<Model_ActivePhases> composition = new();
            string Query = "database_table_custom_query SELECT ActivePhases.*, Phase.Name As PhaseName FROM ActivePhases INNER JOIN Phase ON Phase.ID=ActivePhases.IDPhase WHERE IDProject=" + _ProjectController.SelectedProject.ID;
            string outCommand = _AMCore_Socket.run_lua_command(Query, "");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        private static Model_ActivePhases fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 4) throw new Exception("Error: Element RawData is wrong");

            Model_ActivePhases modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDProject = Convert.ToInt32(DataRaw[1]),
                IDPhase = Convert.ToInt32(DataRaw[2]),
                PhaseName = DataRaw[3]
            };

            return modely;
        }
        #endregion

        #region Methods
        public void Refresh() 
        {
            ActivePhases = get_ativePhaselist();
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
