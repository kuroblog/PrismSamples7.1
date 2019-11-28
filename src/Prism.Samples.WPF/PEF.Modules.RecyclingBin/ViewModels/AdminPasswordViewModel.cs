
namespace PEF.Modules.RecyclingBin.ViewModels
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.RecyclingBin.Devices;
    using PEF.Modules.RecyclingBin.Devices.Dtos;
    using PEF.Modules.RecyclingBin.Services;
    using PEF.Modules.RecyclingBin.Services.Dtos;
    using PEF.Modules.RecyclingBin.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;

    public partial class AdminPasswordViewModel : BindableBase
    {
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;
        private readonly RecyclingBinConfig config = null;
        private readonly HttpRequestParameters httpRequestParam = null;
        public readonly IEventAggregator eventAggregator;
        private readonly IRecyclingBinDeviceProxy deviceProxy = null;
        private readonly IRecyclingBinServiceProxy serviceProxy = null;

        public AdminPasswordViewModel(
            ILogger logger,
            IRegionManager region,
            RecyclingBinConfig config,
            HttpRequestParameters httpRequestParam,
            IEventAggregator eventAggregator,
            IRecyclingBinDeviceProxy deviceProxy,
            IRecyclingBinServiceProxy serviceProxy)
        {
            this.logger = logger;
            this.region = region;
            this.config = config;
            this.httpRequestParam = httpRequestParam;
            this.eventAggregator = eventAggregator;
            this.deviceProxy = deviceProxy;
            this.serviceProxy = serviceProxy;
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg => httpRequestParam.Headers.Clear());

        private string adminPassKey = string.Empty;

        public string AdminPassKey
        {
            get { return adminPassKey; }
            set { SetProperty(ref adminPassKey, value); }
        }

        public DelegateCommand ConfirmCommand => new DelegateCommand(() =>
        {
            var tResult = serviceProxy?.TokenRequest(new TokenRequest { CardId = AdminPassKey, ReadStyle = 2 });
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

                var result = serviceProxy.CardQuery(new CardQueryRequest
                {
                    //CardId = Guid.NewGuid().ToString("N"),
                    CardId = AdminPassKey,
                    ReadStyle = 2
                });

                if (result != null && result.HasData && result.IsSuccessful)
                {
                    if (result.Data.UserType != "M")
                    {
                        return;
                    }

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

                    region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
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
        });

        //private void PopupMainNotification(string message, string title = "温馨提示")
        //{
        //    eventAggregator.GetEvent<MainNotificationPopupEvent>().Publish(new PopupEventArg<INotification>
        //    {
        //        Title = title,
        //        Content = message,
        //        Callback = new Action<INotification>(res => { })
        //    });
        //}

        public DelegateCommand ReturnCommand => new DelegateCommand(() =>
        {
            region?.RequestNavigate(RegionNames.Home, typeof(ConfigView).FullName);
        });
    }
}
