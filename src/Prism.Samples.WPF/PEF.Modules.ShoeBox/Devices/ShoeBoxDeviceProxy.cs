
namespace PEF.Modules.ShoeBox.Devices
{
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.ShoeBox.Devices.Dtos;
    using PEF.Modules.ShoeBox.PubSubEvents;
    using PEF.Modules.ShoeBox.Services;
    using PEF.Modules.ShoeBox.Services.Dtos;
    using PEF.Modules.ShoeBox.Views;
    //using PEF.Modules.ShoeBox.PubSubEvents;
    using PEF.Socket.Utilities;
    using PEF.Socket.Utilities.Infrastructures;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Regions;
    using System;

    public interface IShoeBoxDeviceProxy
    {
        /// <summary>
        /// 连接
        /// </summary>
        void Connect();

        void OpenDoor(DeviceOpenDoorDto dto);

        //bool IsConnected { get; }
    }

    public class ShoeBoxDeviceProxy : IShoeBoxDeviceProxy
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILogger logger = null;
        private readonly ShoeBoxConfig config = null;
        private readonly WebSocketClientProxy proxy = null;
        private readonly IShoeBoxServiceProxy service = null;
        private readonly IRegionManager region = null;

        //public bool IsConnected { get; private set; }

        public ShoeBoxDeviceProxy(
            IEventAggregator eventAggregator,
            ILogger logger,
            ShoeBoxConfig config,
            WebSocketClientProxy proxy,
            IShoeBoxServiceProxy service,
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
            //this.proxy.OnWebSockerConnectionStateReport += OnWebSockerConnectionStateReportHandler;
        }

        //private void OnWebSockerConnectionStateReportHandler(object sender, WebSockerConnectionStateReportEventArg e)
        //{
        //    throw new System.NotImplementedException();
        //}

        private void OnDeviceOnlineHandler(object sender, DeviceOnlineEventArg e)
        {
            //IsConnected = true;

            service.SocketStatusReport(new SocketStatusReportRequest { DeviceId = config.DeviceId, Status = 1 });
        }

        private void OnDeviceOfflineHandler(object sender, DeviceOfflineEventArg e)
        {
            //IsConnected = false;

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
                    FrontInfoHandler(e.Message.GetJsonObject<DeviceFrontInfoDto>());
                    break;
                case DeviceMethodTypes.SHReadCard:
                case DeviceMethodTypes.SBReadCard:
                    ReadCardHandler(e.Message.GetJsonObject<DeviceReadCardDto>());
                    break;
                case DeviceMethodTypes.DoorState:
                    DoorStateHandler(e.Message.GetJsonObject<DeviceDoorStateDto>());
                    break;
                //case DeviceMethodTypes.EndRecycle:
                //    DeviceEndRecycleDtoHandler(e.Message.GetJsonObject<DeviceEndRecycleDto>());
                //    break;
                case DeviceMethodTypes.Unknown:
                default:
#if DEBUG
                    //DeviceReturnItemDtoHandler(e.Message.GetJsonObject<DeviceReturnItemDto>());
#endif
                    break;
            }
        }

        protected virtual void FrontInfoHandler(DeviceFrontInfoDto dto)
        {
            if (dto == null)
            {
                return;
            }

            config.SaveAppSetting(nameof(config.DeviceId), dto.Parameter.DeviceId);

            eventAggregator.GetEvent<DeviceIdReceiveEvent>().Publish(dto.Parameter.DeviceId);
        }

        protected virtual void ReadCardHandler(DeviceReadCardDto dto)
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

        protected virtual void DoorStateHandler(DeviceDoorStateDto dto)
        {
            if (dto == null)
            {
                return;
            }

            if (dto.Parameter?.State == "0")
            {
                eventAggregator.GetEvent<DoorCloseReceiveEvent>().Publish(dto.Parameter.DoorId);
            }
            else if (dto.Parameter?.State == "1")
            {
                eventAggregator.GetEvent<DoorOpenReceiveEvent>().Publish(dto.Parameter.DoorId);
            }
            else
            {
                // TODO: 无状态值处理
            }
        }

        protected virtual void Invoke<TParameter>(DeviceDto<TParameter> dto)
            where TParameter : IDeviceParameter
        {
            if (dto == null)
            {
                return;
            }

            proxy.Send(dto.GetJsonString());
        }

        public void OpenDoor(DeviceOpenDoorDto dto) => Invoke(dto);
    }
}
