namespace AMFramework_Lib.AMSystem
{
    public class ParseObject
    {
        public enum PTYPE
        {
            NONE,
            CLASS,
            FUNCTION,
            GLOBAL_VARIABLE,
            LOCAL_VARIABLE
        }

        public string ModuleName = "";
        public PTYPE ObjectType = PTYPE.NONE;
        public string Name = "";
        public string Description = "";
        public string ParametersType = "";
        public List<ParseObject> Parameters = new();
        public List<ParseObject> functions = new();
        public Interfaces.Model_Interface? ObjectModel = null;

    }
}
