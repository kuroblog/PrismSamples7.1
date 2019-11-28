
namespace PEF.Modules.Simulator
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.Simulator.Devices;
    using PEF.Modules.Simulator.ViewModels;
    using PEF.Modules.Simulator.Views;
    using PEF.Socket.Utilities;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
    using Prism.Regions;

    /// <summary>
    /// 模块类
    /// </summary>
    public class SimulatorModule : IModule
    {
        private readonly ILogger logger = null;

        public SimulatorModule(ILogger logger)
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
            containerRegistry.RegisterSingleton<SimulatorConfig>();

            containerRegistry.RegisterSingleton<WebSocketServerProxy>();

            containerRegistry.RegisterSingleton<IDeviceProxy, DeviceProxy>();

            ViewModelLocationProvider.Register<MainView, MainViewModel>();
        }
        #endregion
    }
}
