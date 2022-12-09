using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AMControls.Interfaces.Implementations
{
    public abstract class FontObject_Abstract : IFontObject
    {
        public FontObject_Abstract(Visual visual) 
        { 
            this.Visual = visual;
        }
        
        #region IFontObject
        public FontFamily FontFamily { get; set; } = new("Lucida Sans");
        public int FontSize { get; set; } = 12;
        public FontStyle FontStyle { get; set; } = System.Windows.FontStyles.Normal;
        public FontWeight FontWeight { get; set; } = System.Windows.FontWeights.Bold;
        public FontStretch FontStretch { get; set; }  = System.Windows.FontStretches.Normal;
        public Color Color { get; set; } = Colors.Black;
        public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;
        public FlowDirection FlowDirection { get; set; } = FlowDirection.LeftToRight;
        public Visual Visual { get; set; }
        public string Text { get; set; } = "";
        public Point Location { get; set; } = new(0, 0);
        public Rect Bounds { get; set; } = new();

        public void Draw(System.Windows.Media.DrawingContext dc) 
        {
            dc.DrawText(GetFormattedText(), Location);
        }
        public void Draw(System.Windows.Media.DrawingContext dc, string newText) 
        {
            dc.DrawText(GetFormattedText(newText), Location);
        }
        public void Draw(System.Windows.Media.DrawingContext dc, string newText, Point newLocation) 
        {
            dc.DrawText(GetFormattedText(newText), newLocation);
        }
        public FormattedText GetFormattedText() 
        {
            return GetFormattedText(this.Text);
        }
        public FormattedText GetFormattedText(string newText) 
        {
            Typeface tF = new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch);
            SolidColorBrush scb = new SolidColorBrush(this.Color);
            double PixelsPdpi = VisualTreeHelper.GetDpi(this.Visual).PixelsPerDip;

            FormattedText labelFormat = new(newText,
                                            this.CultureInfo,
                                            this.FlowDirection,
                                            tF,
                                            this.FontSize,
                                            scb,
                                            PixelsPdpi);

            return labelFormat;

        }
        #endregion
    }
}
