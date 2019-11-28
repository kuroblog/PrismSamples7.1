
namespace PEF.Modules.SGDE.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.SGDE.Devices;
    using PEF.Modules.SGDE.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;

    public partial class MainViewModel : BindableBase
    {
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;
        private readonly SGDEConfig config = null;

        private readonly ISGDEDeviceProxy deviceProxy = null;

        public MainViewModel(
            ILogger logger,
            IRegionManager region,
            ISGDEDeviceProxy deviceProxy,
            SGDEConfig config)
        {
            this.logger = logger;
            this.region = region;
            this.config = config;
            this.deviceProxy = deviceProxy;
        }

        //public bool EnableDebugMenu => config.EnableDebugMenu;
        public string DeviceNo => config.DeviceNo;
        public string Version => config.Version;
        public string ServiceCall => config.ServiceCall;
        public string Company => config.Company;
        public string Product => config.Product;
        public string Title => config.Title;
        //public int BufferSize => config.BufferSize;

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
            // test
            //region?.RequestNavigate(RegionNames.Home, typeof(SizeChoiceView).FullName);
            //region?.RequestNavigate(RegionNames.Home, typeof(DistributeResultView).FullName);
            //region?.RequestNavigate(RegionNames.Home, typeof(ScheduleResultView).FullName);
            //region?.RequestNavigate(RegionNames.Home, typeof(SizeConfigView).FullName);

            deviceProxy.Connect();
        });

        public DelegateCommand SettingCommand => new DelegateCommand(() =>
        {
            //var callbackViewName = region?.Regions[RegionNames.RecyclingBinContent].ActiveViews.FirstOrDefault().GetType().FullName;
            //var nParam = new NavigationParameters
            //{
            //    { RecyclingBinKeys.CallbackViewName, callbackViewName }
            //};

            //region?.RequestNavigate(RegionNames.RecyclingBinContent, typeof(ConfigView).FullName, nParam);
        });
    }
}
