using AMControls.Interfaces.Implementations;
using System.Windows.Media;

namespace AMControls.Charts.Fonts
{
    internal class FONT_AxisData : FontObject_Abstract
    {
        public FONT_AxisData(Visual visual) : base(visual)
        {
            this.FontSize = 10;
            this.FontStyle = System.Windows.FontStyles.Normal;
            this.FontWeight = System.Windows.FontWeights.Thin;
            this.FontStretch = System.Windows.FontStretches.Normal;
            this.Color = Colors.Black;
        }
    }
}
