using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_ElementComposition : INotifyPropertyChanged
    {
        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_Cases _CaseController;
        public Controller_ElementComposition(ref Core.IAMCore_Comm socket, Controller_Cases caseController)
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
        List<Model.Model_ElementComposition> _composition;
        public List<Model.Model_ElementComposition> Composition 
        { 
            get { return _composition; } 
        }

        public List<Model.Model_ElementComposition> get_composition_list(int IDCase) 
        {
            List<Model.Model_ElementComposition> composition = new();
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

        private Model.Model_ElementComposition fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 6) throw new Exception("Error: Element RawData is wrong");

            Model.Model_ElementComposition modely = new();
            modely.ID = Convert.ToInt32(DataRaw[0]);
            modely.IDCase = Convert.ToInt32(DataRaw[1]);
            modely.IDElement = Convert.ToInt32(DataRaw[2]);
            modely.TypeComposition = DataRaw[3];
            modely.Value = Convert.ToDouble(DataRaw[4]);
            modely.ElementName = DataRaw[5];

            return modely;
        }

        public void fill_models_with_composition()
        {
            foreach (Model.Model_Case casey in _CaseController.Cases)
            {
                casey.ElementComposition = get_composition_list(casey.ID);
            }
        }
        #endregion

    }
}
