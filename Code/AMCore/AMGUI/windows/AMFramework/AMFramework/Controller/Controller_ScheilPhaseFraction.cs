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
    public class Controller_ScheilPhaseFraction : ControllerAbstract
    {


        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_Cases _CaseController;
        public Controller_ScheilPhaseFraction(ref IAMCore_Comm socket, Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
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
                OnPropertyChanged(nameof(PhaseFraction_DataCount));
            }
        }

        List<Model_ScheilPhaseFraction> _equilibrium = new();
        public List<Model_ScheilPhaseFraction> Equilibrium
        {
            get { return _equilibrium; }
        }

        public List<Model_ScheilPhaseFraction> get_equilibrium_list(int IDCase)
        {
            List<Model_ScheilPhaseFraction> result = new();
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

        private Model_ScheilPhaseFraction fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 6) throw new Exception("Error: Element RawData is wrong");

            Model_ScheilPhaseFraction modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDCase = Convert.ToInt32(DataRaw[1]),
                IDPhase = Convert.ToInt32(DataRaw[2]),
                TypeComposition = DataRaw[3],
                Temperature = Convert.ToDouble(DataRaw[4]),
                Value = Convert.ToDouble(DataRaw[5]),
                PhaseName = DataRaw[6]
            };

            return modely;
        }

        public void fill_models_with_phaseFractions()
        {
            foreach (Model_Case casey in _CaseController.CasesOLD)
            {
                casey.ScheilPhaseFractionsOLD = get_equilibrium_list(casey.ID);
            }
        }

        public void fill_model_phase_fraction(Model_Case casey)
        {
            casey.ScheilPhaseFractionsOLD = get_equilibrium_list(casey.ID);
        }

        public void clear_models_phaseFractions()
        {
            foreach (Model_Case casey in _CaseController.CasesOLD)
            {
                casey.ScheilPhaseFractionsOLD = new();
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
