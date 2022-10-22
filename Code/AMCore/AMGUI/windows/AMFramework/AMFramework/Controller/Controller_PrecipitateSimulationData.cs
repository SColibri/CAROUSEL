using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    internal class Controller_PrecipitateSimulationData : INotifyPropertyChanged
    {

        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_HeatTreatment _heatTreatmentController;
        public Controller_PrecipitateSimulationData(ref Core.IAMCore_Comm socket, Controller_HeatTreatment heatTreatmentController)
        {
            _AMCore_Socket = socket;
            _heatTreatmentController = heatTreatmentController;
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Object

        public static List<Model.Model_PrecipitateSimulationData> get_data_from_IDHeatTreatment(ref Core.IAMCore_Comm socket, int IDCase)
        {
            List<Model.Model_PrecipitateSimulationData> composition = new();
            string Query = "database_table_custom_query SELECT PrecipitateSimulationData.*, PrecipitationPhase.Name FROM PrecipitateSimulationData INNER JOIN PrecipitationPhase ON PrecipitationPhase.ID = PrecipitateSimulationData.IDPrecipitationPhase WHERE IDHeatTreatment = " + IDCase;
            string outCommand = socket.run_lua_command(Query, "");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 8) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        private static Model.Model_PrecipitateSimulationData fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 8) throw new Exception("Error: Element RawData is wrong");

            Model.Model_PrecipitateSimulationData modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDPrecipitationPhase = Convert.ToInt32(DataRaw[1]),
                IDHeatTreatment = Convert.ToInt32(DataRaw[2]),
                Time = Convert.ToDouble(DataRaw[3]),
                PhaseFraction = Convert.ToDouble(DataRaw[4]),
                NumberDensity = Convert.ToDouble(DataRaw[5]),
                MeanRadius = Convert.ToDouble(DataRaw[6]),
                PrecipitationName = DataRaw[7]
            };

            return modely;
        }

        public static void fill_heatTreatment_model(ref Core.IAMCore_Comm socket, Model.Model_HeatTreatment ObjectModel)
        {
            ObjectModel.PrecipitationData = get_data_from_IDHeatTreatment(ref socket, ObjectModel.ID);
        }
        #endregion

    }
}
