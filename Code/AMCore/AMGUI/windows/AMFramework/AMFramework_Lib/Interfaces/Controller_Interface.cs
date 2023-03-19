using System.ComponentModel;

namespace AMFramework_Lib.Interfaces
{
    /// <summary>
    /// Controller interface, used as a wrapper for the data models
    /// </summary>
    internal interface Controller_Interface : INotifyPropertyChanged
    {
        /// <summary>
        /// Refresh models and controller data
        /// </summary>
        public void Refresh();

    }
}
