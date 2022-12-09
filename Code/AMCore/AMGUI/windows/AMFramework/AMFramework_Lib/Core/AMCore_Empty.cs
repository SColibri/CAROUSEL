using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Core
{
    public class AMCore_Empty : IAMCore_Comm
    {
        public string run_lua_command(string command, string parameters)
        {
            return "Error: Empty Core object";
        }

        public void update_path(string apiPath)
        {
            throw new NotImplementedException();
        }
    }
}
