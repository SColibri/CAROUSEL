﻿namespace AMFramework_Lib.Core
{
    public interface IAMCore_Comm
    {
        public string run_lua_command(string command, string parameters);
        public void update_path(string apiPath);

        /// <summary>
        /// Returns true if connected to a library or socket
        /// </summary>
        public bool Connected { get; }

    }
}
