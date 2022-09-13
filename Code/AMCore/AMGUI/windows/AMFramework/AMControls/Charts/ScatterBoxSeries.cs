using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts
{
    public class ScatterBoxSeries:IDataSeries
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

                double xLoc = ChartArea.X + (minX_Value) - xStart*xSize;
                double yLoc = ChartArea.Y + ChartArea.Height - (maxY_Value) + yStart*ySize;
                System.Windows.Rect RBox = new(xLoc - 10, yLoc - 10, (int)(maxX_Value - minX_Value + 20), (int)(maxY_Value - minY_Value + 20));

                if (!ChartArea.IntersectsWith(RBox)) return; 
                System.Windows.Rect RBoxIntersect = System.Windows.Rect.Intersect(RBox, ChartArea);

                // Draw Bounding box
                SolidColorBrush BoxFill = new SolidColorBrush(_ColorBoxBackground);
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

                System.Windows.Point LabelStart = new(LabelBox.X + (LabelBox.Width - txtFormat.Width) / 2, LabelBox.Y + (LabelBox.Height - txtFormat.Height)/2);
                dc.DrawText(txtFormat, LabelStart);

                // Draw Points 

                foreach (var pointy in _DataPoints)
                {
                    double xPLoc = ChartArea.X + (pointy.X * xSize) - xStart * xSize;
                    double yPLoc = ChartArea.Y + ChartArea.Height - (pointy.Y * ySize) + yStart * ySize;
                    System.Windows.Rect PBox = new(xPLoc-2.5, yPLoc-2.5, 5, 5);

                    if (!ChartArea.IntersectsWith(PBox)) continue;
                    System.Windows.Rect PBoxIntersect = System.Windows.Rect.Intersect(PBox, ChartArea);
                    Pen DotPen = new(new SolidColorBrush(_ColorBox), _PointThickness);
                    dc.DrawRectangle(BoxFill, DotPen, PBoxIntersect);

                    // Draw label point
                    if (_isSelected) 
                    {
                        FormattedText DotFormat = new(pointy.Label, System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_dotLabelFontFamily, _dotLabelFontStyle, _dotLabelFontWeight, _dotLabelFontStretch),
                                                  _dotLabelFontSize, new SolidColorBrush(_dotLabelFontColor), VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

                        System.Windows.Point DotStart = new(PBox.X + PBox.Width, PBox.Y);
                        dc.DrawText(DotFormat, DotStart);
                    }
                }

            }

        }

        public void CheckHit(double x, double y)
        {
            if (LabelBox.Contains(x, y)) 
            { 
                _isSelected = true; 
            }
            else { _isSelected = false; }
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
    }
}
