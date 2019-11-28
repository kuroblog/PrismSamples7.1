
namespace PEF.Modules.RecyclingBin.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.RecyclingBin.Devices;
    using PEF.Modules.RecyclingBin.Devices.Dtos;
    using PEF.Modules.RecyclingBin.PubSubEvents;
    using PEF.Modules.RecyclingBin.Services;
    using PEF.Modules.RecyclingBin.Services.Dtos;
    using PEF.Modules.RecyclingBin.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public partial class RecycleViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager region = null;
        private readonly ILogger logger = null;
        private readonly RecyclingBinConfig config = null;
        private readonly IRecyclingBinDeviceProxy deviceProxy = null;
        private readonly IRecyclingBinServiceProxy serviceProxy = null;

        public RecycleViewModel(
            IEventAggregator eventAggregator,
            IRegionManager region,
            ILogger logger,
            RecyclingBinConfig config,
            IRecyclingBinDeviceProxy deviceProxy,
            IRecyclingBinServiceProxy serviceProxy)
        {
            this.eventAggregator = eventAggregator;
            this.region = region;

            this.logger = logger;
            this.config = config;
            this.deviceProxy = deviceProxy;
            this.serviceProxy = serviceProxy;

            eventAggregator.GetEvent<ItemReveiveEvent>().Subscribe(ItemReceiveHandler);
            eventAggregator.GetEvent<CloseDoorEvent>().Subscribe(CloseDoorHandler);
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}
        #endregion

        private void ItemReceiveHandler(string groupId)
        {
#if !DEBUG
            if (string.IsNullOrEmpty(groupId))
            {
                return;
            }
#endif

            #region thread sync test
            //MainDispatcherInvoke(() =>
            //{
            //    Logs.Add("衣物已放入");

            //    ReceiveDate = DateTime.Now.ToString("yyyy年MM月dd日");
            //    ReceiveTime = DateTime.Now.ToString("HH:mm:ss");

            //    Logs.Add("桶门正在关闭，请稍后……");
            //});

            MainDispatcher.Value.Invoke(() =>
            {
                Logs.Add("衣物已放入");

                ReceiveDate = DateTime.Now.ToString("yyyy年MM月dd日");
                ReceiveTime = DateTime.Now.ToString("HH:mm:ss");

                Logs.Add("桶门正在关闭，请稍后……");
            });
            #endregion

            serviceProxy.LogReport(new LogReportRequest { DeviceId = config.DeviceId, LogType = 0 });
        }

        private void CloseDoorHandler(string groupId)
        {
#if !DEBUG
            if (string.IsNullOrEmpty(groupId))
            {
                return;
            }
#endif
            #region thread sync test
            //MainDispatcherInvoke(() => Logs.Add("桶门已关闭"));

            MainDispatcher.Value.Invoke(() => Logs.Add("桶门已关闭"));
            #endregion

            var result = serviceProxy?.RecyclingBinSubmit(new RecyclingBinSubmitRequest
            {
                CardId = userCardId,
                DeviceId = config.DeviceId
            });
            if (result != null && result.IsSuccessful && result.HasData)
            {
                if (result.Data.IsSuccessful)
                {
                    #region thread sync test
                    //MainDispatcherInvoke(() =>
                    //{
                    //    ReceiveResult = "归还成功";
                    //    ConfirmStatus = true;
                    //});

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

                    MainDispatcher.Value.Invoke(() =>
                    {
                        ReceiveResult = "归还成功";
                        ConfirmStatus = true;
                    });

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
            }
            else
            {
                MainDispatcher.Value.ShowMessage(result.Message, action: n => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));
            }
        }

        public ObservableCollection<string> Logs { get; set; } = new ObservableCollection<string>();

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            Logs.Clear();

            //UserName = string.Empty;
            //ReceiveDate = string.Empty;
            //ReceiveTime = string.Empty;

            Logs.Add("桶门正在打开中，请稍后……");

            //Logs.Add(new RecycleBinLog { Message = "桶门正在打开中，请稍后……" });

            deviceProxy.DeviceOpenDoorDtoHandler(new DeviceOpenDoorDto());

            Logs.Add("桶门已打开，请放入衣物");
        });

        private string confirmTitle;

        public string ConfirmTitle
        {
            get => confirmTitle;
            set => SetProperty(ref confirmTitle, value);
        }

        private string receiveDate;

        public string ReceiveDate
        {
            get => receiveDate;
            set => SetProperty(ref receiveDate, value);
        }

        private string receiveTime;

        public string ReceiveTime
        {
            get => receiveTime;
            set => SetProperty(ref receiveTime, value);
        }

        private string receiveResult = "--";

        public string ReceiveResult
        {
            get => receiveResult;
            set => SetProperty(ref receiveResult, value);
        }

        //private string confirmTime;

        //public string ConfirmTime
        //{
        //    get => confirmTime;
        //    set => SetProperty(ref confirmTime, value);
        //}

        public bool confirmStatus;

        public bool ConfirmStatus
        {
            get => confirmStatus;
            set => SetProperty(ref confirmStatus, value);
        }

        public DelegateCommand ConfirmCommand => new DelegateCommand(() => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));

        private string userName;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string userCardId;

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            userCardId = navigationContext.Parameters[RecyclingBinKeys.UserCardId] as string;
            UserName = navigationContext.Parameters[RecyclingBinKeys.UserName] as string;
            ReceiveDate = "";
            ReceiveTime = "";
            ReceiveResult = "--";
            ConfirmStatus = false;
            ConfirmTitle = "确定";
        }
        #endregion
    }
}
