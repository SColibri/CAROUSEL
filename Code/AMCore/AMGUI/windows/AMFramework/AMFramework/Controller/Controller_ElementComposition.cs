using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;

namespace AMFramework.Controller
{
    public class Controller_ElementComposition : ControllerAbstract
    {
        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_Cases _CaseController;
        public Controller_ElementComposition(ref IAMCore_Comm socket, Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
        }
        #endregion

        #region Data
        List<Model_ElementComposition> _composition;
        public List<Model_ElementComposition> Composition 
        { 
            get { return _composition; } 
        }

        public List<Model_ElementComposition> get_composition_list(int IDCase) 
        {
            List<Model_ElementComposition> composition = new();
            string Query = "database_table_custom_query SELECT ElementComposition.*, Element.Name FROM ElementComposition INNER JOIN Element ON Element.ID=ElementComposition.IDElement WHERE IDCase = " + IDCase;
            string outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 6) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        private Model_ElementComposition fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 6) throw new Exception("Error: Element RawData is wrong");

            Model_ElementComposition modely = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDCase = Convert.ToInt32(DataRaw[1]),
                IDElement = Convert.ToInt32(DataRaw[2]),
                TypeComposition = DataRaw[3],
                Value = Convert.ToDouble(DataRaw[4]),
                ElementName = DataRaw[5]
            };

            return modely;
        }

        public void fill_models_with_composition()
        {
            foreach (Model_Case casey in _CaseController.Cases)
            {
                casey.ElementCompositionOLD = get_composition_list(casey.ID);
            }
        }
        #endregion

    }
}
