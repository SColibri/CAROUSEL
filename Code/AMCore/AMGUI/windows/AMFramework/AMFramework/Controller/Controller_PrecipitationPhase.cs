using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    internal class Controller_PrecipitationPhase : INotifyPropertyChanged
    {

        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller.Controller_Cases _CaseController;
        public Controller_PrecipitationPhase(ref Core.IAMCore_Comm socket, Controller.Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
        }

        #endregion

        #region PrecipitationPhase
        private List<Model.Model_PrecipitationPhase> _precipitationPhases = new();
        public List<Model.Model_PrecipitationPhase> PrecipitationPhases 
        { 
            get { return _precipitationPhases; }
            set 
            {
                _precipitationPhases = value;
                OnPropertyChanged("PrecipitationPhases");
            }
        }

        public void Refresh() 
        {
            if (_CaseController.SelectedCase == null) return;
            PrecipitationPhases = Get_model(_AMCore_Socket, _CaseController.SelectedCase.ID);
        }

        public static List<Model.Model_PrecipitationPhase> Get_model(Core.IAMCore_Comm comm, int IDCase)
        {
            List<Model.Model_PrecipitationPhase> model = new();

            string Query = "SELECT PrecipitationPhase.* FROM PrecipitationPhase WHERE IDCase=" + IDCase;
            string outCommand = comm.run_lua_command("database_table_custom_query", Query);
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string rowItem in rowItems)
            {
                List<string> columnItems = rowItem.Split(",").ToList();
                if (columnItems.Count < 13) continue;

                model.Add(FillModel(columnItems));
            }

            return model;
        }

        private static Model.Model_PrecipitationPhase FillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 13) throw new Exception("Error: Element RawData is wrong");

            Model.Model_PrecipitationPhase model = new Model.Model_PrecipitationPhase();
            model.ID = Convert.ToInt32(DataRaw[0]);
            model.IDCase = Convert.ToInt32(DataRaw[1]);
            model.IDPhase = Convert.ToInt32(DataRaw[2]);
            model.NumberSizeClasses = Convert.ToInt16(DataRaw[3]);
            model.Name = DataRaw[4];
            model.NucleationSites = DataRaw[5];
            model.IDPrecipitationDomain = Convert.ToInt32(DataRaw[6]);
            model.CalcType = DataRaw[7];
            model.MinRadius = Convert.ToDouble(DataRaw[8]);
            model.MeanRadius = Convert.ToDouble(DataRaw[9]);
            model.MaxRadius = Convert.ToDouble(DataRaw[10]);
            model.StdDev = Convert.ToDouble(DataRaw[11]);
            model.PrecipitateDistribution = DataRaw[12];

            return model;
        }

        public static void Save(Core.IAMCore_Comm AMCore_Socket, Model.Model_PrecipitationDomain model)
        {
            AMCore_Socket.run_lua_command("spc_precipitation_phase_save", model.Get_csv());
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
