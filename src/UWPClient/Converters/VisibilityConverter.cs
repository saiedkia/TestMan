using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace UWPClient.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool) return (bool)value;
            return value != null || ((value as IEnumerable<object>)?.Count() == 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
