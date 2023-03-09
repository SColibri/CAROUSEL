namespace AMFramework_Lib.Interfaces
{
    internal interface IScripting
    {
        /// <summary>
        /// Returns a scripted format for an object
        /// </summary>
        /// <returns></returns>
        public string Create_Object();

        /// <summary>
        /// Returns a scriptde format for loading an object with specified ID, name or other
        /// </summary>
        /// <returns></returns>
        public string Load_Object();

        /// <summary>
        /// Returns a script format that saves the object
        /// </summary>
        /// <returns></returns>
        public string Save_Object();

    }
}
