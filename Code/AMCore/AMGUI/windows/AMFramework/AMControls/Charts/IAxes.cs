using System.Collections.Generic;

namespace AMControls.Charts
{
    public interface IAxes : Interfaces.IObjectInteraction
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
        /// Visibility flag
        /// </summary>
        /// <returns></returns>
        public bool IsVisible();

        /// <summary>
        /// Linear interval
        /// </summary>
        /// <returns></returns>
        public double Interval { get; set; }

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

    }
}
