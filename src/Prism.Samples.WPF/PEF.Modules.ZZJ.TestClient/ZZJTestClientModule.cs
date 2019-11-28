
namespace PEF.Modules.ZZJ.TestClient
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    //using PEF.Modules.Simulator.Devices;
    using PEF.Modules.ZZJ.TestClient.ViewModels;
    using PEF.Modules.ZZJ.TestClient.Views;
    //using PEF.Socket.Utilities;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
    using Prism.Regions;

    /// <summary>
    /// 模块类
    /// </summary>
    public class ZZJTestClientModule : IModule
    {
        private readonly ILogger logger = null;

        public ZZJTestClientModule(ILogger logger)
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
            containerRegistry.RegisterSingleton<ZZJTestClientConfig>();

            //containerRegistry.RegisterSingleton<WebSocketServerProxy>();

            //containerRegistry.RegisterSingleton<IDeviceProxy, DeviceProxy>();

            ViewModelLocationProvider.Register<MainView, MainViewModel>();
        }
        #endregion
    }
}
