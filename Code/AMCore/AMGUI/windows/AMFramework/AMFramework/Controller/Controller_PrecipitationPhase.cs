using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_PrecipitationPhase : INotifyPropertyChanged
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

            string Query = "SELECT PrecipitationPhase.* , Phase.Name AS PhaseName, PrecipitationDomain.Name FROM PrecipitationPhase INNER JOIN Phase ON Phase.ID=PrecipitationPhase.IDPhase LEFT JOIN PrecipitationDomain ON PrecipitationDomain.ID=PrecipitationPhase.IDPrecipitationDomain WHERE IDCase=" + IDCase;
            string outCommand = comm.run_lua_command("database_table_custom_query", Query);
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string rowItem in rowItems)
            {
                List<string> columnItems = rowItem.Split(",").ToList();
                if (columnItems.Count < 14) continue;

                model.Add(FillModel(columnItems));
            }

            return model;
        }

        private static Model.Model_PrecipitationPhase FillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 14) throw new Exception("Error: Element RawData is wrong");

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
            model.PhaseName = DataRaw[13];

            return model;
        }

        public static void Save(Core.IAMCore_Comm AMCore_Socket, Model.Model_PrecipitationPhase model)
        {
            AMCore_Socket.run_lua_command("spc_precipitation_phase_save", model.Get_csv());
        }

        public void fill_models_with_precipitation_phases()
        {
            foreach (Model.Model_Case casey in _CaseController.Cases)
            {
                casey.PrecipitationPhases = Get_model(_AMCore_Socket, casey.ID);
            }
        }

        #region Handles
        public void Handle_ClickOnSave_AMButton(object sender, EventArgs e) 
        {
            if (!sender.GetType().Equals(typeof(Components.Button.AM_button))) return;
            if (!((Components.Button.AM_button)sender).Tag.GetType().Equals(typeof(Model.Model_PrecipitationPhase))) return;

            Model.Model_PrecipitationPhase phase = (Model.Model_PrecipitationPhase)((Components.Button.AM_button)sender).Tag;
            Save(_AMCore_Socket, phase);
        }
        #endregion
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
