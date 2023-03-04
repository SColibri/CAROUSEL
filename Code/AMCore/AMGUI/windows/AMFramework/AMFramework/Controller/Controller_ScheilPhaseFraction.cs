using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMFramework.Controller
{
    public class Controller_ScheilPhaseFraction : ControllerAbstract
    {


        #region Socket
        private Controller_Cases _CaseController;
        public Controller_ScheilPhaseFraction(ref IAMCore_Comm comm, Controller_Cases caseController) : base(comm)
        {
            _CaseController = caseController;

            if (caseController.SelectedCase != null)
                PhaseFractions = ControllerM_ScheilConfiguration.Get_ScheilSolidificationSimulation_FromIDCase(comm, caseController.SelectedCase.ModelObject.ID);
        }
        #endregion

        #region Data
        private int _phaseFraction_DataCount = 0;
        /// <summary>
        /// Returns the amount of data points
        /// </summary>
        public int PhaseFraction_DataCount
        {
            get { return _phaseFraction_DataCount; }
            set
            {
                _phaseFraction_DataCount = value;
                OnPropertyChanged(nameof(PhaseFraction_DataCount));
            }
        }

        List<ModelController<Model_ScheilPhaseFraction>> _phaseFractions = new();
        /// <summary>
        /// get/set list of phase fraction elements, these are datapoints that come from the simulation
        /// </summary>
        public List<ModelController<Model_ScheilPhaseFraction>> PhaseFractions
        {
            get => _phaseFractions;
            set
            {
                _phaseFractions = value;
                PhaseFraction_DataCount = _phaseFractions.Count;
                OnPropertyChanged(nameof(PhaseFractions));
            }
        }

        public int Phase_fraction_Data_Count_Load(int IDCase)
        {
            string Query = "database_table_custom_query SELECT COUNT(*) FROM SelectedPhases WHERE IDCase = " + IDCase;
            string outCommand = _comm.run_lua_command(Query, "");
            List<string> rowItems = outCommand.Split("\n").ToList();

            if (!rowItems[0].All(char.IsNumber)) return 0;

            return Convert.ToInt32(rowItems[0]);
        }
        #endregion
    }
}
