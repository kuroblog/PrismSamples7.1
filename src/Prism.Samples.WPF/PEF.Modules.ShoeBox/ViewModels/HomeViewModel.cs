
namespace PEF.Modules.ShoeBox.ViewModels
{
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    //using PEF.Modules.ShoeBox.Devices.Dtos;
    //using PEF.Modules.ShoeBox.PubSubEvents;
    using PEF.Modules.ShoeBox.Services;
    using PEF.Modules.ShoeBox.Services.Dtos;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;

    public partial class HomeViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;
        private readonly ShoeBoxConfig config = null;
        private readonly HttpRequestParameters httpRequestParam = null;
        //private readonly IShoeBoxDeviceProxy deviceProxy = null;
        private readonly IShoeBoxServiceProxy serviceProxy = null;

        public HomeViewModel(
            IEventAggregator eventAggregator,
            ILogger logger,
            IRegionManager region,
            ShoeBoxConfig config,
            HttpRequestParameters httpRequestParam,
            //IShoeBoxDeviceProxy deviceProxy,
            IShoeBoxServiceProxy serviceProxy)
        {
            this.eventAggregator = eventAggregator;

            this.logger = logger;
            this.region = region;
            this.config = config;
            this.httpRequestParam = httpRequestParam;
            //this.deviceProxy = deviceProxy;
            this.serviceProxy = serviceProxy;

            // 使用匿名函数会导致访问不到当前对象
            //eventAggregator.GetEvent<DeviceIdReceiveEvent>().Subscribe(DeviceIdReceiveHandler);
            //eventAggregator.GetEvent<CardIdReceiveEvent>().Subscribe(CardIdReceiveHandler);
        }

        private int quantity;

        public int Quantity
        {
            get => quantity;
            set => SetProperty(ref quantity, value);
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            httpRequestParam.Headers.Clear();

            var dsqResult = serviceProxy?.DeviceStateQuery(new DeviceStateQueryRequest { DeviceId = config.DeviceId });
            if (dsqResult != null && dsqResult.IsSuccessful && dsqResult.HasData)
            {
                Quantity = dsqResult.Data.Quantity;
            }
            else
            {
                Quantity = 0;
            }
        });
    }
}
