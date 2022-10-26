using AMFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Scripting
{
    internal class Scripting_case : Scripting_Abstract
    {
        private Model_Case _modelCase;

        public Scripting_case(Model_Case modelCase) 
        {
            _modelCase = modelCase;
            VariableName = modelCase.Get_Scripting_ClassName() + "_" + _count++;
        }

        public string Create_Ranged_Object(Scripting_Project sProject) 
        {
            string result = Create_Object() + 
                            Set_Phases() +
                            Set_Elements(sProject) +
                            Set_ElementComposition();

            return result;
        }

        public override string Create_Object() 
        {
            string result = "local " + VariableName + " = " + _modelCase.Get_Scripting_ClassName() + ":new{ Name = \"" + VariableName + "\"}\n";

            return result;
        }

        private string Set_Phases() 
        {
            string result = VariableName + ":set_phases_ByName(" ;

            string buildPhaseString = "";
            foreach (var item in _modelCase.SelectedPhases)
            {
                buildPhaseString += item.ModelObject.PhaseName + " ";
            }
            buildPhaseString = buildPhaseString.Trim();

            result += buildPhaseString + ")\n";
            return result;
        }

        private string Set_Elements(Scripting_Project sProject) 
        {
            string result = VariableName + ":set_element_composition_from_project(" + sProject.VariableName + ")\n";

            return result;
        }

        private string Set_ElementComposition() 
        {
            string result = "";

            foreach (var item in _modelCase.ElementComposition)
            {
                Scripting_AMRange? tempRange = null;
                if (item.ModelObject.StringValue.Length > 0) 
                {
                    tempRange = new(item.ModelObject.StringValue);
                    result += tempRange.Create_Object(); 
                }

                result += VariableName + ":find_composition_ByName(\"" + item.ModelObject.ElementName + "\").value = ";

                if (tempRange != null) result += tempRange.VariableName;
                else result += item.ModelObject.Value;
            }

            return result;
        }

        public override string ScriptText()
        {
            throw new NotImplementedException();
        }

        public override string Load_Object()
        {
            throw new NotImplementedException();
        }
    }
}
