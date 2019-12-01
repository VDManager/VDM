using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;



namespace VDM.GUI.ValueConverters
{
    public class AutoStartBoolToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsBool = (bool)value;

            return valueAsBool
                ? new SolidColorBrush() { Color = Color.FromRgb(0, 205, 10), Opacity = 1.0 } // #FF00CD0A
                : Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsBrush = (SolidColorBrush)value;

            return (valueAsBrush == Brushes.Red) ? false : true;
        }
    }
}
