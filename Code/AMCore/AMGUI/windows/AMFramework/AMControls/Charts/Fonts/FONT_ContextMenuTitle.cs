using AMControls.Interfaces.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AMControls.Charts.Fonts
{
    internal class FONT_ContextMenuTitle : FontObject_Abstract
    {
        public FONT_ContextMenuTitle(Visual visual) : base(visual)
        {
            this.FontSize = 12;
            this.FontStyle = System.Windows.FontStyles.Normal;
            this.FontWeight = System.Windows.FontWeights.Bold;
            this.FontStretch = System.Windows.FontStretches.Normal;
            this.Color = Colors.Black;
        }
    }
}
