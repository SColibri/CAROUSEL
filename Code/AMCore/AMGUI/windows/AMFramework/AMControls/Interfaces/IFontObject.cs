using System.Windows.Media;

namespace AMControls.Interfaces
{
    /// <summary>
    /// Object used for drawing Text on to a visual, using the method
    /// </summary>
    public interface IFontObject
    {
        // -----------------------------------------------------------
        // FONT properties
        // -----------------------------------------------------------
        public System.Windows.Media.FontFamily FontFamily { get; set; }
        public int FontSize { get; set; }
        public System.Windows.FontStyle FontStyle { get; set; }
        public System.Windows.FontWeight FontWeight { get; set; }
        public System.Windows.FontStretch FontStretch { get; set; }
        public System.Windows.Media.Color Color { get; set; }

        // -----------------------------------------------------------
        // FORMATTED TEXT PROPERTIES
        // -----------------------------------------------------------
        public System.Globalization.CultureInfo CultureInfo { get; set; }
        public System.Windows.FlowDirection FlowDirection { get; set; }
        public System.Windows.Media.Visual Visual { get; set; }
        public string Text { get; set; }
        public FormattedText GetFormattedText();
        public FormattedText GetFormattedText(string newText);

        // -----------------------------------------------------------
        // DRAWING
        // -----------------------------------------------------------
        public System.Windows.Point Location { get; set; }
        public System.Windows.Rect Bounds { get; set; }
        public void Draw(DrawingContext dc);
        public void Draw(DrawingContext dc, string newText);
        public void Draw(DrawingContext dc, string newText, System.Windows.Point newLocation);


    }
}
