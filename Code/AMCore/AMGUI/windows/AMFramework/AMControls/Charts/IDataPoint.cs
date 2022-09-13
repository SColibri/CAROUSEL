using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMControls.Charts
{
    public interface IDataPoint
    {
        /// <summary>
        /// Shape of the data point
        /// </summary>
        public enum Shape
        {
            CIRCLE,
            BOX,
            CROSS,
            NONE
        }

        public double X { get; set; }
        public double Y { get; set; }

        public string Label { get; set; }
    }
}
