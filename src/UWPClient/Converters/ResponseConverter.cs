using BulkTest.Models;
using System;
using System.Text;
using Windows.UI.Xaml.Data;

namespace UWPClient.Converters
{
    public class ResponseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is Response response)) return null;

            StringBuilder strMessage = new StringBuilder();
            strMessage.Append("output: \n" + Environment.NewLine);
            strMessage.Append(response.Body + Environment.NewLine + "statuscode:");
            strMessage.Append(Environment.NewLine + response.StatusCode.ToString());

            return strMessage.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
