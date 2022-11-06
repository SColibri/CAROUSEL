using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Scripting.LUA
{
    public class Scripting_LUA_Case : Scripting_LUA_Abstract
    {
        private Model_Case _modelCase;
        private List<Tuple<IScripting, Model_PrecipitationDomain>> _pDomains = new();

        public Scripting_LUA_Case(Model_Case modelCase)
        {
            _modelCase = modelCase;
            VariableName = modelCase.Get_Scripting_ClassName() + "_" + _count++;
        }

        public string Create_Ranged_Object(Scripting_LUA_Project sProject)
        {
            string result = Create_Object() + "\n" +
                            Set_Phases() + "\n" +
                            Set_Elements(sProject) + "\n" +
                            Set_ScheilConfiguration() + "\n" +
                            Set_EquilibriumConfiguration() + "\n" +
                            Set_ElementComposition() + "\n" +
                            Set_PrecipitationDomains();

            return result;
        }

        public override string Create_Object()
        {
            string result = "local " + VariableName + " = " + _modelCase.Get_Scripting_ClassName() + ":new{ Name = \"" + VariableName + "\"}\n";

            return result;
        }

        private string Set_Phases()
        {
            string result = VariableName + ":set_phases_ByName(\"";

            string buildPhaseString = "";
            foreach (var item in _modelCase.SelectedPhases)
            {
                buildPhaseString += item.ModelObject.PhaseName + " ";
            }
            buildPhaseString = buildPhaseString.Trim();

            result += buildPhaseString + "\")\n";
            return result;
        }

        private string Set_Elements(Scripting_LUA_Project sProject)
        {
            string result = VariableName + ":set_element_composition_from_project(" + sProject.VariableName + ")\n";

            return result;
        }

        private string Set_ElementComposition()
        {
            string result = "";

            foreach (var item in _modelCase.ElementComposition)
            {
                string decalerVar = VariableName + ":find_composition_ByName(\"" + item.ModelObject.ElementName + "\").value = ";

                if (!double.TryParse(item.ModelObject.StringValue, out double tryParseValue))
                {
                    Scripting_LUA_AMRange? tempRange = null;
                    tempRange = new(item.ModelObject.StringValue);
                    result += "\n" + tempRange.Create_Object() + decalerVar + tempRange.VariableName + '\n';
                }
                else
                {
                    result += decalerVar + tryParseValue + '\n';
                }
            }

            return result;
        }

        private string Set_ScheilConfiguration()
        {
            string result = "";

            if (_modelCase.ScheilConfiguration == null) return result;
            Scripting_LUA_Template<Model_ScheilConfiguration> sMSC = new(_modelCase.ScheilConfiguration.ModelObject);

            result = sMSC.Create_Object() + "\n";
            result += VariableName + "." + _modelCase.ScheilConfiguration.ModelObject.Get_Scripting_ClassName() + " = " + sMSC.VariableName + "\n";

            return result;
        }

        private string Set_EquilibriumConfiguration()
        {
            string result = "";

            if (_modelCase.EquilibriumConfiguration == null) return result;
            Scripting_LUA_Template<Model_EquilibriumConfiguration> sMSC = new(_modelCase.EquilibriumConfiguration.ModelObject);

            result = sMSC.Create_Object() + "\n";
            result += VariableName + "." + _modelCase.EquilibriumConfiguration.ModelObject.Get_Scripting_ClassName() + " = " + sMSC.VariableName + "\n";

            return result;
        }

        private string Set_PrecipitationDomains()
        {
            string result = "";

            foreach (var item in _modelCase.PrecipitationDomains)
            {
                Scripting_LUA_Template<Model_PrecipitationDomain> sPD = new(item.ModelObject);
                result += sPD.Create_Object();
                result += VariableName + ".precipitationDomain = " + sPD.VariableName + "\n\n";

                _pDomains.Add(new(sPD, item.ModelObject));
            }

            return result;
        }

        private string Set_HeatTreatments()
        {
            return "";
        }
        public override string Load_Object()
        {
            throw new NotImplementedException();
        }

        public override string Save_Object()
        {
            throw new NotImplementedException();
        }
    }
}
