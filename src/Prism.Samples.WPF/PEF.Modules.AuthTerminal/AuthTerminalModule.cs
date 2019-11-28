
namespace PEF.Modules.AuthTerminal
{
    using PEF.Common;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.AuthTerminal.Devices;
    using PEF.Modules.AuthTerminal.Services;
    using PEF.Modules.AuthTerminal.ViewModels;
    using PEF.Modules.AuthTerminal.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System.Windows;

    /// <summary>
    /// 模块类
    /// </summary>
    public class AuthTerminalModule : IModule
    {
        private readonly ILogger logger = null;

        public AuthTerminalModule(ILogger logger)
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
            containerRegistry.RegisterSingleton<AuthTerminalConfig>();

            containerRegistry.RegisterSingleton<IAuthTerminalDeviceProxy, AuthTerminalDeviceProxy>();

            #region thread sync test
            //var container = (Application.Current as PrismApplication).Container;

            var container = MainDispatcher.Value.Container;
            #endregion
            var config = container.Resolve<AuthTerminalConfig>();

            if (config.IsMockApiResponse)
            {
                containerRegistry.RegisterSingleton<IAuthTerminalServiceProxy, AuthTerminalServiceProxyExample>();
            }
            else
            {
                containerRegistry.RegisterInstance<IAuthTerminalServiceProxy>(new AuthTerminalServiceProxy(container.Resolve<AuthTerminalConfig>(), container.Resolve<HttpClientProxy>(), container.Resolve<ILogger>()));
            }

            ViewModelLocationProvider.Register<MainView, MainViewModel>();

            ViewModelLocationProvider.Register<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<HomeView>(typeof(HomeView).FullName);

            ViewModelLocationProvider.Register<ScanView, ScanViewModel>();
            containerRegistry.RegisterForNavigation<ScanView>(typeof(ScanView).FullName);

            ViewModelLocationProvider.Register<RoleChoiceView, RoleChoiceViewModel>();
            containerRegistry.RegisterForNavigation<RoleChoiceView>(typeof(RoleChoiceView).FullName);

            //ViewModelLocationProvider.Register<ScheduleResultView, ScheduleResultViewModel>();
            //containerRegistry.RegisterForNavigation<ScheduleResultView>(typeof(ScheduleResultView).FullName);

            //ViewModelLocationProvider.Register<SizeConfigView, SizeConfigViewModel>();
            //containerRegistry.RegisterForNavigation<SizeConfigView>(typeof(SizeConfigView).FullName);
        }
        #endregion
    }
}
