using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfClient.Converters
{
    [ValueConversion(typeof(string), typeof(int))]
    public class OnlyNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string number = value as string;

            if (string.IsNullOrEmpty(number) || !int.TryParse(number, out var parsed))
                return 0;

            return parsed;
        }
    }
}
