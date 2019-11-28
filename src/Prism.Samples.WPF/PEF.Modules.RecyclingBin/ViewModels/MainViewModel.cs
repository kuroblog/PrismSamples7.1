
namespace PEF.Modules.RecyclingBin.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.RecyclingBin.Devices;
    using PEF.Modules.RecyclingBin.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using System.Linq;

    public partial class MainViewModel : BindableBase
    {
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;
        private readonly RecyclingBinConfig config = null;

        private readonly IRecyclingBinDeviceProxy deviceProxy = null;

        public MainViewModel(
            ILogger logger,
            IRegionManager region,
            RecyclingBinConfig config,
            IRecyclingBinDeviceProxy deviceProxy)
        {
            this.logger = logger;
            this.region = region;
            this.config = config;
            this.deviceProxy = deviceProxy;
        }

        public bool EnableDebugMenu => config.EnableDebugMenu;
        public string DeviceNo => config.DeviceNo;
        public string Version => config.Version;
        public string ServiceCall => config.ServiceCall;
        public string Company => config.Company;
        public string Product => config.Product;
        public string Title => config.Title;
        public int BufferSize => config.BufferSize;

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            // 导航到指定区域的特定视图
            // 视图必须已经在 IModule 的实现类注册过
            region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
            //region?.RequestNavigate(RegionNames.RecyclingBinContent, typeof(RecycleView).FullName);

            deviceProxy.Connect();
        });

        public DelegateCommand SettingCommand => new DelegateCommand(() =>
        {
            var callbackViewName = region?.Regions[RegionNames.Home].ActiveViews.FirstOrDefault().GetType().FullName;
            var nParam = new NavigationParameters
            {
                { RecyclingBinKeys.CallbackViewName, callbackViewName }
            };

            region?.RequestNavigate(RegionNames.Home, typeof(ConfigView).FullName, nParam);
        });
    }
}
