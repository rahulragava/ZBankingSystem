using System;
using Windows.UI.Xaml.Data;

namespace ZBMS.Util.Converters
{
    public class DateFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;
            DateTime dt = DateTime.Parse(value.ToString());
            return dt.ToString("dd MMM yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotSupportedException();
        }
    }
}