
namespace PEF.Common.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class CustomButton : Button
    {
        public CornerRadius CustomCornerRadius
        {
            get => (CornerRadius)GetValue(CustomCornerRadiusProperty);
            set => SetValue(CustomCornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CustomCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomCornerRadiusProperty =
            DependencyProperty.Register(nameof(CustomCornerRadius), typeof(CornerRadius), typeof(CustomButton), new PropertyMetadata(new CornerRadius(0)));
    }
}
