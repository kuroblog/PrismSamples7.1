
namespace PEF.Modules.SGDE.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class SizeChoiceButtonEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }
            else if (int.TryParse(value.ToString(), out int res) && res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class SizeChoiceButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? Visibility.Collapsed : string.IsNullOrEmpty(value.ToString()) ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class ItemConfigButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Hidden;
            }
            else if (string.IsNullOrEmpty(value.ToString()))
            {
                return Visibility.Hidden;
            }
            //else if (int.TryParse(value.ToString(), out int res) && res != 0)
            //{
            //    return Visibility.Visible;
            //}
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class SizeQuantityRowHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new GridLength(0, GridUnitType.Star);
            }
            else if (string.IsNullOrEmpty(value.ToString()))
            {
                return new GridLength(0, GridUnitType.Star);
            }
            else
            {
                return new GridLength(50, GridUnitType.Star);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
