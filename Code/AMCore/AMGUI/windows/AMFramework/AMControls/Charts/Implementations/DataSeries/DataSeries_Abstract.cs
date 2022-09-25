using AMControls.Charts.Interfaces;
using AMControls.Interfaces.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Implementations.DataSeries
{
    public abstract class DataSeries_Abstract : DrawObject_Abstract, IDataSeries
    {
       ~DataSeries_Abstract()
        {
            // Remove all calls to event DataChanged
            foreach (IDataPoint item in DataPoints)
            {
                item.DataChanged -= Handle_DataChanged;
            }
        }

        // IData series
        public List<IDataPoint> DataPoints { get; set; } = new();
        public List<IDataPoint> ContextMenus { get; set; } = new();
        public int Index { get; set; } = 0;
        public string Label { get; set; } = "New series";
        public abstract Color ColorSeries { get; set; }

        public abstract event EventHandler DataPointSelectionChanged;
        public abstract event EventHandler SeriesSelected;

        // Draw Object abstract
        public override void Draw(DrawingContext dc, Canvas canvas)
        {
            throw new NotImplementedException();
        }

        public abstract void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, double xSize, double ySize, double xStart, double yStart);
        public override abstract void Mouse_Hover_Action(double x, double y);

        public override abstract void Mouse_LeftButton_Action(double x, double y);

        public override abstract void Mouse_RightButton_Action(double x, double y);

        public void Add_DataPoint(IDataPoint dPoint)
        {
            dPoint.DataChanged += Handle_DataChanged;
            DataPoints.Add(dPoint);
            OnDataPoint_Change();
        }

        private void Handle_DataChanged(object? sender, EventArgs e)
        {
            OnDataPoint_Change();
            NeedsUpdate = true;
        }
        protected abstract void OnDataPoint_Change();

        protected List<IDataPoint> _searchResult = new();
        public List<IDataPoint> Search(string searchContent)
        {
            _searchResult.Clear();

            // Check Label
            foreach (var item in DataPoints)
            {
                // check label
                if (item.Label.Contains(searchContent)) 
                {
                    _searchResult.Add(item);
                    continue;
                }

                // check tag
                if(item.Tag is List<string>) 
                {
                    foreach (var sub_string in (List<string>)item.Tag)
                    {
                        if (item.Label.Contains(sub_string))
                        {
                            _searchResult.Add(item);
                            continue;
                        }
                    }
                }
            }

            return _searchResult;
        }

        public List<IDataPoint> Search(double x, double y, double tolerance)
        {
            _searchResult.Clear();

            double min_x = x - tolerance;
            double max_x = x + tolerance;
            double min_y = y - tolerance;
            double max_y = y + tolerance;

            _searchResult = DataPoints.FindAll(e => e.X >= min_x && e.X <= max_x && e.Y >= min_y && e.Y <= max_y);

            return _searchResult;
        }

    }
}
