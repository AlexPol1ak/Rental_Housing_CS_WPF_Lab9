using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Data;

namespace CS_WPF_Lab9_Rental_Housing.Infastructure
{
    internal class ImageSourceConverter : IValueConverter
    {
        string root = Directory.GetCurrentDirectory();
        string ImageDir => Path.Combine(root, "Images");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            return Path.Combine(ImageDir, (string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = (string)value;
            FileInfo image = new FileInfo(imagePath);
            if (image.Exists) return image.Name;
            return imagePath;
        }
    }
}
