
namespace PEF.Modules.SGDE
{
    using PEF.Common;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.SGDE.Devices;
    using PEF.Modules.SGDE.Services;
    using PEF.Modules.SGDE.ViewModels;
    using PEF.Modules.SGDE.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System.Windows;

    /// <summary>
    /// 发衣柜模块类
    /// </summary>
    public class SGDEModule : IModule
    {
        private readonly ILogger logger = null;

        public SGDEModule(ILogger logger)
        {
            this.logger = logger;
        }

        #region IModule
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var rm = containerProvider.Resolve<IRegionManager>();

            rm.RegisterViewWithRegion(RegionNames.Content, typeof(MainView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<SGDEConfig>();

            containerRegistry.RegisterSingleton<ISGDEDeviceProxy, SGDEDeviceProxy>();

            #region thread sync test
            //var container = (Application.Current as PrismApplication).Container;

            var container = MainDispatcher.Value.Container;
            #endregion
            var config = container.Resolve<SGDEConfig>();

            if (config.IsMockApiResponse)
            {
                containerRegistry.RegisterSingleton<ISGDEServiceProxy, SGDEServiceProxyExample>();
            }
            else
            {
                containerRegistry.RegisterInstance<ISGDEServiceProxy>(new SGDEServiceProxy(container.Resolve<SGDEConfig>(), container.Resolve<HttpClientProxy>(), container.Resolve<ILogger>()));
            }

            ViewModelLocationProvider.Register<MainView, MainViewModel>();

            ViewModelLocationProvider.Register<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<HomeView>(typeof(HomeView).FullName);

            ViewModelLocationProvider.Register<SizeChoiceView, SizeChoiceViewModel>();
            containerRegistry.RegisterForNavigation<SizeChoiceView>(typeof(SizeChoiceView).FullName);

            ViewModelLocationProvider.Register<DistributeResultView, DistributeResultViewModel>();
            containerRegistry.RegisterForNavigation<DistributeResultView>(typeof(DistributeResultView).FullName);

            ViewModelLocationProvider.Register<ScheduleResultView, ScheduleResultViewModel>();
            containerRegistry.RegisterForNavigation<ScheduleResultView>(typeof(ScheduleResultView).FullName);

            ViewModelLocationProvider.Register<SizeConfigView, SizeConfigViewModel>();
            containerRegistry.RegisterForNavigation<SizeConfigView>(typeof(SizeConfigView).FullName);
        }
        #endregion
    }
}
