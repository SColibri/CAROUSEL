using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMControls.Charts
{
    public interface IDataPoint_ContextMenu_Decorator : IDataPoint_ContextMenu 
    {
        public IDataPoint_ContextMenu DataPoint_ContextMenu { get; set; }

    }
}
