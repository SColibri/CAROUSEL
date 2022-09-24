using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AMControls.Charts.Interfaces;
using AMControls.Interfaces.Implementations;

namespace AMControls.Charts.Implementations.DataSeries
{
    public class ScatterBoxSeries : DataSeries_Abstract
    {
        private static int IndexCount = 0;

        private Color _ColorBox = Colors.Black;
        private Color _ColorBoxBackground = Colors.Black;
        private int _BoxThickness = 2;
        private int _PointThickness = 2;
        private bool _showBox = false;

        private FontFamily _axisLabelFontFamily = new("Lucida Sans");
        private int _axisLabelFontSize = 9;
        private FontStyle _axisLabelFontStyle = FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private FontWeight _axisLabelFontWeight = FontWeights.Thin; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private FontStretch _axisLabelFontStretch = FontStretches.Normal;
        private Color _axisLabelFontColor = Colors.White;

        private FontFamily _dotLabelFontFamily = new("Lucida Sans");
        private int _dotLabelFontSize = 9;
        private FontStyle _dotLabelFontStyle = FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private FontWeight _dotLabelFontWeight = FontWeights.Thin; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private FontStretch _dotLabelFontStretch = FontStretches.Normal;
        private Color _dotLabelFontColor = Colors.Black;

        public Rect LabelBox { get; set; }

        public ScatterBoxSeries() { Index = IndexCount++; }

