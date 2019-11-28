
namespace PEF.Modules.RecyclingBin.ViewModels
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.RecyclingBin.Devices;
    using PEF.Modules.RecyclingBin.Devices.Dtos;
    using PEF.Modules.RecyclingBin.PubSubEvents;
    using PEF.Modules.RecyclingBin.Services;
    using PEF.Modules.RecyclingBin.Services.Dtos;
    using PEF.Modules.RecyclingBin.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Windows;
    using System.Windows.Threading;

    public partial class HomeViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;
        private readonly RecyclingBinConfig config = null;
        private readonly HttpRequestParameters httpRequestParam = null;
        private readonly IRecyclingBinDeviceProxy deviceProxy = null;
        private readonly IRecyclingBinServiceProxy serviceProxy = null;

        public HomeViewModel(
            IEventAggregator eventAggregator,
            ILogger logger,
            IRegionManager region,
            RecyclingBinConfig config,
            HttpRequestParameters httpRequestParam,
            IRecyclingBinDeviceProxy deviceProxy,
            IRecyclingBinServiceProxy serviceProxy)
        {
            this.eventAggregator = eventAggregator;

            this.logger = logger;
            this.region = region;
            this.config = config;
            this.httpRequestParam = httpRequestParam;
            this.deviceProxy = deviceProxy;
            this.serviceProxy = serviceProxy;

            // 使用匿名函数会导致访问不到当前对象
            eventAggregator.GetEvent<DeviceIdReceiveEvent>().Subscribe(DeviceIdReceiveHandler);
            eventAggregator.GetEvent<CardReceiveEvent>().Subscribe(CardReceiveHandler);
        }

        private void DeviceIdReceiveHandler(string deviceId) { }

        private void CardReceiveHandler(CardReceiveEventParameter arg)
        {
            var tResult = serviceProxy?.TokenRequest(new TokenRequest { CardId = arg.CardId, ReadStyle = int.TryParse(arg.ReadStyle, out int rs) ? rs : 0 });
            if (tResult != null && tResult.IsSuccessful && tResult.HasData)
            {
                var key = "X-Token";

                if (httpRequestParam.Headers.TryGetValue(key, out string token))
                {
                    httpRequestParam.Headers[key] = tResult.Data.Token;
                }
                else
                {
                    httpRequestParam.Headers.Add(key, tResult.Data.Token);
                }

                // 视图跳转无效
                //var container = (Application.Current as PrismApplication).Container;
                //container.Resolve<IRegionManager>().RequestNavigate(RegionNames.RecyclingBinContent, typeof(RecycleView).FullName);
                //region?.RequestNavigate(RegionNames.RecyclingBinContent, typeof(RecycleView).FullName);

                // check cardId
                var result = serviceProxy?.CardQuery(new CardQueryRequest { ReadStyle = int.TryParse(arg.ReadStyle, out int rss) ? rss : 0, CardId = arg.CardId });
                if (result != null && result.IsSuccessful && result.HasData)
                {
                    if (result.Data.UserType == "M")
                    {
                        var cleanResult = serviceProxy.RecyclingBinClean(new RecyclingBinCleanRequest { DeviceId = config.DeviceId });
                        if (cleanResult != null && cleanResult.HasData && cleanResult.IsSuccessful)
                        {
                            deviceProxy.DeviceOpenLockDtoHandler(new DeviceOpenLockDto());

                            serviceProxy.LogReport(new LogReportRequest { DeviceId = config.DeviceId, LogType = 2 });
                        }
                        else
                        {
                            serviceProxy.LogReport(new LogReportRequest { DeviceId = config.DeviceId, LogType = 3 });
                        }

                        MainDispatcher.Value.ShowMessage("回收桶已清空。", action: n => LoadedCommand.Execute(null));
                    }
                    else if (result.Data.UserType == "U")
                    {
                        if (!result.Data.IsRecovery)
                        {
                            // TODO: 没有衣物需要回收
                            return;
                        }

                        // 这里运行在子线程里，所以必须回到主线程（任务调度器或者委托）来响应UI
                        // 任务调度器
                        #region thread sync test
                        //MainDispatcherInvoke(() => region?.RequestNavigate(
                        //    RegionNames.Home,
                        //    typeof(RecycleView).FullName,
                        //    new NavigationParameters {
                        //        { RecyclingBinKeys.UserName, result.Data.UserName },
                        //        { RecyclingBinKeys.UserCardId, arg.CardId} }));

                        MainDispatcher.Value.Invoke(() => region?.RequestNavigate(
                            RegionNames.Home,
                            typeof(RecycleView).FullName,
                            new NavigationParameters {
                            { RecyclingBinKeys.UserName, result.Data.UserName },
                            { RecyclingBinKeys.UserCardId, arg.CardId} }));
                        #endregion
                    }
                    else { }
                }
                else
                {
                    MainDispatcher.Value.ShowMessage(result.Message);
                }
            }
            else
            {
                MainDispatcher.Value.ShowMessage(tResult.Message);
            }
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}
        #endregion

        //private void PopupMainNotification(string message, string title = "温馨提示", Action action = null)
        //{
        //    eventAggregator.GetEvent<MainNotificationPopupEvent>().Publish(new PopupEventArg<INotification>
        //    {
        //        Title = title,
        //        Content = message,
        //        Callback = new Action<INotification>(res => action?.Invoke())
        //    });
        //}

        private int quantity;

        public int Quantity
        {
            get => quantity;
            set => SetProperty(ref quantity, value);
        }

        private int weight;

        public int Weight
        {
            get => weight;
            set => SetProperty(ref weight, value);
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            var recyclingBinQueryResult = serviceProxy?.RecyclingBinQuery(new RecyclingBinQueryRequest { DeviceId = config.DeviceId });
            if (recyclingBinQueryResult != null && recyclingBinQueryResult.IsSuccessful && recyclingBinQueryResult.HasData)
            {
                Quantity = recyclingBinQueryResult.Data.Quantity;
                Weight = recyclingBinQueryResult.Data.Weight;
            }
            else
            {
                Quantity = 0;
                Weight = 0;
            }
        });
    }
}
