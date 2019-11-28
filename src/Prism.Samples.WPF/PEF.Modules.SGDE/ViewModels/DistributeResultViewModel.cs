
namespace PEF.Modules.SGDE.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.SGDE.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public partial class DistributeResultViewModel : BindableBase, INavigationAware
    {
        private readonly SGDEConfig config = null;
        private readonly IRegionManager region = null;
        private readonly ILogger logger = null;

        private string userName;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string boxId;

        public string BoxId
        {
            get => boxId;
            set => SetProperty(ref boxId, value);
        }

        public DistributeResultViewModel(
            SGDEConfig config,
            IRegionManager region,
            ILogger logger)
        {
            this.config = config;
            this.region = region;
            this.logger = logger;
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        //private void AutoReturnHomeView(int counter = 6)
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        for (; ; )
        //        {
        //            //if (ct.IsCancellationRequested || counter == 0)
        //            if (counter == 0)
        //            {
        //                MainDispatcherInvoke(() =>
        //                {
        //                    region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
        //                });

        //                break;
        //            };

        //            Task.Delay(1000).Wait();
        //            counter--;
        //        }
        //    });
        //}

        private void AutoReturnHomeView(int counter = 6)
        {
            Task.Factory.StartNew(() =>
            {
                for (; ; )
                {
                    //if (ct.IsCancellationRequested || counter == 0)
                    if (counter == 0)
                    {
                        MainDispatcher.Value.Invoke(() => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));

                        break;
                    };

                    Task.Delay(1000).Wait();
                    counter--;
                }
            });
        }
        #endregion

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg => AutoReturnHomeView());

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            UserName = navigationContext.Parameters[SGDEKeys.UserName] as string;
            BoxId = navigationContext.Parameters[SGDEKeys.SaveBoxNo].ToString();
        }
        #endregion
    }
}
