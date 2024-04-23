using System;
using Xamarin.Forms;

namespace MauiApp1.NewFolder1
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                var colors = parameter.ToString().Split(',');
                if (boolValue)
                    return Color.FromHex(colors[1]); // Green
                else
                    return Color.FromHex(colors[0]); // Red
            }

            // Return Default if value is not a valid boolean
            return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
