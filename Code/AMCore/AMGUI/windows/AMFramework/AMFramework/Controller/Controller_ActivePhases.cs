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
using AMFramework_Lib.Model.Model_Controllers;
using AMFramework_Lib.Controller;

namespace AMFramework.Controller
{
    /// <summary>
    /// Active phases controller
    /// </summary>
    public class Controller_ActivePhases : AMFramework_Lib.Controller.ControllerAbstract 
    {
        #region Constructor
        private IAMCore_Comm _AMCore_Socket;
        private Controller_Project _projectController;

        public Controller_ActivePhases(Controller_Project projectController)
        {
            _projectController = projectController;
        }
        #endregion


        #region Properties
        #region Activephases
        private List<ModelController<Model_ActivePhases>> _ActivePhases;
        /// <summary>
        /// List of all active phases
        /// </summary>
        public List<ModelController<Model_ActivePhases>> ActivePhases 
        { 
            get => _ActivePhases;
            set 
            {
                _ActivePhases = value;
                OnPropertyChanged(nameof(ActivePhases));
            }
        }

        /// <summary>
        /// Update active phases for current project
        /// </summary>
        private void RefreshActivePhases() 
        {
            if (_projectController.SelectedProject?.MCObject?.ModelObject == null) return;
            ActivePhases = ControllerM_ActivePhases.Get_ActivePhaseList(Controller_Global.ApiHandle, 
                                                    _projectController.SelectedProject.MCObject.ModelObject.ID);
        }
        #endregion
        #endregion

        #region ActivePhases
        private List<Model_ActivePhases> _ActivePhasesOLD;
        public List<Model_ActivePhases> ActivePhasesOLD 
        { 
            get { return _ActivePhasesOLD; } 
            set 
            { 
                _ActivePhasesOLD = value;
                OnPropertyChanged(nameof(ActivePhasesOLD));
            }
        }
        [Obsolete("Save is done in ModelController<>")]
        public void save(Model_ActivePhases model)
        {
            string outComm = _AMCore_Socket.run_lua_command("project_active_phases_save", model.Get_csv());
            if (outComm.CompareTo("-1") == 0)
            {
                MainWindow.notify.ShowBalloonTip(5000, "Error: Case was not saved", outComm, System.Windows.Forms.ToolTipIcon.Error);
            }
        }

        [Obsolete("Use ControllerM for all data manipulations")]
        public List<Model_ActivePhases> get_ativePhaselist()
        {
            List<Model_ActivePhases> composition = new();
            string Query = "database_table_custom_query SELECT ActivePhases.*, Phase.Name As PhaseName FROM ActivePhases INNER JOIN Phase ON Phase.ID=ActivePhases.IDPhase WHERE IDProject="; //_ProjectController.SelectedProject.ID;
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

        [Obsolete("This is done in ModelController when loading data")]
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
        /// <summary>
        /// Update values
        /// </summary>
        public override void Refresh() 
        {
            RefreshActivePhases();
        }
        #endregion
    }
}
