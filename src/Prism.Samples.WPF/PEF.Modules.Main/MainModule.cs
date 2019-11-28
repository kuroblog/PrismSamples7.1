
namespace PEF.Modules.Main
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.Main.ViewModels;
    using PEF.Modules.Main.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
    using Prism.Regions;

    /// <summary>
    /// 主模块类
    /// </summary>
    public class MainModule : IModule
    {
        private readonly ILogger logger = null;

        public MainModule(ILogger logger)
        {
            this.logger = logger;
        }

        #region IModule
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var rm = containerProvider.Resolve<IRegionManager>();
            rm.RegisterViewWithRegion(RegionNames.Main, typeof(MainView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<MainConfig>();

            ViewModelLocationProvider.Register<MainView, MainViewModel>();
        }
        #endregion
    }
}
