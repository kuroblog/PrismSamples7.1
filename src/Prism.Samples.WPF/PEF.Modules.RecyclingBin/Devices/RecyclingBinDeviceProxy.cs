
namespace PEF.Modules.RecyclingBin.Devices
{
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.RecyclingBin.Devices.Dtos;
    using PEF.Modules.RecyclingBin.PubSubEvents;
    using PEF.Modules.RecyclingBin.Services;
    using PEF.Modules.RecyclingBin.Services.Dtos;
    using PEF.Modules.RecyclingBin.Views;
    using PEF.Socket.Utilities;
    using PEF.Socket.Utilities.Infrastructures;
    using Prism.Events;
    using Prism.Regions;
    using System;

    /// <summary>
    /// 回收桶硬件代理接口
    /// </summary>
    public interface IRecyclingBinDeviceProxy
    {
        /// <summary>
        /// 连接
        /// </summary>
        void Connect();

        //void DeviceFrontInfoDtoHandler(DeviceFrontInfoDto dto);

        //void DeviceReadCardDtoHandler(DeviceReadCardDto dto);

        /// <summary>
        /// 非管理员开桶
        /// </summary>
        /// <param name="dto"></param>
        void DeviceOpenDoorDtoHandler(DeviceOpenDoorDto dto);

        /// <summary>
        /// 管理员开桶
        /// </summary>
        /// <param name="dto"></param>
        void DeviceOpenLockDtoHandler(DeviceOpenLockDto dto);
    }

    /// <summary>
    /// 回收桶硬件代理类
    /// </summary>
    public class RecyclingBinDeviceProxy : IRecyclingBinDeviceProxy
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILogger logger = null;
        private readonly RecyclingBinConfig config = null;
        private readonly WebSocketClientProxy proxy = null;
        private readonly IRecyclingBinServiceProxy service = null;
        private readonly IRegionManager region = null;

        public RecyclingBinDeviceProxy(
            IEventAggregator eventAggregator,
            ILogger logger,
            RecyclingBinConfig config,
            WebSocketClientProxy proxy,
            IRecyclingBinServiceProxy service,
            IRegionManager region)
        {
            this.eventAggregator = eventAggregator;

            this.logger = logger;
            this.config = config;
            this.proxy = proxy;
            this.service = service;
            this.region = region;

            this.proxy.Initialize(this.config.SocketUrl);
            this.proxy.OnReceiveMessage += OnReceiveMessageHandler;
            this.proxy.OnDeviceOffline += OnDeviceOfflineHandler;
            this.proxy.OnDeviceOnline += OnDeviceOnlineHandler;
        }

        private void OnDeviceOnlineHandler(object sender, DeviceOnlineEventArg e)
        {
            service.SocketStatusReport(new SocketStatusReportRequest { DeviceId = config.DeviceId, Status = 1 });
        }

        private void OnDeviceOfflineHandler(object sender, DeviceOfflineEventArg e)
        {
            service.SocketStatusReport(new SocketStatusReportRequest { DeviceId = config.DeviceId, Status = 2 });

            MainDispatcher.Value.ShowMessage($"设备离线中……{Environment.NewLine}请重启应用程序或与管理员联系。", action: n => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));
        }

        public void Connect() => proxy?.Connect();

        protected virtual void OnReceiveMessageHandler(object sender, ReceivedMessageEventArg e)
        {
            //logger.Trace(e.Message);

            if (string.IsNullOrEmpty(e.Message))
            {
                return;
            }

            var baseDto = e.Message.GetJsonObject<DeviceBaseDto>();
            if (baseDto == null)
            {
                return;
            }

            switch (baseDto.MethodType)
            {
                case DeviceMethodTypes.FrontInfo:
                    DeviceFrontInfoDtoHandler(e.Message.GetJsonObject<DeviceFrontInfoDto>());
                    break;
                case DeviceMethodTypes.ReadCard:
                    DeviceReadCardDtoHandler(e.Message.GetJsonObject<DeviceReadCardDto>());
                    break;
                case DeviceMethodTypes.ReturnItem:
                    DeviceReturnItemDtoHandler(e.Message.GetJsonObject<DeviceReturnItemDto>());
                    break;
                case DeviceMethodTypes.EndRecycle:
                    DeviceEndRecycleDtoHandler(e.Message.GetJsonObject<DeviceEndRecycleDto>());
                    break;
                case DeviceMethodTypes.Unknown:
                default:
#if DEBUG
                    //DeviceReturnItemDtoHandler(e.Message.GetJsonObject<DeviceReturnItemDto>());
#endif
                    break;
            }
        }

        protected virtual void DeviceFrontInfoDtoHandler(DeviceFrontInfoDto dto)
        {
            if (dto == null)
            {
                return;
            }

            config.SaveAppSetting(nameof(config.DeviceId), dto.Parameter.DeviceId);

            eventAggregator.GetEvent<DeviceIdReceiveEvent>().Publish(dto.Parameter.DeviceId);
        }

        protected virtual void DeviceReadCardDtoHandler(DeviceReadCardDto dto)
        {
            if (dto == null)
            {
                return;
            }

            eventAggregator.GetEvent<CardReceiveEvent>().Publish(new CardReceiveEventParameter
            {
                CardId = dto.Parameter.CardId,
                ReadStyle = dto.Parameter.ReadStyle
            });
        }

        public virtual void DeviceOpenDoorDtoHandler(DeviceOpenDoorDto dto) => Invoke(dto);

        protected virtual void DeviceReturnItemDtoHandler(DeviceReturnItemDto dto)
        {
            if (dto == null)
            {
                return;
            }

            eventAggregator.GetEvent<ItemReveiveEvent>().Publish(dto.Parameter.GroupId);
        }

        protected virtual void DeviceEndRecycleDtoHandler(DeviceEndRecycleDto dto)
        {
            if (dto == null)
            {
                return;
            }

            eventAggregator.GetEvent<CloseDoorEvent>().Publish(dto.Parameter.GroupId);
        }

        public virtual void DeviceOpenLockDtoHandler(DeviceOpenLockDto dto) => Invoke(dto);

        protected virtual void Invoke<TParameter>(DeviceDto<TParameter> dto)
            where TParameter : IDeviceParameter
        {
            if (dto == null)
            {
                return;
            }

            proxy.Send(dto.GetJsonString());
        }
    }
}
