using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.AMSystem
{
    public class AMFramework_startup
    {
        public static void Start(ref Core.IAMCore_Comm comm) 
        {
            AMFramework_StaticLoader.Load_Model_Commands(ref comm);
        }
    }
}
