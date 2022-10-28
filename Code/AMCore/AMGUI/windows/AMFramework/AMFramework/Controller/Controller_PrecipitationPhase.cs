using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Model;
using AMFramework_Lib.Core;

namespace AMFramework.Controller
{
    public class Controller_PrecipitationPhase : INotifyPropertyChanged
    {

        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller.Controller_Cases _CaseController;
        public Controller_PrecipitationPhase(ref IAMCore_Comm socket, Controller.Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
        }

        #endregion

        #region PrecipitationPhase
        private List<Model_PrecipitationPhase> _precipitationPhases = new();
        public List<Model_PrecipitationPhase> PrecipitationPhases 
        { 
            get { return _precipitationPhases; }
            set 
            {
                _precipitationPhases = value;
                OnPropertyChanged(nameof(PrecipitationPhases));
            }
        }

        public void Refresh() 
        {
            if (_CaseController.SelectedCaseOLD == null) return;
            PrecipitationPhases = Get_model(_AMCore_Socket, _CaseController.SelectedCaseOLD.ID);
        }

        public static List<Model_PrecipitationPhase> Get_model(IAMCore_Comm comm, int IDCase)
        {
            List<Model_PrecipitationPhase> model = new();

            string Query = "SELECT PrecipitationPhase.* , Phase.Name AS PhaseName, PrecipitationDomain.Name FROM PrecipitationPhase INNER JOIN Phase ON Phase.ID=PrecipitationPhase.IDPhase LEFT JOIN PrecipitationDomain ON PrecipitationDomain.ID=PrecipitationPhase.IDPrecipitationDomain WHERE PrecipitationPhase.IDCase=" + IDCase;
            string outCommand = comm.run_lua_command("database_table_custom_query", Query);
            List<string> rowItems = outCommand.Split(",\n").ToList();

            foreach (string rowItem in rowItems)
            {
                List<string> columnItems = rowItem.Split(",").ToList();
                if (columnItems.Count < 14) continue;

                model.Add(FillModel(columnItems));
            }

            return model;
        }

        public static List<string> Get_phases_names(IAMCore_Comm comm, int IDProject)
        {
            List<string> Result = new();

            string Query = "SELECT `Case`.ID FROM `Case` WHERE `Case`.IDProject=" + IDProject;
            string outCommand = comm.run_lua_command("database_table_custom_query", Query);
            List<string> rowItems = outCommand.Split(",\n").ToList();
            List<string> cells = rowItems[0].Split(",").ToList();

            Query = "SELECT PrecipitationPhase.Name FROM PrecipitationPhase WHERE IDCase = " + cells[0];

            foreach (var item in rowItems.Skip(1))
            {
                cells = item.Split(",").ToList();
                Query += " OR IDCase = " + cells[0];
            }

            outCommand = comm.run_lua_command("database_table_custom_query", Query);
            rowItems = outCommand.Split(",\n").ToList();

            foreach (var item in rowItems)
            {
                cells = item.Split(",").ToList();
                Result.Add(cells[0]);
            }

            return Result;
        }

        private static Model_PrecipitationPhase FillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 14) throw new Exception("Error: Element RawData is wrong");

            Model_PrecipitationPhase model = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDCase = Convert.ToInt32(DataRaw[1]),
                IDPhase = Convert.ToInt32(DataRaw[2]),
                NumberSizeClasses = Convert.ToInt16(DataRaw[3]),
                Name = DataRaw[4],
                NucleationSites = DataRaw[5],
                IDPrecipitationDomain = Convert.ToInt32(DataRaw[6]),
                CalcType = DataRaw[7],
                MinRadius = Convert.ToDouble(DataRaw[8]),
                MeanRadius = Convert.ToDouble(DataRaw[9]),
                MaxRadius = Convert.ToDouble(DataRaw[10]),
                StdDev = Convert.ToDouble(DataRaw[11]),
                PrecipitateDistribution = DataRaw[12],
                PhaseName = DataRaw[13]
            };

            return model;
        }

        public static void Save(IAMCore_Comm AMCore_Socket, Model_PrecipitationPhase model)
        {
            AMCore_Socket.run_lua_command("spc_precipitation_phase_save", model.Get_csv());
        }

        public void fill_models_with_precipitation_phases()
        {
            foreach (Model_Case casey in _CaseController.Cases)
            {
                casey.PrecipitationPhasesOLD = Get_model(_AMCore_Socket, casey.ID);
            }
        }

        #region Handles
        public void Handle_ClickOnSave_AMButton(object sender, EventArgs e) 
        {
            if (!sender.GetType().Equals(typeof(Components.Button.AM_button))) return;
            if (!((Components.Button.AM_button)sender).Tag.GetType().Equals(typeof(Model_PrecipitationPhase))) return;

            Model_PrecipitationPhase phase = (Model_PrecipitationPhase)((Components.Button.AM_button)sender).Tag;
            Save(_AMCore_Socket, phase);
        }
        #endregion
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
