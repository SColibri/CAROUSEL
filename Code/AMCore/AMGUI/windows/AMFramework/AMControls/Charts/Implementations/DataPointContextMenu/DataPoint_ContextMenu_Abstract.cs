using AMControls.Charts.Interfaces;
using AMControls.Interfaces.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace AMControls.Charts.Implementations.DataPointContextMenu
{
    public abstract class DataPoint_ContextMenu_Abstract: DrawObject_Abstract, IDataPoint_ContextMenu
    {


        #region Interface_implementation

        #region IDataPoint_ContextMenu
        public bool DoAnimation { get; set; } = true;
        #endregion

        #endregion
    }
}
