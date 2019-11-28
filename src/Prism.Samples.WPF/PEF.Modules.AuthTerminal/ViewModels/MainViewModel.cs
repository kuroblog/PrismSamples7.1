
namespace PEF.Modules.AuthTerminal.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.AuthTerminal.Devices;
    using PEF.Modules.AuthTerminal.PubSubEvents;
    using PEF.Modules.AuthTerminal.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System.Windows;

    public partial class MainViewModel : BindableBase
    {
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;
        private readonly IEventAggregator eventAggregator;
        private readonly AuthTerminalConfig config = null;

        private readonly IAuthTerminalDeviceProxy deviceProxy = null;

        public MainViewModel(
            ILogger logger,
            IRegionManager region,
            IEventAggregator eventAggregator,
            IAuthTerminalDeviceProxy deviceProxy,
            AuthTerminalConfig config)
        {
            this.logger = logger;
            this.region = region;
            this.eventAggregator = eventAggregator;
            this.config = config;
            this.deviceProxy = deviceProxy;

            this.eventAggregator.GetEvent<SetAuthUserCommandVisibility>().Subscribe(SetAuthUserCommandVisibilityHandler);
        }

        private Visibility authUserCommandState;

        public Visibility AuthUserCommandState
        {
            get => authUserCommandState;
            set => SetProperty(ref authUserCommandState, value);
        }

        private Visibility returnCommandState;

        public Visibility ReturnCommandState
        {
            get => returnCommandState;
            set => SetProperty(ref returnCommandState, value);
        }

        private void SetAuthUserCommandVisibilityHandler(Visibility state)
        {
            AuthUserCommandState = state;
            ReturnCommandState = state == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        public string Version => config.Version;
        //public bool EnableDebugMenu => config.EnableDebugMenu;
        //public string DeviceNo => config.DeviceNo;
        //public string ServiceCall => config.ServiceCall;
        //public string Company => config.Company;
        //public string Product => config.Product;
        //public string Title => config.Title;
        //public int BufferSize => config.BufferSize;

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            AuthUserCommandState = Visibility.Visible;
            ReturnCommandState = Visibility.Hidden;

            region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
            // test
            //region?.RequestNavigate(RegionNames.Home, typeof(SizeChoiceView).FullName);
            //region?.RequestNavigate(RegionNames.Home, typeof(DistributeResultView).FullName);
            //region?.RequestNavigate(RegionNames.Home, typeof(ScheduleResultView).FullName);
            //region?.RequestNavigate(RegionNames.Home, typeof(SizeConfigView).FullName);

            deviceProxy.Connect();
        });

        public DelegateCommand AuthUserCommand => new DelegateCommand(() =>
        {
            //var callbackViewName = region?.Regions[RegionNames.RecyclingBinContent].ActiveViews.FirstOrDefault().GetType().FullName;
            //var nParam = new NavigationParameters
            //{
            //    { RecyclingBinKeys.CallbackViewName, callbackViewName }
            //};

            //region?.RequestNavigate(RegionNames.RecyclingBinContent, typeof(ConfigView).FullName, nParam);

            region?.RequestNavigate(RegionNames.Home, typeof(ScanView).FullName);

            //eventAggregator.GetEvent<Common.PubSubEvents.MainNotificationPopupEvent>().Publish(new Common.PubSubEvents.PopupEventArg<Prism.Interactivity.InteractionRequest.INotification>
            //{
            //    Title = "温煦提示",
            //    Content = "测试消息",
            //    Callback = new System.Action<Prism.Interactivity.InteractionRequest.INotification>(arg => { })
            //});
        });

        public DelegateCommand ReturnCommand => new DelegateCommand(() => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));
    }
}
