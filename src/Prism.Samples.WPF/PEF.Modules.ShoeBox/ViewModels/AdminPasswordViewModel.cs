
namespace PEF.Modules.ShoeBox.ViewModels
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.ShoeBox.Services;
    using PEF.Modules.ShoeBox.Services.Dtos;
    using PEF.Modules.ShoeBox.Views;
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
        public readonly IEventAggregator eventAggregator;
        private readonly ShoeBoxConfig config = null;
        private readonly HttpRequestParameters httpRequestParam = null;
        //private readonly IShoeBoxDeviceProxy deviceProxy = null;
        private readonly IShoeBoxServiceProxy serviceProxy = null;

        public AdminPasswordViewModel(
            ILogger logger,
            IRegionManager region,
            IEventAggregator eventAggregator,
            ShoeBoxConfig config,
            HttpRequestParameters httpRequestParam,
            //IShoeBoxDeviceProxy deviceProxy,
            IShoeBoxServiceProxy serviceProxy)
        {
            this.logger = logger;
            this.region = region;
            this.eventAggregator = eventAggregator;
            this.config = config;
            this.httpRequestParam = httpRequestParam;
            //this.deviceProxy = deviceProxy;
            this.serviceProxy = serviceProxy;
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg => httpRequestParam.Headers.Clear());

        private string adminPassKey = string.Empty;

        public string AdminPassKey
        {
            get => adminPassKey;
            set => SetProperty(ref adminPassKey, value);
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

                if (result != null && result.HasData && result.IsSuccessful && result.Data.UserType == "M")
                {
                    region?.RequestNavigate(RegionNames.Home, typeof(AdminConfigView).FullName);
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
