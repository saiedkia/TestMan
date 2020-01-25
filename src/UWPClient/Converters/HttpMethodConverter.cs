using BulkTest.Models;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UWPClient.Converters
{
    public class HttpMethodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is RequestType)
                return GetBrush((RequestType)value);
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }



        private AcrylicBrush GetBrush(RequestType requestType)
        {
            AcrylicBrush brush = new AcrylicBrush
            {
                TintOpacity = 0.4
            };

            switch (requestType)
            {
                case RequestType.Get:
                    brush.TintColor = ToUIColor(System.Drawing.Color.LimeGreen);
                    break;
                case RequestType.Post:
                    brush.TintColor = ToUIColor(System.Drawing.Color.ForestGreen);
                    break;
                case RequestType.Delete:
                    brush.TintColor = ToUIColor(System.Drawing.Color.Red);
                    break;
                case RequestType.Put:
                    brush.TintColor = ToUIColor(System.Drawing.Color.Orange);
                    break;
                default:
                    return null;
            }

            return brush;
        }

        private Color ToUIColor(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
