
namespace PEF.Modules.SGDE.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class SizeBorderBox : GroupBox
    {
        public Thickness BoxMargin
        {
            get => (Thickness)GetValue(BoxMarginProperty);
            set => SetValue(BoxMarginProperty, value);
        }

        // Using a DependencyProperty as the backing store for BoxMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxMarginProperty =
            DependencyProperty.Register(nameof(BoxMargin), typeof(Thickness), typeof(SizeBorderBox), new PropertyMetadata(new Thickness(0)));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(SizeBorderBox), new PropertyMetadata(string.Empty));

        public string SizeValue
        {
            get => (string)GetValue(SizeValueProperty);
            set => SetValue(SizeValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for SizeValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeValueProperty =
            DependencyProperty.Register(nameof(SizeValue), typeof(string), typeof(SizeBorderBox), new PropertyMetadata(string.Empty));
    }
}
