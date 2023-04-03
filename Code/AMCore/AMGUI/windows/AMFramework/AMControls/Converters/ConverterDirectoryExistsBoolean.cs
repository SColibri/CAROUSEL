using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AMControls.Converters
{
    /// <summary>
    /// FileExistsBooleanConverter checks if file exists or not and returns a Boolean. Uses System.IO
    /// </summary>
    public class ConverterDirectoryExistsBoolean : IValueConverter
    {
        /// <summary>
        /// Check if filename is valid or not
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Directory.Exists(value as string);
        }

        /// <summary>
        /// Boolean to path is not possible
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
