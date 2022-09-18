using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts
{
    public class ScatterBoxSeries : IDataSeries
    {
        private static int IndexCount = 0;

        private List<IDataPoint> _DataPoints = new();
        private Color _ColorBox = Colors.Black;
        private Color _ColorBoxBackground = Colors.Black;
        private int _BoxThickness = 2;
        private int _PointThickness = 2;
        private string _label = "Series";
        private bool _isSelected = false;
        private bool _isVisible = true;
        private bool _showBox = false;
        private int _index = 0;

        private FontFamily _axisLabelFontFamily = new("Lucida Sans");
        private int _axisLabelFontSize = 9;
        private System.Windows.FontStyle _axisLabelFontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _axisLabelFontWeight = System.Windows.FontWeights.Thin; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _axisLabelFontStretch = System.Windows.FontStretches.Normal;
        private Color _axisLabelFontColor = Colors.White;

        private FontFamily _dotLabelFontFamily = new("Lucida Sans");
        private int _dotLabelFontSize = 9;
        private System.Windows.FontStyle _dotLabelFontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _dotLabelFontWeight = System.Windows.FontWeights.Thin; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _dotLabelFontStretch = System.Windows.FontStretches.Normal;
        private Color _dotLabelFontColor = Colors.Black;

        public System.Windows.Rect LabelBox { get; set; }

        public ScatterBoxSeries() { _index = IndexCount++; }

        public void Draw(DrawingContext dc, Canvas canvas, System.Windows.Rect ChartArea, double xSize, double ySize, double xStart, double yStart)
        {
            if (_DataPoints.Count > 0 && _isVisible == true)
            {
                double minX_Value = _DataPoints.Min(e => e.X) * xSize;
                double maxX_Value = _DataPoints.Max(e => e.X) * xSize;
                double minY_Value = _DataPoints.Min(e => e.Y) * ySize;
                double maxY_Value = _DataPoints.Max(e => e.Y) * ySize;

                double xLoc = ChartArea.X + (minX_Value) - xStart * xSize;
                double yLoc = ChartArea.Y + ChartArea.Height - (maxY_Value) + yStart * ySize;
                System.Windows.Rect RBox = new(xLoc - 10, yLoc - 10, (int)(maxX_Value - minX_Value + 20), (int)(maxY_Value - minY_Value + 20));

                if (!ChartArea.IntersectsWith(RBox)) return;
                System.Windows.Rect RBoxIntersect = System.Windows.Rect.Intersect(RBox, ChartArea);

                // Draw Bounding box
                SolidColorBrush BoxFill = new SolidColorBrush(_ColorBoxBackground);
                if (_showBox)
                {
                    BoxFill.Opacity = 0.1; 

                    Pen BoxPen = new(new SolidColorBrush(_ColorBox), _BoxThickness);
                    dc.DrawRectangle(BoxFill, BoxPen, RBoxIntersect);

                    //Title

                    FormattedText txtFormat = new(Label, System.Globalization.CultureInfo.CurrentCulture,
                                                      System.Windows.FlowDirection.LeftToRight,
                                                      new Typeface(_axisLabelFontFamily, _axisLabelFontStyle, _axisLabelFontWeight, _axisLabelFontStretch),
                                                      _axisLabelFontSize, new SolidColorBrush(_axisLabelFontColor), VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

                    LabelBox = new(RBoxIntersect.X + RBoxIntersect.Width - txtFormat.Width - 20, RBoxIntersect.Y - (txtFormat.Height + 6), txtFormat.Width + 20, txtFormat.Height + 6);
                    dc.DrawRectangle(new SolidColorBrush(_ColorBox), BoxPen, LabelBox);

                    System.Windows.Point LabelStart = new(LabelBox.X + (LabelBox.Width - txtFormat.Width) / 2, LabelBox.Y + (LabelBox.Height - txtFormat.Height) / 2);
                    dc.DrawText(txtFormat, LabelStart);
                }



                // Draw Points 
                List<IDataPoint> dPContext = new();
                foreach (var pointy in _DataPoints)
                {
                    pointy.X_draw = ChartArea.X + (pointy.X * xSize) - xStart * xSize;
                    pointy.Y_draw = ChartArea.Y + ChartArea.Height - (pointy.Y * ySize) + yStart * ySize;
                    System.Windows.Rect PBox = new(pointy.X_draw - 2.5, pointy.Y_draw - 2.5, 5, 5);

                    if (!ChartArea.IntersectsWith(PBox)) continue;
                    System.Windows.Rect PBoxIntersect = System.Windows.Rect.Intersect(PBox, ChartArea);
                    Draw_DataPoint(dc, pointy, PBoxIntersect);


                    // Draw label point
                    if (_isSelected == true || pointy.MouseHover == true)
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

        private void Draw_DataPoint(DrawingContext dc, IDataPoint dP, System.Windows.Rect pointRect) 
        {
            if (dP.Selected || dP.MouseHover) Draw_SelectedPoint(dc, pointRect);
            else Draw_UnselectedPoint(dc, pointRect);
        }

        private void Draw_SelectedPoint(DrawingContext dc, System.Windows.Rect pointRect) 
        {
            SolidColorBrush BoxFill = new SolidColorBrush(_ColorBoxBackground);
            BoxFill.Opacity = 0.5;

            Pen DotPen = new(new SolidColorBrush(_ColorBox), _PointThickness);
            System.Windows.Point center = new(pointRect.X + pointRect.Width/2, pointRect.Y + pointRect.Height/2);
            dc.DrawEllipse(BoxFill, DotPen, center, pointRect.Width, pointRect.Height);
        }

        private void Draw_UnselectedPoint(DrawingContext dc, System.Windows.Rect pointRect)
        {
            SolidColorBrush BoxFill = new SolidColorBrush(_ColorBoxBackground);
            BoxFill.Opacity = 0.1;

            Pen DotPen = new(new SolidColorBrush(_ColorBox), _PointThickness);
            dc.DrawRectangle(BoxFill, DotPen, pointRect);
        }

        private void Draw_DataLabel(DrawingContext dc, Canvas canvas, IDataPoint pointy, System.Windows.Rect pointRect) 
        {
            FormattedText DotFormat = new(pointy.Label, System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_dotLabelFontFamily, _dotLabelFontStyle, _dotLabelFontWeight, _dotLabelFontStretch),
                                                  _dotLabelFontSize, new SolidColorBrush(_dotLabelFontColor), VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            System.Windows.Point DotStart = new(pointRect.X + pointRect.Width, pointRect.Y);
            dc.DrawText(DotFormat, DotStart);
        }

        private void Draw_DataPoint_ContextMenu(DrawingContext dc, Canvas canvas, IDataPoint dP) 
        {
            if (dP.ContextMenu == null) return;
            Point currLoc = new System.Windows.Point(dP.X_draw, dP.Y_draw);
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
                _isSelected = true; 
            }
            else { _isSelected = false; }

            bool selectedPoints = false;
            foreach (var item in _DataPoints)
            {
                if (Check_dataPointHit(x, y, item)) 
                {
                    selectedPoints = true;
                    item.Selected = true;
                    item.ContextMenu.DoAnimation = true;
                    this.IsSelected = true;
                }
                else item.Selected = false;
            }

            if(selectedPoints)  DataPointSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool Check_MouseHover(double x, double y)
        {
            bool Result = false;
            foreach (var item in _DataPoints)
            {
                if (Check_dataPointHit(x, y, item)) 
                {
                    item.MouseHover = true;
                    Result = true;
                }
                else item.MouseHover = false;
            }

            return Result;
        }

        private bool Check_dataPointHit(double x, double y, IDataPoint dP) 
        {
            System.Windows.Rect pointRect = new(dP.X_draw - 5, dP.Y_draw - 5, 10, 10);
            if (pointRect.Contains(x, y)) 
            {
                return true;
            }
            return false;
        }

        public List<IDataPoint> Get_Selected_points() 
        {
            return _DataPoints.FindAll(e => e.Selected == true).ToList();
        }

        public List<IDataPoint> DataPoints { get { return _DataPoints; } set { _DataPoints = value; } }


        #region Getters_setters
        public Color ColorBox { get { return _ColorBox; } set { _ColorBox = value; } }
        public Color ColorBoxBackground { get { return _ColorBoxBackground; } set { _ColorBoxBackground = value; } }

        public Color ColorSeries { get { return _ColorBox; } set { _ColorBox = value; _ColorBoxBackground = value; } }

        public string Label { get { return _label; } set { _label = value; } }

        public int Index { get { return _index; } set { _index = value; } }
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; } }
        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; } }

        #endregion

        #region Events
        public event EventHandler DataPointSelectionChanged;
        public event EventHandler SeriesSelected;
        #endregion
    }
}
