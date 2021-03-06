﻿using System;
using System.Globalization;
using System.Windows.Data;
using Caliburn.Micro;

namespace WpfClient.Converters
{
    public class ToShortDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return DateTime.Parse(value.ToString()).ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
