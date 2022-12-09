using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMControls.Charts.Interfaces
{
    /// <summary>
    /// IChart is an interface used for Plotting objects. Charts display
    /// IDataSeries and represent data contained in each data series.
    /// </summary>
    public interface IChart
    {
        /// <summary>
        /// Removes all series
        /// </summary>
        public void Clear_Series();

        /// <summary>
        /// Add new series
        /// </summary>
        /// <param name="value"></param>
        public void Add_series(IDataSeries value);

        /// <summary>
        /// Save plot image
        /// </summary>
        /// <param name="filename"></param>
        public void Save_Image(string filename);

        /// <summary>
        /// Adjust data to chart area
        /// </summary>
        public void Adjust_axes_to_data();

        /// <summary>
        /// Returns all selected points
        /// </summary>
        /// <returns></returns>
        public List<IDataPoint> Get_Selected_DataPoints();

        /// <summary>
        /// Search data in datapoints
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public List<IDataPoint> Search(string searchString);

        /// <summary>
        /// Search reagion in datapoints
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public List<IDataPoint> Search(double x, double y, double tolerance);

        /// <summary>
        /// Returns the point value of the current mouse position
        /// </summary>
        /// <param name="x_mouse"></param>
        /// <param name="y_mouse"></param>
        /// <returns></returns>
        public IDataPoint Get_Position(double x_mouse, double y_mouse);

        /// <summary>
        /// Event fires when data selection has changed
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Event fires when a context menu has been clicked on
        /// </summary>
        public event EventHandler ContextMenuClicked;

        /// <summary>
        /// Event fires when mouse move inside chart area is detected
        /// </summary>
        public event EventHandler<MouseEventArgs> ChartAreaMouseMove;
    }
}
