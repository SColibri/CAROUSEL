using AMFramework_Lib.Core;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Model.Model_Controllers
{
    public class ControllerM_Case : Controller_Abstract_Models<Model_Case>
    {
        public ControllerM_Case(IAMCore_Comm comm) : base(comm)
        {
            Add_SolidificationConfigurations();
        }
        public ControllerM_Case(IAMCore_Comm comm, ModelController<Model_Case> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods
        public void Load_ElementComposition() 
        {
            
            // TODO Refactor Case object
            //CaseMC.ModelObject.ElementComposition = ModelController<Model_ElementComposition>.LoadIDCase(ref _comm,CaseMC.ModelObject.ID);
        }

        #region ObjectPrivateMethods
        private void Add_SolidificationConfigurations()
        {
            MCObject.ModelObject.EquilibriumConfiguration = new(ref _comm);
            MCObject.ModelObject.ScheilConfiguration = new(ref _comm);
        }
        #endregion

        #region Save_templated_object
        public static void Save_Templated_Object(IAMCore_Comm comm, Model_Case modelCase) 
        {
            if (modelCase.IDProject == -1) throw new Exception("Case has no reference to any project, this is a sad case :´(");


           
        }

        public static List<ModelController<Model_Case>> Get_CasesByIDProject(IAMCore_Comm comm, int IDProject) 
        {
            // Load all cases
            List<ModelController<Model_Case>> result = ModelController<Model_Case>.LoadIDProject(ref comm, IDProject);

            // Load basic data
            foreach (var item in result)
            {
                Load_BasicData(comm, item);
            }

            return result;
        }

        /// <summary>
        /// Loads all lightweight data, no simulation data is loaded here
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="item"></param>
        private static void Load_BasicData(IAMCore_Comm comm, ModelController<Model_Case> item) 
        {
            // Get basic data
            item.ModelObject.SelectedPhases = ControllerM_SelectedPhases.Get_SelectedPhases_FromIDCase(comm, item.ModelObject.ID);
            item.ModelObject.ElementComposition = ControllerM_ElementComposition.Get_ElementComposition_FromIDCase(comm, item.ModelObject.ID);
            item.ModelObject.EquilibriumConfiguration = ControllerM_EquilibriumConfiguration.Get_EquilibriumConfiguration_FromIDCase(comm, item.ModelObject.ID);
            item.ModelObject.PrecipitationPhases = ControllerM_PrecipitationPhase.Get_PrecipitationPhases_FromIDCase(comm, item.ModelObject.ID);
            item.ModelObject.PrecipitationDomains = ControllerM_PrecipitationDomain.Get_PrecipitationDomains_FromIDCase(comm, item.ModelObject.ID);
            item.ModelObject.HeatTreatments = ControllerM_HeatTreatment.Get_heatTreatmentsFromCaseID(comm, item.ModelObject.ID);

            // We set Selected precipitation domain for all heatTreatments.
            foreach (var ht in item.ModelObject.HeatTreatments)
            {
                ht.ModelObject.PrecipitationDomain = item.ModelObject.PrecipitationDomains.Find(e => e.ModelObject.ID == ht.ModelObject.IDPrecipitationDomain);
            }
        }

        private static void LoadData_EquilibriumSolidificationSimulation(IAMCore_Comm comm, ModelController<Model_Case> item)
        {
            item.ModelObject.EquilibriumPhaseFractions = ControllerM_EquilibriumPhaseFraction.Get_EquilibriumPhaseFractions_FromIDCase(comm, item.ModelObject.ID);
        }

        public static List<ModelController<Model_ScheilPhaseFraction>> LoadData_ScheilSolidificationSimulation(IAMCore_Comm comm, ModelController<Model_Case> item)
        {
            return ControllerM_ScheilConfiguration.Get_ScheilSolidificationSimulation_FromIDCase(comm, item.ModelObject.ID);
        }

        private static void LoadData_PrecipitationSimulation(IAMCore_Comm comm, ModelController<Model_Case> item)
        {
            //item.ModelObject.kin = ControllerM_ScheilConfiguration.Get_ScheilSolidificationSimulation_FromIDCase(comm, item.ModelObject.ID);
        }
        #endregion

        #endregion



    }
}
