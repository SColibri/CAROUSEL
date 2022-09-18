using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AMControls.Charts.DataPointContextMenu
{
    public class DataPoint_ContextMenu_Text : IDataPoint_ContextMenu
    {
        private Point _location;
        private Size _sizeObject;
        private bool _doAnimation;
        private string _title = "No title";

        // Title font
        private FontFamily _titleFontFamily = new("Lucida Sans");
        private int _titleFontSize = 9;
        private System.Windows.FontStyle _titleFontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _titleFontWeight = System.Windows.FontWeights.Thin; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _titleFontStretch = System.Windows.FontStretches.Normal;
        private Color _titleFontColor = Colors.Black;

        public DataPoint_ContextMenu_Text() 
        {
            dataPoint_ContextMenu = new DataPoint_BaseContextMenu();
            SizeObject = new(150,150);
            dataPoint_ContextMenu.SizeObject = SizeObject;
        }

        public IDataPoint_ContextMenu dataPoint_ContextMenu;
        public Point Location 
        { 
            get { return _location; } 
            set 
            { 
                _location = value;
                dataPoint_ContextMenu.Location = value;
            }
        }

        public Size SizeObject 
        { 
            get { return _sizeObject; } 
            set 
            { 
                _sizeObject = value;
                dataPoint_ContextMenu.SizeObject = value;
            } 
        }

        public void CheckHit(double x, double y)
        {
            dataPoint_ContextMenu.CheckHit(x, y);
            throw new NotImplementedException();
        }

        public void Draw(DrawingContext dc, Canvas canvas)
        {
            dataPoint_ContextMenu.Draw(dc, canvas);
            Draw_Header(dc, canvas);

            DoAnimation = false;
        }

        private void Draw_Header(DrawingContext dc, Canvas canvas) 
        {
            SolidColorBrush titleBrush = new SolidColorBrush(_titleFontColor);

            if (DoAnimation) 
            {
                DoubleAnimation tileTextAnimation = new(titleBrush.Opacity, new Duration(TimeSpan.FromMilliseconds(800)))
                { From = 0, To = 1 };

                titleBrush.ApplyAnimationClock(SolidColorBrush.OpacityProperty, tileTextAnimation.CreateClock());
            }


            FormattedText DotFormat = new(Title, System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_titleFontFamily, _titleFontStyle, _titleFontWeight, _titleFontStretch),
                                                  _titleFontSize, titleBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            System.Windows.Rect titleBox = new(new Point(Location.X + 10, Location.Y + 2), new Size(SizeObject.Width - 20, DotFormat.Height + 10));
            Point sPoint = new Point(titleBox.X, titleBox.Y + titleBox.Height);
            Point ePoint = new Point(titleBox.X + titleBox.Width, titleBox.Y + titleBox.Height);

            if (DoAnimation)
            {
                PointAnimationBase sAnim = new PointAnimation(sPoint, new Duration(TimeSpan.FromMilliseconds(800)));
                PointAnimationBase eAnim = new PointAnimation(ePoint, new Duration(TimeSpan.FromMilliseconds(800)));

                dc.DrawLine(new Pen(new SolidColorBrush(Colors.DimGray), 0.5),
                            sPoint, sAnim.CreateClock(),
                            sPoint, eAnim.CreateClock());
            }
            else 
            {
                dc.DrawLine(new Pen(new SolidColorBrush(Colors.DimGray), 0.5), sPoint, ePoint);
            }
            
            System.Windows.Point textStart = new(titleBox.X + (titleBox.Width - DotFormat.Width) / 2, 
                                                 titleBox.Y + (titleBox.Height - DotFormat.Height) / 2);
            dc.DrawText(DotFormat, textStart);
            
        }

        public bool DoAnimation
        {
            get { return _doAnimation; }
            set 
            {
                _doAnimation = value;
                dataPoint_ContextMenu.DoAnimation = value;
            }
        }

        public string Title 
        {
            get { return _title; } 
            set 
            { 
                _title = value;
            } 
        }
    }
}
