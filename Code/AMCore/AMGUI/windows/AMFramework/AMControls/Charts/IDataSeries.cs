using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts
{
    public interface IDataSeries
    {
        //Draw data series in bounding box
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
        /// Series is selected
        /// </summary>
        public bool IsSelected { get; set; }
        
        /// <summary>
        /// Series visibility
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Action after mouse hit
        /// </summary>
        public void CheckHit(double x, double y);

        /// <summary>
        /// Set plot color
        /// </summary>
        public Color ColorSeries { get; set; }


    }
}
