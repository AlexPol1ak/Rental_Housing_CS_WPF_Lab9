using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;

namespace CS_WPF_Lab9_Rental_Housing.Infastructure
{
    internal class NullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (value != null && value is string str)
            {
                if( 
                    (int.TryParse(str, out int numberInt) && numberInt ==0) || 
                    (double.TryParse(str, out double numberDouble) && numberDouble == 0) ||
                    (ulong.TryParse(str, out ulong numberUlong) && numberUlong == 0)
                 ) return string.Empty;
            }
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
