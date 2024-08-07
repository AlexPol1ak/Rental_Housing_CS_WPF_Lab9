using System.Globalization;
using System.Windows.Data;

namespace CS_WPF_Lab9_Rental_Housing.Infastructure
{
    internal class NullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (value is int i && i == 0) return string.Empty;
            if (value is double d && d == 0) return string.Empty;

            return value;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is string str &&
                (string.IsNullOrEmpty(str) && string.IsNullOrWhiteSpace(str)))
                return null;

            return value;
        }
    }
}
