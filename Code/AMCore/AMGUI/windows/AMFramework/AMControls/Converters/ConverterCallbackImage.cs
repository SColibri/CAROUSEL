using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using FontAwesome.WPF;

namespace AMControls.Converters
{
    /// <summary>
    /// This class converts 
    /// </summary>
    public class ConverterCallbackImage : IValueConverter
    {
        /// <summary>
        /// Converts enum type to string and compares it with valid callback options
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? optionName = value.ToString();

            if (optionName != null)
            {
                switch (optionName)
                {
                    case "Message":
                        return new ImageAwesome() { Icon = FontAwesomeIcon.FileText, Foreground = Brushes.SlateGray };
                    
                    case "Error":
                        return new ImageAwesome() { Icon = FontAwesomeIcon.ExclamationTriangle, Foreground = Brushes.DarkRed };
                    
                    default:
                        return new ImageAwesome() { Icon = FontAwesomeIcon.BatteryEmpty, Foreground = Brushes.DarkOrange };
                }
            }

            // no valid option
            return new ImageAwesome() { Icon = FontAwesomeIcon.Image, Foreground = Brushes.Yellow };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
