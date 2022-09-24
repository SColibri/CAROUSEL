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

        // Events
        public event EventHandler DataPointSelectionChanged;
        public event EventHandler SeriesSelected;
    }
}
