using AMControls.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AMControls.Charts.Fonts
{
    public class ChartFont_Factory
    {
        public enum ChartFonts
        {
            AxisData,
            AxisLabel,
            DataPointLabel,
            ContextMenuContent,
            ContextMenuSubtitle,
            ContextMenuTitle
        }
        public static IFontObject Get_FontObject(ChartFonts fObject, Visual vObject) 
        {
            switch (fObject)
            {
                case ChartFonts.AxisData:
                    return new FONT_AxisData(vObject);
                case ChartFonts.AxisLabel:
                    return new FONT_AxisLabel(vObject);
                case ChartFonts.DataPointLabel:
                    return new FONT_DataPointLabel(vObject);
                case ChartFonts.ContextMenuContent:
                    return new FONT_ContextMenuContent(vObject);
                case ChartFonts.ContextMenuSubtitle:
                    return new FONT_ContextMenuSubtitle(vObject);
                case ChartFonts.ContextMenuTitle:
                    return new FONT_ContextMenuTitle(vObject);
                default:
                    throw new NotImplementedException();
            }

        }
    }
}
