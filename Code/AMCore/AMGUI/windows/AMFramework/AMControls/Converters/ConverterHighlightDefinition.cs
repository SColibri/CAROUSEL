using AMControls.Custom.Scripting;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AMControls.Converters
{
    internal class ConverterHighlightDefinition : IValueConverter
    {
        private static readonly HighlightingDefinitionTypeConverter Converter = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new LuaTextHighlighter(); //Converter.ConvertFrom(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "Lua";//Converter.ConvertToString(value);
        }
    }
}
