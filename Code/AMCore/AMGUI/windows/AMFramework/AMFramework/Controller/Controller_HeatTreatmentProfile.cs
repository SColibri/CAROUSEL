using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    internal class Controller_HeatTreatmentProfile : INotifyPropertyChanged
    {
        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_HeatTreatment _heatTreatmentController;
        public Controller_HeatTreatmentProfile(ref Core.IAMCore_Comm socket, Controller_HeatTreatment heatTreatmentController)
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

        public static List<Model.Model_HeatTreatmentProfile> get_data_from_IDHeatTreatment(ref Core.IAMCore_Comm socket, int HeatT)
        {
            List<Model.Model_HeatTreatmentProfile> composition = new();
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

        private static Model.Model_HeatTreatmentProfile fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 4) throw new Exception("Error: Element RawData is wrong");

            Model.Model_HeatTreatmentProfile modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDHeatTreatment = (int)Convert.ToDouble(DataRaw[1]),
                Time = Convert.ToDouble(DataRaw[2]),
                Temperature = Convert.ToDouble(DataRaw[3])
            };

            return modely;
        }

        public static void fill_heatTreatment_model(ref Core.IAMCore_Comm socket, Model.Model_HeatTreatment ObjectModel)
        {
            ObjectModel.HeatTreatmentProfileOLD = get_data_from_IDHeatTreatment(ref socket, ObjectModel.ID);
        }
        #endregion
    }
}
