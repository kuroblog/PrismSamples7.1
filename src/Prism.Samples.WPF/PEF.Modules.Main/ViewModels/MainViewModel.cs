
namespace PEF.Modules.Main.ViewModels
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using PEF.Logger.Infrastructures;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Windows;
    using System.Windows.Threading;

    public partial class MainViewModel : BindableBase
    {
        private readonly IRegionManager region = null;
        public readonly IEventAggregator eventAggregator;
        private readonly ILogger logger = null;
        private readonly MainConfig config = null;

        public MainViewModel(
            IRegionManager region,
            IEventAggregator eventAggregator,
            ILogger logger,
            MainConfig config)
        {
            this.region = region;
            this.eventAggregator = eventAggregator;
            this.logger = logger;
            this.config = config;

            this.eventAggregator.GetEvent<MainNotificationPopupEvent>().Subscribe(MessagePopupHandler);
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            //region?.RequestNavigate(RegionNames.Content, config.ContentModule);
        });

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        //private void MessagePopupHandler(PopupEventArg<INotification> arg)
        //{
        //    MainDispatcherInvoke(() => MainNotificationRequest.Raise(new Notification
        //    {
        //        Title = arg.Title,
        //        Content = arg.Content
        //    }, result => arg.Callback(result)));
        //}

        private void MessagePopupHandler(PopupEventArg<INotification> arg)
        {
            MainDispatcher.Value.Invoke(() => MainNotificationRequest.Raise(new Notification
            {
                Title = arg.Title,
                Content = arg.Content
            }, result => arg.Callback?.Invoke(result)));
        }
        #endregion

        public InteractionRequest<INotification> MainNotificationRequest { get; } = new InteractionRequest<INotification>();
    }
}
