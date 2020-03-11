using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfClient.Converters
{
    [ValueConversion(typeof(string), typeof(int?))]
    public class OnlyNumberOrNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            return int.TryParse(value.ToString(), out var parsed) ? (int?)parsed : null;
        }
    }
}
