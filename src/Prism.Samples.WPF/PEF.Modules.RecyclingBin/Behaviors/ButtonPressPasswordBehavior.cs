
namespace PEF.Modules.RecyclingBin.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    /// 扩展 Button 的密码输入
    /// </summary>
    public class ButtonPressPasswordBehavior : Behavior<Button>
    {
        public PasswordBox PasswordBoxObject
        {
            get { return (PasswordBox)GetValue(PasswordBoxObjectProperty); }
            set { SetValue(PasswordBoxObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PasswordBoxObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordBoxObjectProperty =
            DependencyProperty.Register(nameof(PasswordBoxObject), typeof(PasswordBox), typeof(ButtonPressPasswordBehavior), null);

        private void OnClickHandler(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxObject == null)
            {
                return;
            }

            if (AssociatedObject.Content.ToString() == "DEL")
            {
                if (PasswordBoxObject.Password.Length > 0) { PasswordBoxObject.Password = PasswordBoxObject.Password.Remove(PasswordBoxObject.Password.Length - 1, 1); }
                else { }
            }
            else
            {
                PasswordBoxObject.Password += AssociatedObject.Content;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Click += OnClickHandler;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.Click -= OnClickHandler;
            }
        }
    }
}
