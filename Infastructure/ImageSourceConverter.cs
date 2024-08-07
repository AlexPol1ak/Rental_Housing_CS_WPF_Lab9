using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CS_WPF_Lab9_Rental_Housing.Infastructure
{
    public class ImageSourceConverter : IValueConverter
    {
        string root = Directory.GetCurrentDirectory();
        string ImageDir => Path.Combine(root, "Images");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            string photoPath = Path.Combine(ImageDir, (string)value);
            var image = new BitmapImage();
            using (var stram = File.OpenRead(photoPath))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stram;
                image.EndInit();
            }
            return image;
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
