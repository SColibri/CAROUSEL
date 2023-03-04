using System;
using System.Globalization;
using System.Windows.Data;

namespace AMFramework.Components.Converters
{
    [ValueConversion(typeof(string), typeof(double))]
    internal class AdditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value;
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
