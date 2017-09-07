using System;
using System.Globalization;
using System.Windows.Data;

namespace AE.AuditPlanning.Converter
{
    public class EnumToImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format("../img/{0}.png", value.ToString().ToLower());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}