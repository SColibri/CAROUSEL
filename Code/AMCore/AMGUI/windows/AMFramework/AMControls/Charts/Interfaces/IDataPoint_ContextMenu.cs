using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Interfaces
{
    public interface IDataPoint_ContextMenu
    {
        public System.Windows.Point Location { get; set; }
        public System.Windows.Size SizeObject { get; set; }
        public void Draw(DrawingContext dc, Canvas canvas);
        public void CheckHit(double x, double y);
        public bool DoAnimation { get; set; }

    }
}
