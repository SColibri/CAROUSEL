using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AMFramework.Controller
{
    internal class Controller_HeatTreatmentProfile : INotifyPropertyChanged
    {
        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_HeatTreatment _heatTreatmentController;
        public Controller_HeatTreatmentProfile(ref IAMCore_Comm socket, Controller_HeatTreatment heatTreatmentController)
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

        public static List<Model_HeatTreatmentProfile> get_data_from_IDHeatTreatment(ref IAMCore_Comm socket, int HeatT)
        {
            List<Model_HeatTreatmentProfile> composition = new();
            string Query = "database_table_custom_query SELECT HeatTreatmentProfile.* FROM HeatTreatmentProfile WHERE IDHeatTreatment = " + HeatT;
            string outCommand = socket.run_lua_command(Query, "");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 4) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        private static Model_HeatTreatmentProfile fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 4) throw new Exception("Error: Element RawData is wrong");

            Model_HeatTreatmentProfile modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDHeatTreatment = (int)Convert.ToDouble(DataRaw[1]),
                Time = Convert.ToDouble(DataRaw[2]),
                Temperature = Convert.ToDouble(DataRaw[3])
            };

            return modely;
        }

        public static void fill_heatTreatment_model(ref IAMCore_Comm socket, Model_HeatTreatment ObjectModel)
        {
            ObjectModel.HeatTreatmentProfileOLD = get_data_from_IDHeatTreatment(ref socket, ObjectModel.ID);
        }
        #endregion
    }
}
