using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Core
{
    public interface IAMCore_Comm
    {
        public string run_lua_command(string command, string parameters);
    }
}
