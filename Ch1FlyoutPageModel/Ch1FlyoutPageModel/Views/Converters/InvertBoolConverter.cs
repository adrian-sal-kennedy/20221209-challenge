using System;
using System.Globalization;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.Views.Converters
{
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is false;
        }
    }
}
