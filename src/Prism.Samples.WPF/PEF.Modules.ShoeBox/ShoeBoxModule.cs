
namespace PEF.Modules.ShoeBox
{
    using PEF.Common;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.ShoeBox.Devices;
    using PEF.Modules.ShoeBox.Services;
    using PEF.Modules.ShoeBox.ViewModels;
    using PEF.Modules.ShoeBox.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System.Windows;

    /// <summary>
    /// 回收桶模块类
    /// </summary>
    public class ShoeBoxModule : IModule
    {
        private readonly ILogger logger = null;

        public ShoeBoxModule(ILogger logger)
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
            containerRegistry.RegisterSingleton<ShoeBoxConfig>();

            containerRegistry.RegisterSingleton<IShoeBoxDeviceProxy, ShoeBoxDeviceProxy>();

            #region thread sync test
            //var container = (Application.Current as PrismApplication).Container;

            var container = MainDispatcher.Value.Container;
            #endregion
            var config = container.Resolve<ShoeBoxConfig>();

            if (config.IsMockApiResponse)
            {
                containerRegistry.RegisterSingleton<IShoeBoxServiceProxy, ShoeBoxServiceProxyExample>();
            }
            else
            {
                containerRegistry.RegisterInstance<IShoeBoxServiceProxy>(new ShoeBoxServiceProxy(container.Resolve<ILogger>(), container.Resolve<ShoeBoxConfig>(), container.Resolve<HttpClientProxy>()));
            }

            ViewModelLocationProvider.Register<MainView, MainViewModel>();

            ViewModelLocationProvider.Register<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<HomeView>(typeof(HomeView).FullName);

            ViewModelLocationProvider.Register<ConfigView, ConfigViewModel>();
            containerRegistry.RegisterForNavigation<ConfigView>(typeof(ConfigView).FullName);

            ViewModelLocationProvider.Register<AdminConfigView, AdminConfigViewModel>();
            containerRegistry.RegisterForNavigation<AdminConfigView>(typeof(AdminConfigView).FullName);

            ViewModelLocationProvider.Register<AdminPasswordView, AdminPasswordViewModel>();
            containerRegistry.RegisterForNavigation<AdminPasswordView>(typeof(AdminPasswordView).FullName);

            ViewModelLocationProvider.Register<ResultView, ResultViewModel>();
            containerRegistry.RegisterForNavigation<ResultView>(typeof(ResultView).FullName);

            // 注册视图，后手动触发导航（方式二）
        }
        #endregion
    }
}
