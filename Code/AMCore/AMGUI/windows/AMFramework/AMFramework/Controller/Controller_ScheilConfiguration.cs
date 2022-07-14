using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

namespace AMFramework.Controller
{
    public class Controller_ScheilConfiguration : INotifyPropertyChanged
    {
        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_Cases _caseController;
        public Controller_ScheilConfiguration(ref Core.IAMCore_Comm socket, Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _caseController = caseController;
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Data
        Model.Model_ScheilConfiguration _model;
        public Model.Model_ScheilConfiguration Model { get { return _model; } }

        public void save(Model.Model_ScheilConfiguration model)
        {
            string outComm = _AMCore_Socket.run_lua_command("singlepixel_equilibrium_config_save " + model.get_csv(),"");
            if (outComm.CompareTo("OK") != 0)
            {
                MainWindow.notify.ShowBalloonTip(5000, "Error: Case was not saved", outComm, System.Windows.Forms.ToolTipIcon.Error);
            }
        }
        public Model.Model_ScheilConfiguration get_scheil_configuration_case(int IDCase) 
        {
            Model.Model_ScheilConfiguration model = new();
            string Query = "database_table_custom_query SELECT ScheilConfiguration.*, Phase.Name FROM ScheilConfiguration INNER JOIN Phase ON Phase.ID=SelectedPhases.DependentPhase WHERE IDCase = " + IDCase;
            string outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            if (rowItems.Count == 0) return model;
            List<string> columnItems = rowItems[0].Split(",").ToList();
            model = fillModel(columnItems);
            
            return model;
        }

        private static Model.Model_ScheilConfiguration fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 7) throw new Exception("Error: Element RawData is wrong");

            Model.Model_ScheilConfiguration modely = new();
            modely.ID = Convert.ToInt32(DataRaw[0]);
            modely.IDCase = Convert.ToInt32(DataRaw[1]);
            modely.StartTemperature = Convert.ToDouble(DataRaw[2]);
            modely.EndTemperature = Convert.ToDouble(DataRaw[3]);
            modely.StepSize = Convert.ToDouble(DataRaw[4]);
            modely.DependentPhase = Convert.ToInt32(DataRaw[5]);
            modely.MinLiquidFraction = Convert.ToDouble(DataRaw[6]);
            modely.DependentPhaseName = DataRaw[7];
            return modely;
        }
        #endregion
    }
}
