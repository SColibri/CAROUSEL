using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller 
{
    public class Controller_EquilibriumConfiguration : INotifyPropertyChanged
    {

        #region Socket
        private Core.AMCore_Socket _AMCore_Socket;
        private Controller_Cases _CaseController;
        public Controller_EquilibriumConfiguration(ref Core.AMCore_Socket socket, Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
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
        Model.Model_EquilibriumConfiguration _configuration;
        public Model.Model_EquilibriumConfiguration Configuration
        {
            get { return _configuration; }
        }

        public List<Model.Model_EquilibriumConfiguration> get_configuration_list(int IDCase)
        {
            List<Model.Model_EquilibriumConfiguration> composition = new();
            string Query = "database_table_custom_query SELECT EquilibriumConfiguration.* FROM EquilibriumConfiguration WHERE IDCase = " + IDCase;
            string outCommand = _AMCore_Socket.send_receive(Query);
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 6) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        private Model.Model_EquilibriumConfiguration fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 7) throw new Exception("Error: Element RawData is wrong");

            Model.Model_EquilibriumConfiguration modely = new();
            modely.ID = Convert.ToInt32(DataRaw[0]);
            modely.IDCase = Convert.ToInt32(DataRaw[1]);
            modely.Temperature = Convert.ToDouble(DataRaw[2]);
            modely.StartTemperature = Convert.ToDouble(DataRaw[3]);
            modely.EndTemperature = Convert.ToDouble(DataRaw[4]);
            modely.TemperatureType = DataRaw[5];
            modely.StepSize = Convert.ToDouble(DataRaw[6]);
            modely.Pressure = Convert.ToDouble(DataRaw[7]);

            return modely;
        }

        public void fill_models_with_equilibroiumConfiguration()
        {
            foreach (Model.Model_Case casey in _CaseController.Cases)
            {
                casey.EquilibriumConfiguration = get_configuration_list(casey.ID);
            }
        }
        #endregion

    }
}
