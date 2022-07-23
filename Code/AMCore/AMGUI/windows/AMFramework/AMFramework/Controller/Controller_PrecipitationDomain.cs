using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_PrecipitationDomain : INotifyPropertyChanged
    {

        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller.Controller_Cases _CaseController;
        public Controller_PrecipitationDomain(ref Core.IAMCore_Comm socket, Controller.Controller_Cases caseController)
        {
            _AMCore_Socket = socket;
            _CaseController = caseController;
        }

        #endregion

        #region PrecipitationDomain
        private List<Model.Model_PrecipitationDomain> _precipitationDomains = new();
        public List<Model.Model_PrecipitationDomain> PrecipitationDomains
        {
            get { return _precipitationDomains; }
            set
            {
                _precipitationDomains = value;
                OnPropertyChanged("PrecipitationDomains");
            }
        }

        public void Refresh()
        {
            if (_CaseController.SelectedCase == null) return;
            PrecipitationDomains = Get_model(_AMCore_Socket, _CaseController.SelectedCase.ID);
        }

        public static List<Model.Model_PrecipitationDomain> Get_model(Core.IAMCore_Comm comm, int IDCase)
        {
            List<Model.Model_PrecipitationDomain> model = new();

            string Query = "SELECT PrecipitationDomain.* FROM PrecipitationDomain WHERE IDCase=" + IDCase;
            string outCommand = comm.run_lua_command("database_table_custom_query", Query);
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string rowItem in rowItems)
            {
                List<string> columnItems = rowItem.Split(",").ToList();
                if (columnItems.Count < 8) continue;

                model.Add(FillModel(columnItems));
            }

            return model;
        }

        private static Model.Model_PrecipitationDomain FillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 7) throw new Exception("Error: Element RawData is wrong");

            Model.Model_PrecipitationDomain model = new Model.Model_PrecipitationDomain();
            model.ID = Convert.ToInt32(DataRaw[0]);
            model.Name = DataRaw[1];
            model.IDPhase = Convert.ToInt32(DataRaw[2]);
            model.InitialGrainDiameter = Convert.ToDouble(DataRaw[3]);
            model.EquilibriumDiDe = Convert.ToDouble(DataRaw[4]);
            model.VacancyEvolutionModel = DataRaw[5];
            model.ConsiderExVa = Convert.ToInt16(DataRaw[6]);
            model.ExcessVacancyEfficiency = Convert.ToDouble(DataRaw[7]);

            return model;
        }

        public static void Save(Core.IAMCore_Comm AMCore_Socket, Model.Model_PrecipitationDomain model)
        {
            AMCore_Socket.run_lua_command("spc_precipitation_domain_save", model.Get_csv());
        }

        public void fill_models_with_precipitation_domains()
        {
            foreach (Model.Model_Case casey in _CaseController.Cases)
            {
                casey.PrecipitationDomains = Get_model(_AMCore_Socket, casey.ID);
            }
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
