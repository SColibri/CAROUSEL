using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Interfaces
{
    /// <summary>
    /// Model controller interface, This interface is designed to be used as a single model
    /// object controller, meaning that this controller interface will handle how data is
    /// loaded, saved and other by object.
    /// 
    /// Review: This interface is a model view controller for single model objects.
    /// 
    /// 
    /// </summary>
    public interface IModelController: INotifyPropertyChanged
    {
        /// <summary>
        /// Returns the model object that contains the data
        /// </summary>
        public Model_Interface? Model_Object { get; }

    }
}
