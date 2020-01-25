using System;
using Windows.UI.Xaml.Data;

namespace UWPClient.Converters
{
    public class NotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }
    }
}
