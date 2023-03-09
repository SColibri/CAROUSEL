using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Interfaces
{
    /// <summary>
    /// Represents a savable object that has a Save method
    /// </summary>
    public interface ISavable
    {

        /// <summary>
        /// Returns true if save was successful
        /// </summary>
        /// <returns></returns>
        public bool Save();

    }
}
