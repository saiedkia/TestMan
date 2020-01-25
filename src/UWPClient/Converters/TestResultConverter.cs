using BulkTest.Models;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UWPClient.Converters
{
    public class TestResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TestResult result = (TestResult)value;
            switch (result)
            {
                case TestResult.Failed:
                    return new SolidColorBrush(Colors.Red);
                case TestResult.Passed:
                    return new SolidColorBrush(Colors.Green);
                default:
                    return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
