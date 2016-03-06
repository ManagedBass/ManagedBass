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

            double i = (double)value;

            var TS = TimeSpan.FromSeconds(i < 0 ? 0 : i);

            return TS.Hours == 0 ? string.Format("{0:D2}:{1:D2}", TS.Minutes, TS.Seconds)
                                 : string.Format("{0:D2}:{1:D2}:{2:D2}", TS.Hours, TS.Minutes, TS.Seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}