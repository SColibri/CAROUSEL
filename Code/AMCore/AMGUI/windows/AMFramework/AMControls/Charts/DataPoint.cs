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
        private string _Label = "";

        public double X { get { return _x; } set { _x = value; } }
        public double Y { get { return _y; } set { _y = value; } }
        public string Label { get { return _Label; } set { _Label = value; } }
    }
}
