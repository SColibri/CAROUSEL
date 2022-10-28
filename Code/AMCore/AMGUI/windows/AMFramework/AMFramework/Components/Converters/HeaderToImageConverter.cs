using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using AMFramework_Lib.Model;

namespace AMFramework.Components.Converters
{
    /// <summary>
    /// convert image path to image
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    [Obsolete("Parameters are declared as fonawesomeIcons, this is not needed anymore")]
    internal class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Default image
            string imagePath = "Resources/Icons/tablerIcons/fidget-spinner.png";
            string tagType = "";
            string imgName = "";
            // if Tag is of tyype string
            if (value is null) tagType = "PROJECT";
            else if (value.GetType().Equals(typeof(string))) { tagType = (string)value; }
            else if (value.GetType().Equals(typeof(Model_Projects))) { tagType = "PROJECT"; }
            else if (value.GetType().Equals(typeof(Model_Case))) { tagType = "CASEITEM"; }

            if (tagType.ToUpper().CompareTo("PROJECT") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/layout-board.png";
                imgName = "layout-board";
            }
            else if (tagType.ToUpper().CompareTo("SINGLE PIXEL CASES") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/marquee.png";
                imgName = "marquee";
            }
            else if (tagType.ToUpper().CompareTo("OBJECT") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/3d-cube-sphere.png";
                imgName = "3d-cube-sphere";
            }
            else if (tagType.ToUpper().CompareTo("SELECTION") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/clipboard-list.png";
                imgName = "clipboard-list";
            }
            else if (tagType.ToUpper().CompareTo("CASEITEM") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/dots.png";
                imgName = "dots";
            }

            // return new BitmapImage(new Uri($"/{ imagePath }", UriKind.Relative));
            return AMsystem.AMFramework_ImageSource.Get_faIcon(imgName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
