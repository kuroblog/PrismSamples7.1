
namespace PEF.Modules.ShoeBox.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class ItemConfigButton : Button
    {
        //public string Code
        //{
        //    get => (string)GetValue(CodeProperty);
        //    set => SetValue(CodeProperty, value);
        //}

        //// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty CodeProperty =
        //    DependencyProperty.Register(nameof(Code), typeof(string), typeof(ConfigBorderBox), new PropertyMetadata(string.Empty));

        //public string UserName
        //{
        //    get => (string)GetValue(UserNameProperty);
        //    set => SetValue(UserNameProperty, value);
        //}

        //// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty UserNameProperty =
        //    DependencyProperty.Register(nameof(UserName), typeof(string), typeof(ConfigBorderBox), new PropertyMetadata(string.Empty));

        public ConfigState State
        {
            get => (ConfigState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(ConfigState), typeof(ItemConfigButton), new PropertyMetadata(ConfigState.Unused));
    }

    public class ConfigBorderBoxVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? Visibility.Hidden : string.IsNullOrEmpty(value.ToString()) ? Visibility.Hidden : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public enum ConfigState
    {
        [Description("未知")]
        Unknown = 0x00,
        [Description("未使用")]
        Unused = 0x01,
        [Description("使用中")]
        Locked = 0x02,
        [Description("待回收")]
        ToBeRecycled = 0x03,
        [Description("开启中")]
        Opened = 0x04
    }
}
