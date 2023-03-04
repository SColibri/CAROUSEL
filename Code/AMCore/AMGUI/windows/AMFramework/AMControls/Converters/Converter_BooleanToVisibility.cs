using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AMControls.Converters
{
    public class Converter_BooleanToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool) throw new Exception("Incorrect type");

            bool tempRef = (bool)value;
            if (tempRef) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Visibility) throw new Exception("Incorrect type");

            Visibility tempRef = (Visibility)value;
            if (tempRef == Visibility.Visible) return true;

            return false;
        }
    }
}
