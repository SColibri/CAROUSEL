using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AMFramework.Controller
{
    internal class Controller_PrecipitateSimulationData : ControllerAbstract
    {

        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_HeatTreatment _heatTreatmentController;
        public Controller_PrecipitateSimulationData(ref IAMCore_Comm socket, Controller_HeatTreatment heatTreatmentController)
        {
            _AMCore_Socket = socket;
            _heatTreatmentController = heatTreatmentController;
        }
        #endregion

        #region Object

        public static List<Model_PrecipitateSimulationData> get_data_from_IDHeatTreatment(ref IAMCore_Comm socket, int IDCase)
        {
            List<Model_PrecipitateSimulationData> composition = new();
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

        private static Model_PrecipitateSimulationData fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 8) throw new Exception("Error: Element RawData is wrong");

            Model_PrecipitateSimulationData modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDPrecipitationPhase = Convert.ToInt32(DataRaw[1]),
                IDHeatTreatment = Convert.ToInt32(DataRaw[2]),
                Time = Convert.ToDouble(DataRaw[3], CultureInfo.InvariantCulture),
                PhaseFraction = Convert.ToDouble(DataRaw[4], CultureInfo.InvariantCulture),
                NumberDensity = Convert.ToDouble(DataRaw[5], CultureInfo.InvariantCulture),
                MeanRadius = Convert.ToDouble(DataRaw[6], CultureInfo.InvariantCulture),
                PrecipitationName = DataRaw[7]
            };

            return modely;
        }

        public static void fill_heatTreatment_model(ref IAMCore_Comm socket, Model_HeatTreatment ObjectModel)
        {
            ObjectModel.PrecipitationDataOLD = get_data_from_IDHeatTreatment(ref socket, ObjectModel.ID);
        }
        #endregion

    }
}
