using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AMFramework.Components.Converters
{
    /// <summary>
    /// convert image path to image
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    internal class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Default image
            string imagePath = "Resources/Icons/tablerIcons/fidget-spinner.png";
            string tagType = "";
            // if Tag is of tyype string
            if (value is null) tagType = "PROJECT";
            else if (value.GetType().Equals(typeof(string))) { tagType = (string)value; }
            else if (value.GetType().Equals(typeof(Model.Model_Projects))) { tagType = "PROJECT"; }
            else if (value.GetType().Equals(typeof(Model.Model_Case))) { tagType = "CASEITEM"; }

            if (tagType.ToUpper().CompareTo("PROJECT") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/layout-board.png";
            }
            else if (tagType.ToUpper().CompareTo("CASELIST") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/marquee.png";
            }
            else if (tagType.ToUpper().CompareTo("OBJECT") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/3d-cube-sphere.png";
            }
            else if (tagType.ToUpper().CompareTo("SELECTION") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/clipboard-list.png";
            }
            else if (tagType.ToUpper().CompareTo("CASEITEM") == 0)
            {
                imagePath = "Resources/Icons/tablerIcons/dots.png";
            }

            return new BitmapImage(new Uri($"/{ imagePath }", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
