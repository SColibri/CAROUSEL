namespace AMFramework_Lib.Interfaces
{
    /// <summary>
    /// This view model interface manages the save and close actions for any view item. 
    /// This way we standarize the save and close options on all views used on the main window.
    /// </summary>
    public interface ViewModel_Interface
    {
        /// <summary>
        /// Returns if save was possible.
        /// </summary>
        /// <returns></returns>
        public bool Save();

        /// <summary>
        /// Returns if close was possible, this allows prompting the user before closing a tab or object
        /// inside main window.
        /// </summary>
        /// <returns></returns>
        public bool Close();

    }
}
