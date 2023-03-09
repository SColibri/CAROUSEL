using System;
using System.Windows;
using System.Windows.Data;

namespace AMControls.Converters
{
    public class Converter_GridLengthToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is GridLength)
            {
                return ((GridLength)value).Value;
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return new GridLength((double)value);
            }
            else
            {
                return Binding.DoNothing;
            }
        }

    }
}
