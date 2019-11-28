
namespace PEF.Modules.ZZJ.TestClient.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using System.Windows.Media;

    public class FilePathValidationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new SolidColorBrush(Colors.Red);
            if (value == null) { }
            else if (string.IsNullOrEmpty(value.ToString())) { }
            //else if (int.TryParse(value.ToString(), out int res) && res > 0)
            else if (File.Exists(value.ToString().Trim()))
            {
                return new SolidColorBrush(Colors.Green);
            }
            else { }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
