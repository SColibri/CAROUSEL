using AMControls.Charts.Fonts;
using AMControls.Charts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Implementations.DataSeries
{
    public class ParallaxLineSeries : DataSeries_Abstract
    {
        private Color _ColorBox = Colors.Black;
        private Color _ColorBoxBackground = Colors.Black;
        private double _PointThickness = 1;
        private double _LineThickness = 0.5;

        public Color ColorBox { get { return _ColorBox; } set { _ColorBox = value; } }
        public Color ColorBoxBackground { get { return _ColorBoxBackground; } set { _ColorBoxBackground = value; } }
        public override Color ColorSeries { get { return _ColorBox; } set { _ColorBox = value; _ColorBoxBackground = value; } }

        public override event EventHandler? DataPointSelectionChanged;
        public override event EventHandler? SeriesSelected;

        public override void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, double xSize, double ySize, double xStart, double yStart)
        {
            if (DataPoints.Count > 0 && IsVisible == true)
            {
                double minX_Value = DataPoints.Min(e => e.X) * xSize;
                double maxX_Value = DataPoints.Max(e => e.X) * xSize;
                double minY_Value = DataPoints.Min(e => e.Y) * ySize;
                double maxY_Value = DataPoints.Max(e => e.Y) * ySize;
                if (minX_Value > maxX_Value || minY_Value > maxY_Value) return;

                double xLoc = xStart + minX_Value;
                double yLoc = yStart + ChartArea.Height - maxY_Value;
                Rect RBox = new(xLoc - 10, yLoc - 10, (maxX_Value - minX_Value), (maxY_Value - minY_Value));
                Bounds = RBox;

                if (RBox.Width == 0 || RBox.Height == 0) return;
                if (!ChartArea.IntersectsWith(RBox)) return;
                Rect RBoxIntersect = Rect.Intersect(RBox, ChartArea);

                // Update Point position
                ContextMenus.Clear();
                foreach (var pointy in DataPoints)
                {
                    pointy.X_draw = pointy.X * xSize + xStart;
                    pointy.Y_draw = yStart + ChartArea.Height - pointy.Y * ySize;

                    if (pointy.Selected) ContextMenus.Add(pointy);
                }

                Draw_Points_Action(dc, canvas, ChartArea, DataPoints, new SolidColorBrush(_ColorBoxBackground));
            }
        }

        public override void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, List<double> xSize, List<double> ySize, List<double> xStart, List<double> yStart)
        {
            if (DataPoints.Count > 0 && IsVisible == true)
            {
                ContextMenus.Clear();

                for (int n1 = 0; n1 < xSize.Count; n1++)
                {
                    double xLoc = xStart[n1] + DataPoints[n1].X * xSize[n1];
                    double yLoc = ChartArea.Y + ChartArea.Height - (DataPoints[n1].Y - yStart[n1]) * ySize[n1];
                    Rect RBox = new(xLoc - 5, yLoc - 5, 10, 10);
                    Bounds = RBox;

                    if (RBox.Width == 0 || RBox.Height == 0) continue;
                    if (!ChartArea.IntersectsWith(RBox)) continue;
                    Rect RBoxIntersect = Rect.Intersect(RBox, ChartArea);

                    // Update Point position
                    DataPoints[n1].X_draw = DataPoints[n1].X * xSize[n1] + xStart[n1];
                    DataPoints[n1].Y_draw = yLoc;

                    if (DataPoints[n1].Selected) ContextMenus.Add(DataPoints[n1]);
                }


                Draw_Points_Action(dc, canvas, ChartArea, DataPoints, new SolidColorBrush(_ColorBoxBackground));
            }
        }

        public override void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, List<IAxes> axesList)
        { }
        #region Drawing
        private void Draw_DataPoint(DrawingContext dc, IDataPoint dP, Rect pointRect)
        {
            if (dP.IsMouseHover) Draw_SelectedPoint(dc, pointRect);
            else Draw_UnselectedPoint(dc, pointRect);
        }

        private void Draw_SelectedPoint(DrawingContext dc, Rect pointRect)
        {
            SolidColorBrush BoxFill = new SolidColorBrush(_ColorBoxBackground);
            BoxFill.Opacity = 0.5;

            Pen DotPen = new(new SolidColorBrush(_ColorBox), _PointThickness);
            Point center = new(pointRect.X + pointRect.Width / 2, pointRect.Y + pointRect.Height / 2);
            dc.DrawEllipse(BoxFill, DotPen, center, pointRect.Width, pointRect.Height);
        }

        private void Draw_UnselectedPoint(DrawingContext dc, Rect pointRect)
        {
            SolidColorBrush BoxFill = new SolidColorBrush(_ColorBoxBackground);
            BoxFill.Opacity = 0.1;

            Pen DotPen = new(new SolidColorBrush(_ColorBox), _PointThickness);
            dc.DrawRectangle(BoxFill, DotPen, pointRect);
        }
        private void Draw_DataLabel(DrawingContext dc, Canvas canvas, IDataPoint pointy, Rect pointRect)
        {
            FONT_DataPointLabel _fontLabel = new(canvas);
            Point DotStart = new(pointRect.X + pointRect.Width, pointRect.Y);
            _fontLabel.Draw(dc, pointy.Label, DotStart);
        }

        private void Draw_Points_Action(DrawingContext dc, Canvas canvas, Rect ChartArea, List<IDataPoint> DataPoints, SolidColorBrush BackgroundSelection)
        {
            List<Tuple<Point, IDataPoint>> connectingP = new();
            foreach (var pointy in DataPoints)
            {
                // Check if point is in chart area
                Rect PBox = new(pointy.X_draw - 2.5, pointy.Y_draw - 2.5, 5, 5);
                if (!ChartArea.IntersectsWith(PBox)) continue;

                // Get intersect area
                Rect PBoxIntersect = Rect.Intersect(PBox, ChartArea);

                // Highlight selected series if selected
                Point center = new(PBoxIntersect.X + PBoxIntersect.Width / 2, PBoxIntersect.Y + PBoxIntersect.Height / 2);
                connectingP.Add(Tuple.Create(center, pointy));
                if (IsSelected)
                {
                    Pen DotPen = new(new SolidColorBrush(_ColorBox), 0.1);
                    dc.DrawEllipse(BackgroundSelection, DotPen, center, PBoxIntersect.Width * 3, PBoxIntersect.Height * 3);
                }

                if (!pointy.IsVisible) continue;

                // Draw point
                Draw_DataPoint(dc, pointy, PBoxIntersect);

                // Draw label point
                if (IsSelected == true || pointy.IsMouseHover == true)
                {
                    Draw_DataLabel(dc, canvas, pointy, PBox);
                }
            }

            for (int i = 1; i < connectingP.Count; i++)
            {
                dc.DrawLine(new Pen(new SolidColorBrush(_ColorBox), _LineThickness),
                            connectingP[i-1].Item1, connectingP[i].Item1);
            }
        }

        #endregion

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


    }
}
