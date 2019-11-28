
namespace PEF.Modules.SGDE.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class ItemConfigButton : Button
    {
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ItemConfigButton), new PropertyMetadata(string.Empty));

        public string Size
        {
            get => (string)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(string), typeof(ItemConfigButton), new PropertyMetadata(string.Empty));

        public string ConfigValue
        {
            get => (string)GetValue(SizeValueProperty);
            set => SetValue(SizeValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for SizeValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeValueProperty =
            DependencyProperty.Register(nameof(ConfigValue), typeof(string), typeof(ItemConfigButton), new PropertyMetadata(string.Empty));
    }
}
