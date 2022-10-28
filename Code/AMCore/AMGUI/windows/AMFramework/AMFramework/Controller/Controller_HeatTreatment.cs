using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Model;
using AMFramework_Lib.Core;
using AMFramework_Lib.Controller;

namespace AMFramework.Controller
{
    internal class Controller_HeatTreatment : ControllerAbstract
    {
        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_Cases _caseController;
        public Controller_HeatTreatment(ref IAMCore_Comm socket, Controller_Cases projectController):base(socket)
        {
            _AMCore_Socket = socket;
            _caseController = projectController;
        }
        #endregion

        #region Data
        private List<Model_HeatTreatment> _heatTreatments = new();
        public List<Model_HeatTreatment> HeatTreatments { get { return _heatTreatments; } }
        public static List<Model_HeatTreatment> get_heat_treatments_from_case(ref IAMCore_Comm socket, int IDCase)
        {
            List<Model_HeatTreatment> composition = new();
            string Query = "database_table_custom_query SELECT HeatTreatment.* FROM HeatTreatment WHERE IDCase = " + IDCase;
            string outCommand = socket.run_lua_command(Query, "");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 6) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        private static Model_HeatTreatment fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 6) throw new Exception("Error: Element RawData is wrong");

            Model_HeatTreatment modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDCase = Convert.ToInt32(DataRaw[1]),
                Name = DataRaw[2],
                MaxTemperatureStep = Convert.ToInt32(DataRaw[3]),
                IDPrecipitationDomain = Convert.ToInt32(DataRaw[4]),
                StartTemperature = Convert.ToDouble(DataRaw[5])
            };

            return modely;
        }

        public static void fill_case_model(ref IAMCore_Comm socket, Model_Case ObjectModel) 
        {
            ObjectModel.HeatTreatmentsOLD = get_heat_treatments_from_case(ref socket, ObjectModel.ID);
        }
        #endregion
    }
}
