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

        public bool Connected => false;
    }
}
