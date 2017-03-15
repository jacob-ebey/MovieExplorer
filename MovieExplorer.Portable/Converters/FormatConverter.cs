using MvvmCross.Platform.Converters;
using System;
using System.Globalization;

namespace MovieExplorer.Converters
{
    public class FormatConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(parameter as string, value);
        }
    }
}
