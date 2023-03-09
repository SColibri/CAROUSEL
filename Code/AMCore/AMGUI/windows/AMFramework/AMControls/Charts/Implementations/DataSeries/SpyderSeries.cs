using AMControls.Charts.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Implementations.DataSeries
{
    public class SpyderSeries : DataSeries_Abstract
    {
        private Color _ColorBox = Colors.Black;
        private Color _ColorBoxBackground = Colors.Black;
        private double _PointThickness = 1;
        private double _LineThickness = 0.5;

        private StreamGeometry _dataGeometry;

        #region DataSeries_Abstract
        public Color ColorBox { get { return _ColorBox; } set { _ColorBox = value; } }
        public Color ColorBoxBackground { get { return _ColorBoxBackground; } set { _ColorBoxBackground = value; } }
        public override Color ColorSeries { get { return _ColorBox; } set { _ColorBox = value; _ColorBoxBackground = value; } }

        public override event EventHandler? DataPointSelectionChanged;
        public override event EventHandler? SeriesSelected;

        public override void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, double xSize, double ySize, double xStart, double yStart)
        {
            throw new NotImplementedException();
        }

        public override void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, List<double> xSize, List<double> ySize, List<double> xStart, List<double> yStart)
        {
            throw new NotImplementedException();
        }

        public override void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, List<IAxes> axesList)
        {
            if (DataPoints.Count != axesList.Count) return;

            _dataGeometry = new StreamGeometry();
            using (StreamGeometryContext ctx = _dataGeometry.Open())
            {
                ctx.BeginFigure(axesList[0].ValueToPoint(DataPoints[0].X), true, true);
                for (int n1 = 1; n1 < axesList.Count; n1++)
                {
                    ctx.LineTo(axesList[n1].ValueToPoint(DataPoints[n1].X), true, false);
                }
                ctx.LineTo(axesList[0].ValueToPoint(DataPoints[0].X), true, false);
                ctx.Close();
            }



            _dataGeometry.Freeze();
            SolidColorBrush scb = new(ColorSeries) { Opacity = 0.4 };
            dc.DrawGeometry(scb, new Pen(Brushes.Silver, 1), _dataGeometry);
        }

        public override void Mouse_Hover_Action(double x, double y)
        {
            throw new NotImplementedException();
        }

        public override void Mouse_LeftButton_Action(double x, double y)
        {
            throw new NotImplementedException();
        }

        public override void Mouse_RightButton_Action(double x, double y)
        {
            throw new NotImplementedException();
        }

        protected override void OnDataPoint_Change()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
