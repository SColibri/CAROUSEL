using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AMControls.Charts.Interfaces;

namespace AMControls.Charts.Implementations.DataSeries
{
    public class ScatterSeries : DataSeries_Abstract
    {
        private static int IndexCount = 0;

        private int _checkArea = 25;
        private Color _lineColor = Colors.Black;
        private double _lineThickness = 1;
        private bool _showMarkers = false;

        public ScatterSeries()
        {
            Index = IndexCount++;
        }   

        public override void Draw(DrawingContext dc, Canvas canvas, System.Windows.Rect ChartArea, double xSize, double ySize, double xStart, double yStart)
        {

            if (DataPoints.Count > 0 && IsVisible == true)
            {
                System.Windows.Point prevPoint = new(0, 0);
                bool firstPoint = false;
                for (int n1 = 0; n1 < DataPoints.Count; n1++)
                {
                    IDataPoint item = DataPoints[n1];
                    double xLoc = ChartArea.X + item.X * xSize - xStart * xSize;
                    double yLoc = ChartArea.Y + ChartArea.Height - item.Y * ySize + yStart * ySize;

                    if (ChartArea.Contains(xLoc, yLoc))
                    {
                        if (!firstPoint)
                        {
                            firstPoint = true;
                            prevPoint = new(xLoc, yLoc);
                            continue;
                        }

                        dc.DrawLine(new Pen(new SolidColorBrush(_lineColor), _lineThickness), prevPoint, new System.Windows.Point(xLoc, yLoc));
                        prevPoint = new(xLoc, yLoc);
                    }
                }
            }

        }

        public bool Check_MouseHover(double x, double y)
        {
            return false;
            //throw new NotImplementedException();
        }

        public override void Mouse_Hover_Action(double x, double y)
        {
            // Do nothing
        }

        public override void Mouse_LeftButton_Action(double x, double y)
        {
            System.Windows.Rect Area = new(x, y, _checkArea, _checkArea);
            IDataPoint? Pointy = DataPoints.Find(e => Area.Contains(e.X, e.Y));

            if (Pointy != null) { IsSelected = true; }
            else { IsSelected = false; }
        }

        public override void Mouse_RightButton_Action(double x, double y)
        {
            throw new NotImplementedException();
        }

        protected override void OnDataPoint_Change()
        {
            //throw new NotImplementedException();
        }

        public override void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, List<double> xSize, List<double> ySize, List<double> xStart, List<double> yStart)
        {
            throw new NotImplementedException();
        }

        #region Implementation
        public override Color ColorSeries { get { return _lineColor; } set { _lineColor = value; } }

        public override event EventHandler DataPointSelectionChanged;
        public override event EventHandler SeriesSelected;
        #endregion
    }
}
