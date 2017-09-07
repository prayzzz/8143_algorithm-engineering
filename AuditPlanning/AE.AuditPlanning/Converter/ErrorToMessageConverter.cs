using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace AE.AuditPlanning.Converter
{
    public class ErrorToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errors = value as ReadOnlyCollection<ValidationError>;

            if (errors == null)
            {
                return null;
            }

            var errorString = new StringBuilder();

            foreach (var validationError in errors.Where(x => x.ErrorContent != null))
            {
                errorString.Append(validationError.ErrorContent);
            }

            return errorString.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}