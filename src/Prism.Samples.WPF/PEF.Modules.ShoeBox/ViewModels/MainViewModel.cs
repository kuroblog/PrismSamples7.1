
namespace PEF.Modules.ShoeBox.ViewModels
{
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Common.PubSubEvents;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.ShoeBox.Devices;
    using PEF.Modules.ShoeBox.Devices.Dtos;
    using PEF.Modules.ShoeBox.PubSubEvents;
    using PEF.Modules.ShoeBox.Services;
    using PEF.Modules.ShoeBox.Services.Dtos;
    using PEF.Modules.ShoeBox.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    public partial class MainViewModel : BindableBase
    {
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;
        public readonly IEventAggregator eventAggregator;
        private readonly ShoeBoxConfig config = null;
        private readonly HttpRequestParameters httpRequestParam = null;
        private readonly IShoeBoxServiceProxy serviceProxy = null;
        private readonly IShoeBoxDeviceProxy deviceProxy = null;

        public MainViewModel(
            ILogger logger,
            IRegionManager region,
            IEventAggregator eventAggregator,
            ShoeBoxConfig config,
            HttpRequestParameters httpRequestParam,
            IShoeBoxServiceProxy serviceProxy,
            IShoeBoxDeviceProxy deviceProxy)
        {
            this.logger = logger;
            this.region = region;
            this.eventAggregator = eventAggregator;
            this.config = config;
            this.httpRequestParam = httpRequestParam;
            this.serviceProxy = serviceProxy;
            this.deviceProxy = deviceProxy;

            this.eventAggregator.GetEvent<DeviceIdReceiveEvent>().Subscribe(DeviceIdReceiveHandler);
            this.eventAggregator.GetEvent<CardReceiveEvent>().Subscribe(CardReceiveHandler);
            this.eventAggregator.GetEvent<DoorOpenReceiveEvent>().Subscribe(DoorOpenReceiveHandler);
            this.eventAggregator.GetEvent<DoorCloseReceiveEvent>().Subscribe(DoorCloseReceiveHandler);

            this.eventAggregator.GetEvent<ShoeBoxMessagePopupEvent>().Subscribe(MessagePopupHandler);
            this.eventAggregator.GetEvent<ShoeBoxConfirmPopupEvent>().Subscribe(ConfirmPopupHandler);
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}
        #endregion

        private void DoorOpenReceiveHandler(string doorId)
        {
            if (region?.Regions[RegionNames.Home].ActiveViews.FirstOrDefault().GetType().FullName == typeof(AdminConfigView).FullName)
            {
                eventAggregator.GetEvent<AdminOpenDoorTransferEvent>().Publish(doorId);

                serviceProxy.LogReport(new LogReportRequest { DeviceId = config.DeviceId, LogType = 2, JsonLogs = new { doorId }.GetJsonString() });
            }
            else
            {
                #region thread sync test
                //MainDispatcherInvoke(() => region?.RequestNavigate(RegionNames.Home, typeof(ResultView).FullName, new NavigationParameters
                //{
                //    { ShoeBoxKeys.CallbackViewName, typeof(HomeView).FullName },
                //    { ShoeBoxKeys.UserName, userName },
                //    { ShoeBoxKeys.DeviceItemCode, deviceItemNo.ToString() }
                //}));

                MainDispatcher.Value.Invoke(() =>
                    region?.RequestNavigate(
                        RegionNames.Home,
                        typeof(ResultView).FullName,
                        new NavigationParameters
                        {
                            { ShoeBoxKeys.CallbackViewName, typeof(HomeView).FullName },
                            { ShoeBoxKeys.UserName, userName },
                            { ShoeBoxKeys.DeviceItemCode, deviceItemNo.ToString() }
                        }));
                #endregion

                serviceProxy.LogReport(new LogReportRequest { DeviceId = config.DeviceId, LogType = 0, JsonLogs = new { doorId }.GetJsonString() });
            }
        }

        private void DoorCloseReceiveHandler(string doorId)
        {
            if (region?.Regions[RegionNames.Home].ActiveViews.FirstOrDefault().GetType().FullName == typeof(AdminConfigView).FullName)
            {
                eventAggregator.GetEvent<AdminCloseDoorTransferEvent>().Publish(doorId);
            }
        }

        private void DeviceIdReceiveHandler(string deviceId) { }

        private string userName;
        private int deviceItemNo;

        private void CardReceiveHandler(CardReceiveEventParameter arg)
        {
            userName = string.Empty;
            deviceItemNo = 0;

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

                var cqResult = serviceProxy?.CardQuery(new CardQueryRequest
                {
                    CardId = arg.CardId,
                    ReadStyle = int.TryParse(arg.ReadStyle, out int readStyle) ? readStyle : 0,
                    DeviceId = config.DeviceId
                });
                if (cqResult != null && cqResult.IsSuccessful && cqResult.HasData)
                {
                    userName = cqResult.Data.UserName;

                    // M 管理员 | U 普通用户 | 无权限
                    if (cqResult.Data.UserType == "M")
                    {
                        #region thread sync test
                        //MainDispatcherInvoke(() => region?.RequestNavigate(RegionNames.Home, typeof(AdminConfigView).FullName));

                        MainDispatcher.Value.Invoke(() => region?.RequestNavigate(RegionNames.Home, typeof(AdminConfigView).FullName));
                        #endregion
                    }
                    else if (cqResult.Data.UserType == "U")
                    {
                        // 设备无效，重新申请
                        if (cqResult.Data.DeviceItemNo == 0)
                        {
                            #region thread sync test
                            //MainDispatcherInvoke(() =>
                            //    region?.RequestNavigate(
                            //        RegionNames.Home,
                            //        typeof(ResultView).FullName,
                            //        res =>
                            //        {
                            //            var drResult = serviceProxy.DeviceRegistration(new DeviceRegistrationRequest { UserId = cqResult.Data.UserId, UserName = cqResult.Data.UserName, DeviceId = config.DeviceId });
                            //            if (drResult != null && drResult.IsSuccessful && drResult.HasData)
                            //            {
                            //                deviceItemNo = drResult.Data.DeviceItemNo;
                            //                deviceProxy.OpenDoor(new DeviceOpenDoorDto(new[] { drResult.Data.DeviceItemCode }));
                            //            }
                            //            else
                            //            {
                            //                PopupMainNotification(drResult.Message);
                            //            }
                            //        },
                            //        new NavigationParameters
                            //        {
                            //            { ShoeBoxKeys.CallbackViewName, typeof(HomeView).FullName },
                            //            { ShoeBoxKeys.UserName, userName }
                            //        }));

                            MainDispatcher.Value.Invoke(() =>
                                region?.RequestNavigate(
                                    RegionNames.Home,
                                    typeof(ResultView).FullName,
                                    res =>
                                    {
                                        var drResult = serviceProxy.DeviceRegistration(new DeviceRegistrationRequest { UserId = cqResult.Data.UserId, UserName = cqResult.Data.UserName, DeviceId = config.DeviceId });
                                        if (drResult != null && drResult.IsSuccessful && drResult.HasData)
                                        {
                                            deviceItemNo = drResult.Data.DeviceItemNo;
                                            deviceProxy.OpenDoor(new DeviceOpenDoorDto(new[] { drResult.Data.DeviceItemCode }));
                                        }
                                        else
                                        {
                                            MainDispatcher.Value.ShowMessage(drResult.Message, action: n => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));
                                        }
                                    },
                                    new NavigationParameters                                    {
                                        { ShoeBoxKeys.CallbackViewName, typeof(HomeView).FullName },
                                        { ShoeBoxKeys.UserName, userName }
                                    }));
                            #endregion
                        }
                        else
                        {
                            deviceItemNo = cqResult.Data.DeviceItemNo;
                            deviceProxy.OpenDoor(new DeviceOpenDoorDto(new[] { cqResult.Data.DeviceItemCode }));
                        }
                    }
                    else
                    {
                        // TODO: 用户无权限
                    }
                }
                else
                {
                    MainDispatcher.Value.ShowMessage(cqResult.Message);
                }
            }
            else
            {
                MainDispatcher.Value.ShowMessage(tResult.Message);
            }
        }

        //private void PopupMainNotification(string message, string title = "温馨提示", Action action = null)
        //{
        //    eventAggregator.GetEvent<MainNotificationPopupEvent>().Publish(new PopupEventArg<INotification>
        //    {
        //        Title = title,
        //        Content = message,
        //        Callback = new Action<INotification>(res => action?.Invoke())
        //    });
        //}

        #region thread sync test
        //private void MessagePopupHandler(PopupEventArg<INotification> arg)
        //{
        //    MainDispatcherInvoke(() => CustomNotificationRequest.Raise(new Notification
        //    {
        //        Title = arg.Title,
        //        Content = arg.Content
        //    }, result => arg.Callback(result)));
        //}

        //private void ConfirmPopupHandler(PopupEventArg<IConfirmation> arg)
        //{
        //    MainDispatcherInvoke(() => CustomConfirmationRequest.Raise(new Confirmation
        //    {
        //        Title = arg.Title,
        //        Content = arg.Content,
        //    }, result => arg.Callback(result)));
        //}

        private void MessagePopupHandler(PopupEventArg<INotification> arg)
        {
            MainDispatcher.Value.Invoke(() => CustomNotificationRequest.Raise(new Notification
            {
                Title = arg.Title,
                Content = arg.Content
            }, result => arg.Callback(result)));
        }

        private void ConfirmPopupHandler(PopupEventArg<IConfirmation> arg)
        {
            MainDispatcher.Value.Invoke(() => CustomConfirmationRequest.Raise(new Confirmation
            {
                Title = arg.Title,
                Content = arg.Content,
            }, result => arg.Callback(result)));
        }
        #endregion

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            // 导航到指定区域的特定视图
            // 视图必须已经在 IModule 的实现类注册过
            region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
            //region?.RequestNavigate(RegionNames.Home, typeof(ConfigView).FullName);
            //region?.RequestNavigate(RegionNames.Home, typeof(AdminConfigView).FullName);

            deviceProxy.Connect();
        });

        public DelegateCommand SettingCommand => new DelegateCommand(() =>
        {
            var callbackViewName = region?.Regions[RegionNames.Home].ActiveViews.FirstOrDefault().GetType().FullName;
            var nParam = new NavigationParameters
            {
                { ShoeBoxKeys.CallbackViewName, callbackViewName }
            };

            region?.RequestNavigate(RegionNames.Home, typeof(ConfigView).FullName, nParam);
            //region?.RequestNavigate(RegionNames.Home, typeof(ResultView).FullName, nParam);
        });

        public InteractionRequest<INotification> CustomNotificationRequest { get; } = new InteractionRequest<INotification>();

        public InteractionRequest<IConfirmation> CustomConfirmationRequest { get; } = new InteractionRequest<IConfirmation>();
    }
}
