
namespace PEF.Modules.SGDE.ViewModels
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.SGDE.Devices;
    using PEF.Modules.SGDE.Devices.Dtos;
    using PEF.Modules.SGDE.Models;
    using PEF.Modules.SGDE.PubSubEvents;
    using PEF.Modules.SGDE.Services;
    using PEF.Modules.SGDE.Services.Dtos;
    using PEF.Modules.SGDE.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    public partial class HomeViewModel : BindableBase, INavigationAware
    {
        private readonly SGDEConfig config = null;
        private readonly HttpRequestParameters httpRequestParam = null;
        private readonly ISGDEDeviceProxy deviceProxy = null;
        private readonly ISGDEServiceProxy serviceProxy = null;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager region = null;
        private readonly ILogger logger = null;

        private int lSizeQuantity;

        public int LSizeQuantity
        {
            get => lSizeQuantity;
            set => SetProperty(ref lSizeQuantity, value);
        }

        private int xlSizeQuantity;

        public int XLSizeQuantity
        {
            get => xlSizeQuantity;
            set => SetProperty(ref xlSizeQuantity, value);
        }

        public HomeViewModel(
            SGDEConfig config,
            HttpRequestParameters httpRequestParam,
            ISGDEDeviceProxy deviceProxy,
            ISGDEServiceProxy serviceProxy,
            IEventAggregator eventAggregator,
            IRegionManager region,
            ILogger logger)
        {
            this.config = config;
            this.httpRequestParam = httpRequestParam;
            this.deviceProxy = deviceProxy;
            this.serviceProxy = serviceProxy;
            this.eventAggregator = eventAggregator;
            this.region = region;
            this.logger = logger;

            eventAggregator.GetEvent<DeviceIdReceiveEvent>().Subscribe(DeviceIdReceiveHandler);
            eventAggregator.GetEvent<CardReceiveEvent>().Subscribe(CardReceiveHandler);
            //eventAggregator.GetEvent<DeliveryCompletedEvent>().Subscribe(DeliveryCompletedHandler);
            eventAggregator.GetEvent<DeliveryCompletedEvent>().Subscribe(DeliveryCompletedV2Handler);
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}
        #endregion

        private void DeviceIdReceiveHandler(string deviceId) { }

        private PersonModel person = new PersonModel { Card = new CardModel { } };
        private ItemSlotModel itemSlot = new ItemSlotModel { };

        private void CardReceiveHandler(CardReceiveEventParameter arg)
        {
            person = new PersonModel { Card = new CardModel { CardId = arg.CardId } };
            itemSlot = new ItemSlotModel { };

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

                // check cardId
                var cqResult = serviceProxy?.CardQuery(new CardQueryRequest { ReadStyle = int.TryParse(arg.ReadStyle, out int rss) ? rss : 0, CardId = arg.CardId });
                if (cqResult != null && cqResult.IsSuccessful && cqResult.HasData)
                {
                    person.UserId = cqResult.Data.UserId;
                    person.UserName = cqResult.Data.UserName;

                    // M 管理员 | U 普通用户 | 无权限
                    if (cqResult.Data.UserType == "M")
                    {
                        #region thread sync test
                        //MainDispatcherInvoke(() => region?.RequestNavigate(
                        //    RegionNames.Home,
                        //    typeof(SizeConfigView).FullName));

                        MainDispatcher.Value.Invoke(() => region?.RequestNavigate(
                            RegionNames.Home,
                            typeof(SizeConfigView).FullName));
                        #endregion
                    }
                    else if (cqResult.Data.UserType == "U")
                    {
                        if (SqItemI.Quantity == string.Empty && SqItemII.Quantity == string.Empty)
                        {
                            MainDispatcher.Value.ShowMessage("该设备无库存，请与管理员联系。");
                            return;
                        }

                        if (string.IsNullOrEmpty(cqResult.Data.Size))
                        {
                            #region thread sync test
                            //MainDispatcherInvoke(() => region?.RequestNavigate(
                            //    RegionNames.Home,
                            //    typeof(SizeChoiceView).FullName,
                            //    new NavigationParameters {
                            //        { SGDEKeys.UserId, cqResult.Data.UserId },
                            //        { SGDEKeys.SizeQuantityI, SqItemI },
                            //        { SGDEKeys.SizeQuantityII, SqItemII } }));

                            MainDispatcher.Value.Invoke(() => region?.RequestNavigate(
                                RegionNames.Home,
                                typeof(SizeChoiceView).FullName,
                                new NavigationParameters {
                                    { SGDEKeys.UserId, cqResult.Data.UserId },
                                    { SGDEKeys.SizeQuantityI, SqItemI },
                                    { SGDEKeys.SizeQuantityII, SqItemII },
                                    { SGDEKeys.SizeQuantityIII, SqItemIII },
                                    { SGDEKeys.SizeQuantityIV, SqItemIV },
                                    { SGDEKeys.SizeQuantityV, SqItemV },
                                    { SGDEKeys.SizeQuantityVI, SqItemVI } }));
                            #endregion
                        }
                        else
                        {
                            //DoApplySubmit(cqResult.Data.UserId, size: cqResult.Data.Size);
                            DoApplySubmitV2(cqResult.Data.UserId, size: cqResult.Data.Size);
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

        //private void PopupMainNotification(string message, string title = "温馨提示")
        //{
        //    eventAggregator.GetEvent<MainNotificationPopupEvent>().Publish(new PopupEventArg<INotification>
        //    {
        //        Title = title,
        //        Content = message,
        //        Callback = new Action<INotification>(res => { })
        //    });
        //}

        //private void DoApplySubmit(long userId, int applyMode = 1, string size = "")
        //{
        //    var result = serviceProxy.ApplySubmit(new ApplySubmitRequest
        //    {
        //        ApplyMode = applyMode,
        //        DeviceId = config.DeviceId,
        //        UserId = userId,
        //        Size = size
        //    });

        //    if (result != null && result.IsSuccessful && result.HasData)
        //    {
        //        itemSlot.SlotId = result.Data.Id;

        //        // true 有存量 | false 无对应存量
        //        if (result.Data.HasSize)
        //        {
        //            itemSlot.X = result.Data.X;
        //            itemSlot.Y = result.Data.Y;
        //            deviceProxy.DeviceDeliveryDtoHandler(new DeviceDeliveryDto(result.Data.X, result.Data.Y));
        //        }
        //        else
        //        {
        //            // 1 订阅事件调用需要使用任务调度器来响应UI迁移 | 2 主线程事件
        //            if (applyMode == 1)
        //            {
        //                MainDispatcherInvoke(() => region?.RequestNavigate(
        //                    RegionNames.Home,
        //                    typeof(SizeChoiceView).FullName,
        //                    new NavigationParameters {
        //                            { SGDEKeys.UserId, userId },
        //                            { SGDEKeys.SizeQuantityI, SqItemI },
        //                            { SGDEKeys.SizeQuantityII, SqItemII } }));
        //            }
        //            else if (applyMode == 2)
        //            {
        //                region?.RequestNavigate(
        //                    RegionNames.Home,
        //                    typeof(SizeChoiceView).FullName,
        //                    new NavigationParameters {
        //                        { SGDEKeys.UserId, userId },
        //                        { SGDEKeys.SizeQuantityI, SqItemI },
        //                        { SGDEKeys.SizeQuantityII, SqItemII } });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // TODO: 请求发衣服失败
        //    }
        //}

        private ApplyQuerySchedule[] schedules = new ApplyQuerySchedule[] { };
        private int saveNo;

        private void DoApplySubmitV2(long userId, int applyMode = 1, string size = "")
        {
            var result = serviceProxy.ApplySubmitV2(new ApplySubmitRequest
            {
                ApplyMode = applyMode,
                DeviceId = config.DeviceId,
                UserId = userId,
                Size = size
            });

            if (result != null && result.IsSuccessful && result.HasData)
            {
                schedules = result.Data.Schedules;
                saveNo = result.Data.SaveNo;
                itemSlot.SlotId = result.Data.Id;

                // true 有存量 | false 无对应存量
                if (result.Data.HasSize)
                {
                    itemSlot.X = result.Data.X;
                    itemSlot.Y = result.Data.Y;
                    deviceProxy.DeviceDeliveryDtoHandler(new DeviceDeliveryDto(result.Data.X, result.Data.Y));

                    serviceProxy.LogReport(new LogReportRequest { DeviceId = config.DeviceId, LogType = 0 });
                }
                else
                {
                    // 1 订阅事件调用需要使用任务调度器来响应UI迁移 | 2 主线程事件
                    if (applyMode == 1)
                    {
                        #region thread sync test
                        //MainDispatcherInvoke(() => region?.RequestNavigate(
                        //    RegionNames.Home,
                        //    typeof(SizeChoiceView).FullName,
                        //    new NavigationParameters {
                        //        { SGDEKeys.UserId, userId },
                        //        { SGDEKeys.SizeQuantityI, SqItemI },
                        //        { SGDEKeys.SizeQuantityII, SqItemII } }));

                        MainDispatcher.Value.Invoke(() => region?.RequestNavigate(
                            RegionNames.Home,
                            typeof(SizeChoiceView).FullName,
                            new NavigationParameters {
                                { SGDEKeys.UserId, userId },
                                { SGDEKeys.SizeQuantityI, SqItemI },
                                { SGDEKeys.SizeQuantityII, SqItemII },
                                { SGDEKeys.SizeQuantityIII, SqItemIII },
                                { SGDEKeys.SizeQuantityIV, SqItemIV },
                                { SGDEKeys.SizeQuantityV, SqItemV },
                                { SGDEKeys.SizeQuantityVI, SqItemVI } }));
                        #endregion
                    }
                    else if (applyMode == 2)
                    {
                        region?.RequestNavigate(
                            RegionNames.Home,
                            typeof(SizeChoiceView).FullName,
                            new NavigationParameters {
                                { SGDEKeys.UserId, userId },
                                { SGDEKeys.SizeQuantityI, SqItemI },
                                { SGDEKeys.SizeQuantityII, SqItemII },
                                { SGDEKeys.SizeQuantityIII, SqItemIII },
                                { SGDEKeys.SizeQuantityIV, SqItemIV },
                                { SGDEKeys.SizeQuantityV, SqItemV },
                                { SGDEKeys.SizeQuantityVI, SqItemVI } });
                    }
                }
            }
            else
            {
                MainDispatcher.Value.ShowMessage(result.Message);
            }
        }

        //private void DeliveryCompletedHandler(string code)
        //{
        //    // TODO
        //    // 1. 错误提示
        //    // 2. 第二次刷卡的结果先于第一次的发衣结果返回，会导致第一次的 userId、itemId 和 cardId 被覆盖
        //    // 0 成功 | 1 空货 | 2 出货失败
        //    if (code == "0")
        //    {
        //        var result = serviceProxy.ApplyQuery(new ApplyQueryRequest { CardId = person.Card.CardId, Id = itemSlot.SlotId, UserId = person.UserId, UserName = person.UserName, });
        //        if (result != null && result.IsSuccessful && result.HasData)
        //        {
        //            if (result.Data.Schedules != null && result.Data.Schedules.Any())
        //            {
        //                MainDispatcherInvoke(() => region?.RequestNavigate(
        //                    RegionNames.Home,
        //                    typeof(ScheduleResultView).FullName,
        //                    new NavigationParameters {
        //                        { SGDEKeys.UserName, person.UserName },
        //                        { SGDEKeys.SaveBoxNo, result.Data.SaveNo.ToString() },
        //                        { SGDEKeys.UserSchedules, result.Data.Schedules } }));
        //            }
        //            else
        //            {
        //                MainDispatcherInvoke(() => region?.RequestNavigate(
        //                    RegionNames.Home,
        //                    typeof(DistributeResultView).FullName,
        //                    new NavigationParameters {
        //                        { SGDEKeys.UserName, person.UserName },
        //                        { SGDEKeys.SaveBoxNo, result.Data.SaveNo } }));
        //            }
        //        }
        //        // TODO
        //        // 服务端发衣失败
        //        else
        //        {

        //        }
        //    }
        //    else if (code == "1") { }
        //    else if (code == "2") { }
        //    else { }

        //    MainDispatcherInvoke(() => RefreshSizeQuantityItems());
        //}

        #region thread sync test
        //private void DeliveryCompletedV2Handler(string code)
        //{
        //    // TODO
        //    // 1. 错误提示
        //    // 2. 第二次刷卡的结果先于第一次的发衣结果返回，会导致第一次的 userId、itemId 和 cardId 被覆盖
        //    // 0 成功 | 1 空货 | 2 出货失败
        //    if (code == "0")
        //    {
        //        var viewName = string.Empty;
        //        var param = new NavigationParameters {
        //            { SGDEKeys.UserName, person.UserName },
        //            { SGDEKeys.SaveBoxNo, saveNo.ToString() } };

        //        if (schedules != null && schedules.Any())
        //        {
        //            viewName = typeof(ScheduleResultView).FullName;
        //            param.Add(SGDEKeys.UserSchedules, schedules);
        //        }
        //        else
        //        {
        //            viewName = typeof(DistributeResultView).FullName;
        //        }

        //        MainDispatcherInvoke(() => region?.RequestNavigate(RegionNames.Home, viewName, param));
        //    }
        //    else if (code == "1") { }
        //    else if (code == "2") { }
        //    else { }

        //    MainDispatcherInvoke(() => RefreshSizeQuantityItems());
        //}

        private void DeliveryCompletedV2Handler(string code)
        {
            // TODO
            // 1. 错误提示
            // 2. 第二次刷卡的结果先于第一次的发衣结果返回，会导致第一次的 userId、itemId 和 cardId 被覆盖
            // 0 成功 | 1 空货 | 2 出货失败
            if (code == "0")
            {
                var viewName = string.Empty;
                var param = new NavigationParameters {
                    { SGDEKeys.UserName, person.UserName },
                    { SGDEKeys.SaveBoxNo, saveNo.ToString() } };

                if (schedules != null && schedules.Any())
                {
                    viewName = typeof(ScheduleResultView).FullName;
                    param.Add(SGDEKeys.UserSchedules, schedules);
                }
                else
                {
                    viewName = typeof(DistributeResultView).FullName;
                }

                MainDispatcher.Value.Invoke(() => region?.RequestNavigate(RegionNames.Home, viewName, param));
            }
            else if (code == "1") { }
            else if (code == "2") { }
            else { }

            MainDispatcher.Value.Invoke(() => RefreshSizeQuantityItems());
        }
        #endregion

        private SizeQuantityModel sqItemI;

        public SizeQuantityModel SqItemI
        {
            get => sqItemI;
            set => SetProperty(ref sqItemI, value);
        }

        private SizeQuantityModel sqItemII;

        public SizeQuantityModel SqItemII
        {
            get => sqItemII;
            set => SetProperty(ref sqItemII, value);
        }

        private SizeQuantityModel sqItemIII;

        public SizeQuantityModel SqItemIII
        {
            get => sqItemIII;
            set => SetProperty(ref sqItemIII, value);
        }

        private SizeQuantityModel sqItemIV;

        public SizeQuantityModel SqItemIV
        {
            get => sqItemIV;
            set => SetProperty(ref sqItemIV, value);
        }

        private SizeQuantityModel sqItemV;

        public SizeQuantityModel SqItemV
        {
            get => sqItemV;
            set => SetProperty(ref sqItemV, value);
        }

        private SizeQuantityModel sqItemVI;

        public SizeQuantityModel SqItemVI
        {
            get => sqItemVI;
            set => SetProperty(ref sqItemVI, value);
        }

        private Func<List<SizeQueryItem>, int, double, SizeQuantityModel> indexOfSizeQueryItems = (source, index, width) =>
        {
            if (source.Count >= index + 1)
            {
                return new SizeQuantityModel
                {
                    Size = source[index].Size,
                    Quantity = source[index].Quantity.ToString(),
                    Width = width
                };
            }

            return new SizeQuantityModel();
        };

        private List<SizeQueryItem> sizeItems = new List<SizeQueryItem>();

        private void RefreshSizeQuantityItems()
        {
            sizeItems.Clear();

            var result = serviceProxy?.SizeQuery(new SizeQueryRequest { DeviceId = config.DeviceId });
            if (result != null && result.IsSuccessful && result.HasData)
            {
                sizeItems = result.Data.Items.ToList();
            }

            RefreshItems();
        }

        private void RefreshItems()
        {
            var counter = sizeItems.Count() < 2 ? 2 : sizeItems.Count();
            var width = (imageBorder.ActualWidth - counter * 2 * 5) / counter;

            if (sizeItems.Count == 0)
            {
                SqItemI = new SizeQuantityModel
                {
                    Width = width
                };
            }
            else
            {
                SqItemI = indexOfSizeQueryItems(sizeItems, 0, width);
            }

            SqItemII = indexOfSizeQueryItems(sizeItems, 1, width);
            SqItemIII = indexOfSizeQueryItems(sizeItems, 2, width);
            SqItemIV = indexOfSizeQueryItems(sizeItems, 3, width);
            SqItemV = indexOfSizeQueryItems(sizeItems, 4, width);
            SqItemVI = indexOfSizeQueryItems(sizeItems, 5, width);
        }

        private Border imageBorder;

        public DelegateCommand<RoutedEventArgs> LoadedCommand => new DelegateCommand<RoutedEventArgs>(arg =>
        {
            var view = arg.Source as HomeView;
            imageBorder = view.ImageBorder;

            view.SizeChanged += (s, e) => RefreshItems();

            httpRequestParam.Headers.Clear();

            RefreshSizeQuantityItems();
        });

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var viewName = navigationContext.Parameters[SGDEKeys.CallbackViewName] as string;
            if (viewName == typeof(SizeChoiceView).FullName)
            {
                var size = navigationContext.Parameters[SGDEKeys.SelectedSize] as string;
                //DoApplySubmit(person.UserId, 2, size);
                DoApplySubmitV2(person.UserId, 2, size);
            }
        }
        #endregion
    }
}
