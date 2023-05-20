using AMControls.Charts.Interfaces;
using Catel.MVVM;
using Catel.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Views.Project_Map
{
    /// <summary>
    /// View model that displays data in the project level 
    /// </summary>
    internal class ProjectMapViewModel : ViewModelBase
    {
        #region Fields
        /// <summary>
        /// Chart type to be used
        /// </summary>
        private IChart _chartObject;

        /// <summary>
        /// Flag used to show if tooltip should be shown or not
        /// </summary>
        private bool _showToolTip = false;


        #endregion

        #region Commands

        #endregion

        #region Methods
        /// <summary>
        /// Updates data points in chart object
        /// </summary>
        public void RefreshData() 
        { 
        
        }
        #endregion
    }
}
