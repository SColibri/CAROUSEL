using AMControls.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Interfaces
{
    public interface IDataSeries: IDrawable, IObjectInteraction
    {
        /// <summary>
        /// Draws data into bounding box in canvas draw space
        /// </summary>
        /// <param name="dc">Canvas drawingcontext</param>
        /// <param name="canvas">Canvas object, used for DPI awerness in text drawing</param>
        /// <param name="ChartArea">Plotting area (without axes)</param>
        /// <param name="xSize">x step size</param>
        /// <param name="ySize">y step size</param>
        /// <param name="xStart">x start position</param>
        /// <param name="yStart">y start position</param>
        public void Draw(DrawingContext dc, Canvas canvas, System.Windows.Rect ChartArea, double xSize, double ySize, double xStart, double yStart);

        /// <summary>
        /// Series data content
        /// </summary>
        public List<IDataPoint> DataPoints { get; set; }

        /// <summary>
        /// Returns list of points that have open context menus
        /// </summary>
        public List<IDataPoint> ContextMenus { get; set; }

        /// <summary>
        /// Add datapoint to series
        /// </summary>
        /// <param name="dPoint"></param>
        public void Add_DataPoint(IDataPoint dPoint);

        /// <summary>
        /// Position index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Series label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Set plot color
        /// </summary>
        public Color ColorSeries { get; set; }

        /// <summary>
        /// Search for content in datapoint
        /// </summary>
        /// <param name="searchContent"></param>
        /// <returns></returns>
        public List<IDataPoint> Search(string searchContent);

        /// <summary>
        /// find points around a specified tolerance
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public List<IDataPoint> Search(double x, double y, double tolerance);

        // Events
        public event EventHandler DataPointSelectionChanged;
        public event EventHandler SeriesSelected;
    }
}
