
namespace PEF.Modules.RecyclingBin.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    /// 扩展 PasswordBox 初始化时清空密码
    /// </summary>
    public class PasswordBoxOnLoadedCleanBehavior : Behavior<PasswordBox>
    {
        private void OnLoadedHandler(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Clear();
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoadedHandler;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.PasswordChanged -= OnLoadedHandler;
            }
        }
    }
}
