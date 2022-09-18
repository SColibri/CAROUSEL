using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Interfaces
{
    public interface ILegend
    {

        public void Draw(DrawingContext dc, Canvas canvas, System.Windows.Rect ChartArea, List<IDataSeries> seriesList);

        /// <summary>
        /// Location of the legend box relative to the chart area
        /// </summary>
        System.Windows.Point Location { get; set; }

        /// <summary>
        /// Legend size
        /// </summary>
        System.Windows.Size Size { get; set; }

        /// <summary>
        /// Check series name hit
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IDataSeries? Check_series_Hit(double x, double y);

    }
}
