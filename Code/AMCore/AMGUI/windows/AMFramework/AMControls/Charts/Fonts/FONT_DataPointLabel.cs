using AMControls.Interfaces.Implementations;
using System.Windows.Media;

namespace AMControls.Charts.Fonts
{
    internal class FONT_DataPointLabel : FontObject_Abstract
    {
        public FONT_DataPointLabel(Visual visual) : base(visual)
        {
            this.FontSize = 12;
            this.FontStyle = System.Windows.FontStyles.Normal;
            this.FontWeight = System.Windows.FontWeights.Bold;
            this.FontStretch = System.Windows.FontStretches.Normal;
            this.Color = Colors.Black;
        }
    }
}