        public override void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, double xSize, double ySize, double xStart, double yStart)
        {
            if (DataPoints.Count > 0 && IsVisible == true)
            {
                double minX_Value = DataPoints.Min(e => e.X) * xSize;
                double maxX_Value = DataPoints.Max(e => e.X) * xSize;
                double minY_Value = DataPoints.Min(e => e.Y) * ySize;
                double maxY_Value = DataPoints.Max(e => e.Y) * ySize;
                if (minX_Value > maxX_Value || minY_Value > maxY_Value) return;

                double xLoc = ChartArea.X + minX_Value - xStart * xSize;
                double yLoc = ChartArea.Y + ChartArea.Height - maxY_Value + yStart * ySize;
                Rect RBox = new(xLoc - 10, yLoc - 10, (maxX_Value - minX_Value + 20), (maxY_Value - minY_Value + 20));
                Bounds = RBox;

                if (RBox.Width == 0 || RBox.Height == 0) return;
                if (!ChartArea.IntersectsWith(RBox)) return;
                Rect RBoxIntersect = Rect.Intersect(RBox, ChartArea);

                // Background Dots
                SolidColorBrush BackgroundSelection = new SolidColorBrush(_ColorBoxBackground);
                BackgroundSelection.Opacity = 0.4;

                // Draw Bounding box
                SolidColorBrush BoxFill = new SolidColorBrush(_ColorBoxBackground);
                if (_showBox)
                {
                    BoxFill.Opacity = 0.1;

                    Pen BoxPen = new(new SolidColorBrush(_ColorBox), _BoxThickness);
                    dc.DrawRectangle(BoxFill, BoxPen, RBoxIntersect);

                    //Title

                    FormattedText txtFormat = new(Label, System.Globalization.CultureInfo.CurrentCulture,
                                                      FlowDirection.LeftToRight,
                                                      new Typeface(_axisLabelFontFamily, _axisLabelFontStyle, _axisLabelFontWeight, _axisLabelFontStretch),
                                                      _axisLabelFontSize, new SolidColorBrush(_axisLabelFontColor), VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

                    LabelBox = new(RBoxIntersect.X + RBoxIntersect.Width - txtFormat.Width - 20, RBoxIntersect.Y - (txtFormat.Height + 6), txtFormat.Width + 20, txtFormat.Height + 6);
                    dc.DrawRectangle(new SolidColorBrush(_ColorBox), BoxPen, LabelBox);

                    Point LabelStart = new(LabelBox.X + (LabelBox.Width - txtFormat.Width) / 2, LabelBox.Y + (LabelBox.Height - txtFormat.Height) / 2);
                    dc.DrawText(txtFormat, LabelStart);
                }

                // Draw Points 
                List<IDataPoint> dPContext = new();
                foreach (var pointy in DataPoints)
                {
                    pointy.X_draw = ChartArea.X + pointy.X * xSize - xStart * xSize;
                    pointy.Y_draw = ChartArea.Y + ChartArea.Height - pointy.Y * ySize + yStart * ySize;
                    Rect PBox = new(pointy.X_draw - 2.5, pointy.Y_draw - 2.5, 5, 5);

                    if (!ChartArea.IntersectsWith(PBox)) continue;
                    Rect PBoxIntersect = Rect.Intersect(PBox, ChartArea);

                    // Show Selected Series points background
                    if (IsSelected)
                    {
                        Pen DotPen = new(new SolidColorBrush(_ColorBox), 0.1);
                        Point center = new(PBoxIntersect.X + PBoxIntersect.Width / 2, PBoxIntersect.Y + PBoxIntersect.Height / 2);
                        dc.DrawEllipse(BackgroundSelection, DotPen, center, PBoxIntersect.Width*3, PBoxIntersect.Height*3);
                    }

                    Draw_DataPoint(dc, pointy, PBoxIntersect);


                    // Draw label point
                    if (IsSelected == true || pointy.IsMouseHover == true)
                    {
                        Draw_DataLabel(dc, canvas, pointy, PBox);

                        if (pointy.Selected) dPContext.Add(pointy);
                    }
                }

                // Draw ContextMenu
                foreach (var dPointy in dPContext)
                {
                    Draw_DataPoint_ContextMenu(dc, canvas, dPointy);
                }

            }

        }

        #region Darwing

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
            FormattedText DotFormat = new(pointy.Label, System.Globalization.CultureInfo.CurrentCulture,
                                                  FlowDirection.LeftToRight,
                                                  new Typeface(_dotLabelFontFamily, _dotLabelFontStyle, _dotLabelFontWeight, _dotLabelFontStretch),
                                                  _dotLabelFontSize, new SolidColorBrush(_dotLabelFontColor), VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            Point DotStart = new(pointRect.X + pointRect.Width, pointRect.Y);
            dc.DrawText(DotFormat, DotStart);
        }

        private void Draw_DataPoint_ContextMenu(DrawingContext dc, Canvas canvas, IDataPoint dP)
        {
            if (dP.ContextMenu == null) return;
            Point currLoc = new Point(dP.X_draw, dP.Y_draw);
            Rect conWin = new(currLoc, dP.ContextMenu.SizeObject);
            Rect objArea = new(0, 0, canvas.ActualWidth, canvas.ActualHeight);

            if (!objArea.Contains(conWin))
            {
                conWin = new(new Point(currLoc.X - dP.ContextMenu.SizeObject.Width, currLoc.Y), dP.ContextMenu.SizeObject);
            }

            if (!objArea.Contains(conWin))
            {
                conWin = new(new Point(currLoc.X + dP.ContextMenu.SizeObject.Width, currLoc.Y), dP.ContextMenu.SizeObject);
            }

            if (!objArea.Contains(conWin))
            {
                conWin = new(new Point(currLoc.X, currLoc.Y - dP.ContextMenu.SizeObject.Height), dP.ContextMenu.SizeObject);
            }

            if (!objArea.Contains(conWin))
            {
                conWin = new(new Point(currLoc.X, currLoc.Y + dP.ContextMenu.SizeObject.Height), dP.ContextMenu.SizeObject);
            }

            if (!objArea.Contains(conWin))
            {
                conWin = new(new Point(currLoc.X - dP.ContextMenu.SizeObject.Width,
                                       currLoc.Y - dP.ContextMenu.SizeObject.Height), dP.ContextMenu.SizeObject);
            }

            if (!objArea.Contains(conWin))
            {
                conWin = new(new Point(currLoc.X - dP.ContextMenu.SizeObject.Width,
                                       currLoc.Y + dP.ContextMenu.SizeObject.Height), dP.ContextMenu.SizeObject);
            }

            if (!objArea.Contains(conWin))
            {
                conWin = new(new Point(currLoc.X + dP.ContextMenu.SizeObject.Width,
                                       currLoc.Y - dP.ContextMenu.SizeObject.Height), dP.ContextMenu.SizeObject);
            }

            if (!objArea.Contains(conWin))
            {
                conWin = new(new Point(currLoc.X + dP.ContextMenu.SizeObject.Width,
                                       currLoc.Y + dP.ContextMenu.SizeObject.Height), dP.ContextMenu.SizeObject);
            }


            dP.ContextMenu.Location = conWin.Location;
            dP.ContextMenu.Draw(dc, canvas);
        }

        #endregion

        public void CheckHit(double x, double y)
        {
            if (LabelBox.Contains(x, y))
            {
                IsSelected = true;
            }
            else { IsSelected = false; }

            bool selectedPoints = false;
            foreach (var item in DataPoints)
            {
                if (Check_dataPointHit(x, y, item))
                {
                    selectedPoints = true;
                    item.Selected = true;
                    item.ContextMenu.DoAnimation = true;
                    IsSelected = true;
                }
                else item.Selected = false;
            }

            if (selectedPoints) DataPointSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool Check_dataPointHit(double x, double y, IDataPoint dP)
        {
            Rect pointRect = new(dP.X_draw - 5, dP.Y_draw - 5, 10, 10);
            if (pointRect.Contains(x, y))
            {
                return true;
            }
            return false;
        }

        public List<IDataPoint> Get_Selected_points()
        {
            return DataPoints.FindAll(e => e.Selected == true).ToList();
        }


        public override void Mouse_Hover_Action(double x, double y)
        {
            NeedsUpdate = false;
            foreach (IDataPoint item in DataPoints)
            {
                bool Test = IsMouseHover;
                Rect pointRect = new(item.X_draw - 5, item.Y_draw - 5, 10, 10);
                NeedsUpdate = item.Mouse_Hover(x, y);
            }
        }

        public override void Mouse_LeftButton_Action(double x, double y)
        {
            if (LabelBox.Contains(x, y))
            {
                IsSelected = true;
            }
            else { IsSelected = false; }

            bool selectedPoints = false;
            foreach (var item in DataPoints)
            {
                if (Check_dataPointHit(x, y, item))
                {
                    selectedPoints = true;
                    item.Selected = true;
                    item.ContextMenu.DoAnimation = true;
                    IsSelected = true;
                }
                else item.Selected = false;
            }

            if (selectedPoints) DataPointSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public override void Mouse_RightButton_Action(double x, double y)
        {
            throw new NotImplementedException();
        }

        #region Getters_setters
        public Color ColorBox { get { return _ColorBox; } set { _ColorBox = value; } }
        public Color ColorBoxBackground { get { return _ColorBoxBackground; } set { _ColorBoxBackground = value; } }

        public override Color ColorSeries { get { return _ColorBox; } set { _ColorBox = value; _ColorBoxBackground = value; } }

      

        #endregion

        #region Events
        public override event EventHandler DataPointSelectionChanged;
        public override event EventHandler SeriesSelected;
        #endregion
    }
}
