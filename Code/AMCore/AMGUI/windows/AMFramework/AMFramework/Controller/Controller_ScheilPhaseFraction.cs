using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_ScheilPhaseFraction : INotifyPropertyChanged
    {


        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_Cases _CaseController;
        public Controller_ScheilPhaseFraction(ref Core.IAMCore_Comm socket, Controller_Cases caseController)
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
        private int _phaseFraction_DataCount = 0;
        public int PhaseFraction_DataCount 
        { 
            get { return _phaseFraction_DataCount; } 
            set 
            { 
                _phaseFraction_DataCount = value;
                OnPropertyChanged("PhaseFraction_DataCount");
            }
        }

        List<Model.Model_ScheilPhaseFraction> _equilibrium = new();
        public List<Model.Model_ScheilPhaseFraction> Equilibrium
        {
            get { return _equilibrium; }
        }

        public List<Model.Model_ScheilPhaseFraction> get_equilibrium_list(int IDCase)
        {
            List<Model.Model_ScheilPhaseFraction> result = new();
            string Query = "database_table_custom_query SELECT ScheilPhaseFraction.*, Phase.Name FROM ScheilPhaseFraction INNER JOIN Phase ON Phase.ID=ScheilPhaseFraction.IDPhase WHERE IDCase = " + IDCase;
            string outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 6) continue;
                result.Add(fillModel(columnItems));
            }

            return result;
        }

        private Model.Model_ScheilPhaseFraction fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 6) throw new Exception("Error: Element RawData is wrong");

            Model.Model_ScheilPhaseFraction modely = new();
            modely.ID = Convert.ToInt32(DataRaw[0]);
            modely.IDCase = Convert.ToInt32(DataRaw[1]);
            modely.IDPhase = Convert.ToInt32(DataRaw[2]);
            modely.TypeComposition = DataRaw[3];
            modely.Temperature = Convert.ToDouble(DataRaw[4]);
            modely.Value = Convert.ToDouble(DataRaw[5]);
            modely.PhaseName = DataRaw[6];

            return modely;
        }

        public void fill_models_with_phaseFractions()
        {
            foreach (Model.Model_Case casey in _CaseController.Cases)
            {
                casey.ScheilPhaseFractions = get_equilibrium_list(casey.ID);
            }
        }

        public void fill_model_phase_fraction(Model.Model_Case casey)
        {
            casey.ScheilPhaseFractions = get_equilibrium_list(casey.ID);
        }

        public void clear_models_phaseFractions()
        {
            foreach (Model.Model_Case casey in _CaseController.Cases)
            {
                casey.ScheilPhaseFractions = new();
            }
        }

        public int Phase_fraction_Data_Count_Load(int IDCase) 
        {
            string Query = "database_table_custom_query SELECT COUNT(*) FROM SelectedPhases WHERE IDCase = " + IDCase;
            string outCommand = _AMCore_Socket.run_lua_command(Query, "");
            List<string> rowItems = outCommand.Split("\n").ToList();

            if(!rowItems[0].All(char.IsNumber)) return 0;

            return Convert.ToInt32(rowItems[0]);
        }
        #endregion
    }
}
