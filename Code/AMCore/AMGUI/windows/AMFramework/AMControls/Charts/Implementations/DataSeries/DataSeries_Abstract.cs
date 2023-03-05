using AMControls.Charts.Interfaces;
using AMControls.Interfaces.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<IDataPoint> DataPoints { get; set; }
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

        public abstract void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, List<IAxes> axesList);


        public abstract override void Mouse_Hover_Action(double x, double y);

        public abstract override void Mouse_LeftButton_Action(double x, double y);

        public abstract override void Mouse_RightButton_Action(double x, double y);

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
            if (searchContent.Length == 0) return _searchResult;

            // Allow user to refine search
            List<string> splitSearch = searchContent.Split('+').ToList();

            // Check Label
            foreach (var item in DataPoints)
            {
                foreach (var string_segment in splitSearch)
                {
                    item.IsVisible = false;

                    // check label
                    if (item.Label.Contains(string_segment.Trim()))
                    {
                        item.IsVisible = true;
                        continue;
                    }

                    // check tag
                    if (item.Tag is List<string>)
                    {
                        foreach (var sub_string in (List<string>)item.Tag)
                        {
                            if (sub_string.Contains(string_segment.Trim()))
                            {
                                item.IsVisible = true;
                                break;
                            }
                        }
                    }

                    if (!item.IsVisible) break;
                }

                if (item.IsVisible)
                {
                    _searchResult.Add(item);
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

        List<string> IDataSeries.Get_pointLabelList()
        {
            List<string> Result = new();

            foreach (var item in DataPoints)
            {
                Result.Add(item.Label);
            }

            return Result;
        }

        public abstract void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, List<double> xSize, List<double> ySize, List<double> xStart, List<double> yStart);
    }
}
