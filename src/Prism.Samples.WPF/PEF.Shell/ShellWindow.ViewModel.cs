
namespace PEF.Shell
{
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Common.PubSubEvents;
    using PEF.Logger.Infrastructures;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Unity;
    using System;
    using System.Windows;
    using System.Windows.Threading;

    public class ShellWindowViewModel : BindableBase
    {
        private readonly ShellConfig config;
        private readonly IEventAggregator eventAggregator;
        private readonly ILogger logger = null;

        public ShellWindowViewModel(
            ShellConfig config,
            IEventAggregator eventAggregator,
            ILogger logger)
        {
            this.config = config;
            this.eventAggregator = eventAggregator;
            this.logger = logger;

            this.eventAggregator.GetEvent<MessagePopupEvent>().Subscribe(MessagePopupHandler);
            this.eventAggregator.GetEvent<ConfirmPopupEvent>().Subscribe(ConfirmPopupHandler);
            this.eventAggregator.GetEvent<CustomMessagePopupEvent>().Subscribe(CustomMessagePopupHandler);
            this.eventAggregator.GetEvent<CustomPopupEvent>().Subscribe(CustomPopupHandler);
            this.eventAggregator.GetEvent<ShellWindowOpacityEvent>().Subscribe(ShellWindowOpacityHandler);

            // call popup window
            // default message popup window
            //eventAggregator.GetEvent<MessagePopupPubSubEvent>().Publish(new PopupViewPubSubEventArg<INotification>
            //{
            //    Title = "提示",
            //    Content = "提示信息",
            //    Callback = new Action<INotification>(arg => { })
            //});

            // default confirm popup window
            //eventAggregator.GetEvent<ConfirmPopupPubSubEvent>().Publish(new PopupPubSubEventArg<IConfirmation>
            //{
            //    Title = "提示",
            //    Content = "提示信息",
            //    Callback = new Action<IConfirmation>(arg => { })
            //});

            // custom message popup window
            //eventAggregator.GetEvent<CustomMessagePopupPubSubEvent>().Publish(new PopupPubSubEventArg<INotification>
            //{
            //    Title = "提示",
            //    Content = "提示信息",
            //    Callback = new Action<INotification>(arg => { })
            //});

            // custom popup window
            //eventAggregator.GetEvent<CustomPopupPubSubEvent>().Publish(new PopupPubSubEventArg<ICustomPopupNotification<string>>
            //{
            //    Title = "提示",
            //    Content = "提示信息",
            //    Callback = new Action<INotification>(arg => { })
            //});

            Application.Current.DispatcherUnhandledException += AppDispatcherUnhandledExceptionHandler;
        }

        private void AppDispatcherUnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            NotificationRequest.Raise(new Notification
            {
                Title = "error",
                Content = e.Exception.GetJsonString()
            }, result => e.Handled = true);
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        //private void MessagePopupHandler(PopupEventArg<INotification> arg)
        //{
        //    MainDispatcherInvoke(() => NotificationRequest.Raise(new Notification
        //    {
        //        Title = arg.Title,
        //        Content = arg.Content
        //    }, result => arg.Callback(result)));
        //}

        //private void ConfirmPopupHandler(PopupEventArg<IConfirmation> arg)
        //{
        //    MainDispatcherInvoke(() => ConfirmationRequest.Raise(new Confirmation
        //    {
        //        Title = arg.Title,
        //        Content = arg.Content
        //    }, result => arg.Callback(result)));
        //}

        //private void CustomMessagePopupHandler(PopupEventArg<INotification> arg)
        //{
        //    MainDispatcherInvoke(() => CustomMessagePopupRequest.Raise(new Notification
        //    {
        //        Title = arg.Title,
        //        Content = arg.Content
        //    }, result => arg.Callback(result)));
        //}

        //private void CustomPopupHandler(PopupEventArg<ICustomPopupNotification<string>> arg)
        //{
        //    MainDispatcherInvoke(() => CustomPopupRequest.Raise(new CustomPopupNotification
        //    {
        //        Title = arg.Title,
        //        Content = arg.Content
        //    }, result => arg.Callback(result)));
        //}

        private void MessagePopupHandler(PopupEventArg<INotification> arg)
        {
            MainDispatcher.Value.Invoke(() => NotificationRequest.Raise(new Notification
            {
                Title = arg.Title,
                Content = arg.Content
            }, result => arg?.Callback(result)));
        }

        private void ConfirmPopupHandler(PopupEventArg<IConfirmation> arg)
        {
            MainDispatcher.Value.Invoke(() => ConfirmationRequest.Raise(new Confirmation
            {
                Title = arg.Title,
                Content = arg.Content
            }, result => arg?.Callback(result)));
        }

        private void CustomMessagePopupHandler(PopupEventArg<INotification> arg)
        {
            MainDispatcher.Value.Invoke(() => CustomMessagePopupRequest.Raise(new Notification
            {
                Title = arg.Title,
                Content = arg.Content
            }, result => arg?.Callback(result)));
        }

        private void CustomPopupHandler(PopupEventArg<ICustomPopupNotification<string>> arg)
        {
            MainDispatcher.Value.Invoke(() => CustomPopupRequest.Raise(new CustomPopupNotification
            {
                Title = arg.Title,
                Content = arg.Content
            }, result => arg?.Callback(result)));
        }
        #endregion

        public int Width => config.Width;
        public int Height => config.Height;

        private string title = "PEF Demo";

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg => { });

        public InteractionRequest<INotification> NotificationRequest { get; } = new InteractionRequest<INotification>();

        public InteractionRequest<IConfirmation> ConfirmationRequest { get; } = new InteractionRequest<IConfirmation>();

        public InteractionRequest<INotification> CustomMessagePopupRequest { get; } = new InteractionRequest<INotification>();

        public InteractionRequest<ICustomPopupNotification<string>> CustomPopupRequest { get; } = new InteractionRequest<ICustomPopupNotification<string>>();

        private double opacityValue = 1;

        public double OpacityValue
        {
            get => opacityValue;
            set => SetProperty(ref opacityValue, value);
        }

        private void ShellWindowOpacityHandler(double arg)
        {
            OpacityValue = arg;
        }
    }
}
