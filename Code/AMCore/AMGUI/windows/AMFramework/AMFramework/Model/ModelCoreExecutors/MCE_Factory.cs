using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AMFramework.Model.ModelCoreExecutors
{
    public class MCE_Factory
    {
        /// <summary>
        /// Returns the MCE type that contains the name specified in containing string
        /// </summary>
        /// <param name="ContainingString"></param>
        /// <returns></returns>
        public static Type? Get_Type(string ContainingString) 
        { 
            var listTypes = Assembly.GetExecutingAssembly().GetTypes().Where(e => e.Name.Contains(ContainingString, StringComparison.CurrentCultureIgnoreCase) && 
                                                                                  e.Name.Contains("MCE_",StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (listTypes.Count == 0) return null;
            
            return listTypes[0];
        }

    }
}
