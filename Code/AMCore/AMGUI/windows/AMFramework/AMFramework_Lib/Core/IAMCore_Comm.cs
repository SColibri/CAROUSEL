using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Core
{
    public interface IAMCore_Comm
    {
        public string run_lua_command(string command, string parameters);
        public void update_path(string apiPath);
    }
}
