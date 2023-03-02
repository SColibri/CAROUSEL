using AMFramework_Lib.Interfaces;

namespace AMFramework_Lib.Scripting.LUA
{
    /// <summary>
    /// Template class used for creating scripts used for saving single object classes using reflection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Scripting_LUA_Template<T> : Scripting_LUA_Abstract where T : Model_Interface
    {
        /// <summary>
        /// Model interface object
        /// </summary>
        private T _modelTemplate;

        /// <summary>
        /// Constructor for Model Interface object
        /// </summary>
        /// <param name="refObject"></param>
        public Scripting_LUA_Template(T refObject)
        {
            _modelTemplate = refObject;
            VariableName = refObject.Get_Scripting_ClassName() + "_" + _count;

            _count++;
        }

        /// <summary>
        /// Script that creates an object and lists all its parameters
        /// </summary>
        /// <returns></returns>
        public override string Create_Object()
        {
            var pList = _modelTemplate.Get_parameter_list();

            string newScript = VariableName + " = " + _modelTemplate.Get_Scripting_ClassName() + ":new{}\n";
            newScript += "do -- " + VariableName + "_Parameters\n";
            foreach (var item in pList)
            {
                newScript += VariableName + "." + item.Name + " = " + item.GetValue(_modelTemplate) + "\n";
            }
            newScript += "end\n";

            return newScript;
        }

        public override string Load_Object()
        {
            throw new NotImplementedException();
        }

        public override string Save_Object()
        {
            string newScript = VariableName + ":save()";

            return newScript;
        }

    }
}
