using System;
using System.Globalization;
using Xamarin.Forms;

namespace MKDemoApp.Converters
{
    public class DivisionByNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var denominator = PrepareParameter(parameter);

            return value switch
            {
                double doubleValue => (doubleValue / denominator),
                float floatValue => (floatValue / denominator),
                int intValue => (intValue / denominator),
                long longValue => (longValue / denominator),
                short shortValue => (shortValue / denominator),
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static double PrepareParameter(object parameter) =>
            parameter switch
            {
                double doubleParameter => doubleParameter,
                int intParameter => intParameter,
                string stringParameter => double.Parse(stringParameter),
                _ => 1
            };
    }
}