using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Marketplaes02_for_sotrudnik
{
    public  class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value.ToString();
            Uri uri;

            if (path.StartsWith("http"))
            {
                // Для изображений из Интернета
                uri = new Uri(path);
            }
            else
            {
                // Для локальных изображений
                string absolutePath = Path.GetFullPath(path);
                uri = new Uri(absolutePath);
            }

            return new BitmapImage(uri);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}
