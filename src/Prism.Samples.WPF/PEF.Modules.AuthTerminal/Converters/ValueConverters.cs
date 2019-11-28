
namespace PEF.Modules.AuthTerminal.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class ShowAuthMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //=> value == null ? Visibility.Hidden : string.IsNullOrEmpty(value.ToString()) ? Visibility.Hidden : Visibility.Visible;

            if (parameter == null || value == null)
            {
                return Visibility.Hidden;
            }

            if (int.TryParse(parameter.ToString(), out int mode) && bool.TryParse(value.ToString(), out bool isShow))
            {
                if (mode == 1)
                {
                    return isShow ? Visibility.Visible : Visibility.Hidden;
                }
                else
                {
                    return !isShow ? Visibility.Visible : Visibility.Hidden;
                }
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class RoleVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? Visibility.Hidden : string.IsNullOrEmpty(value.ToString()) ? Visibility.Hidden : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
