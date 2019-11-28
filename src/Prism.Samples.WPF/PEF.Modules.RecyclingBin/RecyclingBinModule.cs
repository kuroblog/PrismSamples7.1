
namespace PEF.Modules.RecyclingBin
{
    using PEF.Common;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.RecyclingBin.Devices;
    using PEF.Modules.RecyclingBin.Services;
    using PEF.Modules.RecyclingBin.ViewModels;
    using PEF.Modules.RecyclingBin.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System.Windows;

    /// <summary>
    /// 回收桶模块类
    /// </summary>
    public class RecyclingBinModule : IModule
    {
        private readonly ILogger logger = null;

        public RecyclingBinModule(ILogger logger)
        {
            this.logger = logger;
        }

        #region IModule
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var rm = containerProvider.Resolve<IRegionManager>();

            rm.RegisterViewWithRegion(RegionNames.Content, typeof(MainView));

            // 注册视图至指定区域（方式一）
            //rm.RegisterViewWithRegion(RegionNames.RecyclingBinContent, typeof(HomeView));
            //rm.RegisterViewWithRegion(RegionNames.RecyclingBinContent, typeof(ConfigView));
            //rm.RegisterViewWithRegion(RegionNames.RecyclingBinContent, typeof(RecycleView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<RecyclingBinConfig>();

            containerRegistry.RegisterSingleton<IRecyclingBinDeviceProxy, RecyclingBinDeviceProxy>();

            #region thread sync test
            //var container = (Application.Current as PrismApplication).Container;
            var container = MainDispatcher.Value.Container;
            #endregion
            var config = container.Resolve<RecyclingBinConfig>();

            if (config.IsMockApiResponse)
            {
                containerRegistry.RegisterSingleton<IRecyclingBinServiceProxy, RecyclingBinServiceProxyExample>();
            }
            else
            {
                containerRegistry.RegisterInstance<IRecyclingBinServiceProxy>(new RecyclingBinServiceProxy(container.Resolve<ILogger>(), container.Resolve<RecyclingBinConfig>(), container.Resolve<HttpClientProxy>()));
            }

            ViewModelLocationProvider.Register<MainView, MainViewModel>();
            ViewModelLocationProvider.Register<HomeView, HomeViewModel>();
            ViewModelLocationProvider.Register<ConfigView, ConfigViewModel>();
            ViewModelLocationProvider.Register<RecycleView, RecycleViewModel>();
            ViewModelLocationProvider.Register<AdminPasswordView, AdminPasswordViewModel>();

            // 注册视图，后手动触发导航（方式二）
            containerRegistry.RegisterForNavigation<HomeView>(typeof(HomeView).FullName);
            containerRegistry.RegisterForNavigation<ConfigView>(typeof(ConfigView).FullName);
            containerRegistry.RegisterForNavigation<RecycleView>(typeof(RecycleView).FullName);
            containerRegistry.RegisterForNavigation<AdminPasswordView>(typeof(AdminPasswordView).FullName);
        }
        #endregion
    }
}
