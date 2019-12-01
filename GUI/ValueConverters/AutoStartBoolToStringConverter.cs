using System;
using System.Globalization;
using System.Windows.Data;



namespace VDM.GUI.ValueConverters
{
    public class AutoStartBoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsBool = (bool)value;

            return valueAsBool ? "AutoStart: ON" : "AutoStart: OFF";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsString = (string)value;

            return (valueAsString == "AutoStart: ON") ? true : false;
        }
    }
}
