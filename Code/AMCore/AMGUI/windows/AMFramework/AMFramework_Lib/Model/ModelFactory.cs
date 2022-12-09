using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AMFramework_Lib.Model
{
    internal class ModelFactory
    {

        public enum MODELS
        {
            NONE,
            ACTIVEPHASES,
            ACTIVEPHASESCONFIGURATION,
            ACTIVEPHASESELEMENTCOMPOSITION,
            CASE,
            CONFIGURATION,
            ELEMENT,
            ELEMENTCOMPOSITION,
            EQUILIBRIUMCONFIGURATION,
            EQUILIBRIUMPHASEFRACTION,
            HEATTREATMENT,
            HEATTREATMENTPROFILE,
            PHASE,
            PRECIPITATESIMULATIONDATA,
            PRECIPITATIONDOMAIN,
            PRECIPITATIONPHASE,
            PROJECT,
            SCHEILCONFIGURATION,
            SCHEILPHASEFRACTION,
            SELECTEDELEMENTS,
            SELECTEDPHASES
        }

        [Obsolete("Use GetType", true)]
        public Interfaces.Model_Interface? Get_model_byName(string modelName) 
        {
            Interfaces.Model_Interface? result = null;

            MODELS outEnum = MODELS.NONE;

            //bool findResult = Enum.TryParse(typeof(MODELS), modelName, true,out (object?)outEnum);

            switch (outEnum)
            {
                case MODELS.ACTIVEPHASES:
                    result = new Model.Model_ActivePhases();
                    break;
                case MODELS.ACTIVEPHASESCONFIGURATION:
                    result = new Model.Model_ActivePhasesConfiguration();
                    break;
                case MODELS.ACTIVEPHASESELEMENTCOMPOSITION:
                    result = new Model.Model_ActivePhasesElementComposition();
                    break;
                case MODELS.CASE:
                    result = new Model.Model_Case();
                    break;
                case MODELS.CONFIGURATION:
                    result = new Model.Model_configuration();
                    break;
                case MODELS.ELEMENT:
                    result = new Model.Model_Element();
                    break;
                case MODELS.ELEMENTCOMPOSITION:
                    result = new Model.Model_ElementComposition();
                    break;
                case MODELS.EQUILIBRIUMCONFIGURATION:
                    result = new Model.Model_EquilibriumConfiguration();
                    break;
                case MODELS.EQUILIBRIUMPHASEFRACTION:
                    result = new Model.Model_EquilibriumPhaseFraction();
                    break;
                case MODELS.HEATTREATMENT:
                    result = new Model.Model_HeatTreatment();
                    break;
                case MODELS.HEATTREATMENTPROFILE:
                    result = new Model.Model_HeatTreatmentProfile();
                    break;
                case MODELS.PHASE:
                    result = new Model.Model_Phase();
                    break;
                case MODELS.PRECIPITATESIMULATIONDATA:
                    result = new Model.Model_PrecipitateSimulationData();
                    break;
                case MODELS.PRECIPITATIONDOMAIN:
                    result = new Model.Model_PrecipitationDomain();
                    break;
                case MODELS.PRECIPITATIONPHASE:
                    result = new Model.Model_PrecipitationPhase();
                    break;
                case MODELS.PROJECT:
                    result = new Model.Model_Projects();
                    break;
                case MODELS.SCHEILCONFIGURATION:
                    result = new Model.Model_ScheilConfiguration();
                    break;
                case MODELS.SCHEILPHASEFRACTION:
                    result = new Model.Model_ScheilPhaseFraction();
                    break;
                case MODELS.SELECTEDELEMENTS:
                    result = new Model.Model_SelectedElements();
                    break;
                case MODELS.SELECTEDPHASES:
                    result = new Model.Model_SelectedPhases();
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Returns the model type that contains the name specified in containingstring
        /// </summary>
        /// <param name="ContainingString"></param>
        /// <returns></returns>
        public static Type? Get_Type(string ContainingString)
        {
            var listTypes = Assembly.GetExecutingAssembly().GetTypes().Where(e => e.Name.Contains(ContainingString, StringComparison.CurrentCultureIgnoreCase) && 
                                                                                  e.Name.Contains("Model_", StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (listTypes.Count == 0) return null;

            return listTypes[0];
        }

    }
}
