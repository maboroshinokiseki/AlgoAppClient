using AlgoApp.Models.Data;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AlgoApp.Converters
{
    class QuestionStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((QuestionStatus)value)
            {
                case QuestionStatus.Untouched:
                    return Color.Black;
                case QuestionStatus.Touched:
                    return Color.Gray;
                case QuestionStatus.WrongAnswer:
                    return Color.Red;
                case QuestionStatus.CorrectAnswer:
                    return Color.Green;
            }

            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
