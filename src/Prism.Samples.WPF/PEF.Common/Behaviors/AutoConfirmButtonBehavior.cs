
namespace PEF.Common.Behaviors
{
    using Prism.Unity;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using System.Windows.Threading;

    /// <summary>
    /// 当 Button 可用时，触发绑定的 Command
    /// </summary>
    /// <remarks>
    /// TODO
    /// 1) 未完成，在非主线程时会引发错误
    /// </remarks>
    public class AutoConfirmButtonBehavior : Behavior<Button>
    {
        public int AutoConfirmTime
        {
            get => (int)GetValue(AutoConfirmTimeProperty);
            set => SetValue(AutoConfirmTimeProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoConfirmTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoConfirmTimeProperty =
            DependencyProperty.Register("AutoConfirmTime", typeof(int), typeof(AutoConfirmButtonBehavior), new PropertyMetadata(9));

        private string title;

        protected override void OnAttached()
        {
            base.OnAttached();

            title = AssociatedObject.Content as string;

            AssociatedObject.IsEnabledChanged += OnIsEnabledChangedHandler;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.IsEnabledChanged -= OnIsEnabledChangedHandler;
            }
        }

        private void OnIsEnabledChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (AssociatedObject.IsEnabled)
            {
                var i = AutoConfirmTime;

                for (; ; )
                {
                    #region thread sync test
                    //if (i == 0)
                    //{
                    //    (Application.Current as PrismApplication).Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    //    {
                    //        AssociatedObject.Command?.Execute(null);
                    //        AssociatedObject.Content = $"{title}";
                    //    }));

                    //    break;
                    //}

                    //(Application.Current as PrismApplication).Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => AssociatedObject.Content = $"{title} ({i})"));

                    if (i == 0)
                    {
                        MainDispatcher.Value.Invoke(new Action(() =>
                        {
                            AssociatedObject.Command?.Execute(null);
                            AssociatedObject.Content = $"{title}";
                        }));

                        break;
                    }

                    MainDispatcher.Value.Invoke(new Action(() => AssociatedObject.Content = $"{title} ({i})"));
                    #endregion

                    Task.Delay(1000).Wait();
                    i--;
                }
            }
        }
    }
}