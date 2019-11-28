
namespace PEF.Modules.ShoeBox.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.ShoeBox.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;

    public partial class ConfigViewModel : BindableBase, INavigationAware
    {
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;

        public ConfigViewModel(ILogger logger, IRegionManager region)
        {
            this.logger = logger;
            this.region = region;
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg => { });

        public DelegateCommand AdminOpenCommand => new DelegateCommand(() =>
        {
            region?.RequestNavigate(RegionNames.Home, typeof(AdminPasswordView).FullName);
        });

        private string callbackViewName;

        public DelegateCommand ReturnCommand => new DelegateCommand(() =>
        {
            region?.RequestNavigate(RegionNames.Home, string.IsNullOrEmpty(callbackViewName) ? typeof(HomeView).FullName : callbackViewName);
        });

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            callbackViewName = navigationContext.Parameters[ShoeBoxKeys.CallbackViewName] as string;
        }
        #endregion
    }
}
