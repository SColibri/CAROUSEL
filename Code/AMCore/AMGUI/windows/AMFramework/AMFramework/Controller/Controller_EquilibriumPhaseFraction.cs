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
    public class Controller_EquilibriumPhaseFraction : ControllerAbstract
    {

        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_Cases _CaseController;
        public Controller_EquilibriumPhaseFraction(ref IAMCore_Comm socket, Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
        }
        #endregion

        #region Data
        List<Model_EquilibriumPhaseFraction> _equilibrium;
        public List<Model_EquilibriumPhaseFraction> Equilibrium
        {
            get { return _equilibrium; }
        }

        public List<Model_EquilibriumPhaseFraction> get_equilibrium_list(int IDCase)
        {
            List<Model_EquilibriumPhaseFraction> result = new();
            string Query = "database_table_custom_query SELECT EquilibriumPhaseFraction.*, Phase.Name FROM EquilibriumPhaseFraction INNER JOIN Phase ON Phase.ID=EquilibriumPhaseFraction.IDPhase WHERE IDCase = " + IDCase + " ORDER BY EquilibriumPhaseFraction.Temperature";
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

        private Model_EquilibriumPhaseFraction fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 6) throw new Exception("Error: Element RawData is wrong");

            Model_EquilibriumPhaseFraction modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDCase = Convert.ToInt32(DataRaw[1]),
                IDPhase = Convert.ToInt32(DataRaw[2]),
                //modely.TypeComposition = DataRaw[3];
                Temperature = Convert.ToDouble(DataRaw[3]),
                Value = Convert.ToDouble(DataRaw[4]),
                PhaseName = DataRaw[5]
            };

            return modely;
        }

        public void fill_models_with_phaseFractions()
        {
            foreach (Model_Case casey in _CaseController.Cases)
            {
                casey.EquilibriumPhaseFractionsOLD = get_equilibrium_list(casey.ID);
            }
        }

        public void fill_model_phase_fraction(Model_Case casey) 
        {
            casey.EquilibriumPhaseFractionsOLD = get_equilibrium_list(casey.ID);
        }

        public void clear_models_phaseFractions() 
        {
            foreach (Model_Case casey in _CaseController.Cases)
            {
                casey.EquilibriumPhaseFractionsOLD = new();
            }
        }
        #endregion

    }
}
