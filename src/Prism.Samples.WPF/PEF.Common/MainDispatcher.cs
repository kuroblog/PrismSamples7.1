
namespace PEF.Common
{
    using PEF.Common.PubSubEvents;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Ioc;
    using Prism.Unity;
    using System;
    using System.Windows;
    using System.Windows.Threading;

    public sealed class MainDispatcher
    {
        private static readonly Lazy<MainDispatcher> instance = new Lazy<MainDispatcher>(() => new MainDispatcher());

        public static MainDispatcher Value => instance.Value;

        private MainDispatcher() { }

        private readonly PrismApplication app = Application.Current as PrismApplication;

        public void Invoke(Action action, DispatcherPriority priority = DispatcherPriority.Normal) => app.Dispatcher.BeginInvoke(priority, action);

        public IContainerProvider Container => app.Container;

        public void ShowMessage(string message, string title = "温馨提示", Action<INotification> action = null)
        {
            Container?.Resolve<IEventAggregator>()?.GetEvent<MainNotificationPopupEvent>()?.Publish(new PopupEventArg<INotification>
            {
                Title = title,
                Content = message,
                Callback = action
            });
        }
    }
}
