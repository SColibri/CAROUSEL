using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_Selected_Phases : INotifyPropertyChanged
    {

        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller.Controller_Cases _CaseController;
        public Controller_Selected_Phases(ref Core.IAMCore_Comm socket, Controller.Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
        }

        #endregion

        #region Data
        private List<Model.Model_SelectedPhases> _Phases = new();
        public List<Model.Model_SelectedPhases> Phases
        {
            get { return _Phases; }
        }

        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Methods
        public void refresh()
        {
            load_phases();
        }
        private void load_phases()
        {
            string Query = "database_table_custom_query SELECT SelectedPhases.*, Phase.Name FROM SelectedPhases INNER JOIN Element ON Phase.ID=SelectedPhases.IDPhase WHERE IDProject = " + _CaseController.SelectedCase.ID.ToString();
            string outy = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outy.Split("\n").ToList();
            _Phases.Clear();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();


                if (columnItems.Count > 2)
                {
                    Model.Model_SelectedPhases model = new Model.Model_SelectedPhases();
                    model.ID = Convert.ToInt32(columnItems[0]);
                    model.IDCase = Convert.ToInt32(columnItems[1]);
                    model.IDPhase = Convert.ToInt32(columnItems[2]);
                    model.PhaseName = columnItems[3];

                    _Phases.Add(model);
                }
            }

            OnPropertyChanged("Phases");
        }

        private List<Model.Model_SelectedPhases> get_phas_list(int IDCase)
        {
            string Query = "database_table_custom_query SELECT SelectedPhases.*, Phase.Name FROM SelectedPhases INNER JOIN Phase ON Phase.ID=SelectedPhases.IDPhase WHERE IDCase = " + IDCase;
            string outy = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outy.Split("\n").ToList();
            List<Model.Model_SelectedPhases> PhaseList = new();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();


                if (columnItems.Count > 2)
                {
                    Model.Model_SelectedPhases model = new Model.Model_SelectedPhases();
                    model.ID = Convert.ToInt32(columnItems[0]);
                    model.IDCase = Convert.ToInt32(columnItems[1]);
                    model.IDPhase = Convert.ToInt32(columnItems[2]);
                    model.PhaseName = columnItems[3];

                    PhaseList.Add(model);
                }
            }

            return PhaseList;
        }

        public void fill_models_with_selectedPhases() 
        {
            foreach (Model.Model_Case casey in _CaseController.Cases) 
            {
                casey.SelectedPhases = get_phas_list(casey.ID);
            }
        }

        #endregion

    }
}
