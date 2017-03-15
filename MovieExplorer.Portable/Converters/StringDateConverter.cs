using MvvmCross.Platform.Converters;
using System;
using System.Globalization;

namespace MovieExplorer.Converters
{
    public class StringDateConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = DateTime.Parse(value).ToString("M/d/yyyy");
            if (parameter as string != null)
                return string.Format(parameter as string, date);
            else
                return date;
        }
    }
}
