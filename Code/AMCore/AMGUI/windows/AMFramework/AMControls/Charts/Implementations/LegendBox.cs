using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AMControls.Charts.Interfaces;

namespace AMControls.Charts.Implementations
{
    public class LegendBox : ILegend
    {
        public Point Location { get; set; }
        public Size Size { get; set; }

        private List<Tuple<IDataSeries, Rect>> _seriesEntries = new();
        private List<Tuple<IDataSeries, Rect>> _seriesVisibility = new();
        private List<Tuple<IDataSeries, Rect>> _seriesFocus = new();
        private Rect _scrollNextRect = new();
        private Rect _scrollPrevRect = new();

        private int _scrollIndex = 1; // current scroll index
        private int _seriesDisplay = 10; // max number of series to display per section

        private FontFamily _legendLabelFontFamily = new("Lucida Sans");
        private int _legendLabelFontSize = 12;
        private FontStyle _legendLabelFontStyle = FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private FontWeight _legendLabelFontWeight = FontWeights.Regular; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private FontStretch _legendLabelFontStretch = FontStretches.Normal;
        private Color _legendLabelFontColor = Colors.Black;

        private FontFamily _seriesLabelFontFamily = new("Lucida Sans");
        private int _seriesLabelFontSize = 12;
        private FontStyle _seriesLabelFontStyle = FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private FontWeight _seriesLabelFontWeight = FontWeights.Regular; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private FontStretch _seriesLabelFontStretch = FontStretches.Normal;
        private Color _seriesLabelFontColor = Colors.Black;

        // Resources
        private List<BitmapImage> _imageList = new();
        public LegendBox()
        {
            _imageList.Add(new BitmapImage(new Uri("Charts/Resources/eye.png", UriKind.Relative)));
            _imageList.Add(new BitmapImage(new Uri("Charts/Resources/eye-off.png", UriKind.Relative)));
            _imageList.Add(new BitmapImage(new Uri("Charts/Resources/arrow-autofit-width.png", UriKind.Relative)));
            _imageList.Add(new BitmapImage(new Uri("Charts/Resources/arrow-big-left-line.png", UriKind.Relative)));
            _imageList.Add(new BitmapImage(new Uri("Charts/Resources/arrow-big-right-line.png", UriKind.Relative)));

        }

        public void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, List<IDataSeries> seriesList)
        {
            double yLocation = 0;
            _seriesEntries = new();
            _seriesVisibility = new();
            _seriesFocus = new();

            // Scroll Check
            if (_scrollIndex * _seriesDisplay > seriesList.Count + _seriesDisplay) _scrollIndex--;

            // Legend Title

            // Legend series
            for (int i = (_scrollIndex - 1) * _seriesDisplay; i < Math.Min(seriesList.Count, _scrollIndex * _seriesDisplay); i++)
            {
                var item = seriesList[i];

                FormattedText seriesFormat = new(item.Label, System.Globalization.CultureInfo.CurrentCulture,
                                                 FlowDirection.LeftToRight,
                                                 new Typeface(_seriesLabelFontFamily, _seriesLabelFontStyle, _seriesLabelFontWeight, _seriesLabelFontStretch),
                                                 _seriesLabelFontSize, new SolidColorBrush(_seriesLabelFontColor), VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

                // Legend options
                double totalWidth = 0;

                // ColorBox
                Rect ColorBox = new Rect(ChartArea.X + ChartArea.Width - seriesFormat.Height, ChartArea.Y + yLocation, seriesFormat.Height, seriesFormat.Height);
                dc.DrawRectangle(new SolidColorBrush(item.ColorSeries), new Pen(new SolidColorBrush(Colors.Black), 1), ColorBox);
                totalWidth += seriesFormat.Height + 5;

                // --> Eye
                int viewIndex = 0;
                if (!item.IsVisible) viewIndex = 1;
                Point OptionsStart = new(ChartArea.X + ChartArea.Width - seriesFormat.Height - totalWidth, ChartArea.Y + yLocation);
                dc.DrawImage(_imageList[viewIndex], new Rect(OptionsStart.X, OptionsStart.Y, seriesFormat.Height, seriesFormat.Height));
                totalWidth += seriesFormat.Height + 5;
                _seriesVisibility.Add(Tuple.Create(item, new Rect(OptionsStart.X, OptionsStart.Y, seriesFormat.Height, seriesFormat.Height)));

                // --> Center object
                if (!item.IsVisible) viewIndex = 1;
                OptionsStart = new(ChartArea.X + ChartArea.Width - seriesFormat.Height - totalWidth, ChartArea.Y + yLocation);
                dc.DrawImage(_imageList[2], new Rect(OptionsStart.X, OptionsStart.Y, seriesFormat.Height, seriesFormat.Height));
                totalWidth += seriesFormat.Height + 5;
                _seriesFocus.Add(Tuple.Create(item, new Rect(OptionsStart.X, OptionsStart.Y, seriesFormat.Height, seriesFormat.Height)));


                Point LabelStart = new(ChartArea.X + ChartArea.Width - seriesFormat.Width - totalWidth, ChartArea.Y + yLocation);
                dc.DrawText(seriesFormat, LabelStart);

                yLocation += seriesFormat.Height + 5;

                _seriesEntries.Add(Tuple.Create(item, new Rect(LabelStart.X, LabelStart.Y, seriesFormat.Width, seriesFormat.Height)));
            }

            // scroll menu
            if (seriesList.Count > _seriesDisplay)
            {
                double totalWidth = 0;
                Point ScrollStart = new(ChartArea.X + ChartArea.Width - 25, ChartArea.Y + yLocation);
                dc.DrawImage(_imageList[4], new Rect(ScrollStart.X, ScrollStart.Y, 25, 25));
                totalWidth += 25 + 5;
                _scrollNextRect = new(ScrollStart.X, ScrollStart.Y, 25, 25);

                ScrollStart = new(ScrollStart.X - totalWidth, ChartArea.Y + yLocation);
                dc.DrawImage(_imageList[3], new Rect(ScrollStart.X, ScrollStart.Y, 25, 25));
                _scrollPrevRect = new(ScrollStart.X, ScrollStart.Y, 25, 25);
            }
        }

        public IDataSeries? Check_series_Hit(double x, double y)
        {
            if (Check_Visibility_Hit(x, y)) return null;
            if (Check_Scroll_Hit(x, y)) return null;

            Tuple<IDataSeries, Rect> Result = _seriesVisibility.Find(e => e.Item2.Contains(x, y));

            Result = _seriesEntries.Find(e => e.Item2.Contains(x, y));
            if (Result == null) return null;

            return Result.Item1;
        }

        private bool Check_Visibility_Hit(double x, double y)
        {
            Tuple<IDataSeries, Rect> Result = _seriesVisibility.Find(e => e.Item2.Contains(x, y));
            if (Result == null) return false;

            Result.Item1.IsVisible = !Result.Item1.IsVisible;

            return true;
        }

        private bool Check_Scroll_Hit(double x, double y)
        {
            if (_scrollNextRect.Contains(x, y))
            {
                _scrollIndex += 1;
                return true;
            }
            else if (_scrollPrevRect.Contains(x, y))
            {
                if (_scrollIndex - 1 > 0) _scrollIndex -= 1;
                return true;
            }

            return false;
        }
    }
}
