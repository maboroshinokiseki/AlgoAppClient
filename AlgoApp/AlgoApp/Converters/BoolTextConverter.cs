using System;
using System.Globalization;
using Xamarin.Forms;

namespace AlgoApp.Converters
{
    class BoolTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return "正确";
            }
            else
            {
                return "错误";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string)value;
            if (s == "正确" || s?.ToLower() == "yes" || s?.ToLower() == "correct")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
