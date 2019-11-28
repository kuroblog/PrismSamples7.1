
namespace PEF.Modules.LogView
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.LogView.ViewModels;
    using PEF.Modules.LogView.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
    using Prism.Regions;

    public class LogViewModule : IModule
    {
        private readonly ILogger logger = null;

        public LogViewModule(ILogger logger)
        {
            this.logger = logger;
        }

        #region IModule
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var rm = containerProvider.Resolve<IRegionManager>();
            rm.RegisterViewWithRegion(RegionNames.Content, typeof(TestView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<LogViewConfig>();

            ViewModelLocationProvider.Register<MainView, MainViewModel>();
            ViewModelLocationProvider.Register<TestView, TestViewModel>();
        }
        #endregion
    }
}
