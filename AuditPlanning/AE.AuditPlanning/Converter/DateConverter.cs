using System;
using System.Globalization;
using System.Windows.Data;

namespace AE.AuditPlanning.Converter
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var date = (DateTime)value;
                return date.ToString(parameter.ToString());
            }

            if (value is TimeSpan)
            {
                var date = (TimeSpan)value;
                return date.ToString(parameter.ToString());
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}