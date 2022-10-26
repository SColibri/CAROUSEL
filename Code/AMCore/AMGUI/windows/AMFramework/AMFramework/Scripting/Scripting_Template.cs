using AMFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Scripting
{
    /// <summary>
    /// Template class used for creating scripts used for saving single object classes using reflection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Scripting_Template<T> : Scripting_Abstract where T : Model_Interface
    {
        /// <summary>
        /// Model interface object
        /// </summary>
        private T _modelTemplate;

        /// <summary>
        /// After creating a variable we can further refer to it by its name
        /// </summary>
        private string _variableName;

        /// <summary>
        /// Constructor for Model Interface object
        /// </summary>
        /// <param name="refObject"></param>
        public Scripting_Template(T refObject) 
        { 
            _modelTemplate = refObject;
            _variableName = refObject.Get_Scripting_ClassName() + "_" + _count;

            _count++;
        }

        public override string Create_Object()
        {
            throw new NotImplementedException();
        }

        public override string Load_Object()
        {
            throw new NotImplementedException();
        }

        #region Scripting_Abstract_Implementation
        public override string ScriptText()
        {
            var pList = _modelTemplate.Get_parameter_list();
            
            string newScript = _variableName + " = " + _modelTemplate.Get_Scripting_ClassName() + ":new{}\n";
            foreach (var item in pList)
            {
                newScript += _variableName + "." + item.Name + " = " + item.GetValue(_modelTemplate) + "\n";
            }
            newScript += _variableName + ":save()";

            return newScript;
        }
        #endregion
    }
}
