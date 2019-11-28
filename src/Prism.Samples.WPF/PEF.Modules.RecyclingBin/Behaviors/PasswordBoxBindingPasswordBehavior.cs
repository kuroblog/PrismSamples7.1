
namespace PEF.Modules.RecyclingBin.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    /// 扩展 PasswordBox 支持密码绑定
    /// </summary>
    public class PasswordBoxBindingPasswordBehavior : Behavior<PasswordBox>
    {
        public string PasswordContent
        {
            get { return (string)GetValue(PasswordContentProperty); }
            set { SetValue(PasswordContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PasswordContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordContentProperty =
            DependencyProperty.Register(nameof(PasswordContent), typeof(string), typeof(PasswordBoxBindingPasswordBehavior), new PropertyMetadata(string.Empty));

        private void OnPasswordChangedHandler(object sender, RoutedEventArgs e)
        {
            PasswordContent = AssociatedObject.Password;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += OnPasswordChangedHandler;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.PasswordChanged -= OnPasswordChangedHandler;
            }
        }
    }
}
