using AMControls.Interfaces;
using AMControls.Interfaces.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Interfaces
{
    public interface IDataPoint_ContextMenu : IDrawable, IObjectInteraction
    {
        public bool DoAnimation { get; set; }
    }
}
