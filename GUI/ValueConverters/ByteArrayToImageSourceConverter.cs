using System;
using System.Globalization;
using System.Windows.Data;

using VDM.GUI.Helpers;



namespace VDM.GUI.ValueConverters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsByteArray = (byte[])value;

            if (valueAsByteArray != null)
                return Converters.ByteArrayToImageSource(valueAsByteArray);
            else
                return "pack://application:,,,/VDM;component/Resources/icon_default_vdp.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ImageSource to byte[] conversion not supported.");
        }
    }
}
