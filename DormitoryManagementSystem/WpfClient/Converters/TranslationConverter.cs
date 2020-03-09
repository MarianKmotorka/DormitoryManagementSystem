using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;

namespace WpfClient.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class TranslationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return null;

            var dictionary = IoC.Get<ResourceDictionary>("language");
            var translated = dictionary[value.ToString()].ToString();
            return translated;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
