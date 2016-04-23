using System;
using System.Globalization;
using System.Windows.Data;

namespace MBassWPF
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
                return null;

            var i = (double)value;

            var ts = TimeSpan.FromSeconds(i < 0 ? 0 : i);

            return ts.Hours == 0 ? $"{ts.Minutes:D2}:{ts.Seconds:D2}"
                                 : $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}