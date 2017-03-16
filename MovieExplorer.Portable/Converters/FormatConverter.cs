using MvvmCross.Platform.Converters;
using System;
using System.Globalization;

namespace MovieExplorer.Converters
{
    public class FormatConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(parameter as string, value);
        }
    }
}
