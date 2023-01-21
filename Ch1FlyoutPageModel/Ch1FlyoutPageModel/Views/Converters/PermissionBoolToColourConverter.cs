using System;
using System.Globalization;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.Views.Converters
{
    public class PermissionBoolToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is false)
            {
                return App.ThemeIsDark ? App.Colours["Accent"] : App.Colours["AccentLight"];
            }
            return Color.Transparent;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}