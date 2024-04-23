using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace MauiApp1.NewFolder1
{
    public class BoolToColorConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCorrect && isCorrect)
            {
                return Color.FromRgb(0, 255, 0); // Return Green if the value is true
            }
            else
            {
                return Color.FromRgb(255, 0, 0); // Return Red if the value is false or not a boolean
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
