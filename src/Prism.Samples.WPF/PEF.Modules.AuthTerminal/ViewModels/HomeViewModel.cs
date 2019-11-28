
namespace PEF.Modules.AuthTerminal.ViewModels
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using PEF.Http.Utilities;
    using PEF.Modules.AuthTerminal.PubSubEvents;
    using PEF.Modules.AuthTerminal.Services;
    using PEF.Modules.AuthTerminal.Services.Dtos;
    using PEF.Modules.AuthTerminal.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public partial class HomeViewModel : BindableBase, INavigationAware
    {
        private readonly IAuthTerminalServiceProxy serviceProxy = null;
        private readonly IRegionManager region = null;
        private readonly HttpRequestParameters httpRequestParam = null;
        private readonly IEventAggregator eventAggregator;

        public HomeViewModel(
            IAuthTerminalServiceProxy serviceProxy,
            IRegionManager region,
            HttpRequestParameters httpRequestParam,
            IEventAggregator eventAggregator)
        {
            this.serviceProxy = serviceProxy;
            this.region = region;
            this.httpRequestParam = httpRequestParam;
            this.eventAggregator = eventAggregator;

            eventAggregator.GetEvent<DeviceIdReceiveEvent>().Subscribe(DeviceIdReceiveHandler);
            eventAggregator.GetEvent<CardReceiveEvent>().Subscribe(CardReceiveHandler);

            dt = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            dt.Tick += DispatcherTimerTickHandler;
        }

        private readonly DispatcherTimer dt;

        private string timeInfo;

        public string TimeInfo
        {
            get => timeInfo;
            set => SetProperty(ref timeInfo, value);
        }

        private string dateInfo;

        public string DateInfo
        {
            get => dateInfo;
            set => SetProperty(ref dateInfo, value);
        }

        private string weekInfo;

        public string WeekInfo
        {
            get => weekInfo;
            set => SetProperty(ref weekInfo, value);
        }

        private void DispatcherTimerTickHandler(object sender, EventArgs e)
        {
            var time = DateTime.Now;
            TimeInfo = time.ToString("HH:mm:ss");
            DateInfo = time.ToString("yyyy-MM-dd");
            WeekInfo = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(time.DayOfWeek);
        }

        private bool isShowAuthMessage;

        public bool IsShowAuthMessage
        {
            get => isShowAuthMessage;
            set => SetProperty(ref isShowAuthMessage, value);
        }

        private void DeviceIdReceiveHandler(string deviceId) { }

        private string userName;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string roleName;

        public string RoleName
        {
            get => roleName;
            set => SetProperty(ref roleName, value);
        }

        private int scanCounter = 1;

        private void CardReceiveHandler(CardReceiveEventParameter arg)
        {
            var currentView = region?.Regions[RegionNames.Home].ActiveViews.FirstOrDefault().GetType().FullName;
            if (currentView == typeof(ScanView).FullName)
            {
                if (scanCounter < 3)
                {
                    MainDispatcher.Value.ShowMessage($"请继续刷{3 - scanCounter}次。{Environment.NewLine}谢谢！");
                    scanCounter++;
                }
                else
                {
                    #region thread sync test
                    //MainDispatcherInvoke(() =>
                    //    region?.RequestNavigate(
                    //        RegionNames.Home,
                    //        typeof(RoleChoiceView).FullName,
                    //        new NavigationParameters {
                    //            { AuthTerminalKeys.CardId, arg.CardId },
                    //            { AuthTerminalKeys.GuestReadStyle, arg.ReadStyle.ToString() } }));

                    MainDispatcher.Value.Invoke(() =>
                        region?.RequestNavigate(
                            RegionNames.Home,
                            typeof(RoleChoiceView).FullName,
                            new NavigationParameters {
                                { AuthTerminalKeys.CardId, arg.CardId },
                                { AuthTerminalKeys.GuestReadStyle, arg.ReadStyle.ToString() } }));
                    #endregion
                }

                return;
            }

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

                var result = serviceProxy?.CardQuery(new CardQueryRequest { ReadStyle = currentView == typeof(ScanView).FullName ? 1 : int.TryParse(arg.ReadStyle, out int rss) ? rss : 0, CardId = arg.CardId });
                if (result != null && result.IsSuccessful && result.HasData)
                {
                    if (result.Data.UserType == "M")
                    {
                        #region thread sync test
                        //MainDispatcherInvoke(() =>
                        //{
                        //    UserName = result.Data.UserName;
                        //    IsShowAuthMessage = true;
                        //    RoleName = string.Empty;

                        //    //AutoDisableAuthMessage();

                        //    region?.RequestNavigate(RegionNames.Home, typeof(ScanView).FullName);
                        //});

                        MainDispatcher.Value.Invoke(() =>
                        {
                            UserName = result.Data.UserName;
                            IsShowAuthMessage = true;
                            RoleName = string.Empty;

                            //AutoDisableAuthMessage();

                            region?.RequestNavigate(RegionNames.Home, typeof(ScanView).FullName);
                        });
                        #endregion
                    }
                    else if (result.Data.UserType == "U")
                    {
                        // TODO: 提示无权限授权操作
                    }
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

        //private void PopupMainNotification(string message, string title = "温馨提示")
        //{
        //    eventAggregator.GetEvent<MainNotificationPopupEvent>().Publish(new PopupEventArg<INotification>
        //    {
        //        Title = title,
        //        Content = message,
        //        Callback = new Action<INotification>(res => { })
        //    });
        //}

        private void AutoDisableAuthMessage()
        {
            Task.Factory.StartNew(() =>
            {
                var autoCloseTime = 6;

                for (; ; )
                {
                    if (autoCloseTime == 0)
                    {
                        #region thread sync test
                        //MainDispatcherInvoke(() =>
                        //{
                        //    IsShowAuthMessage = false;
                        //    UserName = string.Empty;
                        //});

                        MainDispatcher.Value.Invoke(() =>
                        {
                            IsShowAuthMessage = false;
                            UserName = string.Empty;
                        });
                        break;
                        #endregion
                    }

                    Task.Delay(1000).Wait();
                    autoCloseTime--;
                }
            });
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}
        #endregion

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            httpRequestParam.Headers.Clear();

            eventAggregator.GetEvent<SetAuthUserCommandVisibility>().Publish(Visibility.Visible);

            dt.Start();

            scanCounter = 1;
        });

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var viewName = navigationContext.Parameters[AuthTerminalKeys.CallbackViewName] as string;

            if (viewName == typeof(RoleChoiceView).FullName)
            {
                var results = navigationContext.Parameters[AuthTerminalKeys.UserRoleBindingResult] as string[];

                UserName = results[0];
                IsShowAuthMessage = true;
                RoleName = $"（{results[1]}）";

                AutoDisableAuthMessage();
            }
        }
        #endregion
    }
}
