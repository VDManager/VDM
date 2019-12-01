using System;
using System.Globalization;
using System.Windows.Data;



namespace VDM.GUI.ValueConverters
{
    public class AutoStartBoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsBool = (bool)value;

            return valueAsBool ? "Yes." : "No.";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsString = (string)value;

            return (valueAsString == "Yes.") ? true : false;
        }
    }
}
