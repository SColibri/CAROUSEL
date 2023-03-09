namespace AMFramework_Lib.AMSystem
{
    public class AMFramework_startup
    {
        public static void Start(ref Core.IAMCore_Comm comm)
        {
            AMFramework_StaticLoader.Load_Model_Commands(ref comm);
        }
    }
}
