using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPClient.Converters
{
    public class IntentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int leftMargin = (int)value;
            return new Thickness(leftMargin, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
