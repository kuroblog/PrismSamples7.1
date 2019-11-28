
namespace PEF.Modules.ShoeBox.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.ShoeBox.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public partial class ResultViewModel : BindableBase, INavigationAware
    {
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;

        public ResultViewModel(ILogger logger, IRegionManager region)
        {
            this.logger = logger;
            this.region = region;
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg => { });

        private string boxId;

        public string BoxId
        {
            get => boxId;
            set => SetProperty(ref boxId, value);
        }

        private string userName;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string confirmTitle;

        public string ConfirmTitle
        {
            get => confirmTitle;
            set => SetProperty(ref confirmTitle, value);
        }

        public bool confirmStatus;

        public bool ConfirmStatus
        {
            get => confirmStatus;
            set
            {
                SetProperty(ref confirmStatus, value);
                RaisePropertyChanged(nameof(IsShowWaittingMessage));
                RaisePropertyChanged(nameof(IsShowResult));
            }
        }

        public Visibility IsShowWaittingMessage
        {
            get => ConfirmStatus ? Visibility.Hidden : Visibility.Visible;
        }

        public Visibility IsShowResult
        {
            get => ConfirmStatus ? Visibility.Visible : Visibility.Hidden;
        }

        public DelegateCommand ConfirmCommand => new DelegateCommand(() =>
        {
            tokenSource?.Cancel();

            //region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
        });

        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        #region thread sync test
        //private void AutoCloseWindows(CancellationToken ct, int counter)
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        for (; ; )
        //        {
        //            if (ct.IsCancellationRequested || counter == 0)
        //            {
        //                MainDispatcherInvoke(() =>
        //                {
        //                    //ConfirmTitle = "确认";
        //                    //ConfirmCommand.Execute();

        //                    region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
        //                });

        //                break;
        //            }

        //            MainDispatcherInvoke(() => ConfirmTitle = $"确认 ({counter})");

        //            Task.Delay(1000).Wait();
        //            counter--;
        //        }
        //    });
        //}

        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        private void AutoCloseWindows(CancellationToken ct, int counter)
        {
            Task.Factory.StartNew(() =>
            {
                for (; ; )
                {
                    if (ct.IsCancellationRequested || counter == 0)
                    {
                        MainDispatcher.Value.Invoke(() =>
                        {
                            //ConfirmTitle = "确认";
                            //ConfirmCommand.Execute();

                            region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
                        });

                        break;
                    }

                    MainDispatcher.Value.Invoke(() => ConfirmTitle = $"确认 ({counter})");

                    Task.Delay(1000).Wait();
                    counter--;
                }
            });
        }
        #endregion

        private void CloseDoorHandler(string groupId)
        {
#if !DEBUG
            if (string.IsNullOrEmpty(groupId))
            {
                return;
            }
#endif
            //MainDispatcherInvoke(() => Logs.Add("桶门已关闭"));

            //var result = serviceProxy?.RecyclingBinSubmit(new RecyclingBinSubmitRequest
            //{
            //    CardId = userCardId,
            //    DeviceId = config.DeviceId
            //});
            //if (result != null && result.IsSuccessful && result.HasData)
            //{
            //    if (result.Data.IsSuccessful)
            //    {
            //        MainDispatcherInvoke(() =>
            //        {
            //            ReceiveResult = "归还成功";
            //            ConfirmStatus = true;
            //        });

            //        Task.Factory.StartNew(() =>
            //        {
            //            var i = 5;
            //            for (; ; )
            //            {
            //                if (i == 0)
            //                {
            //                    MainDispatcherInvoke(() =>
            //                    {
            //                        ConfirmCommand.Execute();

            //                        ConfirmTitle = "确认";
            //                    });

            //                    break;
            //                }

            //                MainDispatcherInvoke(() => ConfirmTitle = $"确认 ({i})");

            //                Task.Delay(1000).Wait();
            //                i--;
            //            }
            //        });
            //    }
            //}

            #region thread sync test
            //Task.Factory.StartNew(() =>
            //{
            //    var i = 5;
            //    for (; ; )
            //    {
            //        if (i == 0)
            //        {
            //            MainDispatcherInvoke(() =>
            //            {
            //                ConfirmCommand.Execute();

            //                ConfirmTitle = "确认";
            //            });

            //            break;
            //        }

            //        MainDispatcherInvoke(() => ConfirmTitle = $"确认 ({i})");

            //        Task.Delay(1000).Wait();
            //        i--;
            //    }
            //});

            Task.Factory.StartNew(() =>
            {
                var i = 5;
                for (; ; )
                {
                    if (i == 0)
                    {
                        MainDispatcher.Value.Invoke(() =>
                        {
                            ConfirmCommand.Execute();

                            ConfirmTitle = "确认";
                        });

                        break;
                    }

                    MainDispatcher.Value.Invoke(() => ConfirmTitle = $"确认 ({i})");

                    Task.Delay(1000).Wait();
                    i--;
                }
            });
            #endregion
        }

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
            ConfirmStatus = false;
            ConfirmTitle = "确定";

            UserName = navigationContext.Parameters[ShoeBoxKeys.UserName] as string;
            BoxId = navigationContext.Parameters[ShoeBoxKeys.DeviceItemCode] as string;

            ConfirmStatus = !string.IsNullOrEmpty(BoxId);
            if (confirmStatus)
            {
                tokenSource = new CancellationTokenSource();
                AutoCloseWindows(tokenSource.Token, 6);
            }
        }
        #endregion
    }
}
