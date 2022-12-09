using AMControls.Interfaces;
using System.Collections.Generic;

namespace AMControls.Charts.Interfaces
{
    public interface IAxes : IObjectInteraction, IDrawable
    {
        /// <summary>
        /// Name of the axis
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// Minimum value in range
        /// </summary>
        /// <returns></returns>
        public double MinValue { get; set; }

        /// <summary>
        /// Maximum value in range
        /// </summary>
        /// <returns></returns>
        public double MaxValue { get; set; }

        /// <summary>
        /// Linear interval
        /// </summary>
        /// <returns></returns>
        public double Interval { get; set; }

        /// <summary>
        /// Linear interval
        /// </summary>
        /// <returns></returns>
        public double DrawInterval { get; set; }

        /// <summary>
        /// Linear drawing lecth for axis
        /// </summary>
        public double DrawLineLength { get; set; }

        /// <summary>
        /// Non-linear intervals
        /// </summary>
        /// <returns></returns>
        public List<double> Intervals();

        /// <summary>
        /// Numeric notation type used for drawing in plot (e.g 1e-9 or 0.0000001)
        /// </summary>
        /// <returns></returns>
        public string IntervalNotation();

        /// <summary>
        /// Axis orientation enumerator
        /// </summary>
        public enum Orientation { HORIZONTAL, VERTICAL }

        /// <summary>
        /// Axis Orientation
        /// </summary>
        public Orientation AxisOrientation { get; set; }

        /// <summary>
        /// Amount of ticks
        /// </summary>
        public int Ticks { get; set; }

        /// <summary>
        /// Origin point of axis
        /// </summary>
        public System.Windows.Point StartPoint { get; set; }
        /// <summary>
        /// end point of axis
        /// </summary>
        public System.Windows.Point EndPoint { get; set; }

        /// <summary>
        /// Get point on axis line based on the value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public System.Windows.Point ValueToPoint(double value);

    }
}
