using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMControls.Charts
{
    public class DataPoint : IDataPoint
    {
        private double _x = 0;
        private double _y = 0;
        private double _z = 0;
        private string _Label = "";

        public double X { get { return _x; } set { _x = value; } }
        public double Y { get { return _y; } set { _y = value; } }
        public double Z { get { return _z; } set { _z = value; } }

        public double X_draw { get; set; }
        public double Y_draw { get; set; }
        public double Z_draw { get; set; }

        public string Label { get { return _Label; } set { _Label = value; } }

        public bool MouseHover { get; set; } = false;
        public bool Selected { get; set; } = false;
        public object? Tag { get; set; }
        public IDataPoint_ContextMenu? ContextMenu { get; set; }
    }
}
