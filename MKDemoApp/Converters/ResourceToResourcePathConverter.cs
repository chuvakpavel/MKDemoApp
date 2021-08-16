using System;
using System.Globalization;
using Xamarin.Forms;

namespace MKDemoApp.Converters
{
    public class ResourceToResourcePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is not string source || string.IsNullOrWhiteSpace(source)
                ? null
                : $"resource://{source}";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}