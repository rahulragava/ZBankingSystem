using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Web.UI;
using ZBMSLibrary.Entities.Enums;

namespace ZBMS.Util.Converters
{
    public class TypeToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case AccountStatus.Active:
                    return new SolidColorBrush(Colors.SeaGreen);
                case AccountStatus.InActive:
                    return new SolidColorBrush(Colors.Orange);
                case AccountStatus.Closed:
                    return new SolidColorBrush(Colors.IndianRed);
                case TransactionType.Credit:
                    return new SolidColorBrush(Colors.IndianRed);
                case TransactionType.Debit:
                    return new SolidColorBrush(Colors.ForestGreen);

            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
