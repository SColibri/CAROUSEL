using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Model;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;

namespace AMFramework.Controller
{
    public class Controller_PrecipitationDomain : ControllerAbstract
    {

        #region Socket
        private Controller.Controller_Cases _CaseController;
        public Controller_PrecipitationDomain(ref IAMCore_Comm comm, Controller.Controller_Cases caseController):base(comm)
        {
            _CaseController = caseController;
            
        }

        #endregion

        #region Parameters

        private List<ModelController<Model_PrecipitationDomain>> _precipitationDomains = new();
        public List<ModelController<Model_PrecipitationDomain>> PrecipitationDomains
        {
            get { return _precipitationDomains; }
            set
            {
                _precipitationDomains = value;
                OnPropertyChanged(nameof(PrecipitationDomains));
            }
        }

        #endregion

        #region PrecipitationDomain
        private List<Model_PrecipitationDomain> _precipitationDomainsOLD = new();
        public List<Model_PrecipitationDomain> PrecipitationDomainsOLD
        {
            get { return _precipitationDomainsOLD; }
            set
            {
                _precipitationDomainsOLD = value;
                OnPropertyChanged(nameof(PrecipitationDomainsOLD));
            }
        }

        public void Refresh()
        {
            if (_CaseController.SelectedCaseOLD == null) return;
            PrecipitationDomainsOLD = Get_model(_comm, _CaseController.SelectedCaseOLD.ID);
        }

        public static List<Model_PrecipitationDomain> Get_model(IAMCore_Comm comm, int IDCase)
        {
            List<Model_PrecipitationDomain> model = new();

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

        [Obsolete("This is now done by The modelcontroller<>")]
        private static Model_PrecipitationDomain FillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 7) throw new Exception("Error: Element RawData is wrong");

            Model_PrecipitationDomain model = new()
            {
                ID = Convert.ToInt32(DataRaw[0]),
                IDCase = Convert.ToInt32(DataRaw[1]),
                Name = DataRaw[2],
                IDPhase = Convert.ToInt32(DataRaw[3]),
                InitialGrainDiameter = Convert.ToDouble(DataRaw[4]),
                EquilibriumDiDe = Convert.ToDouble(DataRaw[5]),
                VacancyEvolutionModel = DataRaw[6],
                ConsiderExVa = Convert.ToInt16(DataRaw[7]),
                ExcessVacancyEfficiency = Convert.ToDouble(DataRaw[8])
            };

            return model;
        }

        [Obsolete("This is now donw by the modelController")]
        public static void Save(IAMCore_Comm AMCore_Socket, Model_PrecipitationDomain model)
        {
            AMCore_Socket.run_lua_command("spc_precipitation_domain_save", model.Get_csv());
        }

        [Obsolete("This is done by the modelcontrollers and the M controllers")]
        public void fill_models_with_precipitation_domains()
        {
            foreach (Model_Case casey in _CaseController.CasesOLD)
            {
                casey.PrecipitationDomainsOLD = Get_model(_comm, casey.ID);
            }
        }
        #endregion


    }
}
