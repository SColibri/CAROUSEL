using AMControls.Charts.Interfaces;
using AMControls.Interfaces.Implementations;

namespace AMControls.Charts.Implementations.DataPointContextMenu
{
    public abstract class DataPoint_ContextMenu_Abstract : DrawObject_Abstract, IDataPoint_ContextMenu
    {


        #region Interface_implementation

        #region IDataPoint_ContextMenu
        public bool DoAnimation { get; set; } = true;
        #endregion

        #endregion
    }
}
