using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Controller;

namespace AMFramework.Controller 
{
    public class Controller_EquilibriumConfiguration : ControllerAbstract
    {

        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_Cases _CaseController;
        public Controller_EquilibriumConfiguration(ref IAMCore_Comm socket, Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
        }
        #endregion

        #region Data
        Model_EquilibriumConfiguration _configuration;
        public Model_EquilibriumConfiguration Configuration
        {
            get { return _configuration; }
        }

        public void save(Model_EquilibriumConfiguration model)
        {
            string outComm = _AMCore_Socket.run_lua_command("singlepixel_equilibrium_config_save " + model.Get_csv(),"");
            if (outComm.CompareTo("OK") != 0)
            {
                MainWindow.notify.ShowBalloonTip(5000, "Error: Case was not saved", outComm, System.Windows.Forms.ToolTipIcon.Error);
            }
        }

        public Model_EquilibriumConfiguration get_configuration_list(int IDCase)
        {
            List<Model_EquilibriumConfiguration> composition = new();
            string Query = "database_table_custom_query SELECT EquilibriumConfiguration.* FROM EquilibriumConfiguration WHERE IDCase = " + IDCase;
            string outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 6) continue;
                composition.Add(fillModel(columnItems));
            }

            if (composition.Count == 0) return new();
            return composition[0];
        }

        private Model_EquilibriumConfiguration fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 7) throw new Exception("Error: Element RawData is wrong");

            Model_EquilibriumConfiguration modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDCase = Convert.ToInt32(DataRaw[1]),
                Temperature = Convert.ToDouble(DataRaw[2]),
                StartTemperature = Convert.ToDouble(DataRaw[3]),
                EndTemperature = Convert.ToDouble(DataRaw[4]),
                TemperatureType = DataRaw[5],
                StepSize = 25,//Convert.ToDouble(DataRaw[6]);
                Pressure = Convert.ToDouble(DataRaw[7])
            };

            return modely;
        }

        public void fill_models_with_equilibroiumConfiguration()
        {
            foreach (Model_Case casey in _CaseController.CasesOLD)
            {
                casey.EquilibriumConfigurationOLD = get_configuration_list(casey.ID);
            }
        }
        #endregion

    }
}
