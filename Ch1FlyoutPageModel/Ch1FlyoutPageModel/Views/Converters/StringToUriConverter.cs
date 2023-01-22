using System;
using System.Globalization;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.Views.Converters
{
    public class StringToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string uriString && targetType is { Name: "Uri" })
            {
                return new Uri(uriString);
            }
            return new Uri("");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}